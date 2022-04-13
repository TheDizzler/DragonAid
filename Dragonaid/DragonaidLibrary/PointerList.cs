using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomosZ.DragonAid.Libraries
{
	public static class PointerList
	{
		/// <summary>
		/// [0,1] 1st part of item name
		/// [2,3] 2nd part of item name
		/// [4,5] = $B3BC monster name list 1
		/// [6,7] = $B8BF End of MonsterName_List
		/// [$0A] = $AA28 Class name list
		/// </summary>
		public static Address NamePointers = new Address("Item, Monster, Class name pointers", 0x0AA0E);

		public static Address WeaponPowers = new Address("Weapon Powers", 0x027990, 32);
		public static Address ArmorPowers = new Address("Armor Powers", 0x0279B0, 24);
		public static Address ShieldPowers = new Address("Shield Powers", 0x0279C8, 7);
		public static Address HelmetPowers = new Address("Helmet Powers", 0x0279CF, 8);
	}
}
