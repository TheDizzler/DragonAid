using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomosZ.DragonAid.Libraries
{
	public class DynamicPointerData
	{
		public List<DynamicSubroutine> subroutines07;
		public List<DynamicSubroutine> subroutines17;
		public List<DynamicSubroutine> localPointers;

		public List<DynamicSubroutine> FindAddress(int address)
		{
			var finds = new List<DynamicSubroutine>();
			foreach (var addr in subroutines07)
				if (addr.prgAddress.pointer == address)
					finds.Add(addr);
			foreach (var addr in subroutines17)
				if (addr.prgAddress.pointer == address)
					finds.Add(addr);
			foreach (var addr in localPointers)
				if (addr.prgAddress.pointer == address)
					finds.Add(addr);
			return finds;
		}
	}
}
