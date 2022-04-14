using AtomosZ.DragonAid.Libraries;

namespace AtomosZ.DragonAid.MonsterAid
{
	internal class MonsterAidSettingsData : AidUserSettings
	{
		public string monsterEditJsonFile = @"";
		public byte monsterIndex = 0;

		public MonsterAidSettingsData()
		{
			userSettingsFile = monsterAidFormUserSettingsFile;
			appExtension = ".dam";
		}

	}
}
