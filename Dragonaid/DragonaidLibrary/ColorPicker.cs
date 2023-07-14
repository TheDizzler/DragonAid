using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtomosZ.DragonAid.Libraries
{
	public partial class ColorPicker : Form
	{
		public int boxSize = 32;
		public byte result;

		public ColorPicker()
		{
			InitializeComponent();
			var size = new Size(boxSize, boxSize);
			for (byte i = 0; i < 4; ++i)
			{
				for (byte j = 0; j < 8; ++j)
				{
					byte colorByte = (byte)(i * 0x10 + j);
					var paletteBox = new CaptionedPictureBox();
					paletteBox.Image = SpriteParser.GetSolidColor(colorByte, size);
					paletteBox.Size = size;
					paletteBox.Text = "0x" + (colorByte).ToString("X2");
					paletteBox.OnMouseClick += OnColorClick;
					palette_tableLayoutPanel.Controls.Add(paletteBox, j, i);
				}
			}

			for (byte i = 0; i < 4; ++i)
			{
				for (byte j = 0; j < 8; ++j)
				{
					byte colorByte = (byte)(i * 0x10 + (j + 8));
					var paletteBox = new CaptionedPictureBox();
					paletteBox.Image = SpriteParser.GetSolidColor(colorByte, size);
					paletteBox.Size = size;
					paletteBox.Text = "0x" + (colorByte).ToString("X2");
					paletteBox.OnMouseClick += OnColorClick;
					palette_tableLayoutPanel.Controls.Add(paletteBox, j, i + 4);
				}
			}
		}

		private void OnColorClick(object sender, MouseEventArgs e)
		{
			var a = sender as PictureBox;
			if (a != null)
				Console.WriteLine(a.GetHashCode());
			var capPicBox = (CaptionedPictureBox)sender;
			var index = palette_tableLayoutPanel.Controls.GetChildIndex(capPicBox);
			var colorText = capPicBox.Text.Replace("0x", "");
			result = Convert.ToByte(colorText, 16);

			DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
