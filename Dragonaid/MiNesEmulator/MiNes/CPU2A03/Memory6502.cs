using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtomosZ.MiNesEmulator.CPU2A03.Mappers;
using AtomosZ.MiNesEmulator.PPU2C02;
using static AtomosZ.MiNesEmulator.PPU2C02.PPU;

namespace AtomosZ.MiNesEmulator.CPU2A03
{
	/// <summary>
	/// Representation of NES memory with cartridge memory.
	/// </summary>
	internal abstract class Memory6502
	{

		/// <summary>
		/// In bytes, of course.
		/// </summary>
		private const int BankLength = 0x4000;

		protected ControlUnit6502 controlUnit;
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
		/// 8 ports, mirrored over and over.<br/>
		/// From 0x2000 0x2007 (0x2008 to 0x3FFF mirrors).<br/>
		/// $2000 PPUCTRL<br/>
		/// $2001 PPUMASK<br/>
		/// $2002 PPUSTATUS<br/>
		/// $2003 OAMADDR<br/>
		/// $2004 OAMDATA<br/>
		/// $2005 PPUSCROLL<br/>
		/// $2006 PPUADDR<br/>
		/// $2007 PPUDATA<br/>
		/// </summary>
		protected PPULatch ppuLatch;
		//protected IRegister[] ppuIOPorts = new IRegister[8];
		/// <summary>
		/// $4000 SQDUTY<br/>
		/// $4001 SQSWEEP<br/>
		/// $4002 SQTIMER<br/>
		/// $4003 SQLENGTH<br/>
		/// $4004 SQDUTY<br/>
		/// $4005 SQSWEEP<br/>
		/// $4006 SQTIMER<br/>
		/// $4007 SQLENGTH<br/>
		/// $4008 TRILINEAR<br/>
		/// $4009 DUMMYREGISTER<br/>
		/// $400A TRITIMER<br/>
		/// $400B TRILENGTH<br/>
		/// $400C NOISEVOLUME<br/>
		/// $400D DUMMYREGISTER<br/>
		/// $400E NOISEPERIOD<br/>
		/// $400F NOISELENGTH<br/>
		/// $4010 DMCFREQ<br/>
		/// $4011 DMCCOUNTER<br/>
		/// $4012 DMCADDR<br/>
		/// $4013 DMCLENGTH<br/>
		/// $4014 OAMDMA<br/>
		/// $4015 APUSTATUS<br/>
		/// $4016 CTRL1<br/>
		/// $4017 CTRL2_FRAMECOUNTER<br/>
		/// </summary>
		protected IRegister[] apuControllerIOPorts = new IRegister[]
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

		internal static Memory6502 Initialize(ControlUnit6502 cu, PPU ppu, byte[] romData)
		{
			// read iNES header
			if (romData[0] != 'N' && romData[1] != 'E' && romData[2] != 'S' && romData[3] != 0x1A)
			{
				throw new Exception("Invalid rom header");
			}

			var prgRomSize = romData[4];
			var chrRomSize = romData[5];

			// flag 6
			//var mirroring = (MirroringMode)(romData[6] & 0x01);
			//var use4ScreenRam = (romData[6] & 0x04) == 0x04;

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
					return new MMC1(cu, ppu, romData, prgRomSize, hasBattery);
				default:
					throw new Exception($"Mapper {mapperNum} has not been emulated");
			}
		}

		internal Memory6502(ControlUnit6502 controlUnit, PPU ppu, byte[] romData, byte prgRomSize)
		{
			this.controlUnit = controlUnit;
			this.romData = romData;
			this.prgRomSize = prgRomSize;

			this.ppuLatch = ppu.ppuLatch;

			this.oamPort = ppu.oamPort; // what is this for again??

			//ppuIOPorts[0] = ppu.ppuCtrl;
			//ppuIOPorts[1] = ppu.ppuMask;
			//ppuIOPorts[2] = ppu.ppuStatus;
			//ppuIOPorts[3] = ppu.oamAddr;
			//ppuIOPorts[4] = ppu.oamData;
			//ppuIOPorts[5] = ppu.ppuScroll;
			//ppuIOPorts[6] = ppu.ppuAddr;
			//ppuIOPorts[7] = ppu.ppuData;


			//if (hasBattery) // DWIII ROM claims there is no battery
			{
				wRAM = new byte[0x2000];
			}
		}

		internal abstract byte[] GetRegisterStates();
		internal abstract void SetRegisterStates(byte[] states);


		/// <summary>
		/// <para>
		/// Copies memory of length bytes to byte array<br/>or<br/>
		/// Copies bytes from byte array to pointer address.<br/>
		/// NOTE: Since this clamps the pointer value from a mirrored address
		///		to a non-mirrored address, this COULD have unexpected behaviour.<br/>
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
					var addrMask = (byte)(address & 0x07);
					//return ppuIOPorts[addrMask].Read();
					return ppuLatch.Read(addrMask);
				}
				if (address == 0x4014)
					throw new Exception("What is expected from a read from $4014?");

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
					//ppuIOPorts[address & 0x08].Write(value);
					ppuLatch.Write((byte)(address & 0x08), value);
					return;
				}
				if (address == 0x4014)
				{
					// This port is located on the CPU. Writing $XX will upload 256 bytes of data
					//	from CPU page $XX00-$XXFF to the internal PPU OAM.
					// The CPU is suspended during the transfer, which will take 513 or 514 cycles
					//	after the $4014 write tick. (1 wait state cycle while waiting for writes
					//	to complete, +1 if on an odd CPU cycle, then 256 alternating read/write cycles.)

					/* @TODO: transfer sprites to OAM
					   * Fortunately we don't have to worry about syncing here because both the CPU and PPU are busy. */


					//oamPort.InitiateOAMCopy();
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
