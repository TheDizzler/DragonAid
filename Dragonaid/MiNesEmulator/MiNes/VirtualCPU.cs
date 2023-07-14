using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtomosZ.DragonAid.Libraries.ASM;
using System.Windows.Forms;

using static AtomosZ.MiNesEmulator.CPU2A03.ControlUnit6502;
using AtomosZ.MiNesEmulator.PPU2C02;

namespace AtomosZ.MiNesEmulator.CPU2A03
{
	/// <summary>
	/// A debug friendly CPU. All functionality that is not standard to the 6502 is placed here.
	/// </summary>
	public class VirtualCPU : CPU
	{
		public HashSet<int> visitedAddrHash;


		/// <summary>
		/// A debug friendly CPU.
		/// </summary>
		public class VirtualInstruction
		{
			/// <summary>
			/// Current state of ControlUnit before instruction executed.
			/// </summary>
			public ControlUnitState state;
			public Instruction instruction;
			/// <summary>
			/// Pointer to data or next instruction.
			/// @TODO: how to tell if data or instruction without parsing instruction?
			/// </summary>
			public int linkedAddress = -1;
			internal int dataAddress = -1;

			/// <summary>
			/// Gets bank id from stored register state flags.
			/// </summary>
			/// <returns></returns>
			/// <exception cref="Exception"></exception>
			public byte GetBankId()
			{
				var addr = instruction.address;
				var reg0 = state.registerStates[1];
				var reg1 = state.registerStates[2];
				var reg3 = state.registerStates[4];
				var bankHighBit = (byte)(reg1 & 0x10);
				var bankLowBits = (byte)(reg3 & 0x0F);
				switch ((reg0 & 0x0C) >> 2)
				{
					case 0:
					case 1:
						throw new Exception("32KB bank switching mode not implemented");
					case 2: // lowBank is fixed to first bank
						if (addr >= 0xC000)
							return (byte)(bankHighBit | bankLowBits);
						else if (addr >= 0x8000)
							return bankHighBit; // first bank of high or low
						throw new Exception($"Should not be getting bank id from instruction at {addr}.");

					case 3: // highBank is fixed to last bank
						if (addr >= 0xC000)
							return (byte)(bankHighBit | 0x0F); // last bank of high or low
						else if (addr >= 0x8000)
							return (byte)(bankHighBit | bankLowBits);
						throw new Exception($"Should not be getting bank id from instruction at {addr}.");
				}

				throw new Exception("Something went wrong");
			}
		}


		public VirtualCPU(PPU ppu) : base(ppu) { }

		public override void Initialize()
		{
			base.Initialize();
			visitedAddrHash = new HashSet<int>();
		}

		public ControlUnitState GetCurrentState()
		{
			return controlUnit.GetState();
		}

		public ControlUnitState GetStateAfter(VirtualInstruction vInstr)
		{
			var ogCUState = controlUnit.GetState();
			controlUnit.SetState(vInstr.state);

			controlUnit.RunInstruction(vInstr.instruction);

			var state = GetCurrentState();
			// return CPU to original state
			controlUnit.SetState(ogCUState);

			return state;
		}

		public List<VirtualInstruction> RunInstructionBlockVirtually(ControlUnitState virtualCUState)
		{
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
					virtualInstr.dataAddress = dataAddr;
				}

				/* @TODO: check if previous branch was opposite of this one, this creating a ControlFlowEnd.
				 *	if state checked hasn't changed. */
				if (instr.opcode.IsBranch())
				{ // get branch instruction and normal flow instruction
					var branchedInstr = PeekBranched(instr);
					var nextInstr = PeekNonBranched(instr);

					// get states required to branch/not branch and save to virtual instructions
					// NOTE: this can result in states that wouldn't normally occur
					if (visitedAddrHash.Add(branchedInstr.address))
					{ // branch location has NOT been visited
						virtualInstr.linkedAddress = branchedInstr.address;
					}
					else
						virtualInstr.linkedAddress = -1;

					/* no, we should let the loop play out IF it's a backwards branch.
					 * As long as this isn't making a comparison
					 * with a register, this shoud be fine. */
					if (MessageBox.Show("Force non-branched control flow?\n"
						+ $"{instr.address}: {instr.ToString()} => {branchedInstr.address}: {branchedInstr.ToString()}"
						, "Branch Detected", MessageBoxButtons.YesNo) == DialogResult.Yes)
					{// force controlUnit down non-branched path
						controlUnit.RunInstructionForceNonBranched(instr);
						continue;
					}
				}
				//else
				controlUnit.RunInstruction(instr);
			}

			// return CPU to original state
			controlUnit.SetState(ogCUState);

			return instrItems;
		}


		/// <summary>
		/// If instruction is pointing to data (LDA, STA, INC, DEC, etc.) will save data
		/// address in targetAddr and return true.
		/// Else, targetAddr is set to -1 and return false.
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

		public Instruction PeekBranched(Instruction instr)
		{
			if (instr.opcode.mode != Opcode.Mode.Relative)
				throw new Exception("This is not a branch opcode");

			var branchAddr = instr.GetRelativeAddress();
			return PeekInstruction(branchAddr);
		}

		public Instruction PeekNonBranched(Instruction instr)
		{
			if (instr.opcode.mode != Opcode.Mode.Relative)
				throw new Exception("This is not a branch opcode");

			return PeekInstruction(instr.address + instr.opcode.bytes);
		}

		/// <summary>
		/// Returns true instruction in current Control Unit/Memory state will branch.
		/// </summary>
		/// <param name="instr"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public bool WillBranch(Instruction instr)
		{
			if (instr.opcode.mode != Opcode.Mode.Relative)
				throw new Exception("This is not a branch opcode");

			ControlUnitState state = controlUnit.PeekRunInstruction(instr);
			return state.pc != instr.address + instr.opcode.bytes;
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
	}
}
