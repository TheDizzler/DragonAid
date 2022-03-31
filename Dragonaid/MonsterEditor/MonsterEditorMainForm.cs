using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace AtomosZ.Dragonaid.MonsterEditor
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


		public MonsterEditorMainForm()
		{
			InitializeComponent();

			byte[] byteData = File.ReadAllBytes(@"D:\github\RomHacking\Working ROMs\Dragon Warrior 3 (U).nes");
			monsterEditorView.LoadMonsterStats(byteData, 0);



			//GetTile(byteData, 0x20010);
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
	}
}
