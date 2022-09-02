using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Windows.Forms;
using AtomosZ.MiNesEmulator.CPU2A03.Mappers;

namespace AtomosZ.MiNesEmulator.CPU2A03
{
	/// <summary>
	/// Representation of NES memory with cartridge memory.
	/// </summary>
	internal abstract class Memory6502
	{
		private enum MirroringMode
		{
			Horizontal = 0x0,
			Vertical = 0x01,
		}

		/// <summary>
		/// In bytes, of course.
		/// </summary>
		private const int BankLength = 0x4000;

		protected ControlUnit6502 cpu;
		/// <summary>
		/// In 16KB units.
		/// </summary>
		protected byte prgRomSize;
		/// <summary>
		/// PRG RAM $6000-7FFF
		/// </summary>
		protected bool hasBattery;
		protected byte mapperNum;

		/// <summary>
		/// RAM from 0 to 0x7FF.
		/// Mirrored 3 times up to 0x2000.
		/// </summary>
		protected byte[] internalRAM = new byte[0x800];
		/// <summary>
		/// 8 ports, mirrored over and over.
		/// From 0x2000 0x2007 (0x2008 to 0x3FFF mirrors).
		/// $2000 PPUCTRL
		/// $2001 PPUMASK
		/// $2002 PPUSTATUS
		/// $2003 OAMADDR
		/// $2004 OAMDATA
		/// $2005 PPUSCROLL
		/// $2006 PPUADDR
		/// $2007 PPUDATA
		/// </summary>
		protected Register[] ppuIOPorts = new Register[]
		{
			new PPUCTRL(),
			new PPUMASK(),
			new PPUSTATUS(),
			new OAMADDR(),
			new OAMDATA(),
			new PPUSCROLL(),
			new PPUADDR(),
			new PPUDATA(),
		};
		/// <summary>
		/// $4000 SQDUTY
		/// $4001 SQSWEEP
		/// $4002 SQTIMER
		/// $4003 SQLENGTH
		/// $4004 SQDUTY
		/// $4005 SQSWEEP
		/// $4006 SQTIMER
		/// $4007 SQLENGTH
		/// $4008 TRILINEAR
		/// $4009 DUMMYREGISTER
		/// $400A TRITIMER
		/// $400B TRILENGTH
		/// $400C NOISEVOLUME
		/// $400D DUMMYREGISTER
		/// $400E NOISEPERIOD
		/// $400F NOISELENGTH
		/// $4010 DMCFREQ
		/// $4011 DMCCOUNTER
		/// $4012 DMCADDR
		/// $4013 DMCLENGTH
		/// $4014 OAMDMA
		/// $4015 APUSTATUS
		/// $4016 CTRL1
		/// $4017 CTRL2_FRAMECOUNTER
		/// </summary>
		protected Register[] apuControllerIOPorts = new Register[]
		{
			new SQDUTY(),
			new SQSWEEP(),
			new SQTIMER(),
			new SQLENGTH(),
			new SQDUTY(),
			new SQSWEEP(),
			new SQTIMER(),
			new SQLENGTH(),
			new TRILINEAR(),
			new DUMMYREGISTER(),
			new TRITIMER(),
			new TRILENGTH(),
			new NOISEVOLUME(),
			new DUMMYREGISTER(),
			new NOISEPERIOD(),
			new NOISELENGTH(),
			new DMCFREQ(),
			new DMCCOUNTER(),
			new DMCADDR(),
			new DMCLENGTH(),
			new OAMDMA(),
			new APUSTATUS(),
			new CTRL1(),
			new CTRL2_FRAMECOUNTER(),
		};
		/* The following memory is on the cartridge. */
		/// <summary>
		/// 0x6000 to 0x7FFF. Optional. Maybe backed up with battery.
		/// </summary>
		protected byte[] wRAM;
		/// <summary>
		/// 0x8000 to 0xFFFF.
		/// </summary>
		protected byte[] cartridgeROM = new byte[0x8000];
		/// <summary>
		/// game data
		/// </summary>
		protected byte[] romData;

		internal static Memory6502 Initialize(ControlUnit6502 cu, byte[] romData)
		{
			// read iNES header
			if (romData[0] != 'N' && romData[1] != 'E' && romData[2] != 'S' && romData[3] != 0x1A)
			{
				throw new Exception("Invalid rom header");
			}

			var prgRomSize = romData[4];
			var chrRomSize = romData[5];

			// flag 6
			var mirroring = (MirroringMode)(romData[6] & 0x01);
			var use4ScreenRam = (romData[6] & 0x04) == 0x04;

			var hasBattery = (romData[6] & 0x02) == 0x02;


			// flag 7
			// 0x00 VS Unisystem
			// 0x01 PlayChoice-10
			// 0x02 & 0x04 == NES 2.0 format (flags 8-15)

			var mapperNum = (byte)(((romData[6] & 0xF0) >> 4) + (romData[7] & 0xF0));
			switch (mapperNum)
			{
				//case 0: // NROM
				//	return new NROM(romData, prgRomSize);
				case 1: // MMC1, SxROM
					return new MMC1(cu, romData, prgRomSize, hasBattery);
				default:
					throw new Exception($"Mapper {mapperNum} has not been emulated");
			}
		}

		internal abstract byte[] GetRegisterStates();
		internal abstract void SetRegisterStates(byte[] states);


		/// <summary>
		/// <para>
		/// <br>Copies memory of length bytes to byte array or </br>
		/// <br>Copies bytes from byte array to pointer address. </br>
		/// <br>NOTE: Since this clamps the pointer value from a mirrored address
		///		to a non-mirrored address, this COULD have unexpected behaviour.</br>
		///	</para>
		/// </summary>
		/// <param name="address"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		internal virtual byte[] this[int address, int length]
		{
			get
			{
				var data = new byte[length];
				for (int i = 0; i < length; ++i)
					data[i] = this[address + i];
				return data;
			}
			set
			{
				for (int i = 0; i < length; ++i)
					this[address + i] = value[i];
			}
		}
		internal virtual byte this[int address]
		{
			get
			{
				if (address < 0x2000)
					return internalRAM[address & 0x07FF];
				if (address < 0x4000)
				{
					var addr = address & 0x07;
					return ppuIOPorts[addr].Read();
				}
				if (address < 0x6000)
					return apuControllerIOPorts[address & 0x17].Read();
				if (address < 0x8000) // Save RAM. May not exist!
					return wRAM[address - 0x6000];

				/* @Consider: should this make a call to an abstract function
				 * so it kind of simulates a cartridge, as opposed to overriding this function?*/
				if (address >= 0x8000)
					return cartridgeROM[address - 0x8000];

				throw new Exception($"Just what are you trying to do throwing around an address like {address}?");
			}
			set
			{
				if (address < 0x2000)
				{
					internalRAM[address & 0x07FF] = value;
					return;
				}
				if (address < 0x4000)
				{
					ppuIOPorts[address & 0x08].Write(value);
					return;
				}
				if (address < 0x6000)
				{
					apuControllerIOPorts[address & 0x17].Write(value);
					return;
				}
				if (address < 0x8000) // Save RAM. May not exist!
				{
					wRAM[address - 0x6000] = value;
					return;
				}

				/* @Consider: should this make a call to an abstract function
				 * so it kind of simulates a cartridge, as opposed to overriding this function?*/
				if (address >= 0x8000)
					throw new Exception($"Game is trying to set {address}. Is this using an unsupported mapper chip?");
				//	cartridgeROM[address - 0x8000];

				throw new Exception($"Just what are you trying to do throwing around an address like {address}?");
			}
		}
	}
}
