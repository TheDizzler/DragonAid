using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using AtomosZ.DragonAid.Libraries;


namespace AtomosZ.DragonAid.MonsterEditor
{
	public partial class MonsterEditorMainForm : Form
	{
		public int scale = 8;

		Brush[] grayscaleBrushes = new Brush[4]
		{
			Brushes.White,
			Brushes.LightGray,
			Brushes.Gray,
			Brushes.Black,
		};

		public class CharacterSpriteData
		{
			public class SpriteAnimation
			{
				public enum SpriteFacing { Up, Down, Right, Left };
				public SpriteFacing facing;
				public int[] frame0Address;
				public int[] frame1Address;
			}

			public string name;

			public SpriteAnimation[] animations;
		}

		private MonsterAidSettingsData monsterAidUserSettings;
		private byte monsterIndex = 0;
		private byte[] romData;

		public MonsterEditorMainForm()
		{
			InitializeComponent();


			if (!File.Exists(MonsterAidSettingsData.monsterAidFormUserSettingsFile))
			{
				while (!AidUserSettings.InitializeUserSettings(out monsterAidUserSettings))
				{ }

				//ExtractDynamicPointers(romData);
			}
			else
			{
				//monsterAidUserSettings = JsonConvert.DeserializeObject<MonsterAidSettingsData>(
				//	File.ReadAllText(monsterAidFormUserSettingsFile));
				//romData = File.ReadAllBytes(monsterAidUserSettings.romFile);
				//if (string.IsNullOrEmpty(pointerAid.dynamicPointersJsonFile))
				//{
				//	ExtractDynamicPointers(romData);
				//}
				//else
				//{
				//	pointerData = JsonConvert.DeserializeObject<DynamicPointerData>(
				//		File.ReadAllText(pointerAid.dynamicPointersJsonFile));
				//}
			}

			//romData = File.ReadAllBytes(@"D:\github\RomHacking\Working ROMs\Dragon Warrior 3 (U).nes");
			//monsterEditorView.LoadMonsterStats(romData, monsterIndex);

			//Initialize();

			//GetTile(byteData, 0x20010);
		}


		//private bool InitializeUserSettings()
		//{
		//	FileDialog fileDialog = new OpenFileDialog();
		//	fileDialog.Filter = "iNES files (*.nes)|*.nes";
		//	fileDialog.CheckFileExists = true;

		//	var result = fileDialog.ShowDialog();
		//	if (result == DialogResult.Cancel)
		//	{
		//		Environment.Exit(0);
		//	}
		//	else if (result == DialogResult.OK)
		//	{
		//		var file = fileDialog.FileName;
		//		romData = File.ReadAllBytes(file);
		//		if (romData[0] != 0x4E && romData[1] != 0x45 && romData[2] != 0x53 && romData[3] != 0x1A)
		//		{
		//			MessageBox.Show("Invalid iNES file");
		//			return false;
		//		}

		//		monsterAidUserSettings = new MonsterAidSettingsData();
		//		monsterAidUserSettings.romFile = file;

		//		File.WriteAllText(monsterAidFormUserSettingsFile,
		//			JsonConvert.SerializeObject(monsterAidUserSettings, Formatting.Indented));
		//	}

		//	return true;
		//}

		private void NextMonster_button_Click(object sender, System.EventArgs e)
		{
			if (++monsterIndex > UniversalConsts.monsterCount)
				monsterIndex = 0;
			monsterEditorView.LoadMonsterStats(romData, monsterIndex);
		}

		private void PrevMonster_button_Click(object sender, System.EventArgs e)
		{
			if (--monsterIndex > UniversalConsts.monsterCount)
				monsterIndex = UniversalConsts.monsterCount;
			monsterEditorView.LoadMonsterStats(romData, monsterIndex);
		}


		/// <summary>
		/// Un-encrypted tiles only.
		/// </summary>
		/// <param name="byteData"></param>
		/// <param name="tileStart"></param>
		public void GetTile(byte[] byteData, int tileStart)
		{
			Bitmap bmp = new Bitmap(64, 64, PixelFormat.Format24bppRgb);
			using (Graphics gfx = Graphics.FromImage(bmp))
			{
				int y = 0;
				for (int tileByte = 0; tileByte < 8; ++tileByte)
				{
					byte b0 = byteData[tileStart + tileByte];
					byte b1 = byteData[tileStart + tileByte + 8];

					for (int i = 0; i < 8; ++i)
					{
						var bit0 = (b0 >> 7 - i) & 1;
						var bit1 = (b1 >> 7 - i) & 1;

						var pallete = bit0 + bit1 * 2;

						gfx.FillRectangle(grayscaleBrushes[pallete], i * scale, y * scale, scale, scale);
					}

					++y;
				}

				//tile_pictureBox.Image = bmp;
			}
		}

		private void LoadROMToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.AddExtension = true;
			dialog.DefaultExt = "*" + AidUserSettings.monsterAidExtension;
			dialog.Filter = $"DragonAid Monster file ({dialog.DefaultExt})|{dialog.DefaultExt}";

			var result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				//monsterAidUserSettings.dynamicPointersJsonFile = dialog.FileName;
				//pointerData = JsonConvert.DeserializeObject<DynamicPointerData>(
				//	File.ReadAllText(pointerAid.dynamicPointersJsonFile));
				//Initialize();

				//SaveNeeded(false);
			}
		}

		private void ExitToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}
	}
}
