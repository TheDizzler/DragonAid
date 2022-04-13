using AtomosZ.DragonAid.Libraries;

namespace AtomosZ.DragonAid.MonsterEditor
{
	internal class MonsterAidSettingsData : AidUserSettings
	{
		public string monsterEditJsonFile = @"";

		public MonsterAidSettingsData()
		{
			userSettingsFile = monsterAidFormUserSettingsFile;
			appExtension = ".dam";
		}

	}
}
