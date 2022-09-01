using System;
using System.Collections.Generic;
using AtomosZ.DragonAid.Libraries;

using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static AtomosZ.DragonAid.ReverseEngineering.ROMPlaceholders;

namespace AtomosZ.DragonAid.ReverseEngineering
{
	internal class Bank00000
	{
		/// <summary>
		/// 0x00222
		/// </summary>
		public static void Map_CheckForEncounter()
		{
			// NOP x5
			if (nesRam[NESRAM.encounterCheckRequired_A] != 0)
				return;
			byte x = zeroPages[ZeroPages.map_WorldPosition_X];
			if (--x >= 0xFE) // invalid position?
				return;
			byte y = zeroPages[ZeroPages.map_WorldPosition_Y];
			if (--y >= 0xFE) // invalid position?
				return;
			if (saveRam[SRAM.encounterCheckRequired_b] < 0x80)
				CheckForEncounter();
		}

		/// <summary>
		/// 0x00240
		/// </summary>
		private static void CheckForEncounter()
		{
			if ((saveRam[SRAM.encounterCheckRequired_c] & 0x90) == 0x80)
				return;
			zeroPages[0x5C] = 0xFF;
			Map_GetEncouterZone();
			if (zeroPages[0x5C] == 0)
				Bank3C000.L3CCBD();
		}

		private static void Map_GetEncouterZone()
		{
			if (zeroPages[0x9A] != 0
			 || (zeroPages[0xAD] & 0x7F) == 0
			 || (--zeroPages[0xAD] & 0x7F) != 0)
			{
				Map_GetLightOrDarkEncounterZone();
			}
			else
			{
				Bank38000.Clear_0647to0664();
			}

		}

		/// <summary>
		/// 0x0028E
		/// </summary>
		private static void Map_GetLightOrDarkEncounterZone()
		{
			zeroPages[0x5E] = 0x00;
			zeroPages[0x4C] = 0x00;
			zeroPages[0x62] = 0x00;
			if ((zeroPages[0x2F] & 0x01) != 0)
			{
				_2FBit0Set();
			}
			else if ((zeroPages[0x2F] & 0x02) == 0)
				Map_GetEncounterZone_LightWorld();
			else
				Map_GetEncounterZone_DarkWorld();

		}

		/// <summary>
		/// 0x002C3
		/// </summary>
		private static void Map_GetEncounterZone_LightWorld()
		{
			zeroPages[0x4A] = (byte)(zeroPages[ZeroPages.map_WorldPosition_X] >> 4); // top 4 bits in low nibble
			byte x = (byte)((zeroPages[ZeroPages.map_WorldPosition_Y] & 0xF0) | zeroPages[0x4A]);

			byte a = romData[ROM.LightWorldEncounterZones.offset + x]; // encouterByte
			CheckValidEncounterZoneAndParse(a);
		}

		private static void CheckValidEncounterZoneAndParse(byte a)
		{
			if (a == 0xFF) // does this ever happen?
				return;
			ParseEncounterTile(a, y);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a">encounterByte</param>
		/// <param name="y"></param>
		private void ParseEncounterZone(byte a, byte y)
		{
			zeroPages[0x4A] = a;
			if (zeroPages[ZeroPages.encounterVariable_A] > 0x01)
				return;
			if (zeroPages[ZeroPages.encounterVariable_A] == 0x01)
			{
				L02E7(y); // never seen
			}
			else
				L02FA();
		}

		/// <summary>
		/// 0x002FA
		/// </summary>
		/// <param name="y"></param>
		private void L02FA()
		{
			byte a = zeroPages[ZeroPages.currentTileType]; // the tile type index currently standing on
			if (a >= 0x08)  // this is out-of-range for the encounter rates so must be special tile
			{ // night time encounter rates?
				if (a == 0x1E)
				{
					L0305();
				}
			}
			else
			{
				// L0307
				zeroPages[0x4F] = a; // current tile
				a = zeroPages[0x4A]; // encounterByte
				GetEncounterZoneMonsterList((byte)(a & 0x3F));
			}
		}

		/// <summary>
		/// 0x0035A
		/// </summary>
		/// <param name="encounterZoneCode"></param>
		private void GetEncounterZoneMonsterList(byte encounterZoneCode)
		{
			zeroPages[0x4B] = encounterZoneCode; // this is top nibble of x pos in low nibble
			if (encounterZoneCode == 0xFF) // can this even happen?
				return;

			byte zeroPageAddress = 0x4B;

			ASMHelper.MultiplyValueAtXByA(zeroPages, 0x0F, zeroPageAddress);

			ASMHelper.IncrementValueAtXBy_AandY(
				zeroPages, romData[ROM.EncounterMonsterListPointer.offset + 0],
				zeroPageAddress, romData[ROM.EncounterMonsterListPointer.offset + 1]);

			int encounterMonstersPointer = zeroPages[zeroPageAddress] + (zeroPages[zeroPageAddress + 1] << 8);
			byte encounterMonsters =
				romData[encounterMonstersPointer - 8000 + Address.iNESHeaderLength];
			if (encounterMonsters != 0)
				GetEncounterRateMultiplier(encounterMonsters);
		}

		/// <summary>
		/// 0x01976
		/// <para>
		/// <br>Character count stored in QuickStorage[0]</br>
		/// <br>Indices stored in QuickStorage[1-4]</br>
		/// </para>
		/// </summary>
		public static void Character_GetCountAndNameIndices()
		{
			byte y = 0;
			byte x = 0;
			while (y != 0x04)
			{
				byte a = saveRam[SRAM.Character_NameIndices + y];
				zeroPages[0x05 + x] = a;
				if (a < 0x80)
					++x;
				++y;
			}
			zeroPages[0x04] = x;
		}

		/// <summary>
		/// 0x01436
		/// <para>Results stored in QuickStorage 0 and 1</para>
		/// </summary>
		public static void Character_GetStatuses()
		{
			byte x = zeroPages[0xCE];
			x <<= 1;
			zeroPages[0x04] = nesRam[NESRAM.Character_Statuses + x + 0];
			zeroPages[0x05] = nesRam[NESRAM.Character_Statuses + x + 1];
		}

		/// <summary>
		/// 0x03F50
		/// </summary>
		public static void ClearRam()
		{
			zeroPages[0x00] = 0;
			zeroPages[0x01] = 0;
			byte y = 0;
			byte x = 0;
			do
			{
				do // clear one page at a time by setting address at 0x00 and 0x01
				{ // skips the stack though
					cpuMemory[zeroPages[0x00] + (zeroPages[0x01] << 8) + y] = 0;
				}
				while (++y != 0);

				x = ++zeroPages[0x01];
				if (x == 0x07)
					break;
				if (x != 0x01)
					continue;
				if (++zeroPages[0x01] != 0)
					continue;

				// does this ever run?
				x = 0x01;
				while (x != 0)
					nesRam[0x6A3E + x++] = 0;
				break;
			} while (true);

			nesRam[NESRAM.PPUControl_2000_Settings] = 0x90;
			nesRam[NESRAM.PPUMask_2001_Settings] = 0x18;
			nesRam[0x06C7] = 0x0E;

			zeroPages[ZeroPages.map_WorldPosition_X] = 0xAC;
			zeroPages[ZeroPages.map_WorldPosition_Y] = 0xDB;

			nesRam[0x06E0] = 0x30;
			zeroPages[0x2D] = 0xFF;
			zeroPages[0x2E] = 0xFF;

			x = 0;
			do
			{
				theStack[x] = 0xFF;
			} while (++x != 0x80);

			x = 0;
			do
			{
				saveRam[0x6D40 + x] = 0xFF;
			} while (++x != 0xA0);
		}
	}
}
