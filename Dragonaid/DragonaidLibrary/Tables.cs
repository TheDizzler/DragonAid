using System.Collections.Generic;

namespace AtomosZ.DragonAid.TextToHex
{
	public static class Tables
	{
		public static Dictionary<byte, string> textTable = new Dictionary<byte, string>()
		{
			{ 0x00, "§" }, // Empty Text at end of names. Dialog text disregards.
			{ 0x01, "0" }, { 0x02, "1" }, { 0x03, "2" }, { 0x04, "3" }, { 0x05, "4" },
			{ 0x06, "5" }, { 0x07, "6" }, { 0x08, "7" }, { 0x09, "8" }, { 0x0A, "9" },
			{ 0x0B, "a" }, { 0x0C, "b" }, { 0x0D, "c" }, { 0x0E, "d" }, { 0x0F, "e" }, { 0x10, "f" },
			{ 0x11, "g" }, { 0x12, "h" }, { 0x13, "i" }, { 0x14, "j" }, { 0x15, "k" }, { 0x16, "l" },
			{ 0x17, "m" }, { 0x18, "n" }, { 0x19, "o" }, { 0x1A, "p" }, { 0x1B, "q" }, { 0x1C, "r" },
			{ 0x1D, "s" }, { 0x1E, "t" }, { 0x1F, "u" }, { 0x20, "v" }, { 0x21, "w" }, { 0x22, "x" },
			{ 0x23, "y" }, { 0x24, "z" },
			{ 0x25, "A" }, { 0x26, "B" }, { 0x27, "C" }, { 0x28, "D" }, { 0x29, "E" }, { 0x2A, "F" },
			{ 0x2B, "G" }, { 0x2C, "H" }, { 0x2D, "I" }, { 0x2E, "J" }, { 0x2F, "K" }, { 0x30, "L" },
			{ 0x31, "M" }, { 0x32, "N" }, { 0x33, "O" }, { 0x34, "P" }, { 0x35, "Q" }, { 0x36, "R" },
			{ 0x37, "S" }, { 0x38, "T" }, { 0x39, "U" }, { 0x3A, "V" }, { 0x3B, "W" }, { 0x3C, "X" },
			{ 0x3D, "Y" }, { 0x3E, "Z" },

			/// Letters used in class abbreviations. The lower case letters are smaller than normal.
			{ 0x40, "H" }, { 0x41, "r" },
			{ 0x42, "S" }, { 0x43, "r" },
			{ 0x44, "P" }, { 0x45, "r" },
			{ 0x46, "W" }, { 0x47, "z" },
			{ 0x48, "F" }, { 0x49, "r" },
			{ 0x4A, "M" }, { 0x4B, "r" },
			{ 0x4C, "G" }, { 0x4D, "f" },
			{ 0x4E, "S" }, { 0x4F, "g" },

			{ 0x50, "•" },  // <SPACE> - As entered in the name selection menu.
			{ 0x51, "_" },	// <blank>

			{ 0x52, "t" },	// thin
			{ 0x53, "u" },	// thin
			{ 0x54, "v" },	// thin
			{ 0x55, "w" },	// thin
			{ 0x56, "x" },	// thin and curly

			{ 0x57, " ." },	// right justified period. Used at $41DD8 and $41027
			{ 0x58, "—" },	// wide dash
			{ 0x5A, "*" },

			{ 0x5B, "Br" },
			{ 0x5C, "Ma" },
			{ 0x5D, "Bi" },
			{ 0x5E, "Me" },


			{ 0x60, "∙" },	// space in speech. This used to mark end of printable text in Text Staging Area.
			{ 0x62, "`" },	// Opening text apostrophe without newline
			{ 0x63, "”" },	// closing double quote
			{ 0x64, "→" },
			{ 0x65, "“" },	// opening double quote
			{ 0x66, "`" },	// Opening text apostrophe with newline
			{ 0x67, "´" },	// Closing single quote
			{ 0x68, "'" },	// apostrophe
			{ 0x69, ".´" },	// speech period with closing quote.
			{ 0x6A, "," },	// comma
			{ 0x6B, "-" },
			{ 0x6C, "." },	// Normal period.
			{ 0x6D, "(" },
			{ 0x6E, ")" },
			{ 0x6F, "?" },
			{ 0x70, "!" },
			{ 0x71, ";" },
			{ 0x72, "'¿" },	// another apostrophe that's never used?
			{ 0x73, "►" },	// menu select arrow
			{ 0x74, "↓" },	// waiting for input arrow
			{ 0x75, ":" },
			{ 0x76, ".." },	// double period. Used BEFORE a regular period to create ellipsis.

			{ 0x86, "A" },	// thinner letters
			{ 0x87, "B" },
			{ 0x88, "C" },
			{ 0x89, "D" },
			{ 0x8A, "F" },
			
			/// monster endings
			{ 0xA0, "{y/ies}" },	// plural ending for special names (Mummy, Grizzly, Fly)
			{ 0xA1, "{an/en}" },	// plural ending for special names (Mummy Man, Henchman)
			{ 0xA2, "{ol/lls?}" },	// plural ending for special names (Toadstool/Toadstolls?) 
			{ 0xA3, "{?/ls}" },		// plural ending for special names (Slime Snai)
			{ 0xA4, "{es}" },		// plural ending for special names (Witch, Goopi)
			{ 0xA5, "{s}" },		// plural ending for normal names  (most)
			{ 0xA6, "{☼}" },		// plural ending for special names (Lav? Lava?)
			{ 0xA7, "{◙}" },		// plural ending for special names (Lumpus, Barnabas, Crabus, Tentacles, Kragacles)

			/// gender pronouns
			{ 0xB0, "{his/her}" },
			{ 0xB1, "{himself/herself}" },
			{ 0xB2, "{he/she}" },

			{ 0xC0, "(s)" },	// if plural add 's'

			{ 0xEB, "¶" },	// Line break
			{ 0xEC, "◘" },	// special end of text (for temp storage only?)
			{ 0xEE, "∩" },	// End of text, don't wait for input
			{ 0xEF, "Ω" },	// end of text, wait for input
			{ 0xFE, "┐" },	// special end of text (used after plural/singular endings, counters, pronouns)

			{ 0xF0, "{Class}" },
			{ 0xF2, "{-A}" },		// A letter indicator for enemies, ie Slime-B. Follows a {name}.
			{ 0xF4, "{ITEM}" },
			{ 0xF5, "{NAME}" },		// Actor name
			{ 0xF7, "{COUNT}" },	// F7 {NAME} appear!
			{ 0xF8, "{NUM}" },		// ????
			{ 0xF9, "{PCNAME}" },		// F9 + character index = character's name

			{ 0xFD, "▼" },	// Wait for input.

			
			/// menu box drawing characters
			{ 0x77, "║" },	// left aligned vertical bar
			{ 0x78,"═" },	// top aligned horizontal bar.
			{ 0x79,"═" },	// top aligned horizontal bar with a notch on the right side for menu title
			{ 0x7A, "╔" },	// top left corner
			{ 0x7B, "╚" },	// bottom left corner
			{ 0x7C, "║" },	// right aligned vertical bar
			{ 0x7D, "╗" },	// top right corner
			{ 0x7E, "═" },	// bottom aligned horizontal bar
			{ 0x7F, "╝" },	// bottom right corner
			{ 0x5F, "║E" },	// Border with E (small) to indicate equiped item
		};

		private static Dictionary<string, byte> reverseDict;

		public static Dictionary<string, byte> textToHexDict
		{
			get
			{
				if (reverseDict == null)
				{
					reverseDict = new Dictionary<string, byte>();
					foreach (var val in textTable)
					{
						if (!reverseDict.ContainsKey(val.Value))
							reverseDict.Add(val.Value, val.Key);
					}
				}

				return reverseDict;
			}
		}
	}
}
