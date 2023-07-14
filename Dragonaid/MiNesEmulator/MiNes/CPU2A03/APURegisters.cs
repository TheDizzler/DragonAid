using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomosZ.MiNesEmulator.CPU2A03
{
	/// <summary>
	/// $4000 & $4004
	/// </summary>
	internal class SQDUTY : IRegister
	{
		public byte Read()
		{
			throw new Exception("Cannot read from SQDUTY!");
		}

		public void Write(byte value)
		{
			Debug.WriteLine("SQDUTY Write ($4000 & $4004) not yet implemented");
		}

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $4001 & $4005
	/// </summary>
	internal class SQSWEEP : IRegister
	{
		public byte Read()
		{
			throw new Exception("Cannot read from SQSWEEP!");
		}

		public void Write(byte value)
		{
			Debug.WriteLine("SQSWEEP Write ($4001 & $4005) not yet implemented");
		}

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $4002 & $4006
	/// </summary>
	internal class SQTIMER : IRegister
	{
		public byte Read()
		{
			throw new Exception("Cannot read from SQTIMER!");
		}

		public void Write(byte value)
		{
			Debug.WriteLine("SQTIMER Write ( $4002 & $4006) not yet implemented");
		}

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $4003 & $4007
	/// </summary>
	internal class SQLENGTH : IRegister
	{
		public byte Read()
		{
			throw new Exception("Cannot read from SQLENGTH!");
		}

		public void Write(byte value)
		{
			Debug.WriteLine("SQLENGTH Write ( $4003 & $4007) not yet implemented");
		}

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $4008
	/// </summary>
	internal class TRILINEAR : IRegister
	{
		public byte Read()
		{
			throw new Exception("Cannot read from TRILINEAR!");
		}

		public void Write(byte value)
		{
			Debug.WriteLine("TRILINEAR Write ($4008) not yet implemented");
		}

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $400A
	/// </summary>
	internal class TRITIMER : IRegister
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

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $400B
	/// </summary>
	internal class TRILENGTH : IRegister
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

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $400C
	/// </summary>
	internal class NOISEVOLUME : IRegister
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

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $400E
	/// </summary>
	internal class NOISEPERIOD : IRegister
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

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $400F
	/// </summary>
	internal class NOISELENGTH : IRegister
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

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $4010
	/// </summary>
	internal class DMCFREQ : IRegister
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

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $4011
	/// </summary>
	internal class DMCCOUNTER : IRegister
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

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $4012
	/// </summary>
	internal class DMCADDR : IRegister
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

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $4013
	/// </summary>
	internal class DMCLENGTH : IRegister
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

		public void Reset()
		{
		}
	}

	/// <summary>
	/// $4015
	/// </summary>
	internal class APUSTATUS : IRegister
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

		public void Reset()
		{
		}
	}

	/// <summary>
	/// $4016
	/// </summary>
	internal class CTRL1 : IRegister
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

		public void Reset()
		{
		}
	}
	/// <summary>
	/// $4017
	/// </summary>
	internal class CTRL2_FRAMECOUNTER : IRegister
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

		public void Reset()
		{
		}
	}

	/// <summary>
	/// $4009 & $400D
	/// </summary>
	internal class DUMMYREGISTER : IRegister
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

		public void Reset()
		{
		}
	}
}
