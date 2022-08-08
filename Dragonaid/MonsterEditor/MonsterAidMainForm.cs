using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using AtomosZ.DragonAid.Libraries;
using Newtonsoft.Json;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;

namespace AtomosZ.DragonAid.MonsterAid
{
	public partial class MonsterAidMainForm : Form
	{
		private MonsterAidSettingsData monsterAidUserSettings;
		private byte[] romData;

		public MonsterAidMainForm()
		{
			InitializeComponent();


			if (!File.Exists(MonsterAidSettingsData.monsterAidFormUserSettingsFile))
			{
				while (!AidUserSettings.InitializeUserSettings(out monsterAidUserSettings))
				{ }

				romData = File.ReadAllBytes(monsterAidUserSettings.romFile);
				monsterAidView.LoadMonsterStatsFromROM(romData, 0);
			}
			else
			{
				monsterAidUserSettings = JsonConvert.DeserializeObject<MonsterAidSettingsData>(
					File.ReadAllText(MonsterAidSettingsData.monsterAidFormUserSettingsFile));
				romData = File.ReadAllBytes(monsterAidUserSettings.romFile);

				if (File.Exists(monsterAidUserSettings.monsterEditJsonFile))
				{
					var monsterStats = JsonConvert.DeserializeObject<List<MonsterStatBlock>>(
						File.ReadAllText(monsterAidUserSettings.monsterEditJsonFile));
					monsterAidView.LoadEditedMonsterStats(romData, monsterStats, monsterAidUserSettings.monsterIndex);
				}
				else
				{
					monsterAidView.LoadMonsterStatsFromROM(romData, monsterAidUserSettings.monsterIndex);
				}
			}
		}

		private void NextMonster_button_Click(object sender, System.EventArgs e)
		{
			if (++monsterAidUserSettings.monsterIndex >= UniversalConsts.MonsterCount)
				monsterAidUserSettings.monsterIndex = 0;
			monsterAidView.LoadMonster(monsterAidUserSettings.monsterIndex);
		}

		private void PrevMonster_button_Click(object sender, System.EventArgs e)
		{
			if (--monsterAidUserSettings.monsterIndex > UniversalConsts.MonsterCount)
				monsterAidUserSettings.monsterIndex = (byte)(UniversalConsts.MonsterCount - 1);
			monsterAidView.LoadMonster(monsterAidUserSettings.monsterIndex);
		}


		private void ExitToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.Control | Keys.S))
			{
				SaveToolStripMenuItem_Click(null, null);
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void SaveNeeded(bool needed)
		{
			saveStatus_label.Visible = true;
			saveStatus_label.Text = (needed ? "Not " : "") + "Saved";
			this.Text = "MonsterAid" + (needed ? "*" : "");
		}

		private void LoadMonsterAidFileToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			var result = MessageBox.Show("You will lose any unsaved data.",
				"Load monster data from file?", MessageBoxButtons.YesNo);

			if (result != DialogResult.Yes)
				return;


			OpenFileDialog dialog = new OpenFileDialog();
			dialog.AddExtension = true;
			dialog.DefaultExt = "*" + AidUserSettings.monsterAidExtension;
			dialog.Filter = $"DragonAid Monster file ({dialog.DefaultExt})|{dialog.DefaultExt}";

			result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				monsterAidUserSettings.monsterEditJsonFile = dialog.FileName;
				var monsterStats = JsonConvert.DeserializeObject<List<MonsterStatBlock>>(
						File.ReadAllText(monsterAidUserSettings.monsterEditJsonFile));
				monsterAidView.LoadEditedMonsterStats(romData, monsterStats, monsterAidUserSettings.monsterIndex);

				SaveNeeded(false);
			}
		}

		private void SaveAs_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!ChooseSaveFile())
				return;

			SaveData();
		}

		private void SaveToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(monsterAidUserSettings.monsterEditJsonFile))
			{
				if (!ChooseSaveFile())
				{
					MessageBox.Show("Must choose a file");
					return;
				}
			}

			SaveData();
		}

		private void SaveData()
		{
			monsterAidView.SaveStatBlockValues();
			File.WriteAllText(monsterAidUserSettings.monsterEditJsonFile,
				JsonConvert.SerializeObject(monsterAidView.monsters, Formatting.Indented));

			File.WriteAllText(AidUserSettings.monsterAidFormUserSettingsFile,
				JsonConvert.SerializeObject(monsterAidUserSettings, Formatting.Indented));

			SaveNeeded(false);
		}

		private bool ChooseSaveFile()
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.AddExtension = true;
			dialog.DefaultExt = "*" + AidUserSettings.monsterAidExtension;
			dialog.Filter = $"MonsterAid file (dialog.DefaultExt)|{dialog.DefaultExt}";

			var result = dialog.ShowDialog();
			if (result != DialogResult.OK)
			{
				return false;
			}

			monsterAidUserSettings.monsterEditJsonFile = dialog.FileName;

			return true;
		}

		private void RestoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var result = MessageBox.Show("You will lose any unsaved data.",
				"Restore monster data from ROM?", MessageBoxButtons.YesNo);

			if (result == DialogResult.Yes)
			{
				monsterAidView.LoadMonsterStatsFromROM(romData, monsterAidUserSettings.monsterIndex);
			}
		}

		private void InsertIntoROMToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveDialog = new SaveFileDialog();
			saveDialog.Filter = "iNES files (*.nes)|*.nes";
			saveDialog.FileName = Path.GetFileNameWithoutExtension(monsterAidUserSettings.romFile) + "_new.nes";
			var result = saveDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				byte[] monsterStats = monsterAidView.GetMonsterStats();
				for (int i = 0; i < monsterStats.Length; ++i)
					romData[ROM.MonsterStatBlockAddress.offset + i] = monsterStats[i];
				File.WriteAllBytes(saveDialog.FileName, romData);
			}
		}
	}
}
