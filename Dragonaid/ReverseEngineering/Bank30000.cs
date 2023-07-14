using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static AtomosZ.DragonAid.ReverseEngineering.ROMPlaceholders;

namespace AtomosZ.DragonAid.ReverseEngineering
{
	internal class Bank30000
	{
		/// <summary>
		/// 0757 - 0x30078
		/// </summary>
		public static void DynamicPointer_30000_A()
		{
			byte a = nesRam[NESRAM.encounterCheckRequired_A];
			if (a != 0 && a < 0x40)
				DynamicPointer_30000_A_Sub();
		}





		/// <summary>
		/// 07C8 - 0x32F95
		/// </summary>
		public static void Map_Scroll_Finish()
		{
			if (zeroPage[ZeroPage.encounterVariable_A] == 0)
			{
				F33041();
				F33080();
				F330B0();
				F3301C();
				F330FA();
			}
			// _L32FA9
			byte a = zeroPage[0x9A];
			if (a == 0)
			{
				CheckSpecialCoordinates();
				return;
			}
			// _L32FB3_ When does this run?
			byte x = 0;
			a = zeroPage[0x8B];
			while (x != 0x17)
			{
				if (a == romData[ROM.Map_Scroll_Finish_Vector.iNESAddress + x])
				{
					F32DFE();
					return;
				}

				++x;
			}
		}

		/// <summary>
		/// 0x33124
		/// <para>
		/// x: 56 y: 48<br/>
		/// this COULD be the area that pushes you back without the locket of love or whatever<br/>
		/// </para>
		/// </summary>
		private static void CheckSpecialCoordinates()
		{
			if ((saveRam[0x60B6] & 0x40) != 0)
				return;
			if (zeroPage[ZeroPage.map_WorldPosition_X] != 0x56)
				return;
			if (zeroPage[ZeroPage.map_WorldPosition_Y] != 0x48)
				return;
		}

		private static void F33041()
		{
			if (zeroPage[ZeroPage.encounterVariable_A] == 0)
				return;
			if ((zeroPage[0xAC] & 0x1F) == 0)
				return;
			...
		}

		/// <summary>
		/// 0x33080
		/// </summary>
		private static void F33080()
		{
			Bank00000.Character_GetCountAndNameIndices();
			zeroPage[0x3C] = zeroPage[0x04]; // character count
			Init_Variables__D0_D1_3D_3E();
			while (zeroPage[0x3C] != zeroPage[0x3D])
			{
				byte a = GetCharacterStatus();
				if (a >= 0x80)
				{
					byte y = zeroPage[0x3D];
					a = zeroPage[ZeroPage.characterStatusRelated_Vector + y];
					a &= 0x02;
					if (a != 0)
					{
						zeroPage[ZeroPage.character_FormationIndex] = y;
						SaveRAM.S06B39();
					}
				}
				// _L3309D_
				++zeroPage[0x3D];
			}
		}

		/// <summary>
		/// 0x330A6
		/// <para>index of character in 0x3E (2 bytes per index)</para>
		/// </summary>
		/// <returns>2nd byte of character status</returns>
		private static byte GetCharacterStatus()
		{
			byte x = zeroPage[0x3D];
			zeroPage[0x3D] += 0x02;
			return nesRam[NESRAM.Character_Statuses + x + 1];
		}

		/// <summary>
		/// 0x330EB
		/// <para>
		/// Sets D0, D1, 3D, 3E to 0.<br/>
		/// Sets CF to 1.<br/>
		/// </para>
		/// </summary>
		private static void Init_Variables__D0_D1_3D_3E()
		{
			zeroPage[ZeroPage.Item_Check_IsEquipped] = 0x01;
			zeroPage[0xD0] = 0;
			zeroPage[0xD1] = 0;
			zeroPage[0x3D] = 0;
			zeroPage[0x3E] = 0;
		}

		/// <summary>
		/// 0x330B0
		/// </summary>
		private static void F330B0()
		{
			if (zeroPage[0x9A] != 0 || zeroPage[ZeroPage.encounterVariable_A] != 0)
				return;
			Bank00000.Character_GetCountAndNameIndices();
			zeroPage[0x3C] = zeroPage[0x04];
			Init_Variables__D0_D1_3D_3E();
			zeroPage[0x3D] = 0;
			do
			{ // __CheckCharacterStatusB_Loop_
				byte a = GetCharacterStatus();
				if (a >= 0x80)
				{
					byte y = zeroPage[0x3F];
					a = zeroPage[ZeroPage.characterStatusRelated_Vector + y];
					a &= 0x04;
					if (a != 0x04)
					{
						zeroPage[ZeroPage.character_FormationIndex] = y;
						LoadDynamicSubroutine_Character_AddXP();
						...
					}
				}
				// _L330DB_
				++zeroPage[0x3F];
			} while (zeroPage[0x3F] != zeroPage[0x3C]);

			if (zeroPage[0x3D] == 0)
				return;

			Bank00000.DynamicSubroutine_00000_B();
		}

		/// <summary>
		/// 0x3301C
		/// </summary>
		private static void F3301C()
		{
			if (zeroPage[ZeroPage.encounterVariable_A] != 0)
				return;
			byte a = saveRam[0x60C9];
			a &= 0x3F;
			if (a == 0)
				return;
			a = --saveRam[0x60C9];
			a &= 0x3F;
			if (a != 0)
				return;
			Bank3C000.WaitForNMI_3C000();
			...
		}

		/// <summary>
		/// 0x330FA
		/// </summary>
		private static void F330FA()
		{
			byte a;
			Bank00000.Character_GetCountAndNameIndices();
			byte y = 0;
			while (y != zeroPage[0x04])
			{
				byte x = zeroPage[0x93 + y];
				x &= 0x1F;
				a = saveRam[0x6DE0 + x];
				if (a == 0x05 || a == 0x06)
					return;
				++y;
			}

			a = zeroPage[0xAC];
			if ((a & 0xA0) != 0xA0)
				return;
			zeroPage[0xAC] &= 0x7F;
		}
	}
}
