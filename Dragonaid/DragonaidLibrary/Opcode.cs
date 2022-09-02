using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomosZ.DragonAid.Libraries.ASM
{
	public class Opcode
	{
		public enum Mode
		{
			Absolute, Absolute_X, Absolute_Y,
			Accumulator,
			Immediate,
			Implied,
			Indirect, Indirect_X, Indirect_Y,
			Relative,
			ZeroPage, ZeroPage_X, ZeroPage_Y
		};
		public string asm;
		public byte opc;
		/// <summary>
		/// Total # of bytes for this opcode.
		/// </summary>
		public byte bytes;
		public byte cycles;
		public Mode mode;


		public Opcode(string asm, byte opc, byte bytes, byte cycles)
		{
			this.asm = asm;
			this.opc = opc;
			this.bytes = bytes;
			this.cycles = cycles;

			string t;
			if (asm.Length == 3)
			{
				switch (asm)
				{
					case "JSR":
						t = "_abs";
						break;
					case "BCC":
					case "BCS":
					case "BEQ":
					case "BMI":
					case "BNE":
					case "BPL":
					case "BVC":
					case "BVS":
						t = "_rel";
						break;
					default:
						t = "_imp";
						break;
				}
			}
			else
				t = asm.Substring(3, asm.Length - 3);
			switch (t)
			{
				case "_abs":
					mode = Mode.Absolute;
					break;
				case "_abx":
					mode = Mode.Absolute_X;
					break;
				case "_aby":
					mode = Mode.Absolute_Y;
					break;

				case "_acc":
					mode = Mode.Accumulator;
					break;

				case "_imm":
					mode = Mode.Immediate;
					break;
				case "_imp":
					mode = Mode.Implied;
					break;

				case "_ind":
					mode = Mode.Indirect;
					break;
				case "_inx":
					mode = Mode.Indirect_X;
					break;
				case "_iny":
					mode = Mode.Indirect_Y;
					break;

				case "_rel":
					mode = Mode.Relative;
					break;

				case "_zpg":
					mode = Mode.ZeroPage;
					break;
				case "_zpx":
					mode = Mode.ZeroPage_X;
					break;
				case "_zpy":
					mode = Mode.ZeroPage_Y;
					break;
			}
		}

		/// <summary>
		/// Control Flow is JMP, JSR, BRK, RTS, RTI, and all branch codes.
		/// </summary>
		/// <returns></returns>
		public bool IsControlFlow()
		{
			switch (mode)
			{
				case Mode.Relative:
					return true;
				case Mode.Accumulator:
				case Mode.Immediate:
				case Mode.Absolute_X:
				case Mode.Absolute_Y:
				case Mode.Indirect_X:
				case Mode.Indirect_Y:
				case Mode.ZeroPage:
				case Mode.ZeroPage_X:
				case Mode.ZeroPage_Y:
					return false;
			}

			switch (opc)
			{
				case Opcodes.JSR:
				case Opcodes.JMP_abs:
				case Opcodes.JMP_ind:
				case Opcodes.BRK:
				case Opcodes.RTI:
				case Opcodes.RTS:
					return true;
			}

			return false;
		}

		/// <summary>
		/// Controlflow can end when a BNE is followed by a BEQ, and other branch equivalents.
		/// Let's just ignore those.
		/// </summary>
		/// <returns></returns>
		public bool IsControlFlowEnd()
		{
			switch (opc)
			{
				case Opcodes.JMP_abs:
				case Opcodes.JMP_ind:
				case Opcodes.RTI:
				case Opcodes.RTS:
					return true;
			}

			return false;
		}

		public bool IsBranch()
		{
			return mode == Mode.Relative;
		}
	}
}
