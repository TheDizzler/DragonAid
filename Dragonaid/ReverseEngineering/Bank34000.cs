using System;
using System.Collections.Generic;
using AtomosZ.DragonAid.Libraries;

using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static AtomosZ.DragonAid.ReverseEngineering.ROMPlaceholders;


namespace AtomosZ.DragonAid.ReverseEngineering
{
	public static class Bank34000
	{
		/// <summary>
		/// 35DC0
		/// </summary>
		public static void DynamicSubroutine_34000_B()
		{
			if ((byte)(zeroPages[0xAC] & 0x1F) != 0)
				return;
			if (zeroPages[0x87] == 0 // mapScrollCheck+1
					&& zeroPages[0x86] == 0)
				return;

			// 35DCF - _DynamicSubroutine_34000_B_cont
			if (zeroPages[ZeroPage.encounterVariable_A] != 0)
				return;
			if (zeroPages[0xAC & 0x1F] != 0) // AGAIN for some reason
				return;

			zeroPages[0x73] = 0x01;
			zeroPages[0x72] = 0x04;
			// JSR F3C1FD
			Bank3C000.L3C1FX(0xFF, 0x04, 0x04);
			// 9DE8
			byte a = zeroPages[0x97];
			byte x = zeroPages[0x8F];
			if (x >= 0x12 || (a & 0x7F) < 0x40)
			{
				L35E76(); // L35DFC();
				return;
			}

			Adjust_SpriteDMA();
			// _DynamicSubroutine_34000_B_cont_cont
			x = zeroPages[0x8F];
			if (x >= 0x22 || (zeroPages[0x97] & 0x3F) < 0x20)
			{
				L35E76();
				return;
			}

			Adjust_SpriteDMA();
			// _DynamicSubroutine_34000_B_cont_cont_cont
			if ((zeroPages[0x97] & 0x1F) >= 0x10)
				Adjust_SpriteDMA();
		}

		private static void Adjust_SpriteDMA()
		{
			byte characterIndex = zeroPages[0x72];
			characterIndex >>= 1;
			byte a = nesRam[NESRAM.Character_Statuses + characterIndex];
			if (a < 80) // negative flag not set
				return; // cancel scroll if all characters dead?
			byte x = zeroPages[ZeroPage.dynamicSubroutineAddr];
			byte y = 0x03;
			int address = zeroPages[0x72] + zeroPages[0x73] << 4;
			a = nesRam[address + y];
			a &= 0x03;
			if (a == 0)
			{ // L35E5B();
				--nesRam[NESRAM.PPU_SpriteDMA + x + 0];
				--nesRam[NESRAM.PPU_SpriteDMA + x + 4];
				--nesRam[NESRAM.PPU_SpriteDMA + x + 8];
				--nesRam[NESRAM.PPU_SpriteDMA + x + 12];
			}
			else if (a == 0x01)
			{ // L35E4C();
				++nesRam[NESRAM.PPU_SpriteDMA + x + 3];
				++nesRam[NESRAM.PPU_SpriteDMA + x + 7];
				++nesRam[NESRAM.PPU_SpriteDMA + x + 11];
				++nesRam[NESRAM.PPU_SpriteDMA + x + 15];
			}
			else if (a == 0x02)
			{ // L35E6A()
				++nesRam[NESRAM.PPU_SpriteDMA + x + 0];
				++nesRam[NESRAM.PPU_SpriteDMA + x + 4];
				++nesRam[NESRAM.PPU_SpriteDMA + x + 8];
				++nesRam[NESRAM.PPU_SpriteDMA + x + 12];
			}
			else
			{ // scrolled left
				--nesRam[NESRAM.PPU_SpriteDMA + x + 3];
				--nesRam[NESRAM.PPU_SpriteDMA + x + 7];
				--nesRam[NESRAM.PPU_SpriteDMA + x + 11];
				--nesRam[NESRAM.PPU_SpriteDMA + x + 15];
			}

			L35E76();
		}

		private static void L35E76()
		{
			bool hasCarry = false;
			zeroPages[0x72] = ASMHelper.ADC(zeroPages[0x72], 0x04, ref hasCarry);
			hasCarry = false;
			zeroPages[ZeroPage.dynamicSubroutineAddr] = ASMHelper.ADC(zeroPages[ZeroPage.dynamicSubroutineAddr], 0x10, ref hasCarry);
		}


		/// <summary>
		/// 35B13
		/// </summary>
		public static void DynamicSubroutine_34000_C()
		{
			if ((zeroPages[0x2F] & 0x01) == 0)
			{
				L35AB0();
				return;
			}

			// L35B1C
			if (nesRam[NESRAM.encounterCheckRequired_A] == 0)
			{
				L35B3A();
				return;
			}

			byte x = 0;
			zeroPages[0x73 + 1] = 0x01; // dialogSegmentPointer+1
			zeroPages[0x73 + 0] = nesRam[0x06E4 + x];
			theStack.Push(x);

			L35B45();
		}

		private static void L35AB0()
		{
			if ((zeroPages[ZeroPage.lightOrDarkWorld] & 0x02) != 0)
				return;
			if ((zeroPages[0x9D] & 0x08) != 0)
				return;
		}




		public static void DynamicSubroutine_34000_D()
		{
			if ((byte)(zeroPages[0x2F] & 0x01) != 0)
				DynamicSubroutine_34000_isDarkWorld();
			else
				DynamicSubroutine_34000_isLightWorld();
		}

		private static void DynamicSubroutine_34000_isLightWorld()
		{
			if ((byte)(zeroPages[0x90] & 0x3F) != 0
				|| (byte)(zeroPages[0x2F] & 0x02) != 0
				|| zeroPages[0x8F] != 0)
				return;

			var x = zeroPages[0x9B];
			var y = zeroPages[0x9C];
			if (x != zeroPages[ZeroPage.map_WorldPosition_X]
				|| y != zeroPages[ZeroPage.map_WorldPosition_Y])
				__DynamicSubroutine_34000_D_isLightWorld(x, y);
		}

		private static void __DynamicSubroutine_34000_D_isLightWorld(byte x, byte y)
		{
			if ((byte)(zeroPages[0x97] & 0x10) == 0)
				return; // bit 4 == 0

			zeroPages[0x80] = x;
			zeroPages[0x81] = y;
			STA_WorldMapCoordsTo_06_07(x, y);

		}

		/// <summary>
		/// 3F7B7
		/// </summary>
		private static void STA_WorldMapCoordsTo_06_07(byte x, byte y)
		{
			zeroPages[0x06] = zeroPages[ZeroPage.map_WorldPosition_X];
			zeroPages[0x07] = zeroPages[ZeroPage.map_WorldPosition_Y];
			L3F7E0(x, y);
		}

		/// <summary>
		/// 3F7E0
		/// </summary>
		private static void L3F7E0(byte x, byte y)
		{
			zeroPages[0x08] = 0x08;
			zeroPages[0x09] = 0x07;
			zeroPages[0x04] = x;
			zeroPages[0x05] = y;
			byte a = ASMHelper.SBC(zeroPages[0x06], zeroPages[0x08], out bool hasCarry);
			if (!hasCarry)
				a = 0x0;
			if (a > zeroPages[0x04])
			{ //	L3F7FD();
				hasCarry = false;
				a = ASMHelper.ADC(zeroPages[0x06], zeroPages[0x09], ref hasCarry);
				if (hasCarry)
					a = 0xFF;
				if (a < zeroPages[0x04])
					return;
				// have not seen from here run
				a = zeroPages[0x07];
				hasCarry = false;
				a = ASMHelper.SBC(a, zeroPages[0x09], out hasCarry);
				if (!hasCarry)
					a = 0;
				if (a <= zeroPages[0x05])
				{
					//F81B
				}
			}
		}



		private static void DynamicSubroutine_34000_isDarkWorld()
		{
			throw new NotImplementedException();
		}
	}
}
