using System;

namespace AtomosZ.DragonAid.Libraries
{
	public static class UniversalConsts
	{
		/// <summary>
		/// THIS SHOULD BE FETCHED FROM ROM/EDIT SAVE DATA.
		/// Timer wraps at 204 (0xCC).
		/// 
		/// Hardcoded at:
		/// 0x003BD (GetEncounterRate)
		/// 0x35067 (SetMenuColor)
		/// </summary>
		public static byte NightBattleStartTime = 0x78;
		/// <summary>
		/// Time that day/night timer rolls over.
		/// 
		/// Hardcoded at:
		/// 0x3D03D (IncrementClock)
		/// </summary>
		public static byte DayTimer = 0xCC;

		/// <summary>
		/// Hardcoded at:
		/// 0x003E4
		/// 0x003E8
		/// </summary>
		public static byte MaxEncounterRateChance = 0x64;

		/// <summary>
		/// Hardcoded at:
		/// 0x003D8
		/// </summary>
		public static byte GoldClawEncounterRateChance = 0x64;

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

		/// <summary>
		/// 0xFFFA-0xFFFB
		/// </summary>
		public static int NMI_Pointer = 0xFFFA;
		/// <summary>
		/// 0xFFFC-0xFFFD
		/// </summary>
		public static int RESET_Pointer = 0xFFFC;
		/// <summary>
		/// 0xFFFE-0xFFFF
		/// </summary>
		public static int IRQBRK_Pointer = 0xFFFE;
	}
}
