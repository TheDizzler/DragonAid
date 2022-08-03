using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtomosZ.DragonAid.Libraries.Pointers;
using AtomosZ.DragonAid.TextToHex;

namespace AtomosZ.DragonAid.Libraries
{
	public static class Names
	{
		public static string GetMonsterName(byte[] romData, byte monsterIndex)
		{
			int monsterList1Address = romData[ROMPointers.NamePointers.offset + 4]
				+ (romData[ROMPointers.NamePointers.offset + 5] << 8) + Address.INESHeaderLength;
			int monsterList2Address = romData[ROMPointers.NamePointers.offset + 6]
				+ (romData[ROMPointers.NamePointers.offset + 7] << 8) + Address.INESHeaderLength;


			string name = "";

			name += GetNamePart(romData, monsterList1Address, monsterIndex);
			name += " " + GetNamePart(romData, monsterList2Address, monsterIndex);

			return name;
		}

		public static List<string> GetItemNames(byte[] romData)
		{
			int itemList1Address = romData[ROMPointers.NamePointers.offset + 0]
				+ (romData[ROMPointers.NamePointers.offset + 1] << 8) + Address.INESHeaderLength;
			int itemList2Address = romData[ROMPointers.NamePointers.offset + 2]
				+ (romData[ROMPointers.NamePointers.offset + 3] << 8) + Address.INESHeaderLength;

			List<string> items = new List<string>();
			for (byte i = 0; i <= 0x7F; ++i)
			{
				string name = i.ToString("X2") + " - ";

				name += GetNamePart(romData, itemList1Address, i);
				name += " " + GetNamePart(romData, itemList2Address, i);
				items.Add(name);
			}

			items.Add("FF - Nothing");
			return items;
		}

		private static string GetNamePart(byte[] romData, int nameAddress, byte nameIndex)
		{
			int nameCount = 0;
			int charCount = 0;
			byte currentChar;
			string name = "";
			while (nameCount < nameIndex)
			{
				while ((currentChar = romData[nameAddress + charCount++]) != 0xFF)
				{ }
				++nameCount;
			}

			while ((currentChar = romData[nameAddress + charCount++]) != 0xFF)
			{
				name += Tables.textTable[currentChar];
			}

			return name;
		}
	}
}
