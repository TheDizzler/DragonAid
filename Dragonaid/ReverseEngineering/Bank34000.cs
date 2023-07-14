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
		public static void AdjustSpriteDMA()
		{
			if ((byte)(zeroPage[0xAC] & 0x1F) != 0)
				return;
			if (zeroPage[0x87] == 0 // mapScrollCheck+1
					&& zeroPage[0x86] == 0)
				return;

			// 35DCF - _DynamicSubroutine_34000_B_cont
			if (zeroPage[ZeroPage.encounterVariable_A] != 0)
				return;
			if ((zeroPage[0xAC] & 0x1F) != 0) // AGAIN for some reason
				return;

			zeroPage[0x73] = 0x01;
			zeroPage[0x72] = 0x04;
			// JSR F3C1FD
			Bank3C000.L3C1FX(0xFF, 0x04, 0x04);
			// 9DE8
			byte a = zeroPage[0x97];
			byte x = zeroPage[0x8F];
			if (x >= 0x12 || (a & 0x7F) < 0x40)
			{
				L35E76(); // L35DFC();
				return;
			}

			Adjust_SpriteDMA();
			// _DynamicSubroutine_34000_B_cont_cont
			x = zeroPage[0x8F];
			if (x >= 0x22 || (zeroPage[0x97] & 0x3F) < 0x20)
			{
				L35E76();
				return;
			}

			Adjust_SpriteDMA();
			// _DynamicSubroutine_34000_B_cont_cont_cont
			if ((zeroPage[0x97] & 0x1F) >= 0x10)
				Adjust_SpriteDMA();
		}

		private static void Adjust_SpriteDMA()
		{
			byte characterIndex = zeroPage[0x72];
			characterIndex >>= 1;
			byte a = nesRam[NESRAM.Character_Statuses + characterIndex];
			if (a < 80) // negative flag not set
				return; // cancel scroll if all characters dead?
			byte x = zeroPage[ZeroPage.dynamicSubroutine_21];
			byte y = 0x03;
			int address = zeroPage[0x72] + zeroPage[0x73] << 4;
			a = nesRam[address + y];
			a &= 0x03;
			if (a == 0)
			{ // L35E5B();
				--nesRam[NESRAM.PPU_SpriteDMA_200 + x + 0];
				--nesRam[NESRAM.PPU_SpriteDMA_200 + x + 4];
				--nesRam[NESRAM.PPU_SpriteDMA_200 + x + 8];
				--nesRam[NESRAM.PPU_SpriteDMA_200 + x + 12];
			}
			else if (a == 0x01)
			{ // L35E4C();
				++nesRam[NESRAM.PPU_SpriteDMA_200 + x + 3];
				++nesRam[NESRAM.PPU_SpriteDMA_200 + x + 7];
				++nesRam[NESRAM.PPU_SpriteDMA_200 + x + 11];
				++nesRam[NESRAM.PPU_SpriteDMA_200 + x + 15];
			}
			else if (a == 0x02)
			{ // L35E6A()
				++nesRam[NESRAM.PPU_SpriteDMA_200 + x + 0];
				++nesRam[NESRAM.PPU_SpriteDMA_200 + x + 4];
				++nesRam[NESRAM.PPU_SpriteDMA_200 + x + 8];
				++nesRam[NESRAM.PPU_SpriteDMA_200 + x + 12];
			}
			else
			{ // scrolled left
				--nesRam[NESRAM.PPU_SpriteDMA_200 + x + 3];
				--nesRam[NESRAM.PPU_SpriteDMA_200 + x + 7];
				--nesRam[NESRAM.PPU_SpriteDMA_200 + x + 11];
				--nesRam[NESRAM.PPU_SpriteDMA_200 + x + 15];
			}

			L35E76();
		}

		private static void L35E76()
		{
			bool hasCarry = false;
			zeroPage[0x72] = ASMHelper.ADC(zeroPage[0x72], 0x04, ref hasCarry);
			hasCarry = false;
			zeroPage[ZeroPage.dynamicSubroutine_21] = ASMHelper.ADC(zeroPage[ZeroPage.dynamicSubroutine_21], 0x10, ref hasCarry);
		}


		/// <summary>
		/// 35B13
		/// </summary>
		public static void DynamicSubroutine_34000_C()
		{
			if ((zeroPage[0x2F] & 0x01) == 0)
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
			zeroPage[0x73 + 1] = 0x01; // dialogSegmentPointer+1
			zeroPage[0x73 + 0] = nesRam[0x06E4 + x];
			theStack.Push(x);

			L35B45();
		}

		private static void L35AB0()
		{
			if ((zeroPage[ZeroPage.lightOrDarkWorld] & 0x02) != 0)
				return;
			if ((zeroPage[0x9D] & 0x08) != 0)
				return;
		}




		public static void DynamicSubroutine_34000_D()
		{
			if ((byte)(zeroPage[0x2F] & 0x01) != 0)
				DynamicSubroutine_34000_isDarkWorld();
			else
				DynamicSubroutine_34000_isLightWorld();
		}

		private static void DynamicSubroutine_34000_isLightWorld()
		{
			if ((byte)(zeroPage[0x90] & 0x3F) != 0
				|| (byte)(zeroPage[0x2F] & 0x02) != 0
				|| zeroPage[0x8F] != 0)
				return;

			var x = zeroPage[0x9B];
			var y = zeroPage[0x9C];
			if (x != zeroPage[ZeroPage.map_WorldPosition_X]
				|| y != zeroPage[ZeroPage.map_WorldPosition_Y])
				__DynamicSubroutine_34000_D_isLightWorld(x, y);
		}

		private static void __DynamicSubroutine_34000_D_isLightWorld(byte x, byte y)
		{
			if ((byte)(zeroPage[0x97] & 0x10) == 0)
				return; // bit 4 == 0

			zeroPage[0x80] = x;
			zeroPage[0x81] = y;
			STA_WorldMapCoordsTo_06_07(x, y);

		}

		/// <summary>
		/// 3F7B7
		/// </summary>
		private static void STA_WorldMapCoordsTo_06_07(byte x, byte y)
		{
			zeroPage[0x06] = zeroPage[ZeroPage.map_WorldPosition_X];
			zeroPage[0x07] = zeroPage[ZeroPage.map_WorldPosition_Y];
			L3F7E0(x, y);
		}

		/// <summary>
		/// 3F7E0
		/// </summary>
		private static void L3F7E0(byte x, byte y)
		{
			zeroPage[0x08] = 0x08;
			zeroPage[0x09] = 0x07;
			zeroPage[0x04] = x;
			zeroPage[0x05] = y;
			bool hasCarry = true;
			byte a = ASMHelper.SBC(zeroPage[0x06], zeroPage[0x08], ref  hasCarry);
			if (!hasCarry)
				a = 0x0;
			if (a > zeroPage[0x04])
			{ //	L3F7FD();
				hasCarry = false;
				a = ASMHelper.ADC(zeroPage[0x06], zeroPage[0x09], ref hasCarry);
				if (hasCarry)
					a = 0xFF;
				if (a < zeroPage[0x04])
					return;
				// have not seen from here run
				a = zeroPage[0x07];
				hasCarry = false; // is this really supposed to be false?
				a = ASMHelper.SBC(a, zeroPage[0x09], ref hasCarry);
				if (!hasCarry)
					a = 0;
				if (a <= zeroPage[0x05])
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
