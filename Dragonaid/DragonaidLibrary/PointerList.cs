using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AtomosZ.DragonAid.Libraries
{
	public static class PointerList
	{
		/* No specific bank */
		public static Address[] NightTimeCheckAddresses = new Address[]
		{
			new Address("SetMenuColor", 0x35067, 1),
		};

		/* Bank 0 $00000 */
		/// <summary>
		/// Index derived from EncounterMonsterLists ( >>= 5)
		/// </summary>
		public static Address PreEncounterRateVector = new Address("Unknown", 0x00628, 4);
		/// <summary>
		/// $DB8A
		/// </summary>
		public static Address EncounterMonsterListPointer = new Address("Incorrect? This is pre encounter roll", 0x00648, 2);
		public static Address EncounterRates = new Address("Encounter Rates per Tile", 0x00934, 18)
		{
			notes = "0-8 Day encounter rates\n"
				+ "9-16 Night encounter rates\n\n"
				+ "day\n"
				+ "[0]: 04 - water\n"
				+ "[1]: 0F - woods/desert/ice\n"
				+ "[2]: 0A - grass\n"
				+ "[3]: 0F - woods/desert/ice\n"
				+ "[4]: 12 - heavy forest\n"
				+ "[5]: 19 - hills\n"
				+ "[6]: 54 - dungeon after room change\n"
				+ "[7]: 12 - swamp\n"
				+ "[8]: 0A - dungeon\n\n"
				+ "night\n"
				+ "[9]: 05 - water\n"
				+ "[10]: 13 - woods/desert/ice\n"
				+ "[11]: 0D - grass\n"
				+ "[12]: 0F - woods/desert/ice\n"
				+ "[13]: 16 - heavy forest\n"
				+ "[14]: 1F - hills\n"
				+ "[15]: 54 - dungeon after room change\n"
				+ "[16]: 16 - swamp\n"
				+ "[17]: 0A - dungeon\n"
		};
		/// <summary>
		/// Index is derived from position on map. 
		/// (XXXX 0000 >> 4) + YYYY 0000 = YYYY XXXX
		/// </summary>
		public static Address LightWorldEncounterTiles = new Address("Encounter Tiles on the Light World Map", 0x00946, 256);
		public static Address EncounterMonsterLists = new Address("Monster lists for encounter areas", 0x00ADB, 76);

		/// <summary>
		/// [0,1] Experience
		/// [2,3] Strength
		/// [4,5] Agility
		/// [6,7] Vitality
		/// </summary>
		public static Address CharacterStatPointers = new Address("This requires a better name", 0x027F4, 8);
		public static Address CharacterStatsPointer = new Address("This requires a better name too", 0x02800);
		/// <summary>
		/// Theses probably point to stat baselines.
		/// </summary>
		public static Address CharacterLevelUpPointers = new Address("This requires a better name three", 0x02802);
		public static Address MonsterStatBlockAddress = new Address("Monster Data 1", 0x0032D3, UniversalConsts.MonsterStatLength);

		/* Bank 1 $04000 */
		public static Address MonsterRegeneration = new Address("Monster Regeneration", 0x047EF);

		/* Bank 2 $08000 */
		/// <summary>
		/// [0,1] 1st part of item name
		/// [2,3] 2nd part of item name
		/// [4,5] = $B3BC monster name list 1
		/// [6,7] = $B8BF End of MonsterName_List
		/// [$0A] = $AA28 Class name list
		/// </summary>
		public static Address NamePointers = new Address("Item, Monster, Class name pointers", 0x0AA0E);

		/* Bank 4 $10000 */
		public static Address MonsterActionChancesType1 = new Address("ActionChancesType1", 0x1342A);
		public static Address MonsterActionChancesType2 = new Address("ActionChancesType2", 0x13432);

		/* Bank 5 $14000 */
		public static Address TileBatchSomethingPointerA = new Address("Unclear what this vector does", 0x16DD0, 2);
		public static Address TileBatchSomethingPointerB = new Address("Unclear what this vector does", 0x16DD2, 2);
		public static Address TileBatches_Characters = new Address("Tile batch instructions for character sprites", 0x16DD4);

		public static Address PPUAddressTable = new Address("Unclear what this vector does", 0x17438, 15)
		{
			notes = "Entry 1 (map tiles):"
				+ "[$00] - [$01]:$18D0 -> STA $2006 PPUWrite address "
				+ "[$02] - [$03]: $7200 -> CHR_TileIndex_Vector are tiles are stored (for reference later?)"
				+ "[$04]: $8D -> offset to write location in PPU\n"
				+ "Entry 2 (bird):"
				+ "[$05] - [$06]: $0800 -> STA $2006 PPUWrite address "
				+ "[$07] - [$08]: $6F00 -> Sprite_Index_Vector "
				+ "[$09]: $80 -> offset to write location in PPU\n"
				+ "Entry 3 (PC sprite):"
				+ "[$10]: $0000 "
				+ "[$12]: $6E00 -> staging area for sprites before passing to PPU"
				+ "[$13]: $00\n",
		};
		public static Address OffsetsToNextSpriteInTileBatch = new Address("", 0x17456, 88)
		{
			notes = "The value at nextSpriteAddressOffset is added to instructionByte[1] "
				+ "to get the address (before << 4) of the sprite to load. "
				+ "The Address is saved in zeroPages[0x04 + x].",
		};
		public static Address TileBatchVectorA = new Address("Unclear what this vector does", 0x174AD, 88);
		public static Address TileDynamicOffsets = new Address("Tile dynamic offsets", 0x174BC, 5)
		{
			notes = "paired bytes:\n"
				+ "1st byte is added to dynamicSubroutine (pointer to instruction bytes)\n"
				+ "2nd byte is tileSomethingVectorCOffset",
		};
		public static Address TileBatchSpriteOrder = new Address("Order sprites are parsed from zeroPages[0x04]", 0x174CC, 5)
		{
			notes = "Order sprites are parsed from zeroPages[0x04 + x] where x is from one of the lists below:\n"
				+ "Standard order: 00 02 04 06\n"
				+ "Other orders not observed yet",
		};

		/* Bank 6 $18000 */
		public static Address LocalPointers_18000 = new Address("Pointer table for $18000 bank", 0x18000, 24)
		{
			notes = "[$0A]: $B387 \n"
				+ "[$16]: $B755 (Day/Night Palettes)\n",
		};
		public static Address DayNightPalettes = new Address("Only day/night palettes?", 0x1B755, 84)
		{
			notes = "Clock 00: $00-$0B; Clock 1E: $0C-$17; Clock 3C: $18-$23; Clock 5A: $24-$29; "
				+ "Clock 78 (night start): $30-$3B; Clock 96: $3C-$47; Clock B4: $48-$53",
		};

		/* Bank 9 $24000 */
		public static Address WeaponPowers = new Address("Weapon Powers", 0x027990, UniversalConsts.WeaponCount);
		public static Address ArmorPowers = new Address("Armor Powers", 0x0279B0, UniversalConsts.ArmorCount);
		public static Address ShieldPowers = new Address("Shield Powers", 0x0279C8, UniversalConsts.ShieldCount);
		public static Address HelmetPowers = new Address("Helmet Powers", 0x0279CF, UniversalConsts.HelmetCount);

		/* Bank A $28000 */
		/* Bank B $2C000 */
		/* Bank C $30000 */
		/* Bank D $34000 */
		public static Address MenuColorQuarterHP = new Address("Color used when character's HP below quarter max", 0x355F9, 1)
		{
			notes = "#$2A (green) by default",
		};
		public static Address MenuColorStatusNormal = new Address("Color used when status normal", 0x3560B, 1)
		{
			notes = "#$30 (white) by default",
		};
		public static Address MenuColorNightTime = new Address("Color used at night time", 0x3560E, 1)
		{
			notes = "#$21 (light blue) by default",
		};
		public static Address MenuColorQCharacterDead = new Address("Color used when character dead", 0x35613, 1)
		{
			notes = "#$27 (orange) by default",
		};
		public static Address MenuColorUnknown = new Address("Color used when value at 6A58 (Save RAM) is 0", 0x3561A, 1)
		{
			notes = "#$30 (white) by default",
		};

		/* Bank E $38000 */
		/// <summary>
		/// Add 0x23 and you get the DynamicSubroutine07 Index
		/// 
		/// [$00]: Item Command ($2F + $23 => $52)
		/// [$01]: ?	($26 + $23 => $49?)
		/// [$02]: ?	($1D + $23 => $40?)
		/// [$03]: ?	($1F + $23 => $42?)
		/// [$04]: ?	($1E + $23 => $41?)
		///	[$05]: ?	($20 + $23 => $43?)
		///	[$06]: ?	($2F + $23 => $52)
		///	AE EB 6A D0 04 C9 79 F0 03 9D 00 04
		/// 
		/// $52 -> $9976 Character_GetCountAndNameIndices
		/// </summary>
		public static Address AdjustableHeightInstructions = new Address("Menu adjustable height instructions", 0x38DB2)
		{
			length = 7, // at least
			notes = "Add 0x23 and you get the DynamicSubroutine07 Index" +
			"[$00]: Item Command ($2F + $23 => $52) "
				+ "$23-> Character_GetCurrentHP"
				+ "$27-> $932C Character_GetCurrentMP"
				+ "$2D-> $93D0 Character_CheckStatus"
				+ "$3A-> $9586 TransferXPToQuickStorage"
				+ "$3C-> $95ED Character_GetTotalGold "
				+ "$4E-> $9944 GetCharacterClass_JMP"
				+ "$52-> $9976 Character_GetCountAndNameIndices"
			,
		};
		/// <summary>
		/// [$02]:$9074							[$0A]:$910E -> Status 2 Characters
		/// [$18]:$9CFE -> (#69)				[$26]:$93C7 ->
		/// [$28]:$93F1 -> Menu_MainCommand		[$2A]:$9420 -> (#15) [$34]:$94BB -> 
		/// [$2C]: Item character select
		/// [$36]:$94DB -> FIGHT.PARRY.ITEM.	[$69]: Item window	[$72]:$96A9 -> Enemy Display, Interactive
		/// [$7A]:$96DD -> Dialog Text			[$7C]:$96E9 -> (Someone died) Redirects to $96DD
		/// [$7E]:$96ED -> Enemy Display, Non-Interactive
		/// </summary>
		public static Address MenuPointers = new Address("Menu Adresses", 0x38F84)
		{
			notes = "There are at least 2 type of pointer in this list: "
			+ "1) normal instructions and 2) redirect to normal instructions.\n"
			+ "If byte 0 is >= $80, then it is case 2). Bytes 2 and 3 is the new menu pointer, "
			+ "and byte 1 is menuPositionA & B.\n"
			+ "How to read normal instructions:\n"
			+ "[$00]: "
			+ "[$01]: if >= 10, the height is dynamic. Otherwise, this number *2 == height of menu in 8 pixel lines.", 
		};
		public static Address MenuTitles = new Address("Menu titles", 0x39D2B);
		public static Address LoadSpritesVector = new Address("Unknown", 0x3BA34);

		/* Bank F $3C000 (low default) */
		public static Address MapScrollVectorB = new Address("Unknown", 0x3C2AB, 5)
		{
			notes = "Used in character sprite parser so...what is this?",
		};

		public static Address Load07BankIds = new Address("DynamicSubroutine_BankIds_07", 0x3E917);
		public static Address Load17BankIds = new Address("DynamicSubroutine_BankIds_17", 0x3E997);
		public static Address Load07PointerIndices = new Address("DynamicSubroutine_PointerIndex_07", 0x3E9ED, 256);
		public static Address Load17PointerIndices = new Address("DynamicSubroutine_PointerIndex_17", 0x3EAED, 256);


		/* Bank 16 $58000 */
		public static Address PaletterStoreOffsets = new Address(
			"Order in which palettes are written to $03E7 (PPU_BGPaletteColor_Store)", 0x58152);
		/// <summary>
		/// offsets to palettes for different times of day in DayNightPalettes vector (0x1B755)
		/// </summary>
		public static Address TimeOfDayDayNightPalettesIndices = new Address("", 0x583A3, 7);
		public static Address TimeOfDayChangeTimes = new Address("Time-of-Day Change Clock Values", 0x583AA, 7)
		{
			notes = "Clock Value: 00, 1E, 3C, 5A, 78 (night start), 96, B4",
		};


		/* Bank 1F $7C000 (high default) */
		/// <summary>
		/// Mirror of 0x3E917 Load07BankIds
		/// </summary>
		public static Address LocalPointerBanks = new Address("Compressed bank indices", 0x7E917, 128)
		{
			notes = "Index used is half index used for LocalPointerIndices.\n"
				+ "Data is compressed: high nibble & low nibbles are two different bank Ids",
		};
		/// <summary>
		/// Mirror of 0x3E9ED Load07PointerIndices 
		/// </summary>
		public static Address LocalPointerIndices = new Address("Indices to LocalPointer", 0x7E9ED, 256)
		{
			notes = "The index of the pointer in the LocalPointers in the bank to be loaded.\n"
				+ "The index used in this vector determines which nibble to use from LocalPointerBanks: "
				+ "Even: use high nibble. Odd: use low nibble.\n"
				+ "[$8B]: (Day/Night Palettes) 0B",
		};


		public static Address GetBankAddressFromId(byte bankId)
		{
			int address = 0;
			if ((bankId & 0x10) == 0x10)
				address += 0x40000;
			address += (bankId & 0x0F) * 0x4000;
			return new Address(address.ToString(), address);
		}
	}
}
