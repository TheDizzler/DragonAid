using System;
using System.Collections.Generic;
using AtomosZ.DragonAid.Libraries;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static AtomosZ.DragonAid.ReverseEngineering.ROMPlaceholders;

namespace AtomosZ.DragonAid.ReverseEngineering
{
	internal class Bank38000
	{
		/// <summary>
		/// 0x38072
		/// </summary>
		public static void ReadControllerInput()
		{
			if (zeroPage[ZeroPage.mapScrollCheck + 1] == 0
				&& zeroPage[ZeroPage.mapScrollCheck + 0] != 0)
			{
				if (zeroPage[0x8F] != 0)
					return;
				Controller_1_ReadAndSet();
				nesRam[0x06BD] |= zeroPage[ZeroPage.controller1_ButtonStore];
				if ((zeroPage[0x90] & 0x0F) == 0)
					ReadControllerInput_Sub_A();
			}
			else
				ReadControllerInput_cont();
		}

		private static void ReadControllerInput_cont()
		{
			if (nesRam[NESRAM.encounterCheckRequired_A] == 0)
				return;
			...
		}

		/// <summary>
		/// 0x3B118
		/// </summary>
		public static void DynamicSubroutine_38000_D()
		{
			AnyCharacterStatus1_Bit6_Set();
			...
		}

		/// <summary>
		/// 0x3B16A
		/// </summary>
		private static void AnyCharacterStatus1_Bit6_Set()
		{
			if (saveRam[SRAM.encounterCheckRequired_b] >= 0x80)
				return;
			if ((saveRam[SRAM.encounterCheckRequired_c] & 0x90) == 0x80)
				return;
			byte a = (byte)(nesRam[NESRAM.Character_Statuses + 1]
				| nesRam[NESRAM.Character_Statuses + 3]
				| nesRam[NESRAM.Character_Statuses + 5]
				| nesRam[NESRAM.Character_Statuses + 7]);
			a &= 0x40;
			if (a != 0)
			{
				Bank00000.Character_GetCountAndNameIndices();
				...
			}
			else
			{ // L3B1B7 // status normal?
				GetCharacterStatuses_something();
				CheckForScreenFlash();
			}
		}

		/// <summary>
		/// 0x3B1BD
		/// </summary>
		private static void GetCharacterStatuses_something()
		{
			Bank00000.Character_GetCountAndNameIndices();
			zeroPage[0x65] = zeroPage[0x04]; // character count
			zeroPage[0x66] = 0;
			zeroPage[0xCE] = 0;

			while (zeroPage[0xCE] != zeroPage[0x65])
			{
				Bank00000.Character_GetStatuses();
				if (zeroPage[0x05] >= 0x80)
				{
					byte x = zeroPage[0x66];
					zeroPage[0x68 + x] = zeroPage[0xCE];
					++zeroPage[0x66];
				}

				++zeroPage[0xCE];
			}

			zeroPage[0x48] = zeroPage[0x66]; // # characters checked
			zeroPage[0x67] = 0;
			zeroPage[0x62] = 0;
			zeroPage[0xD0] = 0;
		}

		/// <summary>
		/// 0x3A2C5
		/// </summary>
		/// <param name="v"></param>
		public static void Menu_CloseAllMenus(byte a)
		{
			theStack.Push(zeroPage[0x80]);
			theStack.Push(zeroPage[0x81]);

			Menu_CloseAllMenus_sub_A(a);
			Menu_CloseAllMenus_Loop();

			zeroPage[0x81] = theStack.Pop();
			zeroPage[0x80] = theStack.Pop();
		}

		/// <summary>
		/// 0x3A325
		/// <para>sets 0x7F, 0x82, NESRAM.menu_WriteDimensions, 
		/// NESRAM.menu_PositionA from ROM.Menu_CloseAllMenus_Vector.offset</para>
		/// </summary>
		/// <param name="a">sets ZeroPages.menu_PointerIndex and 1/2 used 
		/// as index to ROM.Menu_CloseAllMenus_Vector.offset</param>
		private static void Menu_CloseAllMenus_sub_A(byte a)
		{
			zeroPage[ZeroPage.menu_PointerIndex] = a;
			a >>= 1;
			byte x = a;
			a = romData[ROM.Menu_CloseAllMenus_Vector.iNESAddress + x];
			zeroPage[0x7F] = (byte)(a << 4);
			zeroPage[0x82] = a;
			nesRam[NESRAM.menu_WriteDimensions_471] = (byte)((a & 0x0F) | 0x10);
			nesRam[NESRAM.menu_PositionA_470] = romData[ROM.Menu_CloseAllMenus_Vector.iNESAddress + x + 1];
		}

		/// <summary>
		/// 0x3A2DA
		/// </summary>
		private static void Menu_CloseAllMenus_Loop()
		{
			while (true)
			{
				F3A38D();
				F39D86();
				if (--zeroPage[0x7F] == 0)
					break;

				nesRam[NESRAM.menu_PositionA_470] -= 10;
			}
			// Menu_CloseAllMenus_Loop_break
		}

		/// <summary>
		/// 0x3A38D
		/// </summary>
		private static void F3A38D()
		{
			F3A5DD();
			byte a = zeroPage[ZeroPage.menu_PointerIndex];
			switch (a)
			{
				case 0x01:
				case 0x0E:
					Menu_PointerIndex_01_0E();
					break;
				case 0x02:
					Menu_PointerIndex_02();
					break;
				case 0x08:
					Menu_PointerIndex_08();
					break;
				case 0x13:
					Menu_PointerIndex_13();
					break;
				default:
					Menu_PointerIndex_Other();
					break;
			}
		}

		/// <summary>
		/// 0x3A472
		/// <para>All menu pointers except 0x01, 0x02, 0x08, 0x0E, 0x13.
		/// Sets 64 bytes of NESRAM.menu_WriteBlock to 0xFF.<br/></para>
		/// </summary>
		private static void Menu_PointerIndex_Other()
		{
			byte x = 0;
			while (x != 0x40)
			{
				nesRam[NESRAM.PPU_WriteBlock_400 + x++] = 0xFF;
			}

			byte a = nesRam[NESRAM.menu_PositionA_470];
			a >>= 4;
			a += zeroPage[0x75];
			zeroPage[0x11] = (byte)(a - 0x07);

			a = nesRam[NESRAM.menu_PositionA_470];
			a &= 0x0F;
			a += zeroPage[0x74];
			zeroPage[0x10] = (byte)(a - 0x08);

			a = nesRam[NESRAM.menu_WriteDimensions_471];
			a &= 0x0F;
			zeroPage[0x06] = a;

			a <<= 1;
			zeroPage[0x07] = (byte)(a - 0x01);

			zeroPage[0x04 + 1] = 0;

			while ()
			{
				Bank3C000.F3EC99(0x10, 0x11);
			}
		}

		/// <summary>
		/// 0xF3A5DD
		/// <para>sets zeroPages[0x74] and zeroPages[0x75] to world map pos 
		/// or zeroPages[0x30] and zeroPages[0x31]</para>
		/// </summary>
		private static void F3A5DD()
		{
			zeroPage[0x74] = zeroPage[ZeroPage.map_WorldPosition_X];
			zeroPage[0x75] = zeroPage[ZeroPage.map_WorldPosition_Y];
			if ((zeroPage[0x2F] & 0x01) == 0)
				return;
			zeroPage[0x74] = zeroPage[0x30];
			zeroPage[0x75] = zeroPage[0x31];
		}
	}
}
