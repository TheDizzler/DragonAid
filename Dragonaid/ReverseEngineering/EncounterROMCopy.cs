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
			var encounterRateMultiplier = romData[ROM.EncounterRateMultipliers.iNESAddress + x];
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
				a = romData[ROM.EncounterRates.iNESAddress + 0 + encounterRateTileIndex];
			}
			else
			{ // night time rates			
				a = romData[ROM.EncounterRates.iNESAddress + 9 + encounterRateTileIndex];
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
