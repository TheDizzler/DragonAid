using System;
using System.Collections.Generic;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.PointerList;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;

namespace ClassAid
{
	public static class Stats
	{
		public static byte[] zeroPage = new byte[0xFF];
		//private static int statFormulasPointer;

		/// <summary>
		/// zeroPage[0x10]
		/// </summary>
		private static byte statIndex
		{
			get { return zeroPage[0x10]; }
			set { zeroPage[0x10] = value; }
		}
		/// <summary>
		/// zeroPage[0x11]
		/// index that character level was found in statPointer.
		/// Is this actually used anywhere?
		/// </summary>
		private static byte characterLevelIndex
		{
			get { return zeroPage[0x11]; }
			set { zeroPage[0x11] = value; }
		}
		/// <summary>
		/// zeroPage[0x13]
		/// </summary>
		private static byte characterLevel
		{
			get { return zeroPage[0x13]; }
			set { zeroPage[0x13] = value; }
		}
		/// <summary>
		/// zeroPage[0x3C] + zeroPage[0x3D] << 8
		/// </summary>
		private static int statPointer
		{
			get { return zeroPage[0x3C] + (zeroPage[0x3D] << 8); }
		}
		/// <summary>
		/// zeroPage[0x3E] + zeroPage[0x3F] << 8
		/// </summary>
		private static int statFormulasPointer
		{
			get { return zeroPage[0x3E] + (zeroPage[0x3F] << 8); }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="romData"></param>
		/// <param name="characterLvl"></param>
		/// <param name="classIndex">0 = Hero 1 = Wizard 2 = Pilgrim 3 = Sage 4 = Soldier 5 = Merchant 6 = Fighter 7 = Goof-off</param>
		/// <returns></returns>
		public static int GetXPForNextLevel(byte[] romData, byte characterLvl, byte classIndex)
		{
			characterLevel = characterLvl;
			statIndex = 0;
			GetStatBaseLine(romData, classIndex);
			return zeroPage[0x04] + (zeroPage[0x05] << 8) + (zeroPage[0x06] << 16);
		}

		/// <summary>
		/// name is not very good.
		/// This is used to parse stat baselines (I believe) and determin XP required for next level.
		/// </summary>
		/// <param name="romData"></param>
		/// <param name="classIndex"></param>
		public static void GetStatBaseLine(byte[] romData, byte classIndex)
		{ //A725
			zeroPage[0x3C] = romData[ROM.CharacterStatPointers.offset + statIndex * 2];
			zeroPage[0x3D] = romData[ROM.CharacterStatPointers.offset + 1 + statIndex * 2];

			//GetCharacterClass(0);
			while (--classIndex < 0x80)
			{ // not hero
				ASMHelper.Add16Bit(0x05, ref zeroPage[0x3C], ref zeroPage[0x3D]);
			}

			if (statIndex != 0) // A74F
			{ // if stat != Experience
				var index = 0;
				if (romData[(statPointer - 8000)] >= 0x80)
					index += 2;
				zeroPage[0x3E] = romData[ROM.CharacterLevelUpPointers.offset + index];
				zeroPage[0x3F] = romData[ROM.CharacterLevelUpPointers.offset + index + 1];
			}
			else
			{ // A747
				zeroPage[0x3E] = romData[ROM.CharacterStatsPointer.offset];
				zeroPage[0x3F] = romData[ROM.CharacterStatsPointer.offset + 1];
			}

			zeroPage[0x12] = 0;
			byte i = 4;
			// should run 5 times
			while (i >= 0)
			{ // A76B
				byte statByte = (byte)(romData[(statPointer - 8000) + i] & 0x80);
				int halves = i;
				while (halves-- >= 0)
				{ // 5x, then 4x, 3x, etc.
					statByte >>= 1;
				}

				zeroPage[0x12] |= statByte;
				--i;
			}

			//			Hero					Soldier
			//	($FF)$80 -> $04 0000 0100	($7F)$00 -> $00	0000 0000
			//	($1C)$00 -> $00 0000 0000	($16)$00 -> $00 0000 0000
			//	($84)$80 -> $10 0001 0000	($88)$80 -> $10 0001 0000
			//	($88)$80 -> $20 0010 0000	($88)$80 -> $40 0010 0000
			//	($81)$80 -> $40 0100 0000	($02)$00 -> $00 0000 0000
			//	$74 0111 0100				$30 0011 0000
			//	$74 >> 2 == $1D				$30 >> 2 == $0C
			zeroPage[0x12] >>= 2;  // XP required for first level

			if (statIndex != 0)
			{ // not Experience
				zeroPage[0x12] &= 0x0F;
			}

			i = 0;
			zeroPage[0x04] = 0;

			while (i != 5)
			{ // A790
				zeroPage[0x04] += (byte)(romData[(statPointer - 8000) + i] & 0x7F);
				if (zeroPage[0x04] == characterLevel)
					break;
				++i;
			}

			characterLevelIndex = i;

			for (i = 0x0E; i >= 4; --i) // A7A8
				zeroPage[i] = 0;

			zeroPage[0x07] = 0x10;
			if (statIndex != 0)
			{ // not experience
				zeroPage[0x04] = zeroPage[0x12];
				--characterLevel;
				return;
			}

			do// A7BE
			{
				i = zeroPage[0x0E];
				byte a = (byte)(romData[(statPointer - 8000) + i] & 0x7F);
				if (a != 0) // A7C4
				{
					zeroPage[0x0F] = a;

					do // A7C8
					{
						byte x = 0x07;
						byte y = 0x0B;
						CalculateExpRequiredForNextLevel(zeroPage[0x12], x, y);
						x = 0x0B;
						y = 0x04;
						F2888(ref x, ref y);

						if (statIndex == 0) // A7DA
						{ // IS experience
							y = zeroPage[0x0E];
							a = romData[(statFormulasPointer - 8000) + y]; // Character_Something_Vector
							x = 0x07;
							y = 0x07;
							CalculateExpRequiredForNextLevel(a, x, y);
						}
						// A7E7
						if (--characterLevel == 0)
							return;
					} while (--zeroPage[0x0F] != 0); // A7ED
				}
			} while (++zeroPage[0x0E] != 0); // A7EF


		}

		private static void F2888(ref byte x, ref byte y)
		{
			zeroPage[y + 0] += zeroPage[x + 0];
			zeroPage[y + 1] += zeroPage[x + 1];
			zeroPage[y + 2] += zeroPage[x + 2];
		}

		/// <summary>
		/// Is it total XP required or difference between levels?
		/// </summary>
		/// <param name="a"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private static void CalculateExpRequiredForNextLevel(byte a, byte x, byte y)
		{
			F282B(a, ref x, ref y);
			zeroPage[0x47] = 4;
			a = x = y;

			while (zeroPage[0x47]-- != 0)
			{
				zeroPage[0x46] = ASMHelper.LSR(zeroPage[0x46], 1, out bool hasCarry);
				zeroPage[x + 2] = ASMHelper.ROR(zeroPage[x + 2], 1, ref hasCarry);
				zeroPage[x + 1] = ASMHelper.ROR(zeroPage[x + 1], 1, ref hasCarry);
				zeroPage[x + 0] = ASMHelper.ROR(zeroPage[x + 0], 1, ref hasCarry);
			}
		}

		/// <summary>
		/// 64 bit arthimatic?
		/// </summary>
		/// <param name="a"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private static void F282B(byte a, ref byte x, ref byte y)
		{
			zeroPage[0x42] = a;
			zeroPage[0x43] = 0;
			zeroPage[0x44] = 0;
			zeroPage[0x45] = 0;
			zeroPage[0x46] = 0;
			zeroPage[0x47] = 0;

			zeroPage[y] = zeroPage[x];
			zeroPage[y + 1] = zeroPage[x + 1];
			zeroPage[y + 2] = zeroPage[x + 2];

			do
			{ // A849
				zeroPage[0x42] = ASMHelper.LSR(zeroPage[0x42], 1, out bool hasCarry);
				if (hasCarry)
				{ // A84D
					zeroPage[0x42] >>= 1;
					zeroPage[0x43] += zeroPage[y];
					zeroPage[0x44] += zeroPage[y + 1];
					zeroPage[0x45] += zeroPage[y + 2];
					zeroPage[0x46] += 47;
				}

				x = y;
				zeroPage[y] = ASMHelper.ASL(zeroPage[y], 1, out hasCarry);
				zeroPage[y + 1] = ASMHelper.ROL(zeroPage[y + 1], 1, ref hasCarry);
				zeroPage[y + 2] = ASMHelper.ROL(zeroPage[y + 2], 1, ref hasCarry);
				zeroPage[0x46] = ASMHelper.ROL(zeroPage[0x46], 1, ref hasCarry);
			} while (zeroPage[0x42] != 0);

			zeroPage[y] = zeroPage[0x43];
			zeroPage[y + 1] = zeroPage[0x44];
			zeroPage[y + 2] = zeroPage[0x45];
		}

		/// <summary>
		///  0 = Hero
		///  1 = Wizard
		///  2 = Pilgrim
		///  3 = Sage
		///  4 = Soldier
		///  5 = Merchant
		///  6 = Fighter
		///  7 = Goof-off
		/// </summary>
		/// <param name="formationIndex"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		private static int GetCharacterClass(int formationIndex)
		{
			// this accesses Save RAM
			return 0;
		}
	}
}
