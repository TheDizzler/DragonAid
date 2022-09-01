using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.ASM;
using static AtomosZ.MiNesEmulator.CPU2A03.ControlUnit6502;

namespace AtomosZ.MiNesEmulator.CPU2A03
{
	/// <summary>
	/// @TODO: (after syncing to git) move to CPU2a03 directory.
	/// <para>
	/// Representational class to encapsulate all relevant components of a computing unit
	/// (Ex: NES => 2A03 (6502CPU + APU), PPU, and cartridge).
	/// <br>In this way, this class acts as a sort of memory buss (but I don't know if I want to keep it this way)</br>
	/// <br>Includes methods for running, reseting, and running virtually.</br>
	/// </para>
	/// </summary>
	public class CPU
	{
		internal ControlUnit6502 controlUnit;
		internal Memory6502 memory;
		private byte[] romData;

		public CPU()
		{
			//mem = new CPUMemory();
			//theStack = new Stack(this);
			controlUnit = new ControlUnit6502(this);
		}

		public void LoadRom(byte[] romData)
		{
			this.romData = romData;
			Initialize();
		}

		public void Reset()
		{
			controlUnit.Reset();
			Initialize();
		}

		/// <summary>
		/// Sets program counter to Reset pointer
		/// </summary>
		public void Initialize()
		{
			memory = Memory6502.Initialize(controlUnit, romData);
			controlUnit.pc = GetPointerAt(UniversalConsts.RESET_Pointer);
		}

		/// <summary>
		/// This isn't really needed. Maybe just hold on to it for readability?
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		internal byte Read(int address)
		{
			return memory[address];
		}

		/// <summary>
		/// This isn't really needed. Maybe just hold on to it for readability?
		/// </summary>
		/// <param name="address"></param>
		/// <param name="value"></param>
		internal void Write(int address, byte value)
		{
			memory[address] = value;
		}


		public class VirtualInstruction
		{
			/// <summary>
			/// Current state of ControlUnit before instruction executed.
			/// </summary>
			public ControlUnitState state;
			public Instruction instruction;
			public byte bankId;
			/// <summary>
			/// Pointer to data or next instruction.
			/// @TODO: how to tell if data or instruction without parsing instruction?
			/// </summary>
			public int linkedAddress;

		}

		/// <summary>
		/// Traverses a block of instructions until it hits a end control flow op (RTS, RTI, JMP)
		/// from the input address, using whatever state the ControlUnit is currently in.
		/// ControlUnit is reset back to original state after completion.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public List<VirtualInstruction> RunInstructionBlockVirtually(int address)
		{
			var ogCUState = controlUnit.GetState();
			controlUnit.pc = address;

			var instrItems = RunInstructionBlockVirtually(ogCUState);

			// return CPU to original state
			controlUnit.SetState(ogCUState);
			return instrItems;
		}

		public List<VirtualInstruction> RunInstructionBlockVirtually(ControlUnitState virtualCUState)
		{
			var visitedAddrHash = new HashSet<int>();
			var instrItems = new List<VirtualInstruction>();
			var ogCUState = controlUnit.GetState();

			controlUnit.SetState(virtualCUState);

			// traverse code from address
			while (true)
			{
				var instr = PeekNextInstruction();
				var virtualInstr = new VirtualInstruction();
				virtualInstr.instruction = instr;
				virtualInstr.state = controlUnit.GetState();
				instrItems.Add(virtualInstr);
				visitedAddrHash.Add(controlUnit.pc);

				if (instr.opcode.IsControlFlow())
				{
					if (GetPointerFromInstruction(instr, out int targetAddr))
					{
						// check if target already visited
						//if (visitedAddrHash.Add(targetAddr))
						{ // not visited
							virtualInstr.linkedAddress = targetAddr;
						}
					}
					else
						MessageBox.Show($"What's the deal with {controlUnit.pc} : {instr.ToString()}?");

					if (instr.opcode.IsControlFlowEnd())
					{
						if (instr.opcode.asm == "RTI")
						{
							controlUnit.PullByteFromStack(); // the Processor State flag, which we don't need
							virtualInstr.linkedAddress = controlUnit.PullPointerFromStack();
						}
						else if (instr.opcode.asm == "RTS")
						{
							virtualInstr.linkedAddress = controlUnit.PullPointerFromStack() + 1;
						}
						break;
					}
				}
				else if (GetDataPointerFromInstruction(instr, out int dataAddr))
				{
					virtualInstr.linkedAddress = dataAddr;
				}

				controlUnit.RunInstruction(instr);
			}

			// return CPU to original state
			controlUnit.SetState(ogCUState);

			return instrItems;
		}

		/// <summary>
		/// If instruction is pointing to data (LDA, STA, INC, DEC, etc.) will save data
		/// address in targetAddr and return true.
		/// Else, targetAddr= -1 and return false.
		/// Will return address of next instruction if control flow (JMP, JSR)
		/// </summary>
		/// <param name="instruction"></param>
		/// <param name="targetAddr"></param>
		/// <returns></returns>
		private bool GetDataPointerFromInstruction(Instruction instruction, out int targetAddr)
		{
			switch (instruction.opcode.mode)
			{
				case Opcode.Mode.Absolute:
					targetAddr = instruction.GetPointer();
					return true;
				case Opcode.Mode.Absolute_X:
					targetAddr = instruction.GetPointer() + controlUnit.x;
					return true;
				case Opcode.Mode.Absolute_Y:
					targetAddr = instruction.GetPointer() + controlUnit.y;
					return true;

				case Opcode.Mode.Indirect:
					targetAddr = GetPointerAt(instruction.GetPointer());
					return true;
				case Opcode.Mode.Indirect_X:
					targetAddr = GetPointerAt(instruction.operands[0] + controlUnit.x);
					return true;
				case Opcode.Mode.Indirect_Y:
					targetAddr = GetPointerAt(instruction.operands[0]) + controlUnit.x;
					return true;

				case Opcode.Mode.ZeroPage:
					targetAddr = instruction.operands[0];
					return true;
				case Opcode.Mode.ZeroPage_X:
					targetAddr = instruction.operands[0] + controlUnit.x;
					return true;
				case Opcode.Mode.ZeroPage_Y:
					targetAddr = instruction.operands[0] + controlUnit.y;
					return true;
			}

			targetAddr = -1;
			return false;
		}

		/// <summary>
		/// Gets address from instruction, or the stack if RTI/RTS.
		/// </summary>
		/// <param name="instr"></param>
		/// <param name="targetAddr"></param>
		/// <returns></returns>
		private bool GetPointerFromInstruction(Instruction instr, out int targetAddr)
		{
			switch (instr.opcode.mode)
			{
				case Opcode.Mode.Relative:  // any branch
					targetAddr = instr.GetRelativeAddress();
					return true;

				case Opcode.Mode.Absolute: // JMP, JSR $4444
					targetAddr = instr.GetPointer();
					return true;

				case Opcode.Mode.Indirect: // JMP ($4444)
					targetAddr = GetPointerAt(instr.GetPointer());
					return true;

				case Opcode.Mode.Implied: // RTI, RTS - pull address from stack
					if (instr.opcode.opc == Opcodes.RTI) // SR is on stack before address
					{
						var sr = memory[0x100 + (controlUnit.sp + 1)];
						var lowByte = memory[0x100 + (controlUnit.sp + 2)];
						var highByte = memory[0x100 + (controlUnit.sp + 3)];
						targetAddr = lowByte + (highByte << 8);
					}
					else
					{
						var lowByte = memory[0x100 + (controlUnit.sp + 1)];
						var highByte = memory[0x100 + (controlUnit.sp + 2)];
						targetAddr = lowByte + (highByte << 8);
					}

					return true;

				default: // BRK
					targetAddr = -1;
					return false;
			}
		}

		public int GetPointerAt(int lowByteAddress)
		{
			return memory[lowByteAddress] + (memory[lowByteAddress + 1] << 8);
		}

		/// <summary>
		/// Use ViewNextInstruction() for human readable string of instruction.
		/// </summary>
		/// <returns></returns>
		public Instruction PeekNextInstruction()
		{
			return GetInstruction(controlUnit.pc);
		}

		public Instruction PeekInstruction(int address)
		{
			return GetInstruction(address);
		}

		/// <summary>
		/// Use PeekNextInstruction() to get next Instruction.
		/// </summary>
		/// <returns>human readable string of instruction</returns>
		public string ViewNextInstruction()
		{
			var instr = GetInstruction(controlUnit.pc);
			return controlUnit.Peek(instr);
		}

		public string ViewNextInstruction(Instruction instr)
		{
			return controlUnit.Peek(instr);
		}

		public void ParseAndRunNextInstruction()
		{
			var instr = GetInstruction(controlUnit.pc);
			controlUnit.RunInstruction(instr);
		}

		private Instruction GetInstruction(int address)
		{
			var instrByte = memory[address];
			if (!Opcodes.opcodes.TryGetValue(instrByte, out Opcode opc))
			{
				throw new Exception($"Invalid opcode $({instrByte:X2)}) at ${address:X4}");
			}

			var instr = new Instruction();
			instr.opcode = opc;
			instr.address = address;
			instr.operands = new byte[opc.bytes - 1];
			for (int i = 1; i < opc.bytes; ++i)
			{
				instr.operands[i - 1] = memory[address + i];
			}

			return instr;
		}


		internal int GetBRKPointer()
		{
			var BRKPointer = UniversalConsts.IRQBRK_Pointer;
			return memory[BRKPointer] + (memory[BRKPointer + 1] << 8);
		}
	}
}
