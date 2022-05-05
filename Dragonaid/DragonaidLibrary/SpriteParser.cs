using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;

namespace AtomosZ.DragonAid.Libraries
{
	public class SpriteParser
	{
		public static int scale = 8;

		public static Brush[] grayscaleBrushes = new Brush[4]
		{
			Brushes.White,
			Brushes.LightGray,
			Brushes.Gray,
			Brushes.Black,
		};

		/// <summary>
		/// RGB values eyedropped from NerdyNights NES Tutorials pdf.
		/// </summary>
		public static Dictionary<byte, SolidBrush> paletteBrushes = new Dictionary<byte, SolidBrush>()
		{
			[0x3F] = new SolidBrush(Color.FromArgb(9, 9, 9)),
			[0x2F] = new SolidBrush(Color.FromArgb(5, 5, 5)),
			[0x1F] = new SolidBrush(Color.FromArgb(4, 4, 4)),
			[0x0F] = new SolidBrush(Color.FromArgb(0, 0, 0)),

			[0x3E] = new SolidBrush(Color.FromArgb(9, 9, 9)),
			[0x2E] = new SolidBrush(Color.FromArgb(5, 5, 5)),
			[0x1E] = new SolidBrush(Color.FromArgb(4, 4, 4)),
			[0x0E] = new SolidBrush(Color.FromArgb(0, 0, 0)),

			[0x3D] = new SolidBrush(Color.FromArgb(214, 214, 214)),
			[0x2D] = new SolidBrush(Color.FromArgb(74, 74, 74)),
			[0x1D] = new SolidBrush(Color.FromArgb(20, 20, 20)),
			[0x0D] = new SolidBrush(Color.FromArgb(0, 0, 0)), // !Do not use!

			[0x3C] = new SolidBrush(Color.FromArgb(67, 255, 252)),
			[0x2C] = new SolidBrush(Color.FromArgb(0, 255, 255)),
			[0x1C] = new SolidBrush(Color.FromArgb(0, 139, 198)),
			[0x0C] = new SolidBrush(Color.FromArgb(0, 48, 84)),

			[0x3B] = new SolidBrush(Color.FromArgb(100, 244, 209)),
			[0x2B] = new SolidBrush(Color.FromArgb(0, 246, 144)),
			[0x1B] = new SolidBrush(Color.FromArgb(0, 123, 63)),
			[0x0B] = new SolidBrush(Color.FromArgb(0, 55, 30)),

			[0x3A] = new SolidBrush(Color.FromArgb(110, 238, 156)),
			[0x2A] = new SolidBrush(Color.FromArgb(0, 246, 0)),
			[0x1A] = new SolidBrush(Color.FromArgb(0, 130, 0)),
			[0x0A] = new SolidBrush(Color.FromArgb(0, 57, 0)),

			[0x39] = new SolidBrush(Color.FromArgb(199, 229, 123)),
			[0x29] = new SolidBrush(Color.FromArgb(103, 227, 0)),
			[0x19] = new SolidBrush(Color.FromArgb(0, 113, 0)),
			[0x09] = new SolidBrush(Color.FromArgb(0, 53, 0)),

			[0x38] = new SolidBrush(Color.FromArgb(255, 246, 130)),
			[0x28] = new SolidBrush(Color.FromArgb(255, 172, 0)),
			[0x18] = new SolidBrush(Color.FromArgb(210, 69, 0)),
			[0x08] = new SolidBrush(Color.FromArgb(82, 29, 0)),

			[0x37] = new SolidBrush(Color.FromArgb(255, 235, 143)),
			[0x27] = new SolidBrush(Color.FromArgb(255, 131, 0)),
			[0x17] = new SolidBrush(Color.FromArgb(240, 0, 0)),
			[0x07] = new SolidBrush(Color.FromArgb(143, 0, 0)),

			[0x36] = new SolidBrush(Color.FromArgb(252, 187, 148)),
			[0x26] = new SolidBrush(Color.FromArgb(255, 106, 9)),
			[0x16] = new SolidBrush(Color.FromArgb(255, 0, 0)),
			[0x06] = new SolidBrush(Color.FromArgb(204, 0, 0)),

			[0x35] = new SolidBrush(Color.FromArgb(255, 149, 164)),
			[0x25] = new SolidBrush(Color.FromArgb(255, 55, 120)),
			[0x15] = new SolidBrush(Color.FromArgb(255, 0, 56)),
			[0x05] = new SolidBrush(Color.FromArgb(221, 0, 18)),

			[0x34] = new SolidBrush(Color.FromArgb(255, 142, 252)),
			[0x24] = new SolidBrush(Color.FromArgb(255, 0, 246)),
			[0x14] = new SolidBrush(Color.FromArgb(255, 0, 170)),
			[0x04] = new SolidBrush(Color.FromArgb(171, 0, 74)),

			[0x33] = new SolidBrush(Color.FromArgb(227, 151, 234)),
			[0x23] = new SolidBrush(Color.FromArgb(229, 98, 255)),
			[0x13] = new SolidBrush(Color.FromArgb(130, 16, 255)),
			[0x03] = new SolidBrush(Color.FromArgb(60, 0, 136)),

			[0x32] = new SolidBrush(Color.FromArgb(137, 235, 255)),
			[0x22] = new SolidBrush(Color.FromArgb(56, 147, 255)),
			[0x12] = new SolidBrush(Color.FromArgb(0, 62, 255)),
			[0x02] = new SolidBrush(Color.FromArgb(10, 0, 167)),

			[0x31] = new SolidBrush(Color.FromArgb(100, 255, 255)),
			[0x21] = new SolidBrush(Color.FromArgb(0, 213, 255)),
			[0x11] = new SolidBrush(Color.FromArgb(0, 99, 255)),
			[0x01] = new SolidBrush(Color.FromArgb(0, 42, 155)),

			[0x30] = new SolidBrush(Color.FromArgb(255, 255, 255)),
			[0x20] = new SolidBrush(Color.FromArgb(255, 255, 255)),
			[0x10] = new SolidBrush(Color.FromArgb(188, 188, 188)),
			[0x00] = new SolidBrush(Color.FromArgb(109, 109, 109)),
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

		/// <summary>
		/// Un-encrypted tiles only.
		/// </summary>
		/// <param name="romData"></param>
		/// <param name="tileStart"></param>
		public static Bitmap GetTile(byte[] romData, int tileStart)
		{
			Bitmap bmp = new Bitmap(64, 64, PixelFormat.Format24bppRgb);
			using (Graphics gfx = Graphics.FromImage(bmp))
			{
				int y = 0;
				for (int tileByte = 0; tileByte < 8; ++tileByte)
				{
					byte b0 = romData[tileStart + tileByte];
					byte b1 = romData[tileStart + tileByte + 8];

					for (int i = 0; i < 8; ++i)
					{
						var bit0 = (b0 >> 7 - i) & 1;
						var bit1 = (b1 >> 7 - i) & 1;
						var pallete = bit0 + bit1 * 2;
						gfx.FillRectangle(grayscaleBrushes[pallete], i * scale, y * scale, scale, scale);
					}
					++y;
				}
			}

			return bmp;
		}

		public static Bitmap GetSolidColor(byte colorCode, Size bmpSize)
		{
			Bitmap bmp = new Bitmap(bmpSize.Width, bmpSize.Height, PixelFormat.Format24bppRgb);
			using (Graphics gfx = Graphics.FromImage(bmp))
			{
				int y = 0;
				gfx.FillRectangle(paletteBrushes[colorCode], 0, 0, bmpSize.Width, bmpSize.Height);
			}

			return bmp;
		}
	}
}
