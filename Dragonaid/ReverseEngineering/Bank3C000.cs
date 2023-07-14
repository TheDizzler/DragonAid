using System;
using System.Collections.Generic;
using System.Net;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.PointerList;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static AtomosZ.DragonAid.ReverseEngineering.ROMPlaceholders;


namespace AtomosZ.DragonAid.ReverseEngineering
{
	/// <summary>
	/// This bank or Bank7C000 is always set to $C000-$FFFF
	/// </summary>
	public static class Bank3C000
	{
		/// <summary>
		/// 0x3C000
		/// </summary>
		public static void ResetNMI_ClearData()
		{
			// resets a bunch of variables
		}

		/// <summary>
		/// $3C5FF
		/// </summary>
		/// <param name="offset"></param>
		/// <param name="pointerAddress"></param>
		/// <param name="bankId"></param>
		/// <returns></returns>
		public static byte GetVariable_FromDynamic(byte offset, byte pointerAddress, byte bankId)
		{
			var variableAddress = GetPointerAt(romData, bankId, pointerAddress);
			return romData[variableAddress];
		}

		/// <summary>
		/// 0x3CC5C. Does not run when a menu is open (looping somewhere else, probably)
		/// </summary>
		public static void BottomOfStack()
		{
			while (true)
				GameLoop();
		}

		private static void GameLoop()
		{
			Bank38000.ReadControllerInput();
			F3CE52();
			F3CE1A();
			Bank34000.DynamicSubroutine_34000_D();
			Map_ScrollCheck();
			Bank30000.DynamicPointer_30000_A();
			F3CDCF();
			SetPPUScroll_And_MapScrollCheck();
			Bank34000.AdjustSpriteDMA();
			Bank34000.DynamicSubroutine_34000_C();
			F3CDC0();
			F3F116();
			WaitForNMI_3C000();
			F3D5AB();
			Bank28000.DynamicSubroutine_28000_B();
			++zeroPage[0x90];
		}

		/// <summary>
		/// 3CD89
		/// </summary>
		private static void Map_ScrollCheck()
		{
			byte a = zeroPage[ZeroPage.mapScrollCheck + 1];
			if (a == 0x10 || a == 0x08)
				Map_Scroll_HorizontalCheck();
			else if (a == 0x0F || a == 0x07)
				Map_Scroll_Vertical_Check();
		}

		/// <summary>
		/// 0x3CDA8
		/// </summary>
		private static void Map_Scroll_Vertical_Check()
		{
			byte a = nesRam[NESRAM.walkDirection];
			if (a == 0x01)
				Map_Scroll_RightCheck();
		}

		/// <summary>
		/// 0x3CD9C
		/// </summary>
		private static void Map_Scroll_HorizontalCheck()
		{
			byte a = nesRam[NESRAM.walkDirection];
			if (a == 0x03)
				Map_Scroll_LeftCheck();
			else if ((a &= 0x01) == 0)
				Map_Scroll_RightCheck();
		}

		/// <summary>
		/// 3CDB0
		/// </summary>
		private static void Map_Scroll_LeftCheck()
		{
			if (zeroPage[ZeroPage.mapScrollCheck + 1] != 0x08)
				return;
			Map_Scroll_RightCheck();
		}

		/// <summary>
		/// 3CDB6
		/// </summary>
		private static void Map_Scroll_RightCheck()
		{
			if (zeroPage[0x9A] != 0)
				L3F517();
			else
				L3F3A6();
		}

		/// <summary>
		/// 0x3F3A6
		/// </summary>
		private static void L3F3A6()
		{
			F3F3CE();
			if ((zeroPage[0x2F] & 0x02) != 0)
				return;
			Set_3C_to_3E(out byte x, out byte y);
			F3F3B8(x, y);
			F3F305(out x, out y);
			F3F3B8(x, y);
		}

		/// <summary>
		/// 0x3F305
		/// </summary>
		private static void F3F305(out byte x, out byte y)
		{
			zeroPage[0x3C] = 0x05;
			x = 0x18;
			if (zeroPage[ZeroPage.encounterVariable_A] == 0x02)
				x = 0;
			zeroPage[0x3D] = x;
			x = zeroPage[0x9E];
			y = zeroPage[0x9F];
			zeroPage[0x3E] = zeroPage[0xA0];
		}

		/// <summary>
		/// 0x3F3B8
		/// </summary>
		private static void F3F3B8(byte x, byte y)
		{
			zeroPage[0x80] = x;
			zeroPage[0x81] = y;
			zeroPage[0x78] = (byte)(zeroPage[0x9D] & 0x03);
			zeroPage[0x79] = zeroPage[0x3C];
			nesRam[0x06BB] = zeroPage[0x3D];
			L3F3E4(x, y);
		}

		/// <summary>
		/// 0x3F2F4
		/// </summary>
		private static void Set_3C_to_3E(out byte x, out byte y)
		{
			x = zeroPage[0x9B];
			y = zeroPage[0x9C];
			zeroPage[0x3C] = 0x04;
			zeroPage[0x3D] = 0x14;
			zeroPage[0x3E] = zeroPage[0x9D];
		}

		/// <summary>
		/// 0x3F3CE
		/// </summary>
		private static void F3F3CE()
		{
			byte x = zeroPage[0x80] = zeroPage[0x2D];
			byte y = zeroPage[0x81] = zeroPage[0x2E];
			nesRam[0x06BA] = zeroPage[0x78];
			zeroPage[0x79] = 0x04;
			nesRam[0x06BB] = 0x10;
			L3F3E4(x, y);
		}

		/// <summary>
		/// 0x3F3E4
		/// </summary>
		private static void L3F3E4(byte x, byte y)
		{
			if (!F3F832(x, y))
				return;
			...
		}

		/// <summary>
		/// 0x3F832
		/// </summary>
		private static bool F3F832(byte x, byte y)
		{
			zeroPage[0x06] = zeroPage[ZeroPage.map_WorldPosition_X];
			zeroPage[0x07] = zeroPage[ZeroPage.map_WorldPosition_Y];

			zeroPage[0x04] = x;
			zeroPage[0x05] = y;

			byte a = nesRam[NESRAM.walkDirection];
			if (a == 0)
			{
				return WalkDirection_Up();
			}

			if (a == 0x01)
			{
				return WalkDirection_Right();
			}

			if (a == 0x03)
			{
				return WalkDirection_Left();
			}

			// WalkDirection_Down
			a = zeroPage[0x07];
			a += 0x07;
			if (a == zeroPage[0x05])
				return L3F877();

			return false;
		}

		/// <summary>
		/// 0x3F86C
		/// </summary>
		/// <returns></returns>
		private static bool WalkDirection_Left()
		{
			byte a = (byte)(zeroPage[0x06] - 0x08);
			if (a == zeroPage[0x04])
				return L3F877();
			return false;
		}

		/// <summary>
		/// 0x3CE1A
		/// </summary>
		private static void F3CE1A()
		{
			if (zeroPage[0x8F] < 0x02)
				return;
			Bank38000.3DynamicSubroutine_38000_F();
		}

		/// <summary>
		/// 0x03CE52
		/// </summary>
		private static void F3CE52()
		{
			byte a = (byte)(zeroPage[0x90] & 0x0F);
			if (a != 0)
				return;
			a = zeroPage[ZeroPage.mapScrollCheck + 1];
			if (a != 0)
				return;

			byte y = 0x03;
			while (true)
			{
				a = theStack[0x10 + y];
				if (a == 0xFF)
				{
					F3CE52_cont();
					return;
				}
				else
				{
					a &= 0x40;
					if (a != 0)
						return;
					y += 4;
				}
			}
		}


		private static void F3CE52_cont()
		{
			if (zeroPage[ZeroPage.mapScrollCheck] != 0)
			{ // L3CE95();
				if (zeroPage[0x8F] != 0)
					return;
			}
			else
			{
				++saveRam[0x6ACE];
				Bank30000.Map_Scroll_Finish();
				F3D049();
				if (zeroPage[0x8F] != 0)
					return;
				if (zeroPage[ZeroPage.encounterVariable_A] != 0x02)
					Bank00000.Map_CheckForEncounter();
			}
			ReadControllerInput();
		}

		/// <summary>
		/// 0x3CED8
		/// </summary>
		private static void ReadControllerInput()
		{
			F3CE25();
			Controllers_ReadAndSet();
			if (zeroPage[ZeroPage.encounterVariable_A] == 0x02)
			{
				byte a = (byte)(zeroPage[ZeroPage.controller1_ButtonStore] & 0x01);
				if (a == 0)
					L3CEEE();
				else
					nesRam[0x06BD] = a;
			}
			else
			{ // L3CF04
				if (zeroPage[ZeroPage.encounterVariable_A] == 0
					|| zeroPage[ZeroPage.encounterVariable_A] >= 0x80)
					L3CF53();
				else if ((nesRam[0x06E2] & 0x20) != 0) // it might be imperative that this is checked before below check
					L3CF1A();
				else if ((nesRam[0x06E2] & 0x80) == 0)
					L3CF53();
			}
		}

		/// <summary>
		/// 0X3CF53
		/// </summary>
		private static void L3CF53()
		{
			byte a = zeroPage[0x9A];
			if (a != 0x1B)
				L3CFA2(a);
			else
			{
				...
			}
		}

		private static void L3CFA2(byte a)
		{
			ASMHelper.BIT(a, saveRam[0x60C6], out bool n, out bool v, out bool z);
			if (v)
				Controller_ReadButtonPresses();
			else
			{
				...
			}
		}

		/// <summary>
		/// 0x3CFBB
		/// </summary>
		private static void Controller_ReadButtonPresses()
		{
			zeroPage[0x3C] = zeroPage[0x30];
			zeroPage[0x3D] = zeroPage[0x31];
			zeroPage[ZeroPage.controller1_ButtonStore] = ASMHelper.ASL(
				zeroPage[ZeroPage.controller1_ButtonStore], 1, out bool hasCarry);
			if (hasCarry)
			{
				Controller_RightPushed();
				return;
			}
			zeroPage[ZeroPage.controller1_ButtonStore] = ASMHelper.ASL(
				zeroPage[ZeroPage.controller1_ButtonStore], 1, out hasCarry);
			if (hasCarry)
			{
				Controller_LeftPushed();
				return;
			}
			zeroPage[ZeroPage.controller1_ButtonStore] = ASMHelper.ASL(
				zeroPage[ZeroPage.controller1_ButtonStore], 1, out hasCarry);
			if (hasCarry)
			{
				Controller_DownPushed();
				return;
			}
			zeroPage[ZeroPage.controller1_ButtonStore] = ASMHelper.ASL(
				zeroPage[ZeroPage.controller1_ButtonStore], 1, out hasCarry);
			if (hasCarry)
			{
				Controller_UpPushed();
				return;
			}

			if (zeroPage[ZeroPage.encounterVariable_A] != 0
				|| zeroPage[0x8E] >= 0x80)
			{
				return;
			}

			zeroPage[0x8E] += 0x10;
			if (zeroPage[0x8E] < 0x80)
				return;

			Bank38000.Clear_0647to0664();
		}

		/// <summary>
		/// 0x3D601
		/// </summary>
		private static void Controller_UpPushed()
		{
			Controller_DirectionPushed();
			if (zeroPage[0x8E] >= 0x80)
			{
				zeroPage[0x8E] = 0;
				Bank38000.Menu_CloseAllMenus(0x04);
			}
			// L3D7B3
		}

		/// <summary>
		/// 0x3D7A4
		/// </summary>
		private static void Controller_DirectionPushed()
		{

		}

		/// <summary>
		/// 0x3CB53
		/// </summary>
		private static void Controllers_ReadAndSet()
		{
			Controller_ReadAndConfirm(0); // controller 1
			Controller_ReadAndConfirm(0x01); // controller 2
		}

		/// <summary>
		/// 0x3CB5A
		/// </summary>
		/// <param name="x">controller 1 == 0, controller 2 == 1</param>
		private static void Controller_ReadAndConfirm(byte x)
		{
			Controller_Read_3C000(x);

			// check for error (because of DCM?)

			byte y = 0;
			if (zeroPage[ZeroPage.controllerInputStore] != 0)
			{
				y = zeroPage[0x18 + x];
				if (y == 0)
					y = 0x08;
				if (--y == 0)
					zeroPage[0x16 + x] = y;
				y = 0x0B;
			}
			// _Controller_ReadAndConfirm_Finish_
			zeroPage[0x18 + x] = y;
			zeroPage[0x16 + x] &= zeroPage[ZeroPage.controllerInputStore];
			zeroPage[0x16 + x] ^= zeroPage[ZeroPage.controllerInputStore];
		}

		/// <summary>
		/// 0x3CB86
		/// <para>results stored in zeroPages[ZeroPages.controllerInputStore]</para>
		/// </summary>
		/// <param name="x">controller 1 == 0, controller 2 == 1</param>
		private static void Controller_Read_3C000(byte x)
		{
			registers[Registers.Ctrl1] = 0x01;
			registers[Registers.Ctrl1] = 0x00; // tell controllers to latch buttons

			byte y = 0x08;

			while (y-- > 0)
			{
				byte a = registers[Registers.Ctrl1 + x];
				a = ASMHelper.LSR(a, 1, out bool hasCarry);
				if (!hasCarry)
					a = ASMHelper.LSR(a, 1, out hasCarry); // what does this accomplish?
				zeroPage[ZeroPage.controllerInputStore]
					= ASMHelper.ROR(zeroPage[ZeroPage.controllerInputStore], 1, ref hasCarry);
			}
		}

		/// <summary>
		/// 0x3CE25
		/// </summary>
		private static void F3CE25()
		{
			if (zeroPage[ZeroPage.encounterVariable_A] == 0)
				return;
			...
		}

		/// <summary>
		/// 0x3D049
		/// </summary>
		private static void F3D049()
		{
			zeroPage[ZeroPage.mapScrollCheck] = 0xFF;
			nesRam[0x06BC] = nesRam[NESRAM.walkDirection];
			if (zeroPage[0x9A] == 0)
			{
				IncrementClock();
				return;
			}
			...
		}

		/// <summary>
		/// 0x3D022
		/// </summary>
		private static void IncrementClock()
		{
			if ((zeroPage[0x2F] & 0x02) != 0)
				return; // no day/night cycle in dark world
			byte a = zeroPage[ZeroPage.currentRNGSeed];
			a &= 0x1F;
			a += 0xDF;
			bool hasCarry = false;
			nesRam[NESRAM.timeSubValue] = ASMHelper.ADC(a, nesRam[NESRAM.timeSubValue], ref hasCarry);
			if (hasCarry)
			{
				if (++nesRam[NESRAM.timeOfDay] >= UniversalConsts.DayTimer)
					nesRam[NESRAM.timeOfDay] = 0;
			}

			// 3D22D PostIncrementClock
			if (zeroPage[0x8F] != 0)
			{
				Bank38000.DynamicSubroutine_38000_E();
				return;
			}

			if (zeroPage[ZeroPage.encounterVariable_A] == 0x02)
				return;
			zeroPage[0x74] = zeroPage[ZeroPage.map_WorldPosition_X];
			zeroPage[0x75] = zeroPage[ZeroPage.map_WorldPosition_Y];
			a = zeroPage[ZeroPage.currentTileType];
			byte x = 0x0F;
			if (a == 0x0A || a == 0x0B)
				TileType_0A_0B();
			else if (a == 0x0C || a == 0x0D)
				TileType_0C_0D();
			else if (a == 0x11 || a == 0x15)
				TileType_11_15();
			else if (a == 0x0E || a == 0x0F || a == 0x13)
				TileType_0E_0F_13();
			else
			{
				if (F3D92E())
					L3D2B0();
				else if (zeroPage[ZeroPage.map_WorldPosition_X] != zeroPage[0x9B]
					|| zeroPage[0x2F] != 0)
					Bank38000.DynamicSubroutine_38000_D();
				else
					F3D342();
			}
		}

		/// <summary>
		/// 0x3D92E
		/// </summary>
		/// <returns>carry flag</returns>
		private static bool F3D92E()
		{
			byte x = 0x6C;
			GetDynamicSubroutineAddress(0x5D, x);
			int vectorAddress = zeroPage[x] + zeroPage[x + 1] << 8;
			byte y = 0;

			while (true)
			{
				byte a = romData[vectorAddress + y];
				if (a == 0xFF)
					return false;
				if (a == 0x74)
					return PostClockIncrement_Vector_Equals_74();
				bool hasCarry = false;
				zeroPage[x] = ASMHelper.ADC(zeroPage[x], 0x06, ref hasCarry);
				if (hasCarry)
					++zeroPage[x + 1];
			}



		}

		/// <summary>
		/// $3C1D7<br/>
		/// Pushes bytes stored in address pointed to by $21 to PPU.
		/// </summary>
		public static void BattleInit_WriteToPPU()
		{
			byte a = registers[0x2002]; // reset the latch
			registers[0x2006] = zeroPage[ZeroPage.PPU_WriteAddress_1F + 1]; // address to start pushing into PPU
			registers[0x2006] = zeroPage[ZeroPage.PPU_WriteAddress_1F + 0];
			byte x = 0x21; // address in zeropage where address to sprite to be copied is located 
			bool z = false;
			while (z)
			{
				while (z)
				{
					a = LDA_FromZeroPages(x);
					registers[0x2007] = a;
					a = zeroPage[ZeroPage.dynamicSubroutine_21 + 2];
					a = ASMHelper.EOR(a, zeroPage[ZeroPage.dynamicSubroutine_21 + 0], out z, out bool _);
				}

				a = zeroPage[ZeroPage.dynamicSubroutine_21 + 1];
				a = ASMHelper.EOR(a, zeroPage[ZeroPage.bankSwitch_LastBankIndex_24], out z, out bool _);
			}
		}

		/// <summary>
		/// $3C020<br/>
		/// 
		/// </summary>
		/// <param name="x"></param>
		public static byte LDA_FromZeroPages(byte x)
		{
			var addr = zeroPage[x] + zeroPage[x + 1] << 8;
			var a = nesRam[addr];
			ASMHelper.Add16Bit(1, ref nesRam[x], ref nesRam[x + 1]);
			return a;
		}

		/// <summary>
		/// <para>Called from DynamicSubroutine_34000_B</para>
		/// <para>Reads and writes to PPU_SpriteDMA. My hypothesis is that this is scrolling the the BG.</para>
		/// F3C1F9 (a = 0x80), FC1FD (a = 0xFF), F3C204 (a = 0x00)
		/// </summary>
		/// <param name="a"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void L3C1FX(byte a, byte x, byte y)
		{
			zeroPage[ZeroPage.dynamicSubroutine_21 + 1] = a;

			// L3C208
			theStack.Push(x);
			theStack.Push(y);
			theStack.Push(y);

			zeroPage[0x1B] |= 0x01;
			a = theStack.Pop(out bool z, out bool n);
			zeroPage[ZeroPage.dynamicSubroutine_21 + 0] = 0;
			y = 0x05;

			do
			{ // C21A
				--y;
				if (y >= 0x80)
				{
					__L3C251_(ref a, ref x);
					return;
				}
			}
			while (y < romData[Pointers.ROM.MapScrollVectorB.iNESAddress + y]);

			theStack.Push(a);

			bool hasCarry = true;
			a = y;
			a = ASMHelper.ROL(a, 1, ref hasCarry);

			//hasCarry = true; // not sure if this is supposed to be set or not
			a = ASMHelper.SBC(a, nesRam[0x06D7], ref hasCarry); // Post_NMI_Const_NOT_00_Variable
			if (a >= 0x80)
			{
				hasCarry = false;
				a = ASMHelper.ADC(a, 0x0A, ref hasCarry);
			}
			// C22F
			a = ASMHelper.LSR(a, 1, out hasCarry);
			if (!hasCarry)
			{
				a ^= 0x7F; // EOR
				hasCarry = false;
				a = ASMHelper.ADC(a, 0x85, ref hasCarry);
			}
			// C237
			zeroPage[ZeroPage.dynamicSubroutine_21] = a;
			a <<= 1;
			a += zeroPage[ZeroPage.dynamicSubroutine_21];
			a <<= 4;
			hasCarry = false;
			zeroPage[ZeroPage.dynamicSubroutine_21] = ASMHelper.ADC(a, 0x10, ref hasCarry);

			a = theStack.Pop();

			hasCarry = true; // probably supposed to be set?
			a = ASMHelper.SBC(a, 0x04, ref hasCarry);

			// L3C24A Loop
			hasCarry = true;
			while (hasCarry)
			{
				a = ASMHelper.SBC(a, 0x0C, ref hasCarry);
			}
			__L3C251_(a, x);

		}

		/// <summary>
		/// 3C251
		/// </summary>
		/// <param name="a"></param>
		private static void __L3C251_(ref byte a, ref byte x)
		{
			a <<= 2;
			bool hasCarry = false;
			a = ASMHelper.ADC(a, zeroPage[ZeroPage.dynamicSubroutine_21], ref hasCarry);
			if (hasCarry)
				a = ASMHelper.ADC(a, 0x0F, ref hasCarry);
			byte y = a;
			zeroPage[ZeroPage.dynamicSubroutine_21] = y;
			a = zeroPage[ZeroPage.dynamicSubroutine_21 + 1];
			if (a == 0)
			{
				TransferQuickStorageToPPU_SpriteDMA(x, y);
			}
			else if (a == 0x80)
			{  // L3C274(x, y);
				a = nesRam[NESRAM.PPU_SpriteDMA_200 + 0 + y];
				a &= nesRam[NESRAM.PPU_SpriteDMA_200 + 3 + y];
				if (a != 0xF8)
				{
					++x;
					++y;
					nesRam[NESRAM.PPU_SpriteDMA_200 + y] = zeroPage[x];
					++x;
					++y;
					nesRam[NESRAM.PPU_SpriteDMA_200 + y] = zeroPage[x];
				}
			}
			else
			{ // 3C265 - TransferPPU_SpriteDMAToQuickStorage();
				do
				{
					zeroPage[x] = nesRam[NESRAM.PPU_SpriteDMA_200 + y];
					++x;
					++y;
				}
				while ((y &= 0x03) != 0);
			}

			// L3C29B()
			zeroPage[0x1B] &= 0xFE; // switch the bit set or release PPU trap
			y = theStack.Pop();
			hasCarry = false;
			x = ASMHelper.ADC(theStack.Pop(), 0x04, ref hasCarry);
		}


		/// <summary>
		/// 0x3C28F
		/// Transfer until ++y is multiple of 4
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private static void TransferQuickStorageToPPU_SpriteDMA(byte x, byte y)
		{
			do
			{
				byte a = zeroPage[0x04 + x];
				nesRam[NESRAM.PPU_SpriteDMA_200 + y] = a;
				++x;
				++y;
			}
			while ((y & 0x03) != 0);
		}

		/// <summary>
		/// 0x3C2BA
		/// </summary>
		private static void WaitForNMI_IfNeeded()
		{
			byte a;
			while (true)
			{
				a = 0x04;
				a &= zeroPage[ZeroPage.loopTrap_flag_1B];
				if (a == 0)
					break;
			}

			a = 0x40;
			a &= zeroPage[ZeroPage.loopTrap_flag_1B];
			zeroPage[ZeroPage.loopTrap_flag_1B] = a;
		}

		/// <summary>
		/// 0x3C2CF<br/>
		/// SetPPUAddress for what exactly?
		/// </summary>
		private static void SetPPUAddress()
		{
			byte a = nesRam[NESRAM.PPUControl_2000_Settings_6D3];
			a <<= 2;
			a &= 0x04;
			a |= 0x20; // 0010 0X00 -> 0x20 || 0x24
			zeroPage[ZeroPage.PPU_WriteAddress_1F + 1] = a;
			a = zeroPage[0x04];
			a <<= 3;
			bool c = false;
			zeroPage[ZeroPage.PPU_WriteAddress_1F + 0] = ASMHelper.ADC(a, nesRam[NESRAM.PPUScroll_X_6D0], ref c);
			if (c)
			{
				zeroPage[ZeroPage.PPU_WriteAddress_1F + 1] |= 0x04;
			}
			// __L3C2ED_
			a = nesRam[NESRAM.PPUScroll_Y_6D1];
			a >>= 3; // ppu scroll Y in sprites
			c = false;
			a = ASMHelper.ADC(a, zeroPage[0x05], ref c);
			ASMHelper.CMP(a, 0x1E, out c, out bool z, out bool n);
			if (!c)
				a = ASMHelper.SBC(a, 0x1E, ref c);
			//__L3C2FC_
			a = ASMHelper.LSR(a, 1, out c);
			zeroPage[ZeroPage.PPU_WriteAddress_1F + 0] = ASMHelper.ROR(zeroPage[ZeroPage.PPU_WriteAddress_1F + 0], 1, ref c);
			a = ASMHelper.LSR(a, 1, out c);
			zeroPage[ZeroPage.PPU_WriteAddress_1F + 0] = ASMHelper.ROR(zeroPage[ZeroPage.PPU_WriteAddress_1F + 0], 1, ref c);
			zeroPage[ZeroPage.PPU_WriteAddress_1F + 1] = ASMHelper.ORA(a, zeroPage[ZeroPage.PPU_WriteAddress_1F + 1], out z, out n);
		}

		/// <summary>
		/// 0x3C341
		/// Loop until waitForNMI_Variable changes (changed in NMI)
		/// </summary>
		public static void WaitForNMI_3C000()
		{
			byte a = nesRam[NESRAM.waitForNMI_flag];
			while (a == nesRam[NESRAM.waitForNMI_flag]) { }
			WaitForNMI_Post();
		}


		/// <summary>
		/// 0x3C35B<br/>
		/// </summary>
		public static void CopyStagedPalettesToPPU()
		{
			byte a = 0;
			zeroPage[ZeroPage.dynamicSubroutine_21 + 0] = a; // this gets subtracted from palette color
			SetupBGPalettes();
		}

		/// <summary>
		/// 0x3C383
		/// </summary>
		private static void SetupBGPalettes()
		{
			byte a = 0x3F;
			zeroPage[ZeroPage.PPU_WriteAddress_1F + 1] = a;
			zeroPage[ZeroPage.PPU_WriteAddress_1F + 0] = 0;
			byte x = 0;
			for (x = 0; x < 0x18; ++x)
			{
				a = zeroPage[ZeroPage.PPU_WriteAddress_1F];
				byte y = nesRam[NESRAM.PPU_BGPaletteColor_Store_3E7 + 0];
				a &= 0x03;
				if (a != 0)
				{
					y = nesRam[NESRAM.PPU_BGPaletteColor_Store_3E7 + 1 + x];
					++x;
				}
				a = y;
				bool c = true;
				a = ASMHelper.SBC(a, zeroPage[ZeroPage.dynamicSubroutine_21 + 0], ref c);
				ASMHelper.CMP(a, 0x40, out c, out bool z, out bool n);
				if (c)
					a = nesRam[NESRAM.PPU_BGPaletteColor_Store_3E7 + 0];
				zeroPage[0x1E] = a;
				zeroPage[ZeroPage.dynamicSubroutine_21 + 2] = x;
				WaitForVBlank_And_WritePPUInstructions();
				x = zeroPage[ZeroPage.dynamicSubroutine_21 + 2]; // not needed in this context;
			}

			WaitForNMI_IfNeeded();
		}

		/// <summary>
		/// 0x3C30A<br/>
		/// </summary>
		private static void WaitForVBlank_And_WritePPUInstructions()
		{
			byte a;
			while (true)
			{
				while (true) // wait for NMI
				{
					a = 0x04;
					a &= zeroPage[ZeroPage.loopTrap_flag_1B];
					if (a == 0)
						break;
				}

				a = nesRam[NESRAM.PPU_DrawBackgrounLineCount_6D9];
				if (a < 0x28)
					break;

				WaitForNMI_IfNeeded();
			}

			// _WriteDataToPPU:
			byte x = nesRam[0x6D8];
			a = zeroPage[ZeroPage.PPU_WriteAddress_1F + 1];
			a &= 0x3F;
			nesRam[NESRAM.PPU_StagingArea_300 + x] = a;
			++x;
			nesRam[NESRAM.PPU_StagingArea_300 + x] = zeroPage[ZeroPage.PPU_WriteAddress_1F];
			++x;
			nesRam[NESRAM.PPU_StagingArea_300 + x] = zeroPage[0x1E];
			++x;
			nesRam[0x6D8] = x;
			++nesRam[0x6D9];
			if (++zeroPage[0x1F + 0] == 0)
				++zeroPage[0x1F + 1];
		}

		/// <summary>
		/// 0x3C427
		/// </summary>
		private static void WaitForVBlank_HideSprites_SetLoopTrap()
		{
			WaitForVBlank_HideSprites();
			zeroPage[ZeroPage.loopTrap_flag_1B] = ASMHelper.ORA(0x10, zeroPage[ZeroPage.loopTrap_flag_1B], out bool z, out bool n);
		}



		/// <summary>
		/// 0x3C431 <br/>
		/// Negates #$10 from loopTrap_flag_1B, so if loopTrap_flag_1B == $10 it's now 0.
		/// </summary>
		public static void Negate_LoopTragFlag()
		{
			byte a = 0x10;
			a ^= 0xFF;
			a &= zeroPage[ZeroPage.loopTrap_flag_1B];
			zeroPage[ZeroPage.loopTrap_flag_1B] = a;
		}

		/// <summary>
		/// 0x3C456
		/// </summary>
		private static void WaitForVBlank_HideSprites()
		{
			byte y = 0;
			// reset high/low latch
			byte a = registers[Registers.PPU_Status];
			// _WaitForVBlank_Loop
			while (a < 0x80)
			{
				a = registers[Registers.PPU_Status];
			}

			registers[Registers.PPU_Mask] = 0;
			nesRam[NESRAM.PPUMask_2001_Settings_6D4] = 0;
		}


		/// <summary>
		/// 0x3C74C
		/// </summary>
		public static void PPU_Copy2LinesAndAttributesTo_StagingArea()
		{
			byte a = 0;
			PPU_WaitForNMI_GetPPUPos_CopyLines(a);
			WaitForNMI_IfNeeded();
			WaitForNMI_3C000();
		}

		/// <summary>
		/// 0x3C757
		/// </summary>
		private static void PPU_WaitForNMI_GetPPUPos_CopyLines(byte a)
		{
			theStack.Push(a);
			a = nesRam[0x6D9];
			if (a != 0)
				WaitForNMI_3C000();
			a = 0;
			zeroPage[ZeroPage.bankSwitch_LastBankIndex_24] = a;
			a = theStack.Pop(out bool z, out bool n);
			PPU_GetPosition_SetPPUAddress(a);

			zeroPage[0x25] = zeroPage[0x26] = 0;
			a = nesRam[NESRAM.menu_WriteDimensions_471];
			theStack.Push(a);
			a &= 0xF0;
			a >>= 3;
			zeroPage[ZeroPage.dynamicSubroutine_21 + 1] = a; // amount of lines to copy (this applies to menus but does it apply to sprites?)
			a = theStack.Pop(out z, out n);
			a &= 0x0F;
			a <<= 1;
			zeroPage[ZeroPage.dynamicSubroutine_21 + 2] = a; // length of line to copy (this applies to menus but does it apply to sprites?)
			zeroPage[0x27] = a;
			byte x = nesRam[0x6D8];

			... // eh this is getting away from what I am digging for
		}

		/// <summary>
		/// 0x3C878
		/// </summary>
		private static void PPU_GetPosition_SetPPUAddress(byte a)
		{
			theStack.Push(a);
			PPU_GetSpritePosition_SetPPUAddress();
			a = theStack.Pop(out bool z, out bool n);
			if (z)
				return;
			// L3C880;

		}

		/// <summary>
		/// 0x3C887<br/>
		/// Sprite X stored in low nybble and Y stored in high nybble 0x470.
		/// </summary>
		private static void PPU_GetSpritePosition_SetPPUAddress()
		{
			byte a = nesRam[0x470];
			a <<= 1;
			a &= 0x1E;
			zeroPage[0x04] = a; // x pos in sprites (8x8 pixels)
			a = nesRam[0x470];
			a >>= 3;
			a &= 0x1E;
			zeroPage[0x05] = a; // y pos in sprites (8x8 pixels)
			SetPPUAddress();
		}

		/// <summary>
		/// 0x3C90A
		/// </summary>
		private static void WaitForNMI_Post()
		{
			if (zeroPage[0x32] != 0xFD // 0x32 - post_NMI_store
				&& (nesRam[0x60B7] & 0x20) == 0) // 0x60B7 - post_NMI_const
				Swap_SpriteDMA_Positions_orSomething();
		}


		/// <summary>
		/// 0x3CDC0
		/// </summary>
		private static void F3CDC0()
		{
			if (nesRam[NESRAM.walkDirection] != 0x03)
				return;
			// walking left
			// 3CDC8()
			if (zeroPage[ZeroPage.mapScrollCheck] != 0)
				return;
		}

		/// <summary>
		/// 0x3CE10
		/// </summary>
		private static void Add8To0x645()
		{
			nesRam[0x0645] += 0x08;
		}


		/// <summary>
		/// 3D5AB
		/// Called right after WaitForNMI_3C000
		/// </summary>
		private static void F3D5AB()
		{
			byte x = zeroPage[ZeroPage.currentTileType];
			x &= 0x1F;
			// PPU_TileBatchInstructions_3rdByte
			if (saveRam[0x0DE0 + x] != 0
				|| (zeroPage[0x90] & 0x03) != 0
				|| (nesRam[NESRAM.walkDirection] & 0x01) != 0)
				return;

			WaitForNMI_X_Frames(0x03);
		}

		/// <summary>
		/// 3F116
		/// Called write before WaitForNMI_3C000. Sprite stuff calls this twice.
		/// </summary>
		public static void F3F116()
		{
			if ((zeroPage[ZeroPage.lightOrDarkWorld] & 0x01) == 0)
			{
				SetSegmentPointerToStack();
				return;
			}

			byte a = (byte)(zeroPage[0x90] & 0x0F);
			if (a == 0 || a == 0x0F)
				return;
			a -= 0x01;
			a <<= 3;
			zeroPage[0x72 + 0] = a;
			zeroPage[0x72 + 1] = 0x01;
			L3F135();
		}

		/// <summary>
		/// 3F0EA
		/// </summary>
		private static void SetSegmentPointerToStack()
		{
			byte x = 0;
			byte a = (byte)(zeroPage[0x90] & 0x0F);
			if (a != 0x01)
			{
				x = 0x08;
				if (a != 0x02)
					return;
			}
			// _L3F0FA_
			zeroPage[0x72] = x;
			zeroPage[0x73] = 0x01;
			a = (byte)(zeroPage[0xAC] & 0x1F);
			if (a == 0)
			{
				L3F1A4();
				L3F1A4();
			}

			L3F295();
		}

		/// <summary>
		/// 3F1A4
		/// </summary>
		private static void L3F1A4()
		{
			byte x = zeroPage[0x72];
			x >>= 1;
			byte a = zeroPage[NESRAM.Character_Statuses + x + 0];
			if (a < 80)
			{
				L3F1F8();
				return;
			}

			a = (byte)(zeroPage[NESRAM.Character_Statuses + x + 1] & 0x40);
			if (a == 0)
			{
				L3F1C5();
				return;
			}

			theStack.Push(nesRam[0x0645]);
			nesRam[0x0645] = 0;
			L3F1F5();
			... // keeps going but I can't see it yet...
		}

		private static void L3F1C5()
		{
			if (zeroPage[0x72] < 0x10) // dialogSegmentPointer
			{
				L3F1F5();
				return;
			}

			byte x = zeroPage[0x80]; // charsCopied
			byte y = zeroPage[0x81]; // linesWrittenToStagingArea
			L3EF34();
			... // keeps going but I can't see it yet...
		}

		private static void L3F1F5()
		{
			F3F259();
			L3F1F8();
		}
		/// <summary>
		/// Add 4 to dialogSegmentPointer+0.
		/// </summary>
		private static void L3F1F8()
		{
			zeroPage[0x72] += 0x04;
		}

		private static void F3F259()
		{
			byte y = 0x02;
			byte a = nesRam[zeroPage[0x72] + zeroPage[0x73] << 4 + y];
			a &= 0x0F;
			F3F8D7();
			y = 0x03;
			a = nesRam[zeroPage[0x72] + zeroPage[0x73] << 4 + y];
			theStack.Push(a);
			y = nesRam[NESRAM.encounterCheckRequired_A];
			if (y >= 0x07 && y < 0x0A)
				a = 0x02;
			//_L3F274_
			F3F909();
			a = theStack.Pop();
			a &= 0x3C;
			y = a;
			if (y != 0 || zeroPage[0x72] != 0)
			{   //L3F282()
				F3F385();
				L3F32F();
			}
		}



		/// <summary>
		/// 0xF3CDCF
		/// </summary>
		public static void F3CDCF()
		{
			if (zeroPage[0xAA] == 0)
				L3CDDA();
			else
			if ((byte)(zeroPage[0x90] & 0x01) == 0)
			{
				// CDE0()
			}
		}

		private static void L3CDDA()
		{
			if ((byte)(zeroPage[ZeroPage.mapScrollCheck + 1] & 0x01) == 0)
				return;
			Bank18000.CheckDayNightTransitionTimes();
			if (zeroPage[ZeroPage.mapScrollCheck + 1] != 0)
				return;

		}


		/// <summary>
		/// F3DAF7
		/// </summary>
		public static void SetPPUScroll_And_MapScrollCheck()
		{
			BankSwitch_SaveNextBank_3C000(0x05); // Load Bank14000 for world map data
			SetPPUScroll();
			Bank14000.Map_Scroll_Check();
		}

		private static void SetPPUScroll()
		{
			if (zeroPage[0x87] == 0) // mapScrollCheck+1
				return;
			byte a = 0xFF;
			zeroPage[0x86] = a; // mapScrollCheck+0
			a = --zeroPage[0x87]; // mapScrollCheck+1
			if (a == 0)
				zeroPage[0x86] = a; // mapScrollCheck+0
			F3DF08();
			switch (nesRam[NESRAM.walkDirection])
			{
				case 0:
					Set_PPU_Scroll_Up();
					break;

				case 1:
					Set_PPU_Scroll_Right();
					break;

				case 2:
					Set_PPU_Scroll_Down();
					break;

				case 3:
					Set_PPU_Scroll_Left();
					break;
			}

		}

		private static void Set_PPU_Scroll_Left()
		{
			if (--nesRam[NESRAM.PPUScroll_X_6D0] == 0xFF)
				PPUScrollWrap();
		}


		private static void F3DF08()
		{ // 3DF08
			zeroPage[0x09] = (byte)(zeroPage[ZeroPage.lightOrDarkWorld] & 0x01); // 0x09 - menu_PPUAddress_1
			zeroPage[0x0B] = 0;
			if ((byte)(walkDirection & 0x01) != 0)
				WalkDirection_Horizontal();
			else
				WalkDirection_Vertical();
		}

		/// <summary>
		/// 3DF51
		/// </summary>
		private static void WalkDirection_Horizontal()
		{
			byte a = (byte)(zeroPage[0x87] & 0x07); // mapScrollCheck+1
			if (a == 0 || a == 0x07)
				return;
			// 3DF5B
			if (zeroPage[ZeroPage.mapScrollCheck + 1] == 0x0B)
				F3E1E8();
			else if (zeroPage[ZeroPage.mapScrollCheck + 1] > 0x0B)
				F3E198();
			else if (zeroPage[ZeroPage.mapScrollCheck + 1] >= 0x09)
				F3DD75();
			else if (zeroPage[ZeroPage.mapScrollCheck + 1] == 0x03)
				return;
			else if (zeroPage[ZeroPage.mapScrollCheck + 1] > 0x03)
				F3DCCB();
			else // < 0x03
				F3DD7B();
		}

		private static void F3E198()
		{
			byte x = 0x08;
			if (nesRam[NESRAM.walkDirection] != 0x01)
				x = 0xF7; // if not walking right
						  // _F3E1A3_
			zeroPage[0x10] = x;
			byte a = F3DF1C();
			zeroPage[0x10] += a;

			a = zeroPage[ZeroPage.mapScrollCheck + 1];
			zeroPage[0x05] = (byte)(2 * a);
			zeroPage[0x04] = (byte)(18 * a);
			bool hasCarry = false;
			var result = ASMHelper.ADC((byte)(2 * a), (byte)(4 * a), ref hasCarry);
			zeroPage[0x11] = (byte)(result - 7 - (hasCarry ? 0 : 1)); // -8 if above has no carry

			a = F3DF26(); // world pos Y (if on world map? Otherwise directionToWalk_vertical?)
			zeroPage[0x11] += a;
			F3E1D9();
			F3E1D9();
			F3E1D9();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>ZeroPage.map_WorldPosition_Y if zeroPage[0x09] == 0 else zeroPages[0x31]</returns>
		private static byte F3DF26()
		{
			if (zeroPage[0x09] == 0)
				return zeroPage[ZeroPage.map_WorldPosition_Y];
			return zeroPage[0x31]; // directionToWalk_vertical
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>ZeroPage.map_WorldPosition_X if zeroPage[0x09] == 0 else zeroPages[0x30]</returns>
		private static byte F3DF1C()
		{
			if (zeroPage[0x09] == 0)
				return zeroPage[ZeroPage.map_WorldPosition_X];
			return zeroPage[0x30]; // directionToWalk_horizontal
		}

		private static void F3E1D9()
		{
			F3E1DC();
			F3E1DC();
		}

		private static void F3E1DC()
		{
			F3E01F();
			zeroPage[0x04] += 3;
			++zeroPage[0x11];
		}

		private static void F3E01F()
		{
			F3ECB9(zeroPage[0x10], zeroPage[0x11]);
			byte x = zeroPage[0x04]; // this is 18 * a from F3E198()
			nesRam[0x0580 + x + 0] = zeroPage[0x7B]; // menu_vector+0
			nesRam[0x0580 + x + 1] = zeroPage[0x7C];
			nesRam[0x0580 + x + 2] = zeroPage[0x7D];
		}

		/// <summary>
		/// 0x3ECB9
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private static void F3ECB9(byte x, byte y)
		{
			zeroPage[0x74] = x;
			zeroPage[0x4B] = x;
			zeroPage[0x77] = y;
			Zero_7A_thru_7D_4C();
			if (zeroPage[0x2F] != 0)
			{
				if (zeroPage[0x2F] == 0x02)
					F3ECE3();
				else
					F3EF3E();
				return;
			}
			// _L3ECCD_
			if (x < 0x04
				|| x >= 0xFC
				|| y < 0x04
				|| y >= 0xFC)
				// L3ECFA
				return 0;
			else
			{
				Map_GetData(y);
				L3EDC9();
			}
		}

		private static void Map_GetData(byte y)
		{
			bool isDarkWorld = (zeroPage[0x2F] != 0x02);

			byte x = ASMHelper.ASL(y, 1, out bool hasCarry);
			int address;
			if (!hasCarry)
			{
				if (isDarkWorld)
					address = ROM.Map_DarkWorld.iNESAddress;
				else
					address = ROM.Map_LightWorld_Data_A.iNESAddress;
			}
			else // haven't seen the dark world version of this but is probably the similar to above
			{ // _GetMapBData_AddrAtX
				address = ROM.Map_LightWorld_Data_B.iNESAddress;
			}

			zeroPage[0x6C] = romData[address + x + 0];
			zeroPage[0x6D] = romData[address + x + 1];
		}

		private static void L3EDC9()
		{
			zeroPage[0x75] = 0;
			nesRam[0x0642] = zeroPage[0x6C];
			nesRam[0x0643] = zeroPage[0x6D];
			--zeroPage[0x74];
			byte y = 0;
			if (zeroPage[0x74] < 0x80)
			{
				L3EE19();
				return;
			}

			zeroPage[0x74] += 2;
			L3EDE3();
		}

		private static void L3EDE3()
		{
			Set_75();
			byte y = zeroPage[0x77]; // menu_positionLowNibble
			++y;
			Map_GetData(y);
			y = (byte)(zeroPage[0x6C + 0] - nesRam[0x0642]);
			--y;
			zeroPage[0x6C + 1] = nesRam[0x0643]; // dynamicPointerSpace+1
			zeroPage[0x6C + 0] = nesRam[0x0642]; // dynamicPointerSpace+0
			Map_GetTileAt_6C_Loop(y);
		}

		/// <summary>
		/// 0x3EE6A
		/// </summary>
		public static void Map_GetTileAt_6C_Loop(byte y)
		{
			while (true)
			{
				int tileAddress = zeroPage[0x6C] + zeroPage[0x6C + 1] << 8;
				byte a = romData[tileAddress + y];
				zeroPage[0x79] = a;

				if ((a & 0xE0) == 0xE0)
				{ // MapTile_E0
					a = (byte)(zeroPage[0x79] & 0x1F);
					if (a >= 0x08)
						a = 0;
				}
				else
					a &= 0x1F;

				if (!_L3EE78_(ref a, ref y))
					break;
			}
		}

		private static bool _L3EE78_(ref byte a, ref byte y)
		{
			zeroPage[0x78] = (byte)(a + 0x01);
			zeroPage[0x75] -= zeroPage[0x78];
			return !_Map_GetTileAt_6C_inner_Loop(ref a, ref y);
		}

		private static bool _Map_GetTileAt_6C_inner_Loop(ref byte a, ref byte y)
		{
			while (true)
			{
				// _Map_GetTileAt_6C_inner_Loop
				if (zeroPage[0x75] < zeroPage[0x74])
				{   // Map_GetTileAt_6C_LoopBreak
					byte x = zeroPage[0x7A];
					zeroPage[0x7B + x] = zeroPage[0x79];
					zeroPage[0x7A] = ++x;
					if (x == 0x03)
					{
						--zeroPage[0x7A];
						a = zeroPage[0x75];
						continue;
					}
					// _L3EEAF_
					F3EEC3();
					F3EECF();
					F3EEC9();
					x = zeroPage[0x7B];
					y = zeroPage[0x7D];
					zeroPage[0x7D] = x;
					zeroPage[0x7B] = y;
					Set_DynamicPointer_to_0x0642();
					return false; // end loop
				}
				else
				{   // _L3EE8A_
					--y;
					return true; // return to loop
				}
			}
		}

		/// <summary>
		/// 0x3EF29
		/// </summary>
		private static void Set_DynamicPointer_to_0x0642()
		{
			zeroPage[0x6C + 1] = nesRam[0x0643]; // dynamicPointerSpace+1
			zeroPage[0x6C + 0] = nesRam[0x0642]; // dynamicPointerSpace+0
		}

		public static void F3EEC3()
		{
			++zeroPage[0x4B];
			F3EEE9(0x0);
		}
		public static void F3EEC9()
		{
			++zeroPage[0x4B];
			F3EEE9(0x01);
		}
		public static void F3EECF()
		{
			zeroPage[0x4B] -= 0x02;
			F3EEE9(0x02);
		}
		public static void F3EED7()
		{
			--zeroPage[0x4B];
			F3EEE9(0x0);
		}
		public static void F3EEDD()
		{
			--zeroPage[0x4B];
			F3EEE9(0x01);
		}
		public static void F3EEE3()
		{
			zeroPage[0x4B] += 0x02;
			F3EEE9(0x01);
		}

		private static void F3EEE9(byte x)
		{
			byte a = zeroPage[0x7B + x];
			a &= 0xE0;
			if (a != 0xE0)
			{
				bool hasCarry = false;
				a = ASMHelper.ROL(a, 4, ref hasCarry);
			}
			else
			{ // L3EEF9
				a = zeroPage[0x7B + x];
				a &= 0x1F;
				if (a < 0x08)
					a = 0x07;
			}
			// _L3EF03_
			zeroPage[0x78 + x] = a;
			if (zeroPage[0x4C] == 0)
				GetCurrentTile(a, x);
		}

		/// <summary>
		/// 0x3EF0B
		/// </summary>
		/// <param name="a"></param>
		/// <param name="x"></param>
		private static void GetCurrentTile(byte a, byte x)
		{
			F3EFDF(ref a);
			// F3F07A();
			if ((saveRam[0x60B6] & 0x08) != 0)
				L3F084();
			// F3F0A0();
			if ((saveRam[0x60B6] & 0x04) != 0)
				L3F0AA();
			// F3F0D0();
			if (saveRam[SRAM.encounterCheckRequired_b] >= 0x80)
				L3F0F8();
			zeroPage[0x7B + x] = a;
		}

		/// <summary>
		/// 0x3EFDF
		/// </summary>
		/// <param name="a"></param>
		private static void F3EFDF(ref byte a)
		{
			if (a != 0x01)
			{   // L3EFFA();
				if ((zeroPage[0x2F] & 0x02) == 0
					|| saveRam[0x60B6] < 0x80
					|| zeroPage[0x77] != 0x3A
					|| a != 0
					|| zeroPage[0x74] < 0x52
					|| zeroPage[0x74] >= 0x55)
					return; // a is unchanged

				a = 0x1B;
			}
			else
			{
				if ((zeroPage[0x2F] & 0x02) != 0)
					return; // a is unchanged
				if (zeroPage[0x77] < 0x64
					|| zeroPage[0x77] >= 0xE6)
				{ // _L3EFF4_
					a = 0x1E;
				}
			}
		}

		/// <summary>
		/// if light world (zeroPages[0x2F])
		///		0x75 = FF
		/// else
		///		0x75 = 0x9D
		/// </summary>
		public static void Set_75()
		{
			if ((zeroPage[0x2F] & 0x02) == 0)
				zeroPage[0x75] = 0xFF; // is light world
			else
				zeroPage[0x75] = 0x9D;
		}

		private static void Zero_7A_thru_7D_4C()
		{
			zeroPage[0x7A] = 0;
			zeroPage[0x7B] = 0;
			zeroPage[0x7C] = 0;
			zeroPage[0x7D] = 0;
			zeroPage[0x4C] = 0;
		}

		/// <summary>
		/// 0x3EC99
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void F3EC99(int x, int y)
		{
			...
		}



		private static byte mapDataPointer_6C = 0x6C;


		/// <summary>
		/// 0x3D2EB
		/// </summary>
		/// <param name="a"></param>
		private static void F3D2EB(byte a)
		{
			theStack.Push(a);
			F3DA32();
			F3E2FC();
			F3D4AD();
			...
		}

		/// <summary>
		/// 0x3D4AD
		/// </summary>
		private static void F3D4AD()
		{
			WaitForVBlank_HideSprites_SetLoopTrap();
			F3F927();
		}


		/// <summary>
		/// 0x3F927
		/// </summary>
		private static void F3F927()
		{
			byte a = zeroPage[0x9A];
			byte x = 0x06;
			if (a >= 0x0C)
				++x;
			a = x;
			BankSwitch_SaveNextBank_3C000(a);
			F3F960();
			...
		}

		/// <summary>
		/// 0x3F960
		/// </summary>
		private static void F3F960()
		{
			F3F994();
			F3F9A8();
			...
		}

		/// <summary>
		/// 0x3F994
		/// </summary>
		private static void F3F994()
		{
			byte a = 0x03;
			bool c = false;
			zeroPage[mapDataPointer_6C + 0] = ASMHelper.ADC(a, zeroPage[mapDataPointer_6C + 0], ref c);
			if (c)
				++zeroPage[mapDataPointer_6C + 1];
			zeroPage[0x74] = 0;
			zeroPage[0x75] = 0;
			zeroPage[0x7D] = 0;
		}

		/// <summary>
		/// 0x3F9A8
		/// </summary>
		private static void F3F9A8()
		{
			F3F9CD();
			...
		}

		/// <summary>
		/// 0x3F9CD
		/// </summary>
		private static void F3F9CD()
		{
			byte a = zeroPage[0x8D];
			F3F9ED(a);
		}

		/// <summary>
		/// 0x3F9ED
		/// </summary>
		/// <param name="a"></param>
		private static void F3F9ED(byte a)
		{
			byte x = a;
			zeroPage[0x04] = 0;
			zeroPage[0x05] = 0;
			Map_Decompress_A(out bool c);
		}

		/// <summary>
		/// 0x3FA01<br/>
		/// @TODO(Tristan): Need to find where zeroPage[0x6C] zeroPage[0x6D] are set
		/// </summary>
		private static void Map_Decompress_A(out bool c)
		{
			byte a = 0x80;
			byte y = zeroPage[0x74];
			while (y != 0)
			{
				a >>= 1;
				--y;
			}
			// _L3FA0C_
			y = 0;
			int address = zeroPage[0x6C] + (zeroPage[0x6D] << 8) + y;
			a &= cpuMemory[address]; // get next byte map info
			zeroPage[0x13] = a;
			a = ++zeroPage[0x74];
			if (a >= 0x08)
			{
				a = 0;
				zeroPage[0x74] = a;
				if (++zeroPage[0x6C] == 0)
					++zeroPage[0x6D];
			}
			//_L3FA24_
			a = zeroPage[0x13];
			if (a == 0)
			{
				F3FA2A();
				return;
			}

			c = true;
		}




		/// <summary>
		/// <para>3FF91 then to BankSwitch
		/// Switch out 0x8000 to 0xBFFF with bank from ROM.<br/>
		/// If BankId &lt; 0x10, 0xC000 to 0xFFFF is set to ($0F) $3C000 bank from ROM.<br/>
		/// If BankId &gt;= 0x10, 0xC000 to 0xFFFF is set to ($1F) $7C000 bank from ROM.<br/>
		/// </para>
		/// </summary>
		/// <param name="bankId"></param>
		public static void BankSwitch_SaveNextBank_3C000(byte bankId)
		{
			nesRam[NESRAM.bankSwitch_CurrentBankId] = bankId;
			BankSwitch_3C000(bankId);
		}

		/// <summary>
		/// <para>3FF94 - identical to 7C000.BankSwitch
		/// Switch out 0x8000 to 0xBFFF with bank from ROM.<br/>
		/// If BankId &lt; 0x10, 0xC000 to 0xFFFF is set to ($0F) $3C000 bank from ROM.<br/>
		/// If BankId &gt;= 0x10, 0xC000 to 0xFFFF is set to ($1F) $7C000 bank from ROM.<br/>
		/// </para>
		/// </summary>
		/// <param name="bankId"></param>
		public static void BankSwitch_3C000(byte bankId)
		{

		}

		/// <summary>
		/// 0x3FFD8
		/// <para>
		/// Target of RESET_Pointer_3C000 Called on Reset (and start?) 
		/// </para>
		/// </summary>
		public static void RESET_3C000()
		{
			// SEI - Disable interrupts
			++romData[0x3CFFDF]; // this probably has to do with reseting something in the MMC
			ResetNMI_3C000();
		}

		/// <summary>
		/// 0x3E7E8
		/// <para>
		/// Called during RESET interrupt.
		/// </para>
		/// </summary>
		private static void ResetNMI_3C000()
		{
			// SEI - Disable interrupts - again...
			// CLD - Clear decimal mode - probably pointless
			registers[Registers.PPU_Control] = 0;
			registers[Registers.PPU_Mask] = 0;

			int x = 1;
			while (x-- < 0x80)
			{ // I suspect these loops are a way to wait for the PPU to be ready for drawing
				ASMHelper.BIT(0xFF, registers[Registers.PPU_Status], out bool n, out bool v, out bool z);
				while (!n)
					ASMHelper.BIT(0xFF, registers[Registers.PPU_Status], out n, out v, out z);
				while (n)
					ASMHelper.BIT(0xFF, registers[Registers.PPU_Status], out n, out v, out z);
			}

			// set SP to 0xFF
			registers[Registers.PPU_Control] = 0x08; // disable NMI
			registers[Registers.PPU_Mask] = 0x00; // basically disable everything

			nesRam[0x06C7] = 0x0E;
			nesRam[0x06C8] = 0x10;

			ResetNMI_3C000_sub();
		}

		/// <summary>
		/// 0x3C9EA
		/// </summary>
		private static void ResetNMI_3C000_sub()
		{
			++romData[0x3CFFDF]; // what the hell does this do??
			BankRegisterSwitch(nesRam[0x06C7]);
			BankSwitch_SetHiOrLowBanks_3C000(nesRam[0x06C8]); // switch to high banks
			Bank7C000.ResetNMI_7C000_sub();
		}

		/// <summary>
		/// 0x3E81E
		/// <para>Fall through from ResetNMI_7C000_sub_cont post BankSwitch</para>
		/// </summary>
		public static void ResetNMI_3C000_sub_cont()
		{
			registers[Registers.APUStatus] = 0;
			nesRam[0xA3D] = 0;
			Bank04000.ResetAPU_04000();

			// Reset PPU
			byte a = registers[Registers.PPU_Status];
			registers[Registers.PPU_Addr] = 0x10;
			registers[Registers.PPU_Addr] = 0x00;
			saveRam[0x6A58] = 0;
			byte x = 0x10;
			while (x-- > 0)
				registers[Registers.PPU_Data] = 0;
			Bank00000.ClearRam();
			ResetNMI_ClearData();

			x = 0x19;
			do
			{
				nesRam[NESRAM.PPU_BGPaletteColor_Store_3E7 + x] = 0x0F;
			} while (--x < 0x80);

			nesRam[NESRAM.PPUMask_2001_Settings_6D4] = 0x18;
			registers[Registers.PPU_Mask] = 0x18;

			nesRam[NESRAM.PPUControl_2000_Settings_6D3] = 0x90;
			registers[Registers.PPU_Control] = 0x90;

			F3CBB3();
		}


		/// <summary>
		/// 0x3CBB3
		/// </summary>
		private static void F3CBB3()
		{
			byte x = 0xFF;
			if (saveRam[0x6A58] != 0)
			{
				L3CB33();
				// BREAK?
			}

			if (saveRam[0x6A58] != 0)
			{
				_L3CBF2_();
			}

			Bank5C000.DynamicSubroutine_5C000();
		}

		/// <summary>
		/// 0x3E903
		/// returns here before 
		/// </summary>
		public static void ResetNMI_3C000_sub_cont_pre_return()
		{
			theStack.Push(zeroPage[0x21]);
			// PLP from zeroPages[0x36]
		}


		/// <summary>
		/// Subroutines that should only run during NMI
		/// </summary>
		public static class NMI
		{
			/// <summary>
			/// 3C96C
			/// </summary>
			public static void NMI_VBlank_3C000()
			{
				if ((byte)(zeroPage[ZeroPage.loopTrap_flag_1B] & 0x10) == 0)
				{
					zeroPage[0x33] = 0;
					PPU_Update();
				}
				// _L3C97E_
				//++nesRam[0xFFDF]; // what the hell is this??
				//BankSwitch(0x1E);
				Bank7C000.NMI.NMI_RunAPUEngine();
				//ReturnFrom_RunAPUEngine_78000
				// BankSwitch right back to where we are...what the hell....
				++nesRam[NESRAM.waitForNMI_flag];
				byte x = theStack.stackPointer;
				if (theStack[x + 6] == 0xFF
					&& theStack[x + 5] >= 0xAB
					&& theStack[x + 5] < 0xD5)
				{
					theStack[x + 5] = 0xD5;
					ResetNMI_3C000B();
				}
				// _L3C9C3_
				x = theStack.stackPointer;
				zeroPage[0xA2] = theStack[x + 6];
				zeroPage[0xA1] = theStack[x + 5]; // this the address where when NMI kicked in
				int pcPreNMI = zeroPage[0xA1] + zeroPage[0xA2] << 8;
				byte y = 0;
				byte nextByteAfterNMI = (byte)(romData[0x3C000 + pcPreNMI - 0x8000] & 0x0F);
				if (nextByteAfterNMI == 0x07
					|| nextByteAfterNMI == 0x0F0)
				{
					// Perform a BRK instead of RTI
				}
				//RTI
			}

			/// <summary>
			/// 3CA5C
			/// </summary>
			private static void PPU_Update()
			{
				//byte a = RegisterPointers.PPU_Status; // reading this byte resets write toggle
				byte x = 0;
				byte a = zeroPage[0x32]; // post_NMI_store
				if (a == 0 || a == 0xFD)
					PPU_CheckRenderType(x);
				else if (a == 0xFE)
					PPU_Render();
			}

			private static void PPU_CheckRenderType(byte x)
			{
				byte a = zeroPage[ZeroPage.PPU_renderStart_flag];
				if (a != 0)
				{
					if (a < 0x10)
						L3CA59();
					else if (a != 0x10)
						L3C9FE();
					else if (a == 0x10)
						L3C9FF();
					return;
				}
				//_PPU_Render_cont
				if (zeroPage[ZeroPage.PPU_render_flag] == 0
					&& (zeroPage[ZeroPage.loopTrap_flag_1B] & 0x04) == 0)
				{
					_L3CA99(x);
				}
				else
					PPU_CheckRenderType_cont(x);
			}

			/// <summary>
			/// 3CA84
			/// </summary>
			/// <param name="x"></param>
			private static void PPU_CheckRenderType_cont(byte x)
			{
				zeroPage[ZeroPage.loopTrap_flag_1B] = (byte)(zeroPage[ZeroPage.loopTrap_flag_1B] | 0x04);
				if (zeroPage[ZeroPage.PPU_render_flag] != 0)
				{
					zeroPage[ZeroPage.PPU_render_flag] = x;
					PPU_TransferSpritesFrom0x0200ToPPU(x);
					return;
				}
				// _L3CA92
				if (nesRam[NESRAM.caretUpdate_flag] != 0x02)
				{
					_L3CAAC(x);
				}
				else
					_L3CA99(x);
			}

			/// <summary>
			/// 3CA99
			/// </summary>
			private static void _L3CA99(byte x)
			{
				if ((zeroPage[ZeroPage.loopTrap_flag_1B] & 0x01) == 0)
					PPU_TransferSpritesFrom0x0200ToPPU(x);
				else
					PPU_CheckIfSkipToFinalize(x);
			}

			/// <summary>
			/// 3CA9F
			/// </summary>
			private static void PPU_TransferSpritesFrom0x0200ToPPU(byte x)
			{
				Registers.SpriteDMA = 0x02; // this is high byte of address to copy from
				zeroPage[0x33] = 0x02;
				PPU_CheckIfSkipToFinalize(x);
			}

			/// <summary>
			/// 3CAA6
			/// </summary>
			private static void PPU_CheckIfSkipToFinalize(byte x)
			{
				if ((zeroPage[ZeroPage.loopTrap_flag_1B] & 0x04) == 0)
					PPU_FinalizeRender();
				else
					_L3CAAC(x);
			}

			private static void _L3CAAC(byte x)
			{
				if (nesRam[NESRAM.PPU_DrawBackgrounLineCount_6D9] == 0)
					PPU_SetPPUForPalette();
				else
				{
					// PPU_DrawBGLine_Loop
					byte y = 0x01;
					byte a = nesRam[NESRAM.PPU_StagingArea_300 + x];
					if (a >= 0x80)
					{
						y = a;
						a >>= 4;
						a &= 0x04;
						a |= nesRam[NESRAM.PPUControl_2000_Settings_6D3];
						registers[Registers.PPU_Control] = a;

					}
					//__L3CACC();
				}
			}

			private static void PPU_FinalizeRender()
			{
				registers[Registers.PPU_Control] = nesRam[NESRAM.PPUControl_2000_Settings_6D3];
				registers[Registers.PPU_Mask] = nesRam[NESRAM.PPUMask_2001_Settings_6D4];
				registers[Registers.PPU_Scroll] = nesRam[NESRAM.PPUScroll_X_6D0];
				registers[Registers.PPU_Scroll] = nesRam[NESRAM.PPUScroll_Y_6D1];

			}
		}
	}
}
