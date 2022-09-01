using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtomosZ.DragonAid.Libraries;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static AtomosZ.DragonAid.ReverseEngineering.ROMPlaceholders;

namespace AtomosZ.DragonAid.ReverseEngineering
{
	internal class Bank18000
	{
		/// <summary>
		/// 0x1B6BB
		/// </summary>
		public static void CheckDayNightTransitionTimes()
		{
			byte a = zeroPages[0xAA];
			if ((a & 0x10) != 0)
			{
				L1B6E3();
				return;
			}

			a = ASMHelper.LSR(a, 1, out bool hasCarry);
			if (hasCarry)
			{
				L1B736();
				return;
			}

			byte x = 0;
			while (x < 0x07)
			{ // _DynamicSubroutine_18000_A_Loop_
				a = romData[ROM.DayNightTransitionTimes.offset + x];
				if (a == nesRam[NESRAM.timeOfDay])
				{
					L1B6D6();
					return;
				}

				++x;
			}
		}
	}
}
