using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.PointerList;

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

			var pictureBoxes = new List<PictureBox>()
			{
				palette00_pictureBox, palette01_pictureBox, palette02_pictureBox, palette03_pictureBox,
				palette04_pictureBox, palette05_pictureBox, palette06_pictureBox, palette07_pictureBox,
				palette08_pictureBox, palette09_pictureBox, palette0A_pictureBox, palette0B_pictureBox,
				palette0C_pictureBox, palette0D_pictureBox, palette0E_pictureBox, palette0F_pictureBox,
			};

			paletteBoxes = new List<CaptionedPictureBox>();
			for (int i = 0; i < pictureBoxes.Count; ++i)
			{
				var captionPic = new CaptionedPictureBox();
				var cell = bgPalettes_tableLayoutPanel.GetCellPosition(pictureBoxes[i]);
				bgPalettes_tableLayoutPanel.Controls.Remove(pictureBoxes[i]);
				bgPalettes_tableLayoutPanel.Controls.Add(captionPic, cell.Column, cell.Row);
				//bgPalettes_tableLayoutPanel.SetCellPosition(captionPic, cell);
				paletteBoxes.Add(captionPic);
			}

			romData = File.ReadAllBytes(@"D:\github\RomHacking\Working ROMs\Dragon Warrior 3 (U).nes");
			sprite_pictureBox.Image = SpriteParser.GetTile(romData, (int)address_Spinner.Value);

			SetupDayCycleTimers(romData);
			PaletteTime_spinner_ValueChanged(null, null);
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
			byte paletteOffset = romData[ROM.TimeOfDayDayNightPalettesIndices.offset + timeIndex];

			for (byte i = 0; i < 0x0C; ++i)
			{
				byte color = romData[ROM.DayNightPalettes.offset + paletteOffset + i];
				byte paletteIndex = romData[ROM.PaletterStoreOffsets.offset + i];
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
				timeSpinners[timeIndex].Value = romData[ROM.TimeOfDayChangeTimes.offset + timeIndex];
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
	}
}
