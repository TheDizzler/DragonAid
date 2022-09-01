using System;
using System.Collections.Generic;
using AtomosZ.DragonAid.Libraries.PointerList;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static AtomosZ.DragonAid.ReverseEngineering.ROMPlaceholders;


namespace AtomosZ.DragonAid.ReverseEngineering
{
	public static class Bank04000
	{
		public static void ResetAPU_04000()
		{
			nesRam[NESRAM.bankSwitch_CurrentBankId] = 0x01;
			// BRKInstruction_02EF
			// BRK and BankSwitch shenanigans
			Bank7C000.DynamicSubRoutine_Setup();
		}
	}
}
