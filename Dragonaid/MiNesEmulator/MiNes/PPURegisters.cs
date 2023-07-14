using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using AtomosZ.MiNesEmulator.CPU2A03;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace AtomosZ.MiNesEmulator.PPU2C02
{
	/// <summary>
	/// $2000
	/// <para>
	/// 7  bit  0<br/>
	/// ---- ----<br/>
	/// VPHB SINN<br/>
	/// <br/>
	/// N: Base nametable address
	/// 	(0 = $2000; 1 = $2400; 2 = $2800; 3 = $2C00)<br/>
	/// I: VRAM address increment per CPU read/write of PPUDATA
	///		(0: add 1, going across; 1: add 32, going down)<br/>
	/// S: Sprite pattern table address for 8x8 sprites
	///		(0: $0000; 1: $1000; ignored in 8x16 mode)<br/>
	/// B: Background pattern table address (0: $0000; 1: $1000)
	/// 	Sprite size (0: 8x8 pixels; 1: 8x16 pixels – see PPU OAM#Byte 1)<br/>
	/// P: PPU master/slave select
	/// 	(0: read backdrop from EXT pins; 1: output color on EXT pins)<br/>
	/// V: Generate an NMI at the start of the
	///		vertical blanking interval (0: off; 1: on)<br/>
	/// </para>
	/// </summary>
	internal class PPUCTRL : IRegister
	{
		private int value = 0;

		public byte Read()
		{
			throw new Exception("Cannot read from PPUCTRL ($2000)?");
		}

		/// <summary>
		/// @TODO: Ignores writes if before 29658 cycles after reset.
		/// </summary>
		/// <param name="value"></param>
		public void Write(byte value)
		{
			Debug.WriteLine("PPUCTRL Write ($2000) not yet implemented");
		}

		public void Reset()
		{
			value = 0;
		}
	}
	/// <summary>
	/// $2001
	/// </summary>
	internal class PPUMASK : IRegister
	{
		private int value = 0;
		public byte Read()
		{
			throw new Exception("Cannot read from PPUMASK ($2001)?");
		}

		/// <summary>
		/// Ignores writes if before 29658 cycles after reset.
		/// </summary>
		/// <param name="value"></param>
		public void Write(byte value)
		{
			Debug.WriteLine("PPUMASK Write ($2001) not yet implemented");
		}

		public void Reset()
		{
			value = 0;
		}
	}
	/// <summary>
	/// $2002
	/// </summary>
	internal class PPUSTATUS : IRegister
	{
		private int value = 0; // changes
		public byte Read()
		{
			Debug.WriteLine("PPUSTATUS Read ($2002) not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			throw new Exception("Cannot write to PPUSTATUS ($2002)?");
		}

		public void Reset()
		{
			value = value & 0x80; // this one is pretty complicated
		}
	}
	/// <summary>
	/// $2003
	/// </summary>
	internal class OAMADDR : IRegister
	{
		private byte value = 0;
		public byte Read()
		{
			throw new Exception("Cannot read from PPUADDR ($2003)?");
		}

		public void Write(byte value)
		{
			Debug.WriteLine("OAMADDR ($2003) Write not yet implemented");
		}

		public void Reset()
		{
			value = 0;
		}
	}

	/// <summary>
	/// $2004
	/// </summary>
	internal class OAMDATA : IRegister
	{
		private byte value = 0; // unspecified?
		public byte Read()
		{
			Debug.WriteLine("OAMDATA ($2004) Read not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("OAMDATA ($2004) Write not yet implemented");
		}

		public void Reset()
		{
			value = 0; // unspecified?
		}
	}

	/// <summary>
	/// $2005
	/// </summary>
	internal class PPUSCROLL : IRegister
	{
		private ushort value = 0;
		private bool latch = false;
		private PPU ppu;

		public byte Read()
		{
			throw new Exception("Cannot read from PPUSCROLL ($2005)?");
		}

		/// <summary>
		/// Ignores writes if before 29658 cycles after reset.
		/// </summary>
		/// <param name="value"></param>
		public void Write(byte value)
		{
			Debug.WriteLine("PPUSCROLL ($2005) Write not yet implemented");
		}

		public void Reset()
		{
			value = 0;
			latch = false;
		}
	}

	/// <summary>
	/// $2006
	/// </summary>
	internal class PPUADDR : IRegister
	{
		private ushort value = 0;
		private bool latch = false;

		public byte Read()
		{
			throw new Exception("Cannot read from PPUADDR ($2006)?");
		}

		/// <summary>
		/// @TODO: Ignores writes if before 29658 cycles after reset.
		/// </summary>
		/// <param name="value"></param>
		public void Write(byte value)
		{
			//latchTargetRegister = registerMask;
			//latchValue <<= 8 * writeCount;
			//latchValue |= value;

			//if (++writeCount == 2)
			//{
			//	writeCount = 0;
			//}
		}

		public void Reset()
		{
			// value does not change on reset
			latch = false;
		}
	}
	/// <summary>
	/// $2007
	/// </summary>
	internal class PPUDATA : IRegister
	{
		private byte value = 0;
		public byte Read()
		{
			Debug.WriteLine("PPUDATA ($2007) Read not yet implemented");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("PPUDATA ($2007) Write not yet implemented");
		}

		public void Reset()
		{
			value = 0;
		}
	}

	/// <summary>
	/// $4014 aka SpriteDMA
	/// </summary>
	internal class OAMDMA : IRegister
	{
		public byte Read()
		{
			throw new Exception("Shoud not be reading $4014");
			return 0xFF;
		}

		public void Write(byte value)
		{
			Debug.WriteLine("OAMDMA ($4014) Write not yet implemented");
		}

		public void Reset()
		{
			//value = 0; // ?? unspecified?
		}
	}
}
