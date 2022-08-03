using System;
using System.Collections.Generic;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.Pointers;
using static AtomosZ.DragonAid.ReverseEngineering.ROM;


namespace AtomosZ.DragonAid.ReverseEngineering
{
	/// <summary>
	/// This bank or Bank7C000 is always set to $C000 -$FFFF
	/// </summary>
	public static class Bank3C000
	{
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
			zeroPages[ZeroPagePointers.dynamicSubroutineAddr + 1] = a;

			// L3C208
			theStack.Push(x);
			theStack.Push(y);
			theStack.Push(y);

			zeroPages[0x1B] |= 0x01;
			a = theStack.Pop();
			zeroPages[ZeroPagePointers.dynamicSubroutineAddr + 0] = 0;
			y = 0x05;

			do
			{ // C21A
				--y;
				if (y >= 0x80)
				{
					__L3C251_(a, x);
					return;
				}
			}
			while (y < romData[ROMPointers.MapScrollVectorB.offset + y]);

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
			zeroPages[ZeroPagePointers.dynamicSubroutineAddr] = a;
			a <<= 1;
			a += zeroPages[ZeroPagePointers.dynamicSubroutineAddr];
			a <<= 4;
			hasCarry = false;
			zeroPages[ZeroPagePointers.dynamicSubroutineAddr] = ASMHelper.ADC(a, 0x10, ref hasCarry);

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
			a = ASMHelper.ADC(a, zeroPages[ZeroPagePointers.dynamicSubroutineAddr], ref hasCarry);
			if (hasCarry)
				a = ASMHelper.ADC(a, 0x0F, ref hasCarry);
			byte y = a;
			zeroPages[ZeroPagePointers.dynamicSubroutineAddr] = y;
			a = zeroPages[ZeroPagePointers.dynamicSubroutineAddr + 1];
			if (a == 0)
			{
				TransferQuickStorageToPPU_SpriteDMA(x, y);
			}
			else if (a == 0x80)
			{  // L3C274(x, y);
				a = nesRam[NESRAMPointers.PPU_SpriteDMA + 0 + y];
				a &= nesRam[NESRAMPointers.PPU_SpriteDMA + 3 + y];
				if (a != 0xF8)
				{
					++x;
					++y;
					nesRam[NESRAMPointers.PPU_SpriteDMA + y] = zeroPages[x];
					++x;
					++y;
					nesRam[NESRAMPointers.PPU_SpriteDMA + y] = zeroPages[x];
				}
			}
			else
			{ // 3C265 - TransferPPU_SpriteDMAToQuickStorage();
				do
				{
					zeroPages[x] = nesRam[NESRAMPointers.PPU_SpriteDMA + y];
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
				nesRam[NESRAMPointers.PPU_SpriteDMA + y] = a;
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
			byte a = nesRam[NESRAMPointers.waitForNMI_flag];
			while (a == nesRam[NESRAMPointers.waitForNMI_flag])
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
			if (nesRam[NESRAMPointers.walkDirection] != 0x03)
				return;
			// walking left
			// 3CDC8()
			if (zeroPages[ZeroPagePointers.mapScrollCheck] != 0)
				return;
		}

		private static void F3F116()
		{
			if ((zeroPages[ZeroPagePointers.lightOrDarkWorld] & 0x01) == 0)
			{
				SetSegmentPointerToStack();
				return;
			}

			byte a = (byte)(zeroPages[0x90] & 0x0F);
			if (a == 0)
				return;
			if (a == 0x0F)
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
			byte a = zeroPages[NESRAMPointers.Character_Statuses + x + 0];
			if (a < 80)
			{
				L3F1F8();
				return;
			}

			a = (byte)(zeroPages[NESRAMPointers.Character_Statuses + x + 1] & 0x40);
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
			y = nesRam[NESRAMPointers.encounterCheckRequired_A];
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
			zeroPages[0x09] = (byte)(zeroPages[ZeroPagePointers.lightOrDarkWorld] & 0x01); // 0x09 - menu_PPUAddress_1
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
			// what happens from here?
		}



		/// <summary>
		/// 3C96C
		/// </summary>
		public static void NMI_VBlank_3C000()
		{
			if ((byte)(zeroPages[ZeroPagePointers.loopTrap_flag] & 0x10) == 0)
			{
				zeroPages[0x33] = 0;
				PPU_Update();
			}
			// _L3C97E_
			//++nesRam[0xFFDF]; // what the hell is this??
			BankSwitch(0x1E);
		}

		/// <summary>
		/// 3CA5C
		/// </summary>
		private static void PPU_Update()
		{
			byte a = RegisterPointers.PPU_Status_2002; // reset write toggle
		}
	}
}
