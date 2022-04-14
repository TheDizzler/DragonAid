using System.Collections.Generic;

namespace AtomosZ.DragonAid.MonsterAid
{
	public static class MonsterConsts
	{
		public enum AIType
		{
			RandomTarget,
			SmartTarget1,
			/// <summary>
			/// smart target choice, and action choice is delayed until enemy acts
			/// </summary>
			SmartTarget2,
		}

		public enum ActionChances
		{
			Equal, Type1, Type2, FixedSequence
		};


		public static string[] resistanceDescriptions = new string[4]
		{
			"No chance to evade",
			"77/256 chance to evade (~30)",
			"179/256 chance to evade (~70)",
			"Immune"
		};

		public static string[] itemDropChances = new string[8]
		{
			"1/2048", "1/256", "1/128", "1/64", "1/32", "1/16", "1/8", "100%"
		};

		public static string[] aiDescriptions = new string[3]
		{
			"Random target chosen every turn.",
			"☼Casters will not cast spells on a member protected by Bounce.",
			"☼Similar to Smart Target 1 but waits until it's turn to decide action."
				+ "\n☼Will not cast a spell if not enough MP or afflicted with StopSpell."
				+ "\n☼In cases where an action selector randomly allows one or two actions,\n   more often does 2 actions. (not clear about this)"
		};

		public static List<string> actions = new List<string>()
		{
			"00 = enemy is assessing the situation",
			"01 = enemy protects itself",
			"02 = Regular attack",
			"03 = Attack could be critical",
			"04 = Attack could cause sleep",
			"05 = Attack could cause poison",
			"06 = Attack could cause numbness",
			"07 = Enemy will flee",
			"08 = Call for reinforcements(same as the enemy calling)",
			"09 = Curious dance(steals MP)",
			"0A = Flaming breath(10 - 20 HP attack)",
			"0B = Flaming breath(30 - 50 HP attack)",
			"0C = Flaming breath(80 - 100 HP attack)",
			"0D = Blizzard breath(10 - 20 HP attack)",
			"0E = Blizzard breath(40 - 60 HP attack)",
			"0F = Blizzard breath(100 - 140 HP attack)",
			"10 = emits gales of sweet breath(causes sleep)",
			"11 = emits gales of toxic breath(causes poison)",
			"12 = emits gales of scorching breath(causes numbness)",
			"13 = chants Blaze",
			"14 = chants Blazemore",
			"15 = chants Blazemost",
			"16 = chants Icebolt",
			"17 = chants Firebal",
			"18 = chants Firebane",
			"19 = chants Explodet",
			"1A = chants Snowblast",
			"1B = chants Snowstorm",
			"1C = chants Infernos",
			"1D = chants Infermore",
			"1E = chants Infermost",
			"1F = chants Beat(possible instant death)",
			"20 = chants Defeat(possible instant death)",
			"21 = chants Sacrifice",
			"22 = chants Sleep",
			"23 = chants Stopspell",
			"24 = chants Sap",
			"25 = chants Defence",
			"26 = chants Surround",
			"27 = chants Robmagic",
			"28 = chants Chaos",
			"29 = chants Slow",
			"2A = chants Limbo",
			"2B = freeze beam shoots out of Zoma's fingertip (nullifies spells)",
			"2C = chants Bounce",
			"2D = chants Increase",
			"2E = chants Increase 2",
			"2F = chants Vivify",
			"30 = chants Revive",
			"31 = chants Heal",
			"32 = chants Healmore",
			"33 = chants Healall",
			"34 = chants Healus",
			"35 = chants Healusall",
			"36 = chants Heal 2",
			"37 = chants Healmore 2",
			"38 = chants Healall 2",
			"39 = chants Healus 2",
			"3A = chants Healusall 2",
			"3B = calls for reinforcements(Healer)",
			"3C = calls for reinforcements(Granite Titan)",
			"3D = calls for reinforcements(Hork)",
			"3E = calls for reinforcements(Elysium Bird)",
			"3F = calls for reinforcements(Voodoo Shaman)",
		};

		

		public static int Level = 0x00;
		public static int Evade1 = 0x00;
		public static int Evade2 = 0x09;
		public static int Exp1 = 0x01;
		public static int Exp2 = 0x02;
		public static int Agility = 0x03;
		public static int Gold1 = 0x04;
		public static int Gold2 = 0x12;
		public static int Attack1 = 0x05;
		public static int Attack2 = 0x13;
		public static int Defense1 = 0x06;
		public static int Defense2 = 0x14;
		public static int HP1 = 0x07;
		public static int HP2 = 0x15;
		public static int MP = 0x08;
		public static int ItemDrop = 0x09;
		public static int Action0 = 0x0A;
		public static int Action1 = 0x0B;
		public static int Action2 = 0x0C;
		public static int Action3 = 0x0D;
		public static int Action4 = 0x0E;
		public static int Action5 = 0x0F;
		public static int Action6 = 0x10;
		public static int Action7 = 0x11;
		public static int AISelector1 = 0x0A;
		public static int AISelector2 = 0x0B;
		public static int ActionChance1 = 0x0C;
		public static int ActionChance2 = 0x0D;
		public static int ActionCount1 = 0x0E;
		public static int ActionCount2 = 0x0F;
		public static int Regeneration1 = 0x10;
		public static int Regeneration2 = 0x11;
		public static int Resistance = 0x12;
		public static int FocusFire = 0x17;
		public static int ItemDropChance = 0x17;
	}
}
