using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;

namespace AtomosZ.DragonAid.Libraries
{
	public class Address
	{
		public const int iNESHeaderLength = 0x10;
		public string name { get; private set; }
		/// <summary>
		/// Real address of data (without iNES header).
		/// </summary>
		public int pointer { get; private set; }
		/// <summary>
		/// iNES offset address.
		/// </summary>
		public int iNESAddress
		{
			get
			{
				if (pointer == -1)
					return -1;
				return pointer + iNESHeaderLength;
			}
		}
		/// <summary>
		/// in bytes.
		/// </summary>
		public int length;

		public string notes;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="pointer">Real (without iNES header) address</param>
		/// <param name="length"></param>
		public Address(string name, int pointer, int length = -1)
		{
			this.name = name;
			this.pointer = pointer;
			this.length = length;
		}

		/// <summary>
		/// !Always returns false!
		/// </summary>
		/// <param name="newAddress"></param>
		/// <returns></returns>
		public bool ValidateAddress(string newAddress)
		{
			return false;
		}

		public void Add(byte operand)
		{
			pointer += operand;
		}
	}
}
