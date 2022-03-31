using AtomosZ.Dragonaid.Libraries;

namespace AtomosZ.Dragonaid
{
	public static class PointerTables
	{
		public static Address weaponAttackPowers = new Address("Weapon attack powers", 0x027990)
		{
			length = 32,

			notes = "0x0279A0 = Cypress Stick, 0x0279A1 = Club, 0x0279A2 = Copper Sword, "
				+ "0x0279A3 = Magic Knife, 0x0279A4 = Iron Spear, 0x0279A5 = Battle Axe, "
				+ "0x0279A6 = Broad Sword, 0x0279A7 = Wizard's Wand, 0x0279A8 = Poison Needle, "
				+ "0x0279A9 = Iron Claw, 0x0279AA = Thorn Whip, 0x0279AB = Giant Shears, "
				+ "0x0279AC = Chain Sickle, 0x0279AD = Thor's Sword, 0x0279AE = Snowblast Sword, "
				+ "0x0279AF = Demon Axe, 0x0279B0 = Staff of Rain, 0x0279B1 = Sword of Gaia, "
				+ "0x0279B2 = Staff of Reflection, 0x0279B3 = Sword of Destruction, 0x0279B4 = Multi-Edge Sword, "
				+ "0x0279B5 = Staff of Force, 0x0279B6 = Sword of Illusion, 0x0279B7 = Zombie Slasher, "
				+ "0x0279B8 = Falcon Sword, 0x0279B9 = Sledge Hammer, 0x0279BA = Thunder Sword, "
				+ "0x0279BB = Staff of Thunder, 0x0279BC = Sword of Kings, 0x0279BD = Orochi Sword, "
				+ "0x0279BE = Dragon Killer, 0x0279BF = Staff of Judgement",
		};

		public static Address armorDefencePowers = new Address("Armor Defence Powers", 0x0279B0)
		{
			length = 24,
			notes = "0x0279C0 = Clothes, 0x0279C1 = Training Suit, 0x0279C2 = Leather Armor, " +
				"0x0279C3 = Flashy Clothes, 0x0279C4 = Half Plate Armor, 0x0279C5 = Full Plate Armor, " +
				"0x0279C6 = Magic Armor, 0x0279C7 = Cloak of Evasion, 0x0279C8 = Armor of Radiance, " +
				"0x0279C9 = Iron Apron, 0x0279CA = Animal Suit, 0x0279CB = Fighting Suit, " +
				"0x0279CC = Sacred Robe, 0x0279CD = Armor of Hades, 0x0279CE = Water Flying Cloth, " +
				"0x0279CF = Chain Mail, 0x0279D0 = Wayfarers Clothes, 0x0279D1 = Revealing Swimsuit, " +
				"0x0279D2 = Magic Bikini, 0x0279D3 = Shell Armor, 0x0279D4 = Armor of Terrafirma, " +
				"0x0279D5 = Dragon Mail, 0x0279D6 = Swordedge Armor, 0x0279D7 = Angel's Robe"
		};

		public static Address shieldDefencePowers = new Address("Shield Defence Powers", 0x0279C8)
		{
			length = 7,
			notes = "0x0279D8 = Leather Shield, 0x0279D9 = Iron Shield, 0x0279DA = Shield of Strength, "
				+ "0x0279DB = Shield of Heroes, 0x0279DC = Shield of Sorrow, 0x0279DD = Bronze Shield, "
				+ "0x0279DE = Silver Shield",
		};

		public static Address helmetDefencePowers = new Address("Helmet Defence Powers", 0x0279CF)
		{
			length = 7,
			notes = "0x0279DF = Golden Crown, 0x0279E0 = Iron Helmet, 0x0279E1 = Mysterious Hat, "
				+ "0x0279E2 = Unlucky Helmet, 0x0279E3 = Turban, 0x0279E4 = Noh Mask, "
				+ "0x0279E5 = Leather Helmet, 0x0279E6 = Iron Mask",
		};

		public static Address pointerToTextVariables = new Address("Pointer to pronouns", 0x3AD9D)
		{
			// last byte: $3ADEA
			length = 78,
			notes = "2 bytes per pronoun, little endian. Somehow it knows the first byte of the target address is 03.\n"
				+ "his $EBAD, himself $EFAD, he $F7AD, "
				+ "him $FAAD, man $FEAD, He's $02AE, son $07AE, her $0BAE, herself $0FAE, she $17AE, "
				+ "her $1BAE, lady $1FAE, She's $24AE, daughter $2AAE, y $33AE, an $35AE, ol $38AE, i $3BAE, "
				+ "nothing $3DAE, nothing $3EAE, a $3FAE, nothing $41AE, ies $42AE, en $46AE, lls $49AE, ls $4DAE, "
				+ "es $50AE, s $53AE, e $55AE, nothing $57AE, One $58AE, ? $58AE !Dupe, Two $5CAE, Three $60AE, "
				+ "Four $66AE, Five $6BAE, Six $70AE, Seven $74AE, Eight $7AAE",
		};

		public static Address textVariables = new Address("Pronoun and conjugation related variables", 0x3ADEB)
		{
			// Last byte: $3AE7F (probably) - 149 bytes
			length = 149,

			notes = "his $3ADEB, himself $3ADEF, he $3ADF7, "
				+ "him $FAAD, man $FEAD, He's $02AE, son $07AE, her $AE0B, herself $AE0F, she $AE17, "
				+ "her $AE1B, lady $AE1F, She's $AE24, daughter $AE2A, y $AE33, an $AE35, ol $AE38, i $AE3B, "
				+ "nothing $AE3D, nothing $AE3E, a $AE3F, nothing AE41, ies $AE42, en $AE46, lls $AE49, ls $AE4D, "
				+ "es $AE50, s $AE53, e $AE55, nothing $AE57, One $AE58, ? $AE58 !Dupe, Two $AE5C, Three $AE60, "
				+ "Four $AE66, Five $AE6B, Six $AE70, Seven $AE74, Eight $7AAE",
		};

		//public static DataAddr[] dialogBlocks = new DataAddr[]
		//{
		//	/* Bank 1 (pointer hibyte -80) */
		//	new DataAddr() // GUESS
		//	{ // BLOCK A
		//		pointer = 0x40000,
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x28070 (00 80))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "Combat Text 1/??",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x40081, // NOT CORRECT? Either 4007D or 40099 seems more likely. Maybe this isn't used.
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x28072 (81 80))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "",
		//	},

		//	new DataAddr() // GUESS
		//	{ // BLOCK C
		//		pointer = 0x402FD, // Seems legit
		//		length = 482,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x28074 (FD 82))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "Combat text 2/??",
		//	},

		//	new DataAddr() // GUESS
		//	{ // BLOCK D
		//		pointer = 0x404DE, // Seems legit
		//		length = 493,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x28076 (DE 84))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "Combat text 3/??",
		//	},

		//	new DataAddr() // GUESS
		//	{ // BLOCK E
		//		pointer = 0x406CA, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x28078 (CA 86))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "Combat text 4/??",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x408A8, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x2807A (A8 88))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "Combat text 5/??",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x40A9F, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x2807C (9F 8A))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "Combat text 6/??",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x40CC5, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x2807E (C5 8C))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "Combat text 7/??",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x40ECB, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x28080 (CB 8E))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "Combat text 8/??",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x410D5, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x28082 (D5 90))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "Combat text 9/??",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x412CE, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x28084 (CE 92))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x41492, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x28086 (92 94))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x41679, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x28088 (79 96))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x4185C, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x2808A (5C98))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x41A81, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x2808C (81 9A))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "Goof-off Combat text",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x41C79, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: UNKNOWN (guess: 0x2808E (79 9C))\n"
		//			+ "Page for this block Pointer: UNKNOWN (guess: 0x28156 (70 80))\n"
		//			+ "Goof-off Combat text",
		//	},

		//	new DataAddr()
		//	{ // BLOCK Q
		//		pointer = 0x41E52,
		//		length = 741,
		//		notes = "Pointer to this block: 0x28090 (52 9E)\n"
		//			+ "Page for this block Pointer: 0x28156 (70 80)",
		//	},


		//	new DataAddr()  // GUESS
		//	{
		//		pointer = 0x42136, // seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x28092 (36 A1)\n"
		//			+ "Page for this block Pointer: 0x28156 (70 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x42342, // seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x28094 (42 A3)\n"
		//			+ "Page for this block Pointer: 0x28156 (70 80)",
		//	},

		//	new DataAddr()  // GUESS
		//	{
		//		pointer = 0x425C7,
		//		//length = ,
		//		notes = "Pointer to this block: 0x28096 (C7 A5)\n"
		//			+ "Page for this block Pointer: 0x28156 (70 80)",
		//	},

		//	new DataAddr()
		//	{ // BLOCK U
		//		pointer = 0x4290F,
		//		length = 850,
		//		notes = "Pointer to this block: 0x28098 (0F A9)\n"
		//			+ "Page for this block Pointer: 0x28156 (70 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x42C61,  // seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x2809A (61 AC)\n"
		//			+ "Page for this block Pointer: 0x28156 (70 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x42E98,  // seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x2809C (98 AE)\n"
		//			+ "Page for this block Pointer: 0x28156 (70 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x430BF, // seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x2809E (BF B0)\n"
		//			+ "Page for this block Pointer: 0x28156 (70 80)",
		//	},


		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x435F9, // seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280A0 (F9 B5)\n"
		//			+ "Page for this block Pointer: 0x28156 (70 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x43E37,  // seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280A2 (37 BE)\n"
		//			+ "Page for this block Pointer: 0x28156 (70 80)",
		//	},
			
		//	/* Bank switch (-40 to hi-byte) */
		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x44000, // seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280A4 (00 80)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x444A1, // seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280A6 (A1 84)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x44924, // Seems legit, although...
		//		length = 0x10,
		//		notes = "Pointer to this block: 0x280A8 (24 89)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)\n"
		//			+ "Contains only Ωs",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x44934, // Seems legit, although...
		//		length = 0x10,
		//		notes = "Pointer to this block: 0x280AA (34 89)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)"
		//			+ "Contains only Ωs",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x44944, // Seems legit, although...
		//		length = 0x10,
		//		notes = "Pointer to this block: 0x280AC (44 89)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)"
		//			+ "Contains only Ωs",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x44954, // Seems legit, although...
		//		length = 0x10,
		//		notes = "Pointer to this block: 0x280AE (54 89)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)"
		//			+ "Contains only Ωs",
		//	},


		//	new DataAddr()
		//	{
		//		pointer = 0x44964,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280B0 (64 89)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)"
		//			+ "Starts with 10 Ωs",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x44AC2, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280B2 (C2 8A)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x44D7F, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280B4 (7F 8D)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x4505C, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280B6 (5C 90)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x452FE, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280B8 (FE 92)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr()
		//	{
		//		pointer = 0x455E9,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280BA (E9 95)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr()
		//	{
		//		pointer = 0x45A15,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280BC (15 9A)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr()
		//	{
		//		pointer = 0x45E8B,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280BE (8B 9E)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},


		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x4617B, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280C0 (7B A1)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x46406, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280C2 (06 A4)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x467D6, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280C4 (D6 A7)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x46933,
		//		length = 10,
		//		notes = "Pointer to this block: 0x280C6 (33 A9)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)"
		//			+ "Contains only Ωs",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x46943,
		//		length = 10,
		//		notes = "Pointer to this block: 0x280C8 (43 A9)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)"
		//			+ "Contains only Ωs",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x46953,
		//		length = 10,
		//		notes = "Pointer to this block: 0x280CA (53 A9)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)"
		//			+ "Contains only Ωs",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x46963,
		//		length = 10,
		//		notes = "Pointer to this block: 0x280CC (63 A9)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)"
		//			+ "Contains only Ωs",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x46973,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280CE (73 A9)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)"
		//			+ "Contains only Ωs",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x46983,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280D0 (83 A9)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)\n"
		//			+ "Starts with 10 Ωs",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x46A4F, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280D2 (4F AA)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x46EEC, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280D4 (EC AE)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x4716C, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280D6 (6C B1)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x475B3, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280D8 (B3 B5)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x47882, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280DA (82 B8)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x47B7A, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280DC (7A BB)\n"
		//			+ "Page for this block Pointer: 0x28158 (A4 80)",
		//	},
			
		//	/* Dialog Bank 3 (address as is) */
		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x48000, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280DE (00 80)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},


		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x485CD, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280E0 (CD 85)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x48AB1, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280E2 (B1 8A)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x48DC2, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280E4 (C2 8D)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x49238, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280E6 (38 92)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x496E6, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280E8 (E6 96)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x49CEB, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280EA (EB 9C)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x4A1BE, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280EC (BE A1)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x4A550, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280EE (50 A5)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},

		//	new DataAddr()
		//	{
		//		pointer = 0x4A90D,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F0 (0D A9)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},

		//	new DataAddr()
		//	{
		//		pointer = 0x4AEAF,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F2 (AF AE)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x4B3C3, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F4 (C3 B3)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x4B87B, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F6 (7B B8)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},

		//	new DataAddr() // GUESS
		//	{
		//		pointer = 0x4BC54, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F8 (54 BC)\n"
		//			+ "Page for this block Pointer: 0x2815A (DE 80)",
		//	},

		//	/* Dialog Bank4: +0x40 to hibyte. */

		//	new DataAddr() // UNCONFIRMED
		//	{
		//		pointer = 0x4C000, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280FA (00 80)\n"
		//			+ "Page for this block Pointer: 0x2815C (FA 80)",
		//	},

		//	new DataAddr() // UNCONFIRMED
		//	{
		//		pointer = 0x4C379, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280FC (79 83)\n"
		//			+ "Page for this block Pointer: 0x2815C (FA 80)",
		//	},

		//	new DataAddr() // UNCONFIRMED
		//	{
		//		pointer = 0x4C812, // Seems legit
		//		//length = ,
		//		notes = "Pointer to this block: 0x280FE (12 88)\n"
		//			+ "Page for this block Pointer: 0x2815C (FA 80)",
		//	},

		//	858B
		//	new DataAddr() // UNCONFIRMED
		//	{
		//		pointer = 0x4,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F ()\n"
		//			+ "Page for this block Pointer: 0x2815C (FA 80)",
		//	},
		//	498F
		//	new DataAddr() // UNCONFIRMED
		//	{
		//		pointer = 0x4,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F ()\n"
		//			+ "Page for this block Pointer: 0x2815C (FA 80)",
		//	},
		//	0D93
		//	new DataAddr() // UNCONFIRMED
		//	{
		//		pointer = 0x4,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F ()\n"
		//			+ "Page for this block Pointer: 0x2815C (FA 80)",
		//	},
		//	2196
		//	new DataAddr() // UNCONFIRMED
		//	{
		//		pointer = 0x4,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F ()\n"
		//			+ "Page for this block Pointer: 0x2815C (FA 80)",
		//	},
		//	7C99
		//	new DataAddr() // UNCONFIRMED
		//	{
		//		pointer = 0x4,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F ()\n"
		//			+ "Page for this block Pointer: 0x2815C (FA 80)",
		//	},
		//	739E
		//	new DataAddr() // UNCONFIRMED
		//	{
		//		pointer = 0x4,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F ()\n"
		//			+ "Page for this block Pointer: 0x2815C (FA 80)",
		//	},
		//	57A3
		//	new DataAddr() // UNCONFIRMED
		//	{
		//		pointer = 0x4,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F ()\n"
		//			+ "Page for this block Pointer: 0x2815C (FA 80)",
		//	},
		//	F0A8
		//	new DataAddr() // UNCONFIRMED
		//	{
		//		pointer = 0x4,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F ()\n"
		//			+ "Page for this block Pointer: 0x2815C (FA 80)",
		//	},

		//	CFAD3BB330B640B650B660B670B680B6

		//	90B6ABB78BBB00806A8372877D8CA790

		//	7E94AB98A1A03BA505A986AD83B1CEB6

		//	94BA00801F850A89CA8CD7906B94F397

		//	FD99C79B0AA0


		//	new DataAddr() // UNCONFIRMED
		//	{
		//		pointer = 0x4,
		//		//length = ,
		//		notes = "Pointer to this block: 0x280F ()\n"
		//			+ "Page for this block Pointer: 0x2815C ()",
		//	},





		//};


		//public static DataAddr[] dialogPagePtrs = new DataAddr[]
		//{
		//	new DataAddr()
		//	{
		//		pointer = 0x28156,
		//		length = 2,
		//		notes = "$7080 - Bank 1: -0x80 to hi-byte of dialog address.\n"
		//			+ "0x40000 - 0x43FFF",
		//	},
		//	new DataAddr()
		//	{
		//		pointer = 0x28158,
		//		length = 2,
		//		notes = "$A480 - Bank 2: -0x40 to hi-byte of dialog address."
		//			+ "0x44000 - 0x47FFF",
		//	},
		//	new DataAddr()
		//	{
		//		pointer = 0x2815A,
		//		length = 2,
		//		notes = "$DE80 - Bank 3: dialog address as shown."
		//			+ "0x48000 - 0x4BFFF",
		//	},
		//	new DataAddr() // UNCONFIRMED
		//	{
		//		pointer = 0x2815C,
		//		length = 2,
		//		notes = "$FA80 - Bank 4: +0x40 to hi-byte of dialog address??"
		//			+ "0x4C000 - 0x4FFFF",
		//	},
		//	2681
		//	4281

		//};

		//public static DataAddr GetDialogBlockAddressSubRoutine = new DataAddr()
		//{
		//	pointer = 0x28030,
		//	length = 64,
		//};

		public static Address ptrToMainDialog = new Address("Pointers to main dialog", 0x28070)
		{
			length = 230,

			notes = "2 bytes for address of block where target text is located, 2 bytes for extra math.\n"
				+ "Blocks are read until the correct # of EoT (∩ or Ω). Where does that # come from?"

				// Hi-byte -80: 0FA9 -> 290F
				+ "0x28156[$00] $8070 -> 0x28070[$00]: $8000 -> 0x40000 + 0C\n"		// {NUM} damage points for {NAME}{-A}.∩

				+ "0x28070[16?]: 52 9E & 0x28156: 70 80 -> 0x41E52 + 14? EoTs\n"    // There is no one in that direction $9E52

				+ "0x28070[20?]: 0F A9 & 0x28156: 70 80 -> 0x4290F + 6 EoTs\n"    // examines the ground
				+ "0x28070[20?]: 0F A9 & 0x28156: 70 80 -> 0x4290F + 5 EoTs\n"    // But nothing can be found!

				// Hi-byte -40:	E995 -> 55E9
				+ "0x280B0: 64 89 & 0x28158: A4 80 -> 0x44964 + \n" // Very well. I will record

				+ "0x280BA: E9 95 & 0x28158: A4 80 -> 0x455E9 + \n"     // Adventure hall
				+ "0x280BA: E9 95 & 0x28158: A4 80 -> 0x455E9 + 10 EoTs\n"  // Who will you register? 

				+ "0x280BC: 15 9A & 0x28158: A4 80 -> 0x45A15 + \n"     // Answer No to first question
				+ "0x280BC: 15 9A & 0x28158: A4 80 -> 0x45A15 + \n"     // Answers NO to save
				+ "0x280BC: 15 9A & 0x28158: A4 80 -> 0x45A15 + 3 EoTs\n"    // This is Luisa's Place
																			 /// 

				+ "0x280BE: 8B 9E & 0x28158: A4 80 -> 0x45E8B + \n"     // Come back again!

				+ "0x280C2: 06 A4 & 0x28158: A4 80 -> 0x46406 + \n"    // Answer NO to Luisa save

				+ "0x280C4: D6 A7 & 0x28158: A4 80 -> 0x467D6 + 0 EoTs\n"     // Asks to save

				+ "0x280D0: 83 A9 & 0x28158: A4 80 -> 0x46983 + 15 EoTs\n"    // cat - this block starts with 10 Ω!

				// exact match: 0DA9 -> A90D
				+ "0x280F0: 0D A9 & 0x2815A: DE 80 -> 0x4A90D + 10 EoTs\n"    // bard
				+ "0x280F0: 0D A9 & 0x2815A: DE 80 -> 0x4A90D + 11 EoTs\n"   // Goof-offs are 
				+ "0x280F0: 0D A9 & 0x2815A: DE 80 -> 0x4A90D + EoTs\n"     // soldier 1st floor
				+ "0x280F0: 0D A9 & 0x2815A: DE 80 -> 0x4A90D + 1 EoTs\n"   // If you're going to take anyone


				+ "0x280F2: AF AE & 0x2815A: DE 80 -> 0x4AEAF + 2 EoTs\n"   // 


				+ "0x280:  & 0x281:  -> 0x + EoTs\n"   // 
				+ "0x280:  & 0x281:  -> 0x + bytes\n",  // 
		};


		public static Address musicData = new Address("Music Data", 0x7863E)
		{
			length = -1, // unknown, but atleast 10532

			notes = "0x07864E to 0x0786A0 = Dragon Quest Theme Intro, 0x0787E3 to 0x078A08 = Castle Theme, "
				+ "0x078A09 to 0x078B9A = Main Map Theme, 0x078CBA to 0x078E26 = Jipang Theme, "
				+ "0x079419 to 0x0795F0 = Dungeon Theme, 0x0795F1 to 0x07974D = Tower Theme, "
				+ "0x078E27 to 0x078E43 = Level Up, 0x079419 to 0x0795F0 = Dungeon Theme, "
				+ "0x079834 to 0x0798C4 = Shrine Theme, 0x07A60F to 0x07A71E = Menu Screen,"
				+ "0x07A842 to 0x07AA22 = Village Theme, 0x07AB7B to 0x07AD25 = Town Theme, "
				+ "0x07AD26 to 0x07AF72 = Battle Theme, 0x07BA69 to 0x07BB6F = Title Screen",
		};


		public static Address emptySpace0 = new Address("Empty Space", 0x04DD9)
		{
			length = 12799,
		};

		public static Address emptySpace0b = new Address("Empty Space", 0x0BA49)
		{
			length = 1423,
		};

		public static Address emptySpacePostCredits = new Address("Empty Space", 0x1FDB7)
		{
			length = 545,
		};

		public static Address emptySpace0c = new Address("Empty Space", 0x23CC0)
		{
			length = 704,
		};

		public static Address emptySpace1 = new Address("Empty Space", 0x6A8DD)
		{
			length = 5883,
		};

		public static Address emptySpace2 = new Address("Empty Space", 0x6C350)
		{
			length = 15496,
		};

		public static Address emptySpace3 = new Address("Empty Space", 0x70000)
		{
			length = 16344,
		};

		public static Address emptySpace4 = new Address("Empty Space", 0x74000)
		{
			length = 16344,
		};

		public static Address emptySpace5 = new Address("Empty Space", 0x7BB60)
		{
			length = 1144,
		};

		public static Address emptySpace6 = new Address("Empty Space", 0x7EC98)
		{
			length = 872,
		};

		public static Address emptySpace7 = new Address("Empty Space", 0x7FAB0)
		{
			length = 1246,
		};
	}
}
