using System;
using System.Collections.Generic;
using System.Text;
using AtomosZ.DragonAid.Libraries;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;

namespace AtomosZ.DragonAid.EncounterAid
{
	internal class EncounterROMCopy
	{
		/// <summary>
		/// registers that need to be set before this subroutine:
		/// zeroPages[0x2C]
		/// zeroPages[0x2F] day/night?
		/// zeroPages[0x92]
		/// </summary>
		public byte[] zeroPages = new byte[0x100];
		private byte[] romData;
		private bool isGoldenClawEquipped;

		private Address dynamicSubroutine
		{
			get { return new Address("Dynamic Subroutine", zeroPages[0x21] + (zeroPages[0x22] << 8)); }
		}




		/// <summary>
		/// zeroPages[0x4A]
		/// </summary>
		private byte encounterTile
		{
			get { return zeroPages[0x4A]; }
			set { zeroPages[0x4A] = value; }
		}

		/// <summary>
		/// zeroPages[0x4F]
		/// </summary>
		private byte encounterRateTileIndex
		{
			get { return zeroPages[0x4F]; }
			set { zeroPages[0x4F] = value; }
		}

		/// <summary>
		/// zeroPages[0x92]
		/// </summary>
		private byte currentTileType
		{
			get { return zeroPages[0x92]; }
			set { zeroPages[0x92] = value; }
		}

		/// <summary>
		/// registers that need to be set before this subroutine:
		/// zeroPages[0x2C]
		/// zeroPages[0x2F] day/night?
		/// zeroPages[0x92] currentTileType
		/// _06DF time of day?
		/// </summary>
		/// <param name="romData"></param>
		/// <param name="xMapPos"></param>
		/// <param name="yMapPos"></param>
		/// <param name="y">This is used when 0x2C == 0x01</param>
		public void CheckForEncounterLightWorld(byte[] romData, byte xMapPos, byte yMapPos, bool isGoldenClawEquipped, byte y)
		{ // 82C3
			this.romData = romData;
			this.isGoldenClawEquipped = isGoldenClawEquipped;
			encounterTile = (byte)(xMapPos >> 4); // top 4 bits in low nibble
			byte x = (byte)((yMapPos & 0xF0) | encounterTile);

			byte encounterByte = romData[ROM.LightWorldEncounterZones.offset + x];
			if (encounterByte == 0xFF) // does this ever happen?
				return;
			ParseEncounterTile(encounterByte, y);
		}

		private void ParseEncounterTile(byte encounterByte, byte y)
		{
			encounterTile = encounterByte;
			if (zeroPages[0x2C] > 0x01)
				return;
			if (zeroPages[0x2C] == 0x01)
			{
				L02E7(y); // never seen
			}
			else
				L02FA();
		}

		private void L02E7(byte y)
		{
			byte a = 0;
			encounterRateTileIndex = 0;
			bool carrySet = false;
			ASMHelper.ROL(encounterTile, 1, ref carrySet);
			ASMHelper.ROL(a, 1, ref carrySet);
			ASMHelper.ROL(encounterTile, 1, ref carrySet);
			ASMHelper.ROL(a, 1, ref carrySet);
			carrySet = false;
			encounterTile = ASMHelper.ADC(a, y, ref carrySet);
			//L035A();
		}

		private void L02FA()
		{ // 82FA
			byte a = currentTileType; // the tile type index currently standing on
			if (a >= 0x08)  // this is out-of-range for the encounter rates so must be special tile
			{ // night time encounter rates?
				if (a == 0x1E)
				{
					//L0305();
				}
			}
			else
			{
				// L0307
				encounterRateTileIndex = a;
				GetEncounterZoneMonsterList((byte)(encounterTile & 0x3F));
			}
		}


		private void GetEncounterZoneMonsterList(byte encounterZoneCode)
		{
			zeroPages[0x4B] = encounterZoneCode; // this is top nibble of x pos in low nibble
			if (encounterZoneCode == 0xFF) // can this even happen? (because & 0x3F)
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

		private void GetEncounterRateMultiplier(byte a)
		{
			encounterTile = a;
			if (zeroPages[0x5E] != 0)
			{ // automatic encounter?
			  //EncounterRolled();
				return;
			}

			a >>= 5;
			byte x = a;
			var encounterRateMultiplier = romData[ROM.EncounterRateMultipliers.offset + x];
			if (x >= 0x03)
			{
				Map_GetEncounterRate(encounterRateMultiplier);
			}
			else
			{ // high encounter rate area?
				zeroPages[0x4D] = encounterRateMultiplier;
				ASMHelper.RNG();
			}
		}

		private void Map_GetEncounterRate(byte a)
		{ // 83AE
			zeroPages[0x4D] = a;
			zeroPages[0x4E] = 0;
			if (zeroPages[0x2F] == 0 /*&& _06DF >= UniversalConsts.NightBattleStartTime*/)
			{ // day time rates
				a = romData[ROM.EncounterRates.offset + 0 + encounterRateTileIndex];
			}
			else
			{ // night time rates			
				a = romData[ROM.EncounterRates.offset + 9 + encounterRateTileIndex];
			}
			// 83C9
			ASMHelper.MultiplyValueAtXByA(zeroPages, a, 0x4D);
			if (isGoldenClawEquipped || (zeroPages[0x4E] != 0 || zeroPages[0x4D] >= 0x64))
			{
				zeroPages[0x4D] = 0x64;
			}

			// RollForEncounter
			a = ASMHelper.RNG();
			if (a >= zeroPages[0x4D])
				return;
		}
	}
}
