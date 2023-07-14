using System;
using System.Collections.Generic;
using AtomosZ.DragonAid.TextToHex;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;

namespace AtomosZ.DragonAid.Libraries
{
	public static class Names
	{
		public static string GetMonsterName(byte[] romData, byte monsterIndex)
		{
			int monsterList1Address = romData[ROM.NamePointers.iNESAddress + 4]
				+ (romData[ROM.NamePointers.iNESAddress + 5] << 8) + Address.iNESHeaderLength;
			int monsterList2Address = romData[ROM.NamePointers.iNESAddress + 6]
				+ (romData[ROM.NamePointers.iNESAddress + 7] << 8) + Address.iNESHeaderLength;


			string name = "";

			name += GetNamePart(romData, monsterList1Address, monsterIndex);
			name += " " + GetNamePart(romData, monsterList2Address, monsterIndex);

			return name;
		}

		public static List<string> GetItemNames(byte[] romData)
		{
			int itemList1Address = romData[ROM.NamePointers.iNESAddress + 0]
				+ (romData[ROM.NamePointers.iNESAddress + 1] << 8) + Address.iNESHeaderLength;
			int itemList2Address = romData[ROM.NamePointers.iNESAddress + 2]
				+ (romData[ROM.NamePointers.iNESAddress + 3] << 8) + Address.iNESHeaderLength;

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
