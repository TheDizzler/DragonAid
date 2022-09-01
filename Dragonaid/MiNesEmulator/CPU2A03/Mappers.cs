using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtomosZ.DragonAid.Libraries;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace AtomosZ.MiNesEmulator.CPU2A03.Mappers
{
	/// <summary>
	/// MMC1, specifically the SUROM used in DWIII.
	/// <para>Register info from 
	/// https://www.cnblogs.com/memset/archive/2012/07/18/everynes_nes_specifications.html
	/// and https://www.nesdev.org/wiki/MMC1
	/// </para>
	/// </summary>
	internal class MMC1 : Memory6502
	{
		/// <summary>
		/// <para>
		/// <br>Writing a value with bit 7 set ($80 through $FF) to any address
		/// in $8000-$FFFF clears the shift register to its initial state.</br>
		/// <br>To change a register's value, the CPU writes five times with bit
		/// 7 clear and a bit of the desired value in bit 0. On the first four
		/// writes, the MMC1 shifts bit 0 into a shift register. On the fifth
		/// write, the MMC1 copies bit 0 and the shift register contents into an
		/// internal register selected by bits 14 and 13 of the address, and
		/// then it clears the shift register.</br>
		/// <br>Only on the fifth write does the address matter, and even then,
		/// only bits 14 and 13 of the address matter because the mapper registers
		/// are incompletely decoded like the PPU registers. After the fifth write,
		/// the shift register is cleared automatically, so a write to the shift
		/// register with bit 7 on to reset it is not needed. </br>
		/// </para>
		/// </summary>
		private ShiftRegister shiftRegister;

		/* It is known that reg1 and reg2 are commonly used to switch CHRROM pages and
		 * that reg3 is used to switch PRGROM pages.  reg0 is used to switch between
		 * various MMC states.  Each of the five lowest bits of this register control a
		 * specific state of operation of the MMC.  Some of these states work in
		 * combination, and in some cases, a state will "override" another state.  Some
		 * of these states are affected by the MMC "reset" signal, and others are not. */
		private Register[] registers = new Register[]
		{
			new Reg0(),
			new Reg1(),
			new Reg2(),
			new Reg3(),
		};

		/// <summary>
		/// BankId * 0x4000
		/// </summary>
		private int lowBankAddress;
		/// <summary>
		/// BankId * 0x4000
		/// </summary>
		private int highBankAddress;
		/// <summary>
		/// Last cycle count when shift register was written to.
		/// </summary>
		private int lastCycle = -1;

		public MMC1(ControlUnit6502 cpu, byte[] romData, byte prgRomSize, bool hasBattery)
		{
			this.cpu = cpu;
			this.romData = romData;
			this.prgRomSize = prgRomSize;

			if (hasBattery)
			{
				wRAM = new byte[0x2000];
			}

			shiftRegister = new ShiftRegister();

			/* @TODO: confirm these initial values */
			registers[0].Write(0x0C); // 0 1100
			registers[1].Write(0x00); // 0 0000
			registers[2].Write(0x00); // 0 0000
			registers[3].Write(0x00); // 0 0000

			UpdateBankIds();
		}

		internal override byte this[int address]
		{
			get
			{
				if (address >= 0xC000)
				{
					return romData[highBankAddress + (address - 0xC000) + Address.iNESHeaderLength];
				}
				if (address >= 0x8000)
					return romData[lowBankAddress + (address - 0x8000) + Address.iNESHeaderLength];
				return base[address];
			}
			set
			{
				if (address < 0x8000)
				{
					base[address] = value;
					return;
				}
				/* There must be at least 2 cycles between shift register writes.
				 * If there isn't, the write gets ignored. */
				if (lastCycle >= cpu.cycleCount - 2)
				{
					throw new Exception("There must be at least 2 cycles between writes to the shift register.");
					return; // ignore this write
				}

				lastCycle = cpu.cycleCount;
				if (shiftRegister.Write(value))
				{
					byte reg = (byte)((address & 0x6000) >> 13);
					registers[reg].Write(shiftRegister.Read());
					shiftRegister.Clear();

					UpdateBankIds();
				}
			}
		}

		private void UpdateBankIds()
		{
			var reg0 = registers[0].Read();
			var reg1 = registers[1].Read();
			var reg2 = registers[2].Read();
			var reg3 = registers[3].Read();

			/* @TODO: implement CHR ROM/RAM switching and mirroring (need to implement PPU first).
			 *		After that, maybe have function calls in registers to update appropriate data
			 *		instead of updating everything at the same time. */
			var bankHighBit = (byte)(reg1 & 0x10);
			var bankLowBits = (byte)(reg3 & 0x0F);
			switch ((reg0 & 0x0C) >> 2)
			{
				case 0:
				case 1:
					throw new Exception("32KB bank switching mode not implemented");
					break;
				case 2: // lowBank is fixed to first bank
					lowBankAddress = bankHighBit * 0x4000;
					highBankAddress = (bankHighBit | bankLowBits) * 0x4000;
					break;
				case 3: // highBank is fixed to last bank
					lowBankAddress = (bankHighBit | bankLowBits) * 0x4000;
					highBankAddress = (bankHighBit | 0x0F) * 0x4000;
					break;
			}
		}

		internal override byte[] GetRegisterStates()
		{
			return new byte[]
			{
				shiftRegister.Read(),
				registers[0].Read(),
				registers[1].Read(),
				registers[2].Read(),
				registers[3].Read(),
				(byte)(lastCycle),
				(byte)(lastCycle >> 8),
				(byte)(lastCycle >> 16),
				(byte)(lastCycle >> 24),
			};
		}

		internal override void SetRegisterStates(byte[] states)
		{
			shiftRegister.Write(states[0]);
			registers[0].Write(states[1]);
			registers[1].Write(states[2]);
			registers[2].Write(states[3]);
			registers[3].Write(states[4]);
			lastCycle = states[5] + (states[6] << 8) + (states[7] << 16) + (states[8] << 24) ;

			UpdateBankIds();
		}

		/// <summary>
		/// $8000-$9FFF
		/// <para>
		/// <br>C PPMM</br>
		/// <br>M: Mirroring (0: one-screen, lower bank; 1: one-screen, upper bank;
		///			2: vertical; 3: horizontal)</br>
		/// <br>P: PRG ROM bank mode (0, 1: switch 32 KB at $8000, ignoring low bit of bank number;
		///			2: fix first bank at $8000 and switch 16 KB bank at $C000;
		///			3: fix last bank at $C000 and switch 16 KB bank at $8000)</br>
		/// <br>C: CHR ROM bank mode (0: switch 8 KB at a time; 1: switch two separate 4 KB banks)</br>
		/// </para>
		/// </summary>
		private class Reg0 : Register
		{
			private byte value;
			public byte Read()
			{
				return value;
			}

			public void Write(byte value)
			{
				this.value = value;
			}
		}

		/// <summary>
		/// $A000-$BFFF
		/// <para>
		/// Sets bank id (high bit, first 4 bits are in Reg3)
		/// <br>P SSxC</br>
		/// <br>C: Select 4K CHR RAM bank at PPU $0000 (ignored in 8K mode)</br>
		/// <br>S: Select 8K PRG RAM bank (must be set to same values as Reg2 S value)</br>
		/// <br>P: 0 Low Banks $00 to $0F ($00000-$3FFFF), 1 High Banks $10 to $1F
		///		($40000-$7FFFF) (in 4K mode must be set to same values as Reg2 P value)</br>
		/// </para>
		/// </summary>
		private class Reg1 : Register
		{
			private byte value;
			public byte Read()
			{
				return value;
			}

			public void Write(byte value)
			{
				this.value = value;
			}
		}

		/// <summary>
		/// $C000-$D000
		/// <para>
		/// <br>P SSxC</br>
		/// <br>C: Select 4K CHR RAM bank at PPU $1000 (ignored in 8K mode)	(not implemented on SUROM?)</br>
		/// <br>S: Select 8K PRG RAM bank	(ignored in 8K mode)
		///		(in 4K mode must be set to same values as Reg1 S value) (not implemented on SUROM?)</br>
		/// <br>P: Select 256K PRG ROM bank (ignored in 8K mode)
		///		(in 4K mode must be set to same values as Reg1 P value)</br>
		/// <br>In 4KB CHR bank mode, SNROM's E bit and SO/U/XROM's P and S bits in both
		///		CHR bank registers must be set to the same values, or the PRG ROM and/or
		///		RAM will be bankswitched/enabled as the PPU renders, in a similar fashion
		///		as MMC3's scanline counter. As there is not much of a reason to use 4KB
		///		bankswitching with CHR RAM, it is wise for programs to just set 8KB
		///		bankswitching mode</br>
		/// </para>
		/// </summary>
		private class Reg2 : Register
		{
			private byte value;
			public byte Read()
			{
				return value;
			}

			public void Write(byte value)
			{
				this.value = value;
			}
		}

		/// <summary>
		/// $E000-$FFFF
		/// <para>Sets bank id (first 4 low bits, high bit is in Reg1)
		/// <br>? PPPP</br>
		/// <br>P: 4 bit id of bank (low bit ignored in 32 KB mode)</br>
		/// </para>
		/// </summary>
		private class Reg3 : Register
		{
			private byte value;
			public byte Read()
			{
				return value;
			}

			public void Write(byte value)
			{
				this.value = value;
			}
		}
	}
}
