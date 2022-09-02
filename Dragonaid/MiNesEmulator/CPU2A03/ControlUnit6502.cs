using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtomosZ.DragonAid.Libraries.ASM;
using AtomosZ.DragonAid.Libraries;

namespace AtomosZ.MiNesEmulator.CPU2A03
{
	public class ControlUnit6502
	{
		private CPU cpu;

		public byte a;
		public byte x;
		public byte y;
		public int pc;
		public byte sp = 0xFF;

		public int cycleCount = 0;

		/* processor status flags */
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

		public ControlUnit6502(CPU cpu)
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

		/// <summary>
		/// State of ControlUnit plus first 0x800 bytes of memory and memory mapper register states.
		/// </summary>
		public class ControlUnitState
		{
			public byte a;
			public byte x;
			public byte y;
			public int pc;
			public byte sp;

			public int cycleCount;

			public bool carry;
			public bool zero;
			public bool interrupt;
			public bool overflow;
			public bool negative;

			public byte ps;

			/// <summary>
			/// 0x800 bytes of CPU RAM.
			/// </summary>
			public byte[] memory;
			public byte[] sRAM;

			public byte[] registerStates;
		}

		/// <summary>
		/// Gets current state of ControlUnit, memory (0x0000 to 0x0800 and 0x6000 to 0x7FFF),
		/// and cartridge register states. 
		/// @TODO: PPU/APU/etc. register states.
		/// </summary>
		/// <returns></returns>
		internal ControlUnitState GetState()
		{
			return new ControlUnitState()
			{
				a = this.a,
				x = this.x,
				y = this.y,
				pc = this.pc,
				sp = this.sp,
				carry = this.carry,
				zero = this.zero,
				interrupt = this.interrupt,
				overflow = this.overflow,
				negative = this.negative,
				cycleCount = this.cycleCount,
				ps = this.ps,

				memory = cpu.memory[0, 0x800],
				sRAM = cpu.memory[0x6000, 0x2000],

				registerStates = cpu.memory.GetRegisterStates(),
			};
		}

		internal void SetState(ControlUnitState state)
		{
			a = state.a;
			x = state.x;
			y = state.y;
			pc = state.pc;
			sp = state.sp;
			carry = state.carry;
			zero = state.zero;
			interrupt = state.interrupt;
			overflow = state.overflow;
			negative = state.negative;
			cycleCount = state.cycleCount;
			ps = state.ps;

			cpu.memory[0, 0x800] = state.memory;
			cpu.memory[0x6000, 0x2000] = state.sRAM;

			cpu.memory.SetRegisterStates(state.registerStates);
		}

		private void PushByteToStack(byte value)
		{
			cpu.memory[0x100 + sp--] = value;
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

		internal byte PullByteFromStack()
		{
			return cpu.memory[0x100 + ++sp];
		}

		internal int PullPointerFromStack()
		{
			var lowByte = PullByteFromStack();
			var highByte = PullByteFromStack();

			return lowByte + (highByte << 8);
		}


		public string Peek(Instruction instruction)
		{
			var instrView = instruction.opcode.asm.Substring(0, 3);

			switch (instruction.opcode.mode)
			{
				case Opcode.Mode.Absolute:
					instrView += $" ${instruction.GetPointer().ToString("X4")}"
						+ $" = ${cpu.Read(instruction.GetPointer()).ToString("X2")}";
					break;
				case Opcode.Mode.Absolute_X:
					instrView += $" ${instruction.GetPointer().ToString("X4")},X"
						+ $" @ ${(instruction.GetPointer() + x):X2}"
						+ $"  = ${cpu.Read(instruction.GetPointer() + x).ToString("X2")}";
					break;
				case Opcode.Mode.Absolute_Y:
					instrView += $" ${instruction.GetPointer().ToString("X4")},Y"
						+ $" @ ${(instruction.GetPointer() + y):X2}"
						+ $"  = ${cpu.Read(instruction.GetPointer() + y).ToString("X2")}";
					break;

				case Opcode.Mode.Immediate:
					instrView += $" #${instruction.operands[0].ToString("X2")}";
					break;


				case Opcode.Mode.Indirect:
					instrView += $" (${(instruction.GetPointer()):X4})"
						+ $" @ ${cpu.GetPointerAt(instruction.GetPointer()):X4}"
						+ $" = ${cpu.Read(cpu.GetPointerAt(instruction.GetPointer())).ToString("X4")}";
					break;
				case Opcode.Mode.Indirect_X:
					instrView += $" (${instruction.operands[0].ToString("X2")},X)"
						+ $" @ ${cpu.GetPointerAt(instruction.operands[0] + x).ToString("X2")}"
						+ $" = ${cpu.Read(cpu.GetPointerAt(instruction.operands[0] + x)).ToString("X2")}";
					break;
				case Opcode.Mode.Indirect_Y:
					instrView += $" (${instruction.operands[0].ToString("X2")}),Y"
						+ $" @ ${(cpu.GetPointerAt(instruction.operands[0]) + y).ToString("X2")}"
						+ $" = ${cpu.Read(cpu.GetPointerAt(instruction.operands[0]) + y).ToString("X2")}";
					break;

				case Opcode.Mode.Relative:
					instrView += $" ${instruction.operands[0]:X2}"
							+ $" @ ${instruction.GetRelativeAddress():X4}"
							+ $" = ${cpu.Read(instruction.GetRelativeAddress()):X2}";
					break;


				case Opcode.Mode.ZeroPage:
					instrView += $" ${instruction.operands[0].ToString("X2")}"
						+ $" = ${cpu.Read(instruction.operands[0]).ToString("X2")}";
					break;
				case Opcode.Mode.ZeroPage_X:
					instrView += $" ${instruction.operands[0].ToString("X2")},X"
						+ $" @ ${(instruction.operands[0] + x):X2}"
						+ $" = ${cpu.Read(instruction.operands[0] + x).ToString("X2")}";
					break;
				case Opcode.Mode.ZeroPage_Y:
					instrView += $" ${instruction.operands[0].ToString("X2")},Y"
						+ $" @ ${(instruction.operands[0] + y):X2}"
						+ $" = ${cpu.Read(instruction.operands[0] + y).ToString("X2")}";
					break;
			}

			return instrView;
		}

		/// <summary>
		/// Saves current state of ControlUnit/Memory, runs the instruction,
		/// returns the state after instruction run, then resets ControlUnit/Memory.
		/// <para>@TODO: save and restore PPU/APU/etc. register states.</para>
		/// </summary>
		/// <param name="instr"></param>
		/// <returns></returns>
		public ControlUnitState PeekRunInstruction(Instruction instr)
		{
			var saveState = GetState();
			RunInstruction(instr);
			var virtualState = GetState();
			SetState(saveState);
			return virtualState;
		}

		/// <summary>
		/// Instruction MUST be a Relative mode opcode.
		/// </summary>
		/// <param name="instruction"></param>
		/// <exception cref="Exception"></exception>
		public void RunInstructionForceNonBranched(Instruction instruction)
		{
			if (instruction.opcode.mode != Opcode.Mode.Relative)
				throw new Exception("This is not a branch opcode");

			switch (instruction.opcode.opc)
			{
				case Opcodes.BCC:
					carry = true;
					break;
				case Opcodes.BCS:
					carry = false;
					break;
				case Opcodes.BEQ:
					zero = false;
					break;
				case Opcodes.BNE:
					zero = true;
					break;
				case Opcodes.BMI:
					negative = false;
					break;
				case Opcodes.BPL:
					negative = true;
					break;
				case Opcodes.BVC:
					overflow = true;
					break;
				case Opcodes.BVS:
					overflow = false;
					break;
			}

			cycleCount += instruction.opcode.cycles;
			pc += instruction.opcode.bytes;
		}


		/// <summary>
		/// Reads instruction, performs the appropriate operations, then increments the PC.
		/// </summary>
		/// <param name="instruction"></param>
		public void RunInstruction(Instruction instruction)
		{
			switch (instruction.opcode.opc)
			{
				case Opcodes.ADC_imm:
					ADC(instruction.operands[0]);
					break;

				case Opcodes.ADC_zpg:
					ADC(cpu.Read(instruction.operands[0]));
					break;
				case Opcodes.ADC_zpx:
					ADC(cpu.Read(instruction.operands[0] + x));
					break;

				case Opcodes.ADC_abs:
					ADC(cpu.Read(instruction.GetPointer()));
					break;
				case Opcodes.ADC_abx:
					CheckPageCrossing(instruction.operands[0], x);
					ADC(cpu.Read(instruction.GetPointer() + x));
					break;
				case Opcodes.ADC_aby:
					CheckPageCrossing(instruction.operands[0], y);
					ADC(cpu.Read(instruction.GetPointer() + y));
					break;

				case Opcodes.ADC_inx:
					ADC(cpu.Read(cpu.Read(instruction.GetPointer() + x)));
					break;
				case Opcodes.ADC_iny:
					CheckPageCrossing(instruction.operands[0], y);
					ADC(cpu.Read(cpu.Read(instruction.GetPointer()) + y));
					break;


				case Opcodes.AND_imm:
					AND(instruction.operands[0]);
					break;

				case Opcodes.AND_zpg:
					AND(cpu.Read(instruction.operands[0]));
					break;
				case Opcodes.AND_zpx:
					AND(cpu.Read(instruction.operands[0] + x));
					break;

				case Opcodes.AND_abs:
					AND(cpu.Read(instruction.GetPointer()));
					break;
				case Opcodes.AND_abx:
					CheckPageCrossing(instruction.operands[0], x);
					AND(cpu.Read(instruction.GetPointer() + x));
					break;
				case Opcodes.AND_aby:
					CheckPageCrossing(instruction.operands[0], y);
					AND(cpu.Read(instruction.GetPointer() + y));
					break;
				case Opcodes.AND_inx:
					AND(cpu.Read(cpu.Read(instruction.GetPointer() + x)));
					break;
				case Opcodes.AND_iny:
					CheckPageCrossing(instruction.operands[0], y);
					AND(cpu.Read(cpu.Read(instruction.GetPointer()) + y));
					break;


				case Opcodes.ASL_acc:
					a = ASL(a);
					break;
				case Opcodes.ASL_zpg:
				{
					var addr = instruction.operands[0];
					cpu.Write(addr, ASL(cpu.Read(addr)));
					cpu.memory[addr] = ASL(cpu.memory[addr]);
					break;
				}
				case Opcodes.ASL_zpx:
				{
					var addr = instruction.operands[0] + x;
					cpu.Write(addr, ASL(cpu.Read(addr)));
					break;
				}
				case Opcodes.ASL_abs:
				{
					var addr = instruction.GetPointer();
					cpu.Write(addr, ASL(cpu.Read(addr)));
					break;
				}
				case Opcodes.ASL_abx:
				{
					var addr = instruction.GetPointer() + x;
					cpu.Write(addr, ASL(cpu.Read(addr)));
					break;
				}

				case Opcodes.BCC:
					if (!carry)
						MovePC(instruction.operands[0]);
					break;
				case Opcodes.BCS:
					if (carry)
						MovePC(instruction.operands[0]);
					break;
				case Opcodes.BEQ:
					if (zero)
						MovePC(instruction.operands[0]);
					break;
				case Opcodes.BNE:
					if (!zero)
						MovePC(instruction.operands[0]);
					break;
				case Opcodes.BMI:
					if (negative)
						MovePC(instruction.operands[0]);
					break;
				case Opcodes.BPL:
					if (!negative)
						MovePC(instruction.operands[0]);
					break;
				case Opcodes.BVC:
					if (!overflow)
						MovePC(instruction.operands[0]);
					break;
				case Opcodes.BVS:
					if (overflow)
						MovePC(instruction.operands[0]);
					break;

				case Opcodes.BIT_zpg:
					BIT(cpu.Read(instruction.operands[0]));
					break;
				case Opcodes.BIT_abs:
					BIT(cpu.Read(instruction.GetPointer()));
					break;


				case Opcodes.BRK:
					PushPointerToStack(pc + 2);
					byte sr = (byte)(ps | 0x20); // interrupt flag turned on
					PushByteToStack(sr);
					pc = cpu.GetBRKPointer();
					instruction.opcode.bytes = 0; // skip the pc += opc bytes
					break;


				case Opcodes.CMP_imm:
					CMP(a, instruction.operands[0]);
					break;
				case Opcodes.CMP_zpg:
					CMP(a, cpu.Read(instruction.operands[0]));
					break;
				case Opcodes.CMP_zpx:
					CMP(a, cpu.Read(instruction.operands[0] + x));
					break;
				case Opcodes.CMP_abs:
					CMP(a, cpu.Read(instruction.GetPointer()));
					break;
				case Opcodes.CMP_abx:
					CheckPageCrossing(instruction.operands[0], x);
					CMP(a, cpu.Read(instruction.GetPointer() + x));
					break;
				case Opcodes.CMP_aby:
					CheckPageCrossing(instruction.operands[0], y);
					CMP(a, cpu.Read(instruction.GetPointer() + y));
					break;
				case Opcodes.CMP_inx:
					CMP(a, cpu.Read(cpu.Read(instruction.GetPointer() + x)));
					break;
				case Opcodes.CMP_iny:
					CheckPageCrossing(instruction.operands[0], y);
					CMP(a, cpu.Read(cpu.Read(instruction.GetPointer()) + y));
					break;

				case Opcodes.CPX_imm:
					CMP(x, instruction.operands[0]);
					break;
				case Opcodes.CPX_zpg:
					CMP(x, cpu.Read(instruction.operands[0]));
					break;
				case Opcodes.CPX_abs:
					CMP(x, cpu.Read(instruction.GetPointer()));
					break;

				case Opcodes.CPY_imm:
					CMP(y, instruction.operands[0]);
					break;
				case Opcodes.CPY_zpg:
					CMP(y, cpu.Read(instruction.operands[0]));
					break;
				case Opcodes.CPY_abs:
					CMP(y, cpu.Read(instruction.GetPointer()));
					break;


				case Opcodes.DEC_zpg:
				{
					var addr = instruction.operands[0];
					var result = (byte)(cpu.Read(addr) - 1);
					cpu.Write(addr, result);
					SetNZ(result);
					break;
				}
				case Opcodes.DEC_zpx:
				{
					var addr = instruction.operands[0] + x;
					var result = (byte)(cpu.Read(addr) - 1);
					cpu.Write(addr, result);
					SetNZ(result);
					break;
				}
				case Opcodes.DEC_abs:
				{
					var addr = instruction.GetPointer();
					var result = (byte)(cpu.Read(addr) - 1);
					cpu.Write(addr, result);
					SetNZ(result);
					break;
				}
				case Opcodes.DEC_abx:
				{
					var addr = instruction.GetPointer() + x;
					var result = (byte)(cpu.Read(addr) - 1);
					cpu.Write(addr, result);
					SetNZ(result);
					break;
				}
				case Opcodes.DEX:
					SetNZ(--x);
					break;
				case Opcodes.DEY:
					SetNZ(--y);
					break;

				case Opcodes.INC_zpg:
				{
					var addr = instruction.operands[0];
					var result = (byte)(cpu.Read(addr) + 1);
					cpu.Write(addr, result);
					SetNZ(result);
					break;
				}
				case Opcodes.INC_zpx:
				{
					var addr = instruction.operands[0] + x;
					var result = (byte)(cpu.Read(addr) + 1);
					cpu.Write(addr, result);
					SetNZ(result);
					break;
				}
				case Opcodes.INC_abs:
				{
					var addr = instruction.GetPointer();
					var result = (byte)(cpu.Read(addr) + 1);
					cpu.Write(addr, result);
					SetNZ(result);
					break;
				}
				case Opcodes.INC_abx:
				{
					var addr = instruction.GetPointer() + x;
					var result = (byte)(cpu.Read(addr) + 1);
					cpu.Write(addr, result);
					SetNZ(result);
					break;
				}
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
					EOR(cpu.Read(instruction.operands[0]));
					break;
				case Opcodes.EOR_zpx:
					EOR(cpu.Read(instruction.operands[0] + x));
					break;
				case Opcodes.EOR_abs:
					EOR(cpu.Read(instruction.GetPointer()));
					break;
				case Opcodes.EOR_abx:
					CheckPageCrossing(instruction.operands[0], x);
					EOR(cpu.Read(instruction.GetPointer() + x));
					break;
				case Opcodes.EOR_aby:
					CheckPageCrossing(instruction.operands[0], y);
					EOR(cpu.Read(instruction.GetPointer() + y));
					break;
				case Opcodes.EOR_inx:
					EOR(cpu.Read(cpu.Read(instruction.GetPointer() + x)));
					break;
				case Opcodes.EOR_iny:
					CheckPageCrossing(instruction.operands[0], y);
					EOR(cpu.Read(cpu.Read(instruction.GetPointer()) + y));
					break;


				case Opcodes.JMP_abs:
					pc = instruction.GetPointer();
					instruction.opcode.bytes = 0; // skip the pc += opc bytes
					break;
				case Opcodes.JMP_ind:
					pc = cpu.GetPointerAt(instruction.GetPointer());
					instruction.opcode.bytes = 0; // skip the pc += opc bytes
					break;

				case Opcodes.JSR:
					PushPointerToStack(pc + 2); // address -1 of next operation 
					pc = instruction.GetPointer();
					instruction.opcode.bytes = 0; // skip the pc += opc bytes
					break;



				case Opcodes.LDA_imm:
					a = instruction.operands[0];
					SetNZ(a);
					break;
				case Opcodes.LDA_zpg:
					a = cpu.Read(instruction.operands[0]);
					SetNZ(a);
					break;
				case Opcodes.LDA_zpx:
					a = cpu.Read(instruction.operands[0] + x);
					SetNZ(a);
					break;
				case Opcodes.LDA_abs:
					a = cpu.Read(instruction.GetPointer());
					SetNZ(a);
					break;
				case Opcodes.LDA_abx:
					CheckPageCrossing(instruction.operands[0], x);
					a = cpu.Read(instruction.GetPointer() + x);
					SetNZ(a);
					break;
				case Opcodes.LDA_aby:
					CheckPageCrossing(instruction.operands[0], y);
					a = cpu.Read(instruction.GetPointer() + y);
					SetNZ(a);
					break;
				case Opcodes.LDA_inx:
					a = cpu.Read(cpu.GetPointerAt(instruction.operands[0] + x));
					SetNZ(a);
					break;
				case Opcodes.LDA_iny:
					CheckPageCrossing(instruction.operands[0], y);
					a = cpu.Read(cpu.GetPointerAt(instruction.operands[0]) + y);
					SetNZ(a);
					break;


				case Opcodes.LDX_imm:
					x = instruction.operands[0];
					SetNZ(x);
					break;
				case Opcodes.LDX_zpg:
					x = cpu.Read(instruction.operands[0]);
					SetNZ(x);
					break;
				case Opcodes.LDX_zpy:
					x = cpu.Read(instruction.operands[0] + y);
					SetNZ(x);
					break;
				case Opcodes.LDX_abs:
					x = cpu.Read(instruction.GetPointer());
					SetNZ(x);
					break;
				case Opcodes.LDX_aby:
					CheckPageCrossing(instruction.operands[0], y);
					x = cpu.Read(instruction.GetPointer() + y);
					SetNZ(x);
					break;


				case Opcodes.LDY_imm:
					y = instruction.operands[0];
					SetNZ(y);
					break;
				case Opcodes.LDY_zpg:
					y = cpu.Read(instruction.operands[0]);
					SetNZ(y);
					break;
				case Opcodes.LDY_zpx:
					y = cpu.Read(instruction.operands[0] + x);
					SetNZ(y);
					break;
				case Opcodes.LDY_abs:
					y = cpu.Read(instruction.GetPointer());
					SetNZ(y);
					break;
				case Opcodes.LDY_abx:
					CheckPageCrossing(instruction.operands[0], x);
					y = cpu.Read(instruction.GetPointer() + x);
					SetNZ(y);
					break;


				case Opcodes.LSR_acc:
					a = LSR(a);
					break;
				case Opcodes.LSR_zpg:
				{
					var addr = instruction.operands[0];
					cpu.Write(addr, LSR(cpu.Read(addr)));
					break;
				}
				case Opcodes.LSR_zpx:
				{
					var addr = instruction.operands[0] + x;
					cpu.Write(addr, LSR(cpu.Read(addr)));
					break;
				}
				case Opcodes.LSR_abs:
				{
					var addr = instruction.GetPointer();
					cpu.Write(addr, LSR(cpu.Read(addr)));
					break;
				}
				case Opcodes.LSR_abx:
				{
					var addr = instruction.GetPointer() + x;
					cpu.Write(addr, LSR(cpu.Read(addr)));
					break;
				}

				case Opcodes.NOP:
					// burn a couple cycles
					break;


				case Opcodes.ORA_imm:
					a |= instruction.operands[0];
					SetNZ(a);
					break;
				case Opcodes.ORA_zpg:
					a |= cpu.Read(instruction.operands[0]);
					SetNZ(a);
					break;
				case Opcodes.ORA_zpx:
					a |= cpu.Read(instruction.operands[0] + x);
					SetNZ(a);
					break;
				case Opcodes.ORA_abs:
					a |= cpu.Read(instruction.GetPointer());
					SetNZ(a);
					break;
				case Opcodes.ORA_abx:
					CheckPageCrossing(instruction.operands[0], x);
					a |= cpu.Read(instruction.GetPointer() + x);
					SetNZ(a);
					break;
				case Opcodes.ORA_aby:
					CheckPageCrossing(instruction.operands[0], y);
					a |= cpu.Read(instruction.GetPointer() + y);
					SetNZ(a);
					break;
				case Opcodes.ORA_inx:
					a |= cpu.Read(cpu.Read(instruction.GetPointer() + x));
					SetNZ(a);
					break;
				case Opcodes.ORA_iny:
					CheckPageCrossing(instruction.operands[0], y);
					a |= cpu.Read(cpu.Read(instruction.GetPointer()) + y);
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
				{
					var addr = instruction.operands[0];
					cpu.Write(addr, ROL(cpu.Read(addr)));
					break;
				}
				case Opcodes.ROL_zpx:
				{
					var addr = instruction.operands[0] + x;
					cpu.Write(addr, ROL(cpu.Read(addr)));
					break;
				}
				case Opcodes.ROL_abs:
				{
					var addr = instruction.GetPointer();
					cpu.Write(addr, ROL(cpu.Read(addr)));
					break;
				}
				case Opcodes.ROL_abx:
				{
					var addr = instruction.GetPointer() + x;
					cpu.Write(addr, ROL(cpu.Read(addr)));
					break;
				}

				case Opcodes.ROR_acc:
					a = ROR(a);
					break;
				case Opcodes.ROR_zpg:
				{
					var addr = instruction.operands[0];
					cpu.Write(addr, ROR(cpu.Read(addr)));
					break;
				}
				case Opcodes.ROR_zpx:
				{
					var addr = instruction.operands[0] + x;
					cpu.Write(addr, ROR(cpu.Read(addr)));
					break;
				}
				case Opcodes.ROR_abs:
				{
					var addr = instruction.GetPointer();
					cpu.Write(addr, ROR(cpu.Read(addr)));
					break;
				}
				case Opcodes.ROR_abx:
				{
					var addr = instruction.GetPointer() + x;
					cpu.Write(addr, ROR(cpu.Read(addr)));
					break;
				}


				case Opcodes.RTI:
					if (sp >= 0xFE)
						throw new Exception("Invalid stack state");
					var intrrpt = interrupt;
					ps = PullByteFromStack(); // saved interrupt flag is ignored
					interrupt = intrrpt;
					pc = PullPointerFromStack() - 1; // is this -1 needed?
					instruction.opcode.bytes = 0; // cancel out the + after the switch
					break;
				case Opcodes.RTS:
					if (sp >= 0xFE)
						throw new Exception("Invalid stack state");
					pc = PullPointerFromStack() + 1;
					instruction.opcode.bytes = 0; // cancel out the + after the switch
					break;


				case Opcodes.SBC_imm:
					SBC(instruction.operands[0]);
					break;
				case Opcodes.SBC_zpg:
					SBC(cpu.Read(instruction.operands[0]));
					break;
				case Opcodes.SBC_zpx:
					SBC(cpu.Read(instruction.operands[0] + x));
					break;
				case Opcodes.SBC_abs:
					SBC(cpu.Read(instruction.GetPointer()));
					break;
				case Opcodes.SBC_abx:
					CheckPageCrossing(instruction.operands[0], x);
					SBC(cpu.Read(instruction.GetPointer() + x));
					break;
				case Opcodes.SBC_aby:
					CheckPageCrossing(instruction.operands[0], y);
					SBC(cpu.Read(instruction.GetPointer() + y));
					break;
				case Opcodes.SBC_inx:
					SBC(cpu.Read(cpu.Read(instruction.GetPointer() + x)));
					break;
				case Opcodes.SBC_iny:
					CheckPageCrossing(instruction.operands[0], y);
					SBC(cpu.Read(cpu.Read(instruction.GetPointer()) + y));
					break;


				/* @TODO: verify data from 6502.org that STA does NOT take extra cycles
					if there is a page boundary crossing. 
					UPDATE: According their forum users, this is correct. STA always 
					uses cycles as if it crossed a page boundary. */
				case Opcodes.STA_abs:
					cpu.Write(instruction.GetPointer(), a);
					break;
				case Opcodes.STA_abx:
					cpu.Write(instruction.GetPointer() + x, a);
					break;
				case Opcodes.STA_aby:
					cpu.Write(instruction.GetPointer() + y, a);
					break;
				case Opcodes.STA_inx: // address retrieved from zeropages
					cpu.Write(cpu.GetPointerAt(instruction.operands[0] + x), a);
					break;
				case Opcodes.STA_iny: // address retrieved from zeropages
					cpu.Write(cpu.GetPointerAt(instruction.operands[0]) + y, a);
					break;
				case Opcodes.STA_zpg: // stored to zeropages
					cpu.Write(instruction.operands[0], a);
					break;
				case Opcodes.STA_zpx: // stored to zeropages
					cpu.Write(instruction.operands[0] + x, a);
					break;
				case Opcodes.STX_abs:
					cpu.Write(instruction.GetPointer(), x);
					break;
				case Opcodes.STX_zpg:
					cpu.Write(instruction.operands[0], x);
					break;
				case Opcodes.STX_zpy:
					cpu.Write(instruction.operands[0] + y, x);
					break;
				case Opcodes.STY_abs:
					cpu.Write(instruction.GetPointer(), y);
					break;
				case Opcodes.STY_zpg:
					cpu.Write(instruction.operands[0], y);
					break;
				case Opcodes.STY_zpx:
					cpu.Write(instruction.operands[0] + x, y);
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

			cycleCount += instruction.opcode.cycles;
			pc += instruction.opcode.bytes;
			if (pc > 0xFFFF)
				throw new Exception("RESEARCH: What does the 6502 do in this case?");
		}

		private void CheckPageCrossing(byte addressLowByte, byte offset)
		{
			if (addressLowByte + offset > byte.MaxValue)
				++cycleCount;
		}

		/// <summary>
		/// Moves PC by amount (>= 0x80 is negative) and adds +1 to cycle count,
		/// or +2 if page boundary crossed.
		/// </summary>
		/// <param name="amount"></param>
		private void MovePC(byte amount)
		{
			int prev = pc;
			if (amount >= 0x80)
			{
				var i = 0x100 - amount;
				pc -= i;
			}
			else
				pc += amount;
			if ((prev & 0xFF00) != (pc & 0xFF00))
				++cycleCount; // extra cycle count if branch goes to different page
			++cycleCount;
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
