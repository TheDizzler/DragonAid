using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.Pointers;
using static AtomosZ.DragonAid.ReverseEngineering.ROM;

namespace AtomosZ.DragonAid.ReverseEngineering
{
	public static class Bank14000
	{
		/// <summary>
		/// 0x17A69
		/// DynamicSubroutine 12
		/// </summary>
		public static void Map_Scroll_Check()
		{
			if (ROM.zeroPages[0x86] == 0
				|| ROM.zeroPages[0x87] != 0)
				Map_Scroll();
		}

		/// <summary>
		/// 0x17A7A
		/// Sub of DynamicSubroutine 12
		/// </summary>
		private static void Map_Scroll()
		{
			byte x = walkDirection << 1; // 0x0644
			zeroPages[0x72] = romData[ROMPointers.MapScrollVector.offset + x];
			zeroPages[0x73] = romData[ROMPointers.MapScrollVector.offset + x + 1];

			x = 0x10;
			zeroPages[0x6C] = x;
			zeroPages[0x6D] = 0x02;

			while (x != 0)
			{ // loop through PPU_SpriteDMA until find a value that is NOT 0xF8
				byte a = nesRam[0x200 + 2 + x];
				if (a != 0xF8)
				{
					if (!PPU_SpriteDMA_NOT_F8(x))
						break;
				}
				else // the above sometimes (always?) falls into this code
				{
					a = zeroPages[0x6C];
					bool hasCarry = false;
					a = ASMHelper.ADC(a, 0x04, ref hasCarry);
					zeroPages[0x6C] = a;
					x = a;
				}
			}

			if (nesRam[0x118] == 0)
				return;
			L17B42();
		}

		/// <summary>
		/// 17B42
		/// </summary>
		private static void L17B42()
		{
			byte x = 0;
			while (x != 0x40)
			{
				F17B51(x);
				bool hasCarry = false;
				x = ASMHelper.ADC(x, 0x04, ref hasCarry);
			}
		}

		/// <summary>
		/// 17B51
		/// increments or decrements variables in The Stack....why?
		/// </summary>
		private static void F17B51(byte x)
		{
			if (nesRam[0x130 + x] == 0xF8)
				return;
			if (walkDirection == up
					&& ++nesRam[0x128 + x] == 0xEB)
				Map_Scroll_WalkDirection_Wrap();

			else if (walkDirection == right
					&& --nesRam[0x131 + x] == 0xFF)
				Map_Scroll_WalkDirection_Wrap();

			else if (walkDirection == down
					&& --nesRam[0x128 + x] == 0xF3)
				Map_Scroll_WalkDirection_Wrap();

			else if (++nesRam[0x131 + x] == 0)
				Map_Scroll_WalkDirection_Wrap();
		}

		private static bool PPU_SpriteDMA_NOT_F8(byte x)
		{
			if (zeroPages[ZeroPagePointers.encounterVariable_A] != 0x01)
				return Map_Scroll_JMP_DialogSegmentPointer(x);
			else
			{

			}
		}

		private static bool Map_Scroll_JMP_DialogSegmentPointer(int directionMoving, byte x, bool isZero)
		{
			if (movingLeft)
				isZero = ++nesRam[0x200 + 3 + x] == 0;
			if (movingLeft || movingDown)
			{
				if (isZero)
					Map_Scroll_Wrap();
				else
					return true;
			}
		}
	}
}
