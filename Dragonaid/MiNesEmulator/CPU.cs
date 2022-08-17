using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AtomosZ.DragonAid.Libraries;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;

namespace AtomosZ.MiNesEmulator
{
	public class CPU
	{
		public byte[] mem;
		public Stack theStack;
		public ControlUnit cu;

		public byte[] zeroPages
		{
			get
			{
				return this[0, 0x100];
			}
		}

		public byte[] stack
		{
			get { return this[Stack.stackStart, 0x100]; }
		}

		public byte[] newRAM
		{
			get { return this[0, 0x2000]; }
		}

		public byte[] saveRAM
		{
			get { return this[0x6000, 0x2000]; }
		}

		/// <summary>
		/// Copies bytes from byte array to pointer address.
		/// </summary>
		/// <param name="pointer"></param>
		/// <returns>nothing</returns>
		public byte[] this[int pointer]
		{
			set
			{
				for (int i = 0; i < value.Length; ++i)
				{
					mem[pointer + i] = value[i];
				}
			}
		}

		/// <summary>
		/// Copies memory of length bytes to byte array.
		/// </summary>
		/// <param name="pointer"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public byte[] this[int pointer, int length]
		{
			get
			{
				byte[] b = new byte[length];
				for (int i = 0; i < length; ++i)
					b[i] = mem[pointer + i];

				return b;
			}
		}

		public CPU()
		{
			mem = Enumerable.Repeat((byte)0x00, 0x10000).ToArray();
			theStack = new Stack(this);
			cu = new ControlUnit(this);
		}

		public void Reset()
		{
			mem = Enumerable.Repeat((byte)0x00, 0x10000).ToArray();
			cu.Reset();
			Initialize();
		}

		/// <summary>
		/// Sets program counter to Reset pointer
		/// </summary>
		public void Initialize()
		{
			cu.pc = GetPointerAt(UniversalConsts.RESET_Pointer);
		}


		public string GetASM(int address)
		{
			var line = mem[address];
			if (!Opcodes.opcodes.TryGetValue(line, out Opcode opc))
			{
				return $"??? ({line.ToString("X2")})";
			}

			Instruction instr = new Instruction();
			instr.address = address;
			instr.opcode = opc;

			instr.operands = new byte[opc.bytes - 1];
			for (int i = 1; i < opc.bytes; ++i)
			{
				instr.operands[i - 1] = mem[address + i];
			}

			return instr.ToString();
		}

		private Instruction GetInstruction(int address)
		{
			var line = mem[address];
			if (!Opcodes.opcodes.TryGetValue(line, out Opcode opc))
			{
				throw new Exception($"Invalid opcode $({line.ToString("X2")}) at ${address.ToString("x2")}");
			}

			var instr = new Instruction();
			instr.opcode = opc;

			instr.operands = new byte[opc.bytes - 1];
			for (int i = 1; i < opc.bytes; ++i)
			{
				instr.operands[i - 1] = mem[address + i];
			}

			return instr;
		}

		private int GetPointerAt(int lowByteAddress)
		{
			return mem[lowByteAddress] + (mem[lowByteAddress + 1] << 8);
		}


		private int GetBRKPointer()
		{
			var BRKPointer = UniversalConsts.IRQBRK_Pointer;
			return mem[BRKPointer] + (mem[BRKPointer + 1] << 8);
		}


		/// <summary>
		/// This class stores no data or status. It is just a go-between to the memory
		/// for simplicity.
		/// All indexing is relative to the start of the STACK (not cpu memory).
		/// </summary>
		public class Stack
		{
			internal const int stackStart = 0x100;

			private CPU cpu;


			public Stack(CPU cpu)
			{
				this.cpu = cpu;
			}

			public byte this[byte index]
			{
				get { return cpu.mem[stackStart + index]; }
				set { cpu.mem[stackStart + index] = value; }
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="sp">Stack pointer</param>
			/// <returns></returns>
			public byte PullByte(byte sp)
			{
				return cpu.mem[stackStart + sp];
			}
		}


		/// <summary>
		/// @TODO Implement cycle count.
		/// </summary>
		public class ControlUnit
		{
			private CPU cpu;

			public byte a;
			public byte x;
			public byte y;
			public int pc;
			public byte sp = 0xFF;


			// processor status flags
			public bool carry;
			public bool zero;
			public bool interrupt;
			public bool overflow;
			public bool negative;


			/// <summary>
			/// Processor status flags
			/// </summary>
			public byte ps
			{
				get
				{
					return (byte)((carry ? 0x80 : 0) + (zero ? 0x40 : 0) + (interrupt ? 0x20 : 0)
						+ (overflow ? 0x02 : 0) + (negative ? 0x01 : 0));
				}
				set
				{
					carry = (ps & 0x80) == 1;
					zero = (ps & 0x40) == 1;
					interrupt = (ps & 0x20) == 1;
					overflow = (ps & 0x02) == 1;
					negative = (ps & 0x01) == 1;
				}
			}

			public ControlUnit(CPU cpu)
			{
				this.cpu = cpu;
				Reset();
			}

			public void Reset()
			{
				a = 0x00;
				x = 0x00;
				y = 0x00;
				pc = 0x00;
				sp = 0xFF;

				carry = false;
				zero = false;
				interrupt = false;
				overflow = false;
				negative = false;
			}


			private void PushByteToStack(byte value)
			{
				cpu.theStack[sp--] = value;
			}

			private void PushPointerToStack(byte[] operands)
			{
				PushByteToStack(operands[1]);
				PushByteToStack(operands[0]);
			}

			private void PushPointerToStack(int address)
			{
				PushByteToStack((byte)(address >> 8));
				PushByteToStack((byte)(address));
			}

			private byte PullByteFromStack()
			{
				return cpu.theStack[++sp];
			}

			private int PullPointerFromStack()
			{
				var lowByte = PullByteFromStack();
				var highByte = PullByteFromStack();

				return lowByte + (highByte << 8);
			}


			public void ParseNextInstruction()
			{
				var instr = cpu.GetInstruction(pc);
				Parse(instr);
			}


			/// <summary>
			/// Reads instruction, performs the appropirate operations, then increments the PC.
			/// </summary>
			/// <param name="instruction"></param>
			public void Parse(Instruction instruction)
			{
				switch (instruction.opcode.opc)
				{
					case Opcodes.ADC_imm:
						ADC(instruction.operands[0]);
						break;

					case Opcodes.ADC_zpg:
						ADC(cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.ADC_zpx:
						ADC(cpu.mem[instruction.operands[0] + x]);
						break;

					case Opcodes.ADC_abs:
						ADC(cpu.mem[instruction.GetPointer()]);
						break;
					case Opcodes.ADC_abx:
						ADC(cpu.mem[instruction.GetPointer() + x]);
						break;
					case Opcodes.ADC_aby:
						ADC(cpu.mem[instruction.GetPointer() + y]);
						break;

					case Opcodes.ADC_inx:
						ADC(cpu.mem[cpu.mem[instruction.GetPointer() + x]]);
						break;
					case Opcodes.ADC_iny:
						ADC(cpu.mem[cpu.mem[instruction.GetPointer()] + y]);
						break;


					case Opcodes.AND_imm:
						AND(instruction.operands[0]);
						break;

					case Opcodes.AND_zpg:
						AND(cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.AND_zpx:
						AND(cpu.mem[instruction.operands[0] + x]);
						break;

					case Opcodes.AND_abs:
						AND(cpu.mem[instruction.GetPointer()]);
						break;
					case Opcodes.AND_abx:
						AND(cpu.mem[instruction.GetPointer() + x]);
						break;
					case Opcodes.AND_aby:
						AND(cpu.mem[instruction.GetPointer() + y]);
						break;
					case Opcodes.AND_inx:
						AND(cpu.mem[cpu.mem[instruction.GetPointer() + x]]);
						break;
					case Opcodes.AND_iny:
						AND(cpu.mem[cpu.mem[instruction.GetPointer()] + y]);
						break;


					case Opcodes.ASL_acc:
						a = ASL(a);
						break;

					case Opcodes.ASL_zpg:
						cpu.mem[instruction.operands[0]] = ASL(cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.ASL_zpx:
						cpu.mem[instruction.operands[0] + x] = ASL(cpu.mem[instruction.operands[0] + x]);
						break;
					case Opcodes.ASL_abs:
					{
						var addr = instruction.GetPointer();
						cpu.mem[addr] = ASL(cpu.mem[addr]);
						break;
					}
					case Opcodes.ASL_abx:
					{
						var addr = instruction.GetPointer() + x;
						cpu.mem[addr] = ASL(cpu.mem[addr]);
						break;
					}

					case Opcodes.BCC:
						if (!carry)
						{
							MovePC(instruction.operands[0]);
						}
						break;
					case Opcodes.BCS:
						if (carry)
						{
							MovePC(instruction.operands[0]);
						}
						break;
					case Opcodes.BEQ:
						if (zero)
						{
							MovePC(instruction.operands[0]);
						}
						break;
					case Opcodes.BNE:
						if (!zero)
						{
							MovePC(instruction.operands[0]);
						}
						break;
					case Opcodes.BMI:
						if (negative)
						{
							MovePC(instruction.operands[0]);
						}
						break;
					case Opcodes.BPL:
						if (!negative)
						{
							MovePC(instruction.operands[0]);
						}
						break;
					case Opcodes.BVC:
						if (!overflow)
						{
							MovePC(instruction.operands[0]);
						}
						break;
					case Opcodes.BVS:
						if (overflow)
						{
							MovePC(instruction.operands[0]);
						}
						break;

					case Opcodes.BIT_zpg:
						BIT(cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.BIT_abs:
						BIT(cpu.mem[instruction.GetPointer()]);
						break;


					case Opcodes.BRK:
						PushPointerToStack(pc + 2);
						byte sr = (byte)(ps | 0x20); // interrupt flag turned on
						PushByteToStack(sr);
						pc = cpu.GetBRKPointer();
						return; // skip the pc += opc bytes


					case Opcodes.CMP_imm:
						CMP(a, instruction.operands[0]);
						break;
					case Opcodes.CMP_zpg:
						CMP(a, cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.CMP_zpx:
						CMP(a, cpu.mem[instruction.operands[0] + x]);
						break;
					case Opcodes.CMP_abs:
						CMP(a, cpu.mem[instruction.GetPointer()]);
						break;
					case Opcodes.CMP_abx:
						CMP(a, cpu.mem[instruction.GetPointer() + x]);
						break;
					case Opcodes.CMP_aby:
						CMP(a, cpu.mem[instruction.GetPointer() + y]);
						break;
					case Opcodes.CMP_inx:
						CMP(a, cpu.mem[cpu.mem[instruction.GetPointer() + x]]);
						break;
					case Opcodes.CMP_iny:
						CMP(a, cpu.mem[cpu.mem[instruction.GetPointer()] + y]);
						break;

					case Opcodes.CPX_imm:
						CMP(x, instruction.operands[0]);
						break;
					case Opcodes.CPX_zpg:
						CMP(x, cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.CPX_abs:
						CMP(x, cpu.mem[instruction.GetPointer()]);
						break;

					case Opcodes.CPY_imm:
						CMP(y, instruction.operands[0]);
						break;
					case Opcodes.CPY_zpg:
						CMP(y, cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.CPY_abs:
						CMP(y, cpu.mem[instruction.GetPointer()]);
						break;


					case Opcodes.DEC_zpg:
						SetNZ(--cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.DEC_zpx:
						SetNZ(--cpu.mem[instruction.operands[0] + x]);
						break;
					case Opcodes.DEC_abs:
						SetNZ(--cpu.mem[instruction.GetPointer()]);
						break;
					case Opcodes.DEC_abx:
						SetNZ(--cpu.mem[instruction.GetPointer() + x]);
						break;
					case Opcodes.DEX:
						SetNZ(--x);
						break;
					case Opcodes.DEY:
						SetNZ(--y);
						break;

					case Opcodes.INC_zpg:
						SetNZ(++cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.INC_zpx:
						SetNZ(++cpu.mem[instruction.operands[0] + x]);
						break;
					case Opcodes.INC_abs:
						SetNZ(++cpu.mem[instruction.GetPointer()]);
						break;
					case Opcodes.INC_abx:
						SetNZ(++cpu.mem[instruction.GetPointer() + x]);
						break;
					case Opcodes.INX:
						SetNZ(++x);
						break;
					case Opcodes.INY:
						SetNZ(++y);
						break;


					case Opcodes.EOR_imm:
						EOR(instruction.operands[0]);
						break;
					case Opcodes.EOR_zpg:
						EOR(cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.EOR_zpx:
						EOR(cpu.mem[instruction.operands[0] + x]);
						break;
					case Opcodes.EOR_abs:
						EOR(cpu.mem[instruction.GetPointer()]);
						break;
					case Opcodes.EOR_abx:
						EOR(cpu.mem[instruction.GetPointer() + x]);
						break;
					case Opcodes.EOR_aby:
						EOR(cpu.mem[instruction.GetPointer() + y]);
						break;
					case Opcodes.EOR_inx:
						EOR(cpu.mem[cpu.mem[instruction.GetPointer() + x]]);
						break;
					case Opcodes.EOR_iny:
						EOR(cpu.mem[cpu.mem[instruction.GetPointer()] + y]);
						break;


					case Opcodes.JMP_abs:
						pc = instruction.GetPointer();
						return; // skip the pc += opc bytes
					case Opcodes.JMP_ind:
						pc = cpu.mem[instruction.GetPointer()];
						return; // skip the pc += opc bytes

					case Opcodes.JSR:
						PushPointerToStack(pc + 2);
						pc = instruction.GetPointer();
						return; // skip the pc += opc bytes



					case Opcodes.LDA_imm:
						a = instruction.operands[0];
						SetNZ(a);
						break;
					case Opcodes.LDA_zpg:
						a = cpu.mem[instruction.operands[0]];
						SetNZ(a);
						break;
					case Opcodes.LDA_zpx:
						a = cpu.mem[instruction.operands[0] + x];
						SetNZ(a);
						break;
					case Opcodes.LDA_abs:
						a = cpu.mem[instruction.GetPointer()];
						SetNZ(a);
						break;
					case Opcodes.LDA_abx:
						a = cpu.mem[instruction.GetPointer() + x];
						SetNZ(a);
						break;
					case Opcodes.LDA_aby:
						a = cpu.mem[instruction.GetPointer() + y];
						SetNZ(a);
						break;
					case Opcodes.LDA_inx:
						a = cpu.mem[cpu.GetPointerAt(instruction.operands[0] + x)];
						SetNZ(a);
						break;
					case Opcodes.LDA_iny:
						a = cpu.mem[cpu.GetPointerAt(instruction.operands[0]) + y];
						SetNZ(a);
						break;


					case Opcodes.LDX_imm:
						x = instruction.operands[0];
						SetNZ(x);
						break;
					case Opcodes.LDX_zpg:
						x = cpu.mem[instruction.operands[0]];
						SetNZ(x);
						break;
					case Opcodes.LDX_zpy:
						x = cpu.mem[instruction.operands[0] + y];
						SetNZ(x);
						break;
					case Opcodes.LDX_abs:
						x = cpu.mem[instruction.GetPointer()];
						SetNZ(x);
						break;
					case Opcodes.LDX_aby:
						x = cpu.mem[instruction.GetPointer() + y];
						SetNZ(x);
						break;


					case Opcodes.LDY_imm:
						y = instruction.operands[0];
						SetNZ(y);
						break;
					case Opcodes.LDY_zpg:
						y = cpu.mem[instruction.operands[0]];
						SetNZ(y);
						break;
					case Opcodes.LDY_zpx:
						y = cpu.mem[instruction.operands[0] + x];
						SetNZ(y);
						break;
					case Opcodes.LDY_abs:
						y = cpu.mem[instruction.GetPointer()];
						SetNZ(y);
						break;
					case Opcodes.LDY_abx:
						y = cpu.mem[instruction.GetPointer() + x];
						SetNZ(y);
						break;


					case Opcodes.LSR_acc:
						a = LSR(a);
						break;
					case Opcodes.LSR_zpg:
						cpu.mem[instruction.operands[0]] = LSR(cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.LSR_zpx:
						cpu.mem[instruction.operands[0] + x] = LSR(cpu.mem[instruction.operands[0] + x]);
						break;
					case Opcodes.LSR_abs:
						cpu.mem[instruction.GetPointer()] = LSR(cpu.mem[instruction.GetPointer()]);
						break;
					case Opcodes.LSR_abx:
						cpu.mem[instruction.GetPointer() + x] = LSR(cpu.mem[instruction.GetPointer() + x]);
						break;

					case Opcodes.NOP:
						// burn a couple cycles
						break;


					case Opcodes.ORA_imm:
						a |= instruction.operands[0];
						SetNZ(a);
						break;
					case Opcodes.ORA_zpg:
						a |= cpu.mem[instruction.operands[0]];
						SetNZ(a);
						break;
					case Opcodes.ORA_zpx:
						a |= cpu.mem[instruction.operands[0] + x];
						SetNZ(a);
						break;
					case Opcodes.ORA_abs:
						a |= cpu.mem[instruction.GetPointer()];
						SetNZ(a);
						break;
					case Opcodes.ORA_abx:
						a |= cpu.mem[instruction.GetPointer() + x];
						SetNZ(a);
						break;
					case Opcodes.ORA_aby:
						a |= cpu.mem[instruction.GetPointer() + y];
						SetNZ(a);
						break;
					case Opcodes.ORA_inx:
						a |= cpu.mem[cpu.mem[instruction.GetPointer() + x]];
						SetNZ(a);
						break;
					case Opcodes.ORA_iny:
						a |= cpu.mem[cpu.mem[instruction.GetPointer()] + y];
						SetNZ(a);
						break;

					case Opcodes.PHA:
						PushByteToStack(a);
						break;
					case Opcodes.PLA:
						a = PullByteFromStack();
						SetNZ(a);
						break;
					case Opcodes.PHP:
						PushByteToStack(ps);
						break;
					case Opcodes.PLP:
						ps = PullByteFromStack();
						break;



					case Opcodes.ROL_acc:
						a = ROL(a);
						break;
					case Opcodes.ROL_zpg:
						cpu.mem[instruction.operands[0]] = ROL(cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.ROL_zpx:
						cpu.mem[instruction.operands[0] + x] = ROL(cpu.mem[instruction.operands[0] + x]);
						break;
					case Opcodes.ROL_abs:
						cpu.mem[instruction.GetPointer()] = ROL(cpu.mem[instruction.GetPointer()]);
						break;
					case Opcodes.ROL_abx:
						cpu.mem[instruction.GetPointer() + x] = ROL(cpu.mem[instruction.GetPointer() + x]);
						break;


					case Opcodes.ROR_acc:
						a = ROR(a);
						break;
					case Opcodes.ROR_zpg:
						cpu.mem[instruction.operands[0]] = ROR(cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.ROR_zpx:
						cpu.mem[instruction.operands[0] + x] = ROR(cpu.mem[instruction.operands[0] + x]);
						break;
					case Opcodes.ROR_abs:
						cpu.mem[instruction.GetPointer()] = ROR(cpu.mem[instruction.GetPointer()]);
						break;
					case Opcodes.ROR_abx:
						cpu.mem[instruction.GetPointer() + x] = ROR(cpu.mem[instruction.GetPointer() + x]);
						break;



					case Opcodes.RTI:
						if (sp >= 0xFE)
							throw new Exception("Invalid stack state");
						var i = interrupt;
						ps = cpu.theStack.PullByte(sp++);
						interrupt = i;
						pc = PullPointerFromStack();
						return; // skip the pc += opc bytes
					case Opcodes.RTS:
						if (sp >= 0xFE)
							throw new Exception("Invalid stack state");
						pc = PullPointerFromStack() + 1;
						return; // skip the pc += opc bytes


					case Opcodes.SBC_imm:
						SBC(instruction.operands[0]);
						break;
					case Opcodes.SBC_zpg:
						SBC(cpu.mem[instruction.operands[0]]);
						break;
					case Opcodes.SBC_zpx:
						SBC(cpu.mem[instruction.operands[0] + x]);
						break;
					case Opcodes.SBC_abs:
						SBC(cpu.mem[instruction.GetPointer()]);
						break;
					case Opcodes.SBC_abx:
						SBC(cpu.mem[instruction.GetPointer() + x]);
						break;
					case Opcodes.SBC_inx:
						SBC(cpu.mem[cpu.mem[instruction.GetPointer() + x]]);
						break;
					case Opcodes.SBC_iny:
						SBC(cpu.mem[cpu.mem[instruction.GetPointer()] + y]);
						break;



					case Opcodes.STA_abs:
						cpu.mem[instruction.GetPointer()] = a;
						break;
					case Opcodes.STA_abx:
						cpu.mem[instruction.GetPointer() + x] = a;
						break;
					case Opcodes.STA_aby:
						cpu.mem[instruction.GetPointer() + y] = a;
						break;
					case Opcodes.STA_inx: // address retrieved from zeropages
						cpu.mem[cpu.GetPointerAt(instruction.operands[0] + x)] = a;
						break;
					case Opcodes.STA_iny: // address retrieved from zeropages
						cpu.mem[cpu.GetPointerAt(instruction.operands[0]) + y] = a;
						break;
					case Opcodes.STA_zpg: // stored to zeropages
						cpu.mem[instruction.operands[0]] = a;
						break;
					case Opcodes.STA_zpx: // stored to zeropages
						cpu.mem[instruction.operands[0] + x] = a;
						break;
					case Opcodes.STX_abs:
						cpu.mem[instruction.GetPointer()] = x;
						break;
					case Opcodes.STX_zpg:
						cpu.mem[instruction.operands[0]] = x;
						break;
					case Opcodes.STX_zpy:
						cpu.mem[instruction.operands[0] + y] = x;
						break;
					case Opcodes.STY_abs:
						cpu.mem[instruction.GetPointer()] = y;
						break;
					case Opcodes.STY_zpg:
						cpu.mem[instruction.operands[0]] = y;
						break;
					case Opcodes.STY_zpx:
						cpu.mem[instruction.operands[0] + x] = y;
						break;


					case Opcodes.TAX:
						x = a;
						break;
					case Opcodes.TAY:
						y = a;
						break;
					case Opcodes.TXA:
						a = x;
						break;
					case Opcodes.TYA:
						a = y;
						break;
					case Opcodes.TXS:
						sp = x;
						break;
					case Opcodes.TSX:
						x = sp;
						break;


					case Opcodes.SEC:
						carry = true;
						break;

					case Opcodes.CLC:
						carry = false;
						break;

					case Opcodes.CLD:
					case Opcodes.SED:
						break;

					case Opcodes.SEI:
						interrupt = true;
						break;

					case Opcodes.CLI:
						interrupt = false;
						break;

					case Opcodes.CLV:
						overflow = false;
						break;
				}

				pc += instruction.opcode.bytes;
				if (pc >= cpu.mem.Length)
					throw new Exception("RESEARCH: What does the 6502 do in this case?");
			}

			private void MovePC(byte amount)
			{
				if (amount >= 0x80)
				{
					var i = 0x100 - amount;
					pc -= i;
				}
				else
					pc += amount;
			}


			/// <summary>
			/// Sets N, Z, C.
			/// </summary>
			/// <param name="mem"></param>
			/// <returns></returns>
			private byte ROR(byte mem)
			{
				byte c = (byte)(carry ? 0x80 : 0);
				carry = (mem & 0x01) == 0x01;
				mem >>= 1;
				mem |= c;
				SetNZ(mem);
				return mem;
			}

			/// <summary>
			/// Sets N, Z, C.
			/// </summary>
			/// <param name="mem"></param>
			/// <returns></returns>
			private byte ROL(byte mem)
			{
				byte c = (byte)(carry ? 1 : 0);
				carry = (mem & 0x80) == 0x80;
				mem <<= 1;
				mem += c;
				SetNZ(mem);
				return mem;
			}

			/// <summary>
			/// Sets N = 0, Z, C.
			/// </summary>
			/// <param name="mem"></param>
			/// <returns></returns>
			private byte LSR(byte mem)
			{
				carry = (mem & 0x01) == 0x01;
				mem >>= 1;
				zero = mem == 0;
				negative = false;
				return mem;
			}

			private void SetNZ(byte mem)
			{
				zero = mem == 0;
				negative = mem >= 0x80;
			}

			/// <summary>
			/// Sets N, Z.
			/// </summary>
			/// <param name="mem"></param>
			private void EOR(byte mem)
			{
				a ^= mem;
				negative = a >= 0x80;
				zero = a == 0;
			}

			/// <summary>
			/// Sets N, Z.
			/// </summary>
			/// <param name="mem"></param>
			private void CMP(byte cmpByte, byte mem)
			{
				carry = true;
				var result = ASMHelper.SBC(cmpByte, mem, ref carry);
				negative = result >= 0x80;
				zero = result == 0;
			}


			/// <summary>
			/// BIT sets the Z flag as though the value in the address tested were ANDed with the accumulator.
			/// The N and V flags are set to match bits 7 and 6 respectively in the value stored at the tested address. 
			/// </summary>
			/// <param name="mem"></param>
			private void BIT(byte mem)
			{
				zero = (a & mem) == 0;
				negative = (mem & 0x80) == 0x80;
				overflow = (mem & 0x40) == 0x40;
			}


			/// <summary>
			/// Sets N, Z, C.
			/// </summary>
			/// <param name="mem"></param>
			/// <returns></returns>
			private byte ASL(byte mem)
			{
				carry = (mem & 0x80) == 0x80;
				mem <<= 1;
				zero = mem == 0;
				negative = mem >= 0x80;
				return mem;
			}

			/// <summary>
			/// Sets N, Z.
			/// </summary>
			/// <param name="mem"></param>
			private void AND(byte mem)
			{
				a &= mem;
				zero = a == 0;
				negative = a >= 0x80;
			}

			/// <summary>
			/// Sets N, Z, C, V.
			/// </summary>
			/// <param name="mem"></param>
			private void ADC(byte mem)
			{
				int total = a + mem + (carry ? 1 : 0);
				carry = total > byte.MaxValue;
				zero = total == 0;
				negative = total < 0;

				var byteTotal = (byte)total;
				//overflow = ((a ^ byteTotal) & (mem ^ byteTotal) & 0x80) != 0;
				bool m7 = (a & 0x80) != 0;
				bool n7 = (mem & 0x80) != 0;
				bool s7 = (byteTotal & 0x80) != 0;
				overflow = m7 != s7 && n7 != s7;

				a = byteTotal;
			}

			/// <summary>
			/// Sets N, Z, C, V
			/// </summary>
			/// <param name="mem"></param>
			private void SBC(byte mem)
			{
				ADC((byte)-mem);
			}
		}
	}
}
