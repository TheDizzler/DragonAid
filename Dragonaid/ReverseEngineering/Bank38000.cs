using System;
using System.Collections.Generic;
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
			if (zeroPages[ZeroPage.mapScrollCheck + 1] == 0
				&& zeroPages[ZeroPage.mapScrollCheck + 0] != 0)
			{
				if (zeroPages[0x8F] != 0)
					return;
				Controller_1_ReadAndSet();
				nesRam[0x06BD] |= zeroPages[ZeroPage.controller_SingleButton_store];
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
	}
}
