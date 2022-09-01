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
			if (zeroPages[ZeroPages.mapScrollCheck + 1] == 0
				&& zeroPages[ZeroPages.mapScrollCheck + 0] != 0)
			{
				if (zeroPages[0x8F] != 0)
					return;
				Controller_1_ReadAndSet();
				nesRam[0x06BD] |= zeroPages[ZeroPages.controller1_ButtonStore];
				if ((zeroPages[0x90] & 0x0F) == 0)
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
			zeroPages[0x65] = zeroPages[0x04]; // character count
			zeroPages[0x66] = 0;
			zeroPages[0xCE] = 0;

			while (zeroPages[0xCE] != zeroPages[0x65])
			{
				Bank00000.Character_GetStatuses();
				if (zeroPages[0x05] >= 0x80)
				{
					byte x = zeroPages[0x66];
					zeroPages[0x68 + x] = zeroPages[0xCE];
					++zeroPages[0x66];
				}

				++zeroPages[0xCE];
			}

			zeroPages[0x48] = zeroPages[0x66]; // # characters checked
			zeroPages[0x67] = 0;
			zeroPages[0x62] = 0;
			zeroPages[0xD0] = 0;
		}

		/// <summary>
		/// 0x3A2C5
		/// </summary>
		/// <param name="v"></param>
		public static void Menu_CloseAllMenus(byte a)
		{
			theStack.Push(zeroPages[0x80]);
			theStack.Push(zeroPages[0x81]);

			Menu_CloseAllMenus_sub_A(a);
			Menu_CloseAllMenus_Loop();

			zeroPages[0x81] = theStack.Pop();
			zeroPages[0x80] = theStack.Pop();
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
			zeroPages[ZeroPages.menu_PointerIndex] = a;
			a >>= 1;
			byte x = a;
			a = romData[ROM.Menu_CloseAllMenus_Vector.offset + x];
			zeroPages[0x7F] = (byte)(a << 4);
			zeroPages[0x82] = a;
			nesRam[NESRAM.menu_WriteDimensions] = (byte)((a & 0x0F) | 0x10);
			nesRam[NESRAM.menu_PositionA] = romData[ROM.Menu_CloseAllMenus_Vector.offset + x + 1];
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
				if (--zeroPages[0x7F] == 0)
					break;

				nesRam[NESRAM.menu_PositionA] -= 10;
			}
			// Menu_CloseAllMenus_Loop_break
		}

		/// <summary>
		/// 0x3A38D
		/// </summary>
		private static void F3A38D()
		{
			F3A5DD();
			byte a = zeroPages[ZeroPages.menu_PointerIndex];
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
		/// <br>Sets 64 bytes of NESRAM.menu_WriteBlock to 0xFF.</br></para>
		/// </summary>
		private static void Menu_PointerIndex_Other()
		{
			byte x = 0;
			while (x != 0x40)
			{
				nesRam[NESRAM.menu_WriteBlock + x++] = 0xFF;
			}

			byte a = nesRam[NESRAM.menu_PositionA];
			a >>= 4;
			a += zeroPages[0x75];
			zeroPages[0x11] = (byte)(a - 0x07);

			a = nesRam[NESRAM.menu_PositionA];
			a &= 0x0F;
			a += zeroPages[0x74];
			zeroPages[0x10] = (byte)(a - 0x08);

			a = nesRam[NESRAM.menu_WriteDimensions];
			a &= 0x0F;
			zeroPages[0x06] = a;

			a <<= 1;
			zeroPages[0x07] = (byte)(a - 0x01);

			zeroPages[0x04 + 1] = 0;

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
			zeroPages[0x74] = zeroPages[ZeroPages.map_WorldPosition_X];
			zeroPages[0x75] = zeroPages[ZeroPages.map_WorldPosition_Y];
			if ((zeroPages[0x2F] & 0x01) == 0)
				return;
			zeroPages[0x74] = zeroPages[0x30];
			zeroPages[0x75] = zeroPages[0x31];
		}
	}
}
