using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace AtomosZ.DragonAid.Libraries
{
	public class AidUserSettings
	{
		public static readonly string pointerAidExtension = ".dap";
		public static readonly string pointerAidFormUserSettingsFile = @"PointerAidSettings.dapus";

		public static readonly string monsterAidExtension = ".dam";
		public static readonly string monsterAidFormUserSettingsFile = @"MonsterAidSettings.damus";

		public string userSettingsFile { get; protected set; }
		public string appExtension { get; protected set; }
		public string romFile = @"";


		public static bool InitializeUserSettings<T>(out T userSettings) where T : AidUserSettings, new()
		{
			FileDialog fileDialog = new OpenFileDialog();
			fileDialog.Filter = "iNES files (*.nes)|*.nes";
			fileDialog.CheckFileExists = true;

			var result = fileDialog.ShowDialog();
			if (result == DialogResult.Cancel)
			{
				userSettings = null;
				Environment.Exit(0);

				return false;
			}
			else if (result == DialogResult.OK)
			{
				var file = fileDialog.FileName;
				var romData = File.ReadAllBytes(file);
				if (romData[0] != 0x4E && romData[1] != 0x45 && romData[2] != 0x53 && romData[3] != 0x1A)
				{
					MessageBox.Show("Invalid iNES file");
					userSettings = null;
					return false;
				}

				userSettings = new T();
				userSettings.romFile = file;

				File.WriteAllText(userSettings.userSettingsFile,
					JsonConvert.SerializeObject(userSettings, Newtonsoft.Json.Formatting.Indented));

				return true;
			}

			userSettings = null;
			return false;
		}

		public static void SaveUserSettings<T>(T userSettings) where T : AidUserSettings
		{
			File.WriteAllText(userSettings.userSettingsFile,
				JsonConvert.SerializeObject(userSettings, Newtonsoft.Json.Formatting.Indented));
		}
	}
}
