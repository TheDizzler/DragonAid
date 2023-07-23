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
			Brushes.Black,
			Brushes.White,
			Brushes.LightGray,
			Brushes.Gray,
		};

		/// <summary>
		/// RGB values used by Mesen.
		/// </summary>
		public static Dictionary<byte, SolidBrush> paletteBrushes = new Dictionary<byte, SolidBrush>()
		{
			[0x3F] = new SolidBrush(Color.FromArgb(0, 0, 0)),
			[0x2F] = new SolidBrush(Color.FromArgb(0, 0, 0)),
			[0x1F] = new SolidBrush(Color.FromArgb(0, 0, 0)),
			[0x0F] = new SolidBrush(Color.FromArgb(0, 0, 0)),

			[0x3E] = new SolidBrush(Color.FromArgb(0, 0, 0)),
			[0x2E] = new SolidBrush(Color.FromArgb(0, 0, 0)),
			[0x1E] = new SolidBrush(Color.FromArgb(0, 0, 0)),
			[0x0E] = new SolidBrush(Color.FromArgb(0, 0, 0)),

			[0x3D] = new SolidBrush(Color.FromArgb(184, 184, 184)),
			[0x2D] = new SolidBrush(Color.FromArgb(79, 79, 79)),
			[0x1D] = new SolidBrush(Color.FromArgb(0, 0, 0)),
			[0x0D] = new SolidBrush(Color.FromArgb(0, 0, 0)), // !Do not use!

			[0x3C] = new SolidBrush(Color.FromArgb(181, 235, 242)),
			[0x2C] = new SolidBrush(Color.FromArgb(72, 205, 222)),
			[0x1C] = new SolidBrush(Color.FromArgb(0, 124, 141)),
			[0x0C] = new SolidBrush(Color.FromArgb(0, 64, 77)),

			[0x3B] = new SolidBrush(Color.FromArgb(179, 243, 204)),
			[0x2B] = new SolidBrush(Color.FromArgb(69, 224, 130)),
			[0x1B] = new SolidBrush(Color.FromArgb(0, 143, 50)),
			[0x0B] = new SolidBrush(Color.FromArgb(0, 79, 8)),

			[0x3A] = new SolidBrush(Color.FromArgb(189, 244, 171)),
			[0x2A] = new SolidBrush(Color.FromArgb(92, 228, 48)),
			[0x1A] = new SolidBrush(Color.FromArgb(12, 147, 0)),
			[0x0A] = new SolidBrush(Color.FromArgb(0, 82, 0)),

			[0x39] = new SolidBrush(Color.FromArgb(207, 239, 150)),
			[0x29] = new SolidBrush(Color.FromArgb(136, 216, 0)),
			[0x19] = new SolidBrush(Color.FromArgb(56, 135, 0)),
			[0x09] = new SolidBrush(Color.FromArgb(11, 72, 0)),

			[0x38] = new SolidBrush(Color.FromArgb(228, 229, 148)),
			[0x28] = new SolidBrush(Color.FromArgb(188, 190, 0)),
			[0x18] = new SolidBrush(Color.FromArgb(107, 109, 0)),
			[0x08] = new SolidBrush(Color.FromArgb(51, 53, 0)),

			[0x37] = new SolidBrush(Color.FromArgb(247, 216, 165)),
			[0x27] = new SolidBrush(Color.FromArgb(234, 158, 34)),
			[0x17] = new SolidBrush(Color.FromArgb(153, 78, 0)),
			[0x07] = new SolidBrush(Color.FromArgb(86, 29, 0)),

			[0x36] = new SolidBrush(Color.FromArgb(254, 204, 197)),
			[0x26] = new SolidBrush(Color.FromArgb(254, 129, 112)),
			[0x16] = new SolidBrush(Color.FromArgb(181, 49, 32)),
			[0x06] = new SolidBrush(Color.FromArgb(108, 6, 0)),

			[0x35] = new SolidBrush(Color.FromArgb(254, 196, 234)),
			[0x25] = new SolidBrush(Color.FromArgb(254, 110, 204)),
			[0x15] = new SolidBrush(Color.FromArgb(183, 30, 123)),
			[0x05] = new SolidBrush(Color.FromArgb(110, 0, 64)),

			[0x34] = new SolidBrush(Color.FromArgb(251, 194, 255)),
			[0x24] = new SolidBrush(Color.FromArgb(243, 106, 255)),
			[0x14] = new SolidBrush(Color.FromArgb(160, 26, 204)),
			[0x04] = new SolidBrush(Color.FromArgb(92, 0, 126)),

			[0x33] = new SolidBrush(Color.FromArgb(232, 200, 255)),
			[0x23] = new SolidBrush(Color.FromArgb(198, 118, 255)),
			[0x13] = new SolidBrush(Color.FromArgb(117, 39, 254)),
			[0x03] = new SolidBrush(Color.FromArgb(59, 0, 164)),

			[0x32] = new SolidBrush(Color.FromArgb(211, 210, 255)),
			[0x22] = new SolidBrush(Color.FromArgb(146, 144, 255)),
			[0x12] = new SolidBrush(Color.FromArgb(66, 64, 255)),
			[0x02] = new SolidBrush(Color.FromArgb(20, 18, 167)),

			[0x31] = new SolidBrush(Color.FromArgb(192, 223, 255)),
			[0x21] = new SolidBrush(Color.FromArgb(100, 176, 255)),
			[0x11] = new SolidBrush(Color.FromArgb(21, 95, 217)),
			[0x01] = new SolidBrush(Color.FromArgb(0, 42, 136)),

			[0x30] = new SolidBrush(Color.FromArgb(255, 254, 255)),
			[0x20] = new SolidBrush(Color.FromArgb(255, 254, 255)),
			[0x10] = new SolidBrush(Color.FromArgb(173, 173, 173)),
			[0x00] = new SolidBrush(Color.FromArgb(102, 102, 102)),
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


		public static Brush[] CreatePaletteBrushes(byte[] palette)
		{
			var results = new Brush[palette.Length];
			for (int i = 0; i < palette.Length; ++i)
				results[i] = paletteBrushes[palette[i]];
			return results;
		}

		/// <summary>
		/// Un-encrypted tiles only.
		/// </summary>
		/// <param name="romData"></param>
		/// <param name="tileStart"></param>
		public static Bitmap GetSprite(byte[] romData, int tileStart, byte[] paletteCodes = null)
		{
			Brush[] palette;
			if (paletteCodes == null)
				palette = grayscaleBrushes;
			else
				palette = CreatePaletteBrushes(paletteCodes);

			Bitmap bmp = new Bitmap(8 * scale, 8 * scale, PixelFormat.Format24bppRgb);
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
						var palleteIndex = bit0 + bit1 * 2;
						gfx.FillRectangle(palette[palleteIndex], i * scale, y * scale, scale, scale);
					}
					++y;
				}
			}

			return bmp;
		}


		public static Bitmap GetCHR(byte[] romData, byte tileIndex, byte[] paletteCodes = null, int scale = 2)
		{
			Brush[] palette;
			if (paletteCodes == null)
				palette = grayscaleBrushes;
			else
				palette = CreatePaletteBrushes(paletteCodes);

			Bitmap bmp = new Bitmap(8 * scale, 8 * scale, PixelFormat.Format24bppRgb);
			using (Graphics gfx = Graphics.FromImage(bmp))
			{
				if (tileIndex == 0)
				{
					gfx.FillRectangle(palette[0], 0, 0, 8 * scale, 8 * scale);

					return bmp;
				}

				int tileStart = PointerList.Pointers.ROM.CHR_Alphanumeric_Sprites.iNESAddress + ((tileIndex - 1) * 8);

				int y = 0;
				for (int tileByte = 0; tileByte < 8; ++tileByte)
				{
					byte b0 = romData[tileStart + tileByte];

					for (int i = 0; i < 8; ++i)
					{
						var bit0 = (b0 >> 7 - i) & 1;
						gfx.FillRectangle(palette[bit0], i * scale, y * scale, scale, scale);
					}
					++y;
				}
			}

			return bmp;
		}


		public static Bitmap CreateAlphanumericTileSheet(byte[] romData, byte[] paletteCodes = null, byte scale = 2)
		{
			byte horzSpriteCount = 16;
			byte vertSpriteCount = 9;
			byte spriteWidth = (byte)(scale * 8);

			Bitmap bmp = new Bitmap(horzSpriteCount * spriteWidth, 9 * spriteWidth, PixelFormat.Format24bppRgb);
			using (Graphics gfx = Graphics.FromImage(bmp))
			{
				for (byte i = 0; i < horzSpriteCount; ++i)
				{
					for (byte y = 0; y < vertSpriteCount; ++y)
					{
						var tile = GetCHR(romData, (byte)(i + y * horzSpriteCount), paletteCodes, scale);
						gfx.DrawImage(tile, i * spriteWidth, y * spriteWidth);
					}
				}
			}

			return bmp;
		}


		public static Bitmap GetSolidColor(byte colorCode, Size bmpSize, byte scale = 2)
		{
			Bitmap bmp = new Bitmap(bmpSize.Width * scale, bmpSize.Height * scale, PixelFormat.Format24bppRgb);
			using (Graphics gfx = Graphics.FromImage(bmp))
			{
				int y = 0;
				gfx.FillRectangle(paletteBrushes[colorCode], 0, 0, bmpSize.Width * scale, bmpSize.Height * scale);
			}

			return bmp;
		}


		public static void AddToImage(Image image, Bitmap topLeftCornerSprite, int xPos, int yPos, byte scale = 2)
		{
			byte spriteWidth = (byte)(scale * 8);
			using (Graphics gfx = Graphics.FromImage(image))
			{
				gfx.DrawImage(topLeftCornerSprite, xPos * spriteWidth, yPos * spriteWidth);
			}
		}
	}
}
