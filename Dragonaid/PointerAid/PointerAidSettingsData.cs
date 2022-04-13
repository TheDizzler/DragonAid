using AtomosZ.DragonAid.Libraries;

namespace AtomosZ.DragonAid.PointerAid
{
	internal class PointerAidSettingsData : AidUserSettings
	{
		public string dynamicPointersJsonFile = @"";

		public PointerAidSettingsData()
		{
			userSettingsFile = pointerAidFormUserSettingsFile;
			appExtension = ".dap";
		}
	}
}
