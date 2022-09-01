using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtomosZ.DragonAid.Libraries;

namespace AtomosZ.MiNesEmulator.CPU2A03
{
	internal interface Register
	{
		byte Read();
		void Write(byte value);
	}

	/// <summary>
	/// A 5 bit register.
	/// Reads one bit at a time.
	/// </summary>
	internal class ShiftRegister
	{
		private byte value = 0x10;

		/// <summary>
		/// <para>If bit 7 is set, clears register and returns false.
		///  <br>If bit 7 clear, only the first bit is written and pushed on to the 5th bit of the
		///  register. When 5 bits have been pushed into the shift register,
		///  returns true to denote it is full.</br>
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

	/// <summary>
	/// $2000
	/// </summary>
	internal class PPUCTRL : Register
	{
		public byte Read()
		{
			throw new Exception("Cannot read from PPUCTRL?");
		}

		public void Write(byte value)
		{
			Debug.WriteLine("PPUCTRL Write ($2000) not yet implemented");
		}
	}
	/// <summary>
	/// $2001
	/// </summary>
	internal class PPUMASK : Register
	{
		public byte Read()
		{
			Debug.WriteLine("PPUMASK Read ($2001) not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("PPUMASK Write ($2001) not yet implemented");
		}
	}
	/// <summary>
	/// $2002
	/// </summary>
	internal class PPUSTATUS : Register
	{
		public byte Read()
		{
			Debug.WriteLine("PPUSTATUS Read ($2002) not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("PPUSTATUS Write ($2002) not yet implemented");
		}
	}
	/// <summary>
	/// $2003
	/// </summary>
	internal class OAMADDR : Register
	{
		public byte Read()
		{
			Debug.WriteLine("OAMADDR ($2003) Read not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("OAMADDR ($2003) Write not yet implemented");
		}
	}
	/// <summary>
	/// $2004
	/// </summary>
	internal class OAMDATA : Register
	{
		public byte Read()
		{
			Debug.WriteLine("OAMDATA ($2004) Read not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("OAMDATA ($2004) Write not yet implemented");
		}
	}
	/// <summary>
	/// $2005
	/// </summary>
	internal class PPUSCROLL : Register
	{
		public byte Read()
		{
			Debug.WriteLine("PPUSCROLL ($2005) Read not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("PPUSCROLL ($2005) Write not yet implemented");
		}
	}
	/// <summary>
	/// $2006
	/// </summary>
	internal class PPUADDR : Register
	{
		public byte Read()
		{
			Debug.WriteLine("PPUADDR ($2006) Read not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("PPUADDR ($2006) Write not yet implemented");
		}
	}
	/// <summary>
	/// $2007
	/// </summary>
	internal class PPUDATA : Register
	{
		public byte Read()
		{
			Debug.WriteLine("PPUDATA ($2007) Read not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("PPUDATA ($2007) Write not yet implemented");
		}
	}

	/// <summary>
	/// $4014 aka SpriteDMA
	/// </summary>
	internal class OAMDMA : Register
	{
		public byte Read()
		{
			Debug.WriteLine("OAMDMA ($4014) Read not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("OAMDMA ($4014) Write not yet implemented");
		}
	}


	/// <summary>
	/// $4000 & $4004
	/// </summary>
	internal class SQDUTY : Register
	{
		public byte Read()
		{
			throw new Exception("Cannot read from SQDUTY!");
		}

		public void Write(byte value)
		{
			Debug.WriteLine("SQDUTY Write ($4000 & $4004) not yet implemented");
		}
	}
	/// <summary>
	/// $4001 & $4005
	/// </summary>
	internal class SQSWEEP : Register
	{
		public byte Read()
		{
			throw new Exception("Cannot read from SQSWEEP!");
		}

		public void Write(byte value)
		{
			Debug.WriteLine("SQSWEEP Write ($4001 & $4005) not yet implemented");
		}
	}
	/// <summary>
	/// $4002 & $4006
	/// </summary>
	internal class SQTIMER : Register
	{
		public byte Read()
		{
			throw new Exception("Cannot read from SQTIMER!");
		}

		public void Write(byte value)
		{
			Debug.WriteLine("SQTIMER Write ( $4002 & $4006) not yet implemented");
		}
	}
	/// <summary>
	/// $4003 & $4007
	/// </summary>
	internal class SQLENGTH : Register
	{
		public byte Read()
		{
			throw new Exception("Cannot read from SQLENGTH!");
		}

		public void Write(byte value)
		{
			Debug.WriteLine("SQLENGTH Write ( $4003 & $4007) not yet implemented");
		}
	}
	/// <summary>
	/// $4008
	/// </summary>
	internal class TRILINEAR : Register
	{
		public byte Read()
		{
			throw new Exception("Cannot read from TRILINEAR!");
		}

		public void Write(byte value)
		{
			Debug.WriteLine("TRILINEAR Write ($4008) not yet implemented");
		}
	}
	/// <summary>
	/// $400A
	/// </summary>
	internal class TRITIMER : Register
	{
		public byte Read()
		{
			Debug.WriteLine("Cannot read from TRITIMER ($400A)!");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("TRITIMER Write ($400A) not yet implemented");
		}
	}
	/// <summary>
	/// $400B
	/// </summary>
	internal class TRILENGTH : Register
	{
		public byte Read()
		{
			Debug.WriteLine("Cannot read from TRILENGTH ($400B)!");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("TRILENGTH ($400B) Write not yet implemented");
		}
	}
	/// <summary>
	/// $400C
	/// </summary>
	internal class NOISEVOLUME : Register
	{
		public byte Read()
		{
			Debug.WriteLine("Cannot read from NOISEVOLUME ($400C)!");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("NOISEVOLUME ($400C) Write not yet implemented");
		}
	}
	/// <summary>
	/// $400E
	/// </summary>
	internal class NOISEPERIOD : Register
	{
		public byte Read()
		{
			Debug.WriteLine("Cannot read from NOISEPERIOD ($400E)!");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("NOISEPERIOD ($400E) Write not yet implemented");
		}
	}
	/// <summary>
	/// $400F
	/// </summary>
	internal class NOISELENGTH : Register
	{
		public byte Read()
		{
			Debug.WriteLine("Cannot read from NOISELENGTH ($400F)!");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("NOISELENGTH ($400F) Write not yet implemented");
		}
	}
	/// <summary>
	/// $4010
	/// </summary>
	internal class DMCFREQ : Register
	{
		public byte Read()
		{
			Debug.WriteLine("Cannot read from DMCFREQ ($4010)!");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("DMCFREQ ($4010) Write not yet implemented");
		}
	}
	/// <summary>
	/// $4011
	/// </summary>
	internal class DMCCOUNTER : Register
	{
		public byte Read()
		{
			Debug.WriteLine("Cannot read from DMCCOUNTER ($4011)!");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("DMCCOUNTER ($4011) Write not yet implemented");
		}
	}
	/// <summary>
	/// $4012
	/// </summary>
	internal class DMCADDR : Register
	{
		public byte Read()
		{
			Debug.WriteLine("Cannot read from DMCADDR ($4012)!");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("DMCADDR ($4012) Write not yet implemented");
		}
	}
	/// <summary>
	/// $4013
	/// </summary>
	internal class DMCLENGTH : Register
	{
		public byte Read()
		{
			Debug.WriteLine("Cannot read from DMCLENGTH ($4013)!");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("DMCLENGTH ($4013) Write not yet implemented");
		}
	}

	/// <summary>
	/// $4015
	/// </summary>
	internal class APUSTATUS : Register
	{
		public byte Read()
		{
			Debug.WriteLine("APUSTATUS ($4015) Read not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("APUSTATUS ($4015) Write not yet implemented");
		}
	}

	/// <summary>
	/// $4016
	/// </summary>
	internal class CTRL1 : Register
	{
		public byte Read()
		{
			Debug.WriteLine("CTRL1 ($4016) Read not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("CTRL1 ($4016) Write not yet implemented");
		}
	}
	/// <summary>
	/// $4017
	/// </summary>
	internal class CTRL2_FRAMECOUNTER : Register
	{
		public byte Read()
		{
			Debug.WriteLine("CTRL2_FRAMECOUNTER ($4017) Read not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("CTRL2_FRAMECOUNTER ($4017) Write not yet implemented");
		}
	}

	/// <summary>
	/// $4009 & $400D
	/// </summary>
	internal class DUMMYREGISTER : Register
	{
		public byte Read()
		{
			Debug.WriteLine("This is an empty register");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("This is an empty register");
		}
	}
}
