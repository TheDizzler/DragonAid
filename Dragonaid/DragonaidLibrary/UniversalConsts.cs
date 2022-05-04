using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomosZ.DragonAid.Libraries
{
	public static class UniversalConsts
	{
		/// <summary>
		/// Timer wraps at 204 (0xCC).
		/// </summary>
		public static byte NightBattleStartTime = 0x78;

		public static byte MonsterCount = 0x8B;
		public static byte MonsterStatLength = 0x17;

		public static byte WeaponCount = 32;
		public static byte ArmorCount = 24;
		public static byte ShieldCount = 7;
		public static byte HelmetCount = 8;

		public static byte ClassCount = 0x08;
		/// <summary>
		/// This really needs testing.
		/// </summary>
		public static byte ClassMask
		{
			get
			{
				byte mask = 0;
				int bits = (int)Math.Floor(Math.Log(ClassCount - 1, 2) + 1);
				for (int i = 0; i < bits; ++i)
				{
					mask <<= 1;
					mask |= 0x01;
				}
				return mask;
			}
		}
		public static byte SexMask
		{
			get
			{
				byte mask = 1;
				int bits = (int)Math.Floor(Math.Log(ClassCount - 1, 2) + 1);
				for (int i = 0; i < bits; ++i)
					mask <<= 1;
				return mask;
			}
		}


		public static byte AnimalSuitSpriteIndex = 0x1F;
		public static byte SwimwearSpriteIndex = 0x30;
		/// <summary>
		/// Added to class index.
		/// </summary>
		public static byte FemaleClassSpriteOffset = 0x07;
	}
}
