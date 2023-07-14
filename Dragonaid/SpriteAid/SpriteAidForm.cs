using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AtomosZ.DragonAid.Libraries;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using System.Diagnostics;

namespace AtomosZ.DragonAid.SpriteAid
{
	public partial class SpriteAidForm : Form
	{
		private List<NumericUpDown> timeSpinners;
		private List<CaptionedPictureBox> paletteBoxes;
		private byte[] romData;

		public SpriteAidForm()
		{
			InitializeComponent();

			timeSpinners = new List<NumericUpDown>()
			{
				morningStart_spinner, lateMorning_spinner, afternoon_spinner,
				evening_spinner, dusk_spinner, night_spinner, lateNight_spinner,
			};


			paletteBoxes = new List<CaptionedPictureBox>();
			for (int i = 0; i < 4; ++i)
			{
				for (int j = 0; j < 4; ++j)
				{
					var captionPic = new CaptionedPictureBox();
					captionPic.OnMouseClick += Palette_pictureBox_MouseClick;
					bgPalettes_tableLayoutPanel.Controls.Add(captionPic, j, i);
					paletteBoxes.Add(captionPic);
				}
			}

			romData = File.ReadAllBytes(@"D:\github\RomHacking\Working ROMs\Dragon Warrior 3 (U).nes");
			sprite_pictureBox.Image = SpriteParser.GetTile(romData, (int)address_Spinner.Value);

			SetupDayCycleTimers(romData);
			PaletteTime_spinner_ValueChanged(null, null);

			palette_comboBox.DataSource = new BindingSource(new List<int> { 0, 1, 2, 3 }, null);
			palette_comboBox.SelectedValueChanged += PaletteChanged;

		}


		private void DisplayDayNightPalette(byte[] romData)
		{
			paletteBoxes[0].Image = SpriteParser.GetSolidColor(0x0F, paletteBoxes[0].Size);
			paletteBoxes[0].Text = "0x0F";
			paletteBoxes[0].ForeColor = Color.White;
			paletteBoxes[4].Image = SpriteParser.GetSolidColor(0x0F, paletteBoxes[0].Size);
			paletteBoxes[4].Text = "0x0F";
			paletteBoxes[4].ForeColor = Color.White;
			paletteBoxes[8].Image = SpriteParser.GetSolidColor(0x0F, paletteBoxes[0].Size);
			paletteBoxes[8].Text = "0x0F";
			paletteBoxes[8].ForeColor = Color.White;
			paletteBoxes[12].Image = SpriteParser.GetSolidColor(0x0F, paletteBoxes[0].Size);
			paletteBoxes[12].Text = "0x0F";
			paletteBoxes[12].ForeColor = Color.White;

			int timeIndex = (int)paletteTime_spinner.Value;
			byte paletteOffset = romData[ROM.TimeOfDayDayNightPalettesIndices.iNESAddress + timeIndex];

			for (byte i = 0; i < 0x0C; ++i)
			{
				byte color = romData[ROM.DayNightPalettes.iNESAddress + paletteOffset + i];
				byte paletteIndex = romData[ROM.PaletteStoreOffsets.iNESAddress + i];
				if (paletteIndex >= 4)
					++paletteIndex;
				if (paletteIndex >= 8)
					++paletteIndex;
				if (paletteIndex >= 12)
					++paletteIndex;
				paletteBoxes[paletteIndex].Image = SpriteParser.GetSolidColor(color, paletteBoxes[paletteIndex].Size);
				paletteBoxes[paletteIndex].Text = "0x" + color.ToString("X2");
				if ((color <= 0x0F) || (color & 0x0F) >= 0x0D)
					paletteBoxes[paletteIndex].ForeColor = Color.White;
				else
					paletteBoxes[paletteIndex].ForeColor = Color.Black;
			}
		}



		private void SetupDayCycleTimers(byte[] romData)
		{
			for (int timeIndex = 0; timeIndex < 7; ++timeIndex)
			{
				timeSpinners[timeIndex].Value = romData[ROM.TimeOfDayChangeTimes.iNESAddress + timeIndex];
			}
		}

		private void Address_Spinner_ValueChanged(object sender, EventArgs e)
		{
			sprite_pictureBox.Image = SpriteParser.GetTile(romData, (int)address_Spinner.Value);
		}

		private void DayCycleLength_spinner_ValueChanged(object sender, EventArgs e)
		{
			int maxDec = 1;
			for (int i = timeSpinners.Count - 1; i >= 0; --i)
			{
				timeSpinners[i].Maximum = dayCycleLength_spinner.Value - maxDec++;
			}
		}

		private void PaletteTime_spinner_ValueChanged(object sender, EventArgs e)
		{
			DisplayDayNightPalette(romData);
			int timeIndex = (int)paletteTime_spinner.Value;
			time_label.Text = ((int)timeSpinners[timeIndex - 1].Value).ToString("X2");
		}


		private void PaletteChanged(object sender, EventArgs e)
		{
			var paletteIndex = palette_comboBox.SelectedIndex;
			var selectedPalette = new byte[4];

			for (int i = 0; i < 4; ++i)
			{
				var colorText = paletteBoxes[i + paletteIndex * 4].Text.Replace("0x", "");
				selectedPalette[i] = Convert.ToByte(colorText, 16);
			}

			sprite_pictureBox.Image = SpriteParser.GetTile(romData, (int)address_Spinner.Value, selectedPalette);
		}

		private void Palette_pictureBox_MouseClick(object sender, MouseEventArgs e)
		{
			var colorPicker = new ColorPicker();
			if (colorPicker.ShowDialog() == DialogResult.OK)
			{
				var a = sender as PictureBox;
				if (a != null)
					Console.WriteLine(a.GetHashCode());

				var capPicBox = (CaptionedPictureBox)sender;
				var newColor = colorPicker.result;
				capPicBox.Image = SpriteParser.GetSolidColor(newColor, paletteBoxes[0].Size);
				capPicBox.Text = "0x" + newColor.ToString("X2");
			}
		}

		private void palette00_pictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			Debug.WriteLine("Hey");
		}
	}
}
