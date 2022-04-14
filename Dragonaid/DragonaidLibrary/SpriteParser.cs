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
	}
}
