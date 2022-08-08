using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static AtomosZ.DragonAid.ReverseEngineering.ROMPlaceholders;

namespace AtomosZ.DragonAid.ReverseEngineering
{
	internal class Bank28000
	{
		/// <summary>
		/// 0x28162
		/// </summary>
		public static void DynamicSubroutine_28000_B()
		{
			if ((zeroPages[0x2F] & 0x01) != 0
				&& theStack[0x76] != 0)
				L2816E();
		}
	}
}
