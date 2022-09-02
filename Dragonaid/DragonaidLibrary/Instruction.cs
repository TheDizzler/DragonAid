using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomosZ.DragonAid.Libraries.ASM
{
	/// <summary>
	/// This data structure class is for debugging purposes and could be stream-lined away eventually.
	/// However it's very useful for peeking into the next instruction the CPU is about to execute.
	/// </summary>
	public class Instruction
	{
		public Opcode opcode;
		public byte[] operands;
		public int address;


		/// <summary>
		/// Get operand bytes in High-Low order.
		/// </summary>
		/// <returns></returns>
		public string GetBytesString()
		{
			var text = "$";
			for (int i = operands.Length - 1; i >= 0; --i)
				text += operands[i].ToString("X2");
			return text;
		}

		public int GetPointer()
		{
			return operands[0] + (operands[1] << 8);
		}

		public string byteCode
		{
			get
			{
				string code = opcode.opc.ToString("X2");
				foreach (var oper in operands)
					code += " " + oper.ToString("X2");
				return code;
			}
		}

		/// <summary>
		/// ASM reading of instruction
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string t = opcode.asm.Substring(0, 3);
			switch (opcode.mode)
			{
				case Opcode.Mode.Accumulator:
					t += " A";
					break;

				case Opcode.Mode.Absolute:
				case Opcode.Mode.ZeroPage:
					t += $" {GetBytesString()}";
					break;

				case Opcode.Mode.Indirect:
					t += $" ({GetBytesString()})";
					break;
				case Opcode.Mode.Immediate:
					t += $" #{GetBytesString()}";
					break;

				case Opcode.Mode.Absolute_X:
				case Opcode.Mode.ZeroPage_X:
					t += $" {GetBytesString()},X";
					break;
				case Opcode.Mode.Absolute_Y:
					t += $" {GetBytesString()},Y";
					break;

				case Opcode.Mode.Indirect_X:
					t += $" ({GetBytesString()},X)";
					break;
				case Opcode.Mode.Indirect_Y:
					t += $" ({GetBytesString()}),Y";
					break;

				case Opcode.Mode.Relative:
					t += $" ${GetRelativeAddress():X4}";
					break;

				case Opcode.Mode.Implied:
					break; // nothing to see here
			}

			return t;
		}

		public int GetRelativeAddress()
		{
			if (operands[0] >= 0x80)
			{
				var i = 0x100 - operands[0];
				return address + opcode.bytes - i;
			}
			else
				return address +opcode.bytes;
		}
	}
}
