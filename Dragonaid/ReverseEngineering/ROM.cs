using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomosZ.DragonAid.ReverseEngineering
{
	public static class ROM
	{
		public static byte[] romData;
		public static byte[] zeroPages = new byte[0x100];
		/// <summary>
		/// This is first 2000bytes of CPU Memory, including zeroPages [0x000-0x099] and TheStack [0x100-0x200].
		/// </summary>
		public static byte[] nesRam = new byte[0x2000];

		public static Stack<byte> theStack = new Stack<byte>();
	}
}
