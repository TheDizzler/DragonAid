using System;
using System.Collections.Generic;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.PointerList;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static AtomosZ.DragonAid.ReverseEngineering.ROMPlaceholders;


namespace AtomosZ.DragonAid.ReverseEngineering
{
	/// <summary>
	/// This bank or Bank7C000 is always set to $C000 -$FFFF
	/// </summary>
	public static class Bank3C000
	{
		/// <summary>
		/// 0x3CC5C. Does not run when a menu is open (looping somewhere else, probably)
		/// </summary>
		public static void BottomOfStack()
		{
			GameLoop();
		}

		private static void GameLoop()
		{
			Bank38000.ReadControllerInput();
			F3CE52();
			F3CE1A();
			Bank34000.DynamicSubroutine_34000_D();
		}

		/// <summary>
		/// 0x3CE1A
		/// </summary>
		private static void F3CE1A()
		{
			if (zeroPages[0x8F] < 0x02)
				return;
			Bank38000.3B087();
		}

		/// <summary>
		/// 0x03CE52
		/// </summary>
		private static void F3CE52()
		{
			byte a = (byte)(zeroPages[0x90] & 0x0F);
			if (a != 0)
				return;
			a = zeroPages[ZeroPages.mapScrollCheck + 1];
			if (a != 0)
				return;

			byte y = 0x03;
			while (true)
			{
				a = theStack[0x10 + y];
				if (a == 0xFF)
					F3CE52_cont();
				else
				{
					a &= 0x40;
					if (a != 0)
						return;
					y += 4;
				}
			}
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
			zeroPages[ZeroPage.dynamicSubroutineAddr + 1] = a;

			// L3C208
			theStack.Push(x);
			theStack.Push(y);
			theStack.Push(y);

			zeroPages[0x1B] |= 0x01;
			a = theStack.Pop();
			zeroPages[ZeroPage.dynamicSubroutineAddr + 0] = 0;
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
			while (y < romData[Pointers.ROM.MapScrollVectorB.offset + y]);

			theStack.Push(a);

			bool hasCarry = true;
			a = y;
			a = ASMHelper.ROL(a, 1, ref hasCarry);

			a = ASMHelper.SBC(a, nesRam[0x06D7], out hasCarry); // Post_NMI_Const_NOT_00_Variable
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
			zeroPages[ZeroPage.dynamicSubroutineAddr] = a;
			a <<= 1;
			a += zeroPages[ZeroPage.dynamicSubroutineAddr];
			a <<= 4;
			hasCarry = false;
			zeroPages[ZeroPage.dynamicSubroutineAddr] = ASMHelper.ADC(a, 0x10, ref hasCarry);

			a = theStack.Pop();

			a = ASMHelper.SBC(a, 0x04, out hasCarry);

			// L3C24A Loop
			hasCarry = true;
			while (hasCarry)
			{
				a = ASMHelper.SBC(a, 0x0C, out hasCarry);
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
			a = ASMHelper.ADC(a, zeroPages[ZeroPage.dynamicSubroutineAddr], ref hasCarry);
			if (hasCarry)
				a = ASMHelper.ADC(a, 0x0F, ref hasCarry);
			byte y = a;
			zeroPages[ZeroPage.dynamicSubroutineAddr] = y;
			a = zeroPages[ZeroPage.dynamicSubroutineAddr + 1];
			if (a == 0)
			{
				TransferQuickStorageToPPU_SpriteDMA(x, y);
			}
			else if (a == 0x80)
			{  // L3C274(x, y);
				a = nesRam[NESRAM.PPU_SpriteDMA + 0 + y];
				a &= nesRam[NESRAM.PPU_SpriteDMA + 3 + y];
				if (a != 0xF8)
				{
					++x;
					++y;
					nesRam[NESRAM.PPU_SpriteDMA + y] = zeroPages[x];
					++x;
					++y;
					nesRam[NESRAM.PPU_SpriteDMA + y] = zeroPages[x];
				}
			}
			else
			{ // 3C265 - TransferPPU_SpriteDMAToQuickStorage();
				do
				{
					zeroPages[x] = nesRam[NESRAM.PPU_SpriteDMA + y];
					++x;
					++y;
				}
				while ((y &= 0x03) != 0);
			}

			// L3C29B()
			zeroPages[0x1B] &= 0xFE; // switch the bit set or release PPU trap
			y = theStack.Pop();
			hasCarry = false;
			x = ASMHelper.ADC(theStack.Pop(), 0x04, ref hasCarry);
		}


		/// <summary>
		/// 3C28F
		/// Transfer until ++y is multiple of 4
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private static void TransferQuickStorageToPPU_SpriteDMA(byte x, byte y)
		{
			do
			{
				byte a = zeroPages[0x04 + x];
				nesRam[NESRAM.PPU_SpriteDMA + y] = a;
				++x;
				++y;
			}
			while ((y & 0x03) != 0);
		}

		/// <summary>
		/// 3C341
		/// Loop until waitForNMI_Variable changes (changed in NMI)
		/// </summary>
		private static void WaitForNMI_3C000()
		{
			byte a = nesRam[NESRAM.waitForNMI_flag];
			while (a == nesRam[NESRAM.waitForNMI_flag])
				;
			WaitForNMI_Post();
		}

		/// <summary>
		/// 3C90A
		/// </summary>
		private static void WaitForNMI_Post()
		{
			if (zeroPages[0x32] != 0xFD // 0x32 - post_NMI_store
				&& (nesRam[0x60B7] & 0x20) == 0) // 0x60B7 - post_NMI_const
				Swap_SpriteDMA_Positions_orSomething();
		}

		private static void F3CDC0()
		{
			if (nesRam[NESRAM.walkDirection] != 0x03)
				return;
			// walking left
			// 3CDC8()
			if (zeroPages[ZeroPage.mapScrollCheck] != 0)
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
		/// Called right after WaitForNMI_3C000
		/// </summary>
		private static void F3D5AB()
		{
			byte x = zeroPages[ZeroPage.currentTileType];
			x &= 0x1F;
			// PPU_TileBatchInstructions_3rdByte
			if (saveRam[0x0DE0 + x] != 0
				|| (zeroPages[0x90] & 0x03) != 0
				|| (nesRam[NESRAM.walkDirection] & 0x01) != 0)
				return;

			WaitForNMI_X_Frames(0x03);
		}

		/// <summary>
		/// Called write before WaitForNMI_3C000. Sprite stuff calls this twice.
		/// </summary>
		public static void F3F116()
		{
			if ((zeroPages[ZeroPage.lightOrDarkWorld] & 0x01) == 0)
			{
				SetSegmentPointerToStack();
				return;
			}

			byte a = (byte)(zeroPages[0x90] & 0x0F);
			if (a == 0 || a == 0x0F)
				return;
			a -= 0x01;
			a <<= 3;
			zeroPages[0x72 + 0] = a;
			zeroPages[0x72 + 1] = 0x01;
			L3F135();
		}

		/// <summary>
		/// 3F0EA
		/// </summary>
		private static void SetSegmentPointerToStack()
		{
			byte x = 0;
			byte a = (byte)(zeroPages[0x90] & 0x0F);
			if (a != 0x01)
			{
				x = 0x08;
				if (a != 0x02)
					return;
			}
			// _L3F0FA_
			zeroPages[0x72] = x;
			zeroPages[0x73] = 0x01;
			a = (byte)(zeroPages[0xAC] & 0x1F);
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
			byte x = zeroPages[0x72];
			x >>= 1;
			byte a = zeroPages[NESRAM.Character_Statuses + x + 0];
			if (a < 80)
			{
				L3F1F8();
				return;
			}

			a = (byte)(zeroPages[NESRAM.Character_Statuses + x + 1] & 0x40);
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
			if (zeroPages[0x72] < 0x10) // dialogSegmentPointer
			{
				L3F1F5();
				return;
			}

			byte x = zeroPages[0x80]; // charsCopied
			byte y = zeroPages[0x81]; // linesWrittenToStagingArea
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
			zeroPages[0x72] += 0x04;
		}

		private static void F3F259()
		{
			byte y = 0x02;
			byte a = nesRam[zeroPages[0x72] + zeroPages[0x73] << 4 + y];
			a &= 0x0F;
			F3F8D7();
			y = 0x03;
			a = nesRam[zeroPages[0x72] + zeroPages[0x73] << 4 + y];
			theStack.Push(a);
			y = nesRam[NESRAM.encounterCheckRequired_A];
			if (y >= 0x07 && y < 0x0A)
				a = 0x02;
			//_L3F274_
			F3F909();
			a = theStack.Pop();
			a &= 0x3C;
			y = a;
			if (y != 0 || zeroPages[0x72] != 0)
			{   //L3F282()
				F3F385();
				L3F32F();
			}
		}




		public static void F3CDCF()
		{
			if (zeroPages[0xAA] == 0)
				L3CDDA();
			else
			if ((byte)(zeroPages[0x90] & 0x01) == 0)
			{
				// CDE0()
			}
		}

		private static void L3CDDA()
		{
			if ((byte)(zeroPages[0x87] & 0x01) == 0)
				return;
			Bank18000.DynamicSubroutine_18000_A();
		}



		public static void F3DAF7()
		{
			// Load Bank14000 - not sure why though
			SetPPUScroll();
		}

		private static void SetPPUScroll()
		{
			if (zeroPages[0x87] == 0) // mapScrollCheck+1
				return;
			byte a = 0xFF;
			zeroPages[0x86] = a; // mapScrollCheck+0
			a = --zeroPages[0x87]; // mapScrollCheck+1
			if (a == 0)
				zeroPages[0x86] = a; // mapScrollCheck+0
			F3DF08();
			switch (walkDirection)
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
			// --PPUScroll_X
			// if PPUScrol_X == 0xFF // did it wrap around?
			// PPUScrollWrap()
		}


		private static void F3DF08()
		{ // 3DF08
			zeroPages[0x09] = (byte)(zeroPages[ZeroPage.lightOrDarkWorld] & 0x01); // 0x09 - menu_PPUAddress_1
			zeroPages[0x0B] = 0;
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
			byte a = (byte)(zeroPages[0x87] & 0x07); // mapScrollCheck+1
			if (a == 0 || a == 0x07)
				return;
			// 3DF5B
			if (zeroPages[ZeroPage.mapScrollCheck + 1] == 0x0B)
				F3E1E8();
			else if (zeroPages[ZeroPage.mapScrollCheck + 1] > 0x0B)
				F3E198();
			else if (zeroPages[ZeroPage.mapScrollCheck + 1] >= 0x09)
				F3DD75();
			else if (zeroPages[ZeroPage.mapScrollCheck + 1] == 0x03)
				return;
			else if (zeroPages[ZeroPage.mapScrollCheck + 1] > 0x03)
				F3DCCB();
			else // < 0x03
				F3DD7B();
		}

		public static void BankSwitchPre(byte bankId)
		{
			nesRam[NESRAM.bankSwitch_CurrentBankId] = bankId;
			BankSwitch(bankId);
		}

		/// <summary>
		/// <para>3FF94 - identical to 3C000.BankSwitch
		/// <br>Switch out 0x8000 to 0xBFFF with bank from ROM.</br>
		/// <br>If BankId &lt; 0x10, 0xC000 to 0xFFFF is set to ($0F) $3C000 bank from ROM.</br>
		/// <br>If BankId &gt;= 0x10, 0xC000 to 0xFFFF is set to ($1F) $7C000 bank from ROM.</br>
		/// </para>
		/// </summary>
		/// <param name="bankId"></param>
		public static void BankSwitch(byte bankId)
		{

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
				if ((byte)(zeroPages[ZeroPage.loopTrap_flag] & 0x10) == 0)
				{
					zeroPages[0x33] = 0;
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
				zeroPages[0xA2] = theStack[x + 6];
				zeroPages[0xA1] = theStack[x + 5]; // this the address where when NMI kicked in
				int pcPreNMI = zeroPages[0xA1] + zeroPages[0xA2] << 8;
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
				byte a = zeroPages[0x32]; // post_NMI_store
				if (a == 0 || a == 0xFD)
					PPU_CheckRenderType(x);
				else if (a == 0xFE)
					PPU_Render();
			}

			private static void PPU_CheckRenderType(byte x)
			{
				byte a = zeroPages[ZeroPage.PPU_renderStart_flag];
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
				if (zeroPages[ZeroPage.PPU_render_flag] == 0
					&& (zeroPages[ZeroPage.loopTrap_flag] & 0x04) == 0)
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
				zeroPages[ZeroPage.loopTrap_flag] = (byte)(zeroPages[ZeroPage.loopTrap_flag] | 0x04);
				if (zeroPages[ZeroPage.PPU_render_flag] != 0)
				{
					zeroPages[ZeroPage.PPU_render_flag] = x;
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
				if ((zeroPages[ZeroPage.loopTrap_flag] & 0x01) == 0)
					PPU_TransferSpritesFrom0x0200ToPPU(x);
				else
					PPU_CheckIfSkipToFinalize(x);
			}

			/// <summary>
			/// 3CA9F
			/// </summary>
			private static void PPU_TransferSpritesFrom0x0200ToPPU(byte x)
			{
				Register.SpriteDMA = 0x02; // this is high byte of address to copy from
				zeroPages[0x33] = 0x02;
				PPU_CheckIfSkipToFinalize(x);
			}

			/// <summary>
			/// 3CAA6
			/// </summary>
			private static void PPU_CheckIfSkipToFinalize(byte x)
			{
				if ((zeroPages[ZeroPage.loopTrap_flag] & 0x04) == 0)
					PPU_FinalizeRender();
				else
					_L3CAAC(x);
			}

			private static void _L3CAAC(byte x)
			{
				if (nesRam[NESRAM.PPU_DrawBackgrounLineCount] == 0)
					PPU_SetPPUForPalette();
				else
				{
					// PPU_DrawBGLine_Loop
					byte y = 0x01;
					byte a = nesRam[NESRAM.PPU_StagingArea + x];
					if (a >= 0x80)
					{
						y = a;
						a >>= 4;
						a &= 0x04;
						a |= nesRam[NESRAM.PPUControl_2000_Settings];
						registers[Register.PPU_Control] = a;

					}
					//__L3CACC();
				}
			}

			private static void PPU_FinalizeRender()
			{
				registers[Register.PPU_Control] = nesRam[NESRAM.PPUControl_2000_Settings];
				registers[Register.PPUMask] = nesRam[NESRAM.PPUMask_2001_Settings];
				registers[Register.PPUScroll] = nesRam[NESRAM.PPUScroll_X];
				registers[Register.PPUScroll] = nesRam[NESRAM.PPUScroll_Y];

			}
		}
	}
}
