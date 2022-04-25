using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace AtomosZ.DragonAid.Libraries
{
	public static class PointerList
	{
		/* Bank 0 */
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
		public static Address MonsterStatBlockAddress = new Address("Monster Data 1", 0x0032D3, UniversalConsts.monsterStatLength);

		/* Bank 1 */
		public static Address MonsterRegeneration = new Address("Regeneration", 0x047EF);

		/* Bank 2 */
		/// <summary>
		/// [0,1] 1st part of item name
		/// [2,3] 2nd part of item name
		/// [4,5] = $B3BC monster name list 1
		/// [6,7] = $B8BF End of MonsterName_List
		/// [$0A] = $AA28 Class name list
		/// </summary>
		public static Address NamePointers = new Address("Item, Monster, Class name pointers", 0x0AA0E);

		/* Bank 4 */
		public static Address MonsterActionChancesType1 = new Address("ActionChancesType1", 0x1342A);
		public static Address MonsterActionChancesType2 = new Address("ActionChancesType2", 0x13432);

		/* Bank 5 */
		public static Address TileBatchSomethingPointerA = new Address("Unclear what this vector does", 0x16DD0, 2);
		public static Address TileBatchSomethingPointerB = new Address("Unclear what this vector does", 0x16DD2, 2);
		public static Address TileBatches_Characters = new Address("Tile batch instructions for character sprites", 0x16DD4);

		public static Address PPUAddressTable = new Address("Unclear what this vector does", 0x17438, 15)
		{
			notes = "Entry 1: [$00] - [$01]:$18D0 -> STA $2006 PPUWrite address "
				+ "[$02] - [$03]: $7200 -> CHR_TileIndex_Vector are tiles are stored (for reference later?)"
				+ "[$04]: $8D\n"
				+ "Entry 2 (bird): [$05] - [$06]: $0800 -> STA $2006 PPUWrite address "
				+ "[$07] - [$08]: $6F00 -> Sprite_Index_Vector "
				+ "[$09]: $80\n"
				+ "Entry 3 (PC sprite): [$10]: $0000 "
				+ "[$12]: $6E00 " 
				+ "[$13]: $00\n"
				,
		};
		public static Address TileBatchVectorB = new Address("Unclear what this vector does", 0x17456, 88);
		public static Address TileBatchVectorA = new Address("Unclear what this vector does", 0x174AD, 88);
		public static Address TileDynamicOffsets = new Address("Tile dynamic offsets", 0x174BC, 5)
		{
			notes = "paired bytes:\n"
				+ "1st byte is added to dynamicSubroutine (pointer to instruction bytes)\n"
				+ "2nd byte is tileSomethingVectorCOffset",
		};
		public static Address TileBatchVectorC = new Address("Unclear what this vector does", 0x174CC, 5);

		/* Bank 9 */
		public static Address WeaponPowers = new Address("Weapon Powers", 0x027990, UniversalConsts.WeaponCount);
		public static Address ArmorPowers = new Address("Armor Powers", 0x0279B0, UniversalConsts.ArmorCount);
		public static Address ShieldPowers = new Address("Shield Powers", 0x0279C8, UniversalConsts.ShieldCount);
		public static Address HelmetPowers = new Address("Helmet Powers", 0x0279CF, UniversalConsts.HelmetCount);

		/* Bank E */
		public static Address MenuPointers = new Address("Menu Adresses", 0x38F84);
		public static Address LoadSpritesVector = new Address("Unknown", 0x3BA34);

		public static Address GetBankAddressFromId(byte bankId)
		{
			int address = 0;
			if ((bankId & 0x10) == 0x10)
				address += 40000;
			address += (bankId & 0x0F) * 0x4000;
			return new Address(address.ToString(), address);
		}
	}
}
