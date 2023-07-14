using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtomosZ.DragonAid.Libraries;

namespace AtomosZ.MiNesEmulator.CPU2A03
{
	internal interface IRegister
	{
		byte Read();
		void Write(byte value);
		void Reset();
	}

	/// <summary>
	/// A 5 bit register.
	/// Reads one bit at a time.
	/// </summary>
	internal class ShiftRegister5 : Register
	{
		private byte value = 0x10;

		/// <summary>
		/// <para>If bit 7 is set, clears register and returns false.
		///  If bit 7 clear, only the first bit is written and pushed on to the 5th bit of the
		///  register. When 5 bits have been pushed into the shift register,
		///  returns true to denote it is full.<br/>
		///  </para>
		/// </summary>
		/// <param name="bit"></param>
		/// <returns>Returns true when shift register is full.</returns>
		public bool Write(byte bit)
		{
			if (value >= 0x80)
			{
				Clear();
				return false;
			}
			ASMHelper.ASL(value, 1, out bool hasCarry);
			value |= (byte)((bit & 0x01) << 4);
			return hasCarry;
		}

		public void Clear()
		{
			value = 0x10;
		}

		public byte Read()
		{
			return value;
		}
	}
}
