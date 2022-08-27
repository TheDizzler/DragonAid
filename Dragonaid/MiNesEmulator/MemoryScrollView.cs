using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace AtomosZ.MiNesEmulator
{
	public partial class MemoryScrollView : UserControl
	{
		public MemoryScrollView()
		{
			InitializeComponent();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="bytesToInitialize"></param>
		/// <param name="startFrom">where to start the row header count from</param>
		public void Initialize(int bytesToInitialize, int startFrom = 0x0000)
		{
			if (bytesToInitialize > 0x8000)
			{
				MessageBox.Show("WARNING: Winforms does not play well with scroll areas"
					+ " bigger than what are needed to view over 0x800 rows :(");
			}


			var g = rowHeader_richTextBox.CreateGraphics();
			var fontSize = g.MeasureString("0000", rowHeader_richTextBox.Font);

			rowHeader_richTextBox.Clear();
			exRichTextBox.Clear();

			StringBuilder sbCode = new StringBuilder();
			StringBuilder rowHeader = new StringBuilder();

			int i;
			for (i = 0; i < bytesToInitialize; ++i)
			{
				if (i % 0x0010 == 0)
				{
					if (i != 0)
					{
						sbCode.Remove(sbCode.Length - 1, 1);
						sbCode.Append(Environment.NewLine);
					}
					rowHeader.Append(((i & 0xFFF0) + startFrom).ToString("X4") + Environment.NewLine);
				}

				sbCode.Append((i & 0xFF).ToString("X2") + " ");
				//sbCode.Append("00 ");
				//sbCode.Append((i) + " ");
			}

			var height = (int)(fontSize.Height) * ((bytesToInitialize & 0xFFF0) / 16);

			exRichTextBox.Text = sbCode.ToString().Trim();
			exRichTextBox.Height = height;

			rowHeader_richTextBox.Text = rowHeader.ToString().Trim();
			rowHeader_richTextBox.Height = height;
			rowHeader_richTextBox.Width = (int)fontSize.Width;
		}


		public void SetMemory(byte[] bytes)
		{
			/* !this takes 9 seconds! */
			//nt charIndex = 0;
			//for (int i = 0; i < bytes.Length; ++i)
			//{
			//	exRichTextBox.SelectionStart = charIndex;
			//	exRichTextBox.SelectionLength = 2;
			//	var newByte = bytes[i].ToString("X2");
			//	exRichTextBox.SelectedText = newByte;

			//	charIndex += 3;
			//}

			/* Whereas this takes only .02 seconds! */
			StringBuilder sbCode = new StringBuilder();
			exRichTextBox.Clear();
			for (int i = 0; i < bytes.Length; ++i)
			{
				if (i % 0x0010 == 0)
				{
					if (i != 0)
					{
						sbCode.Remove(sbCode.Length - 1, 1);
						sbCode.Append(Environment.NewLine);
					}
				}

				sbCode.Append(bytes[i].ToString("X2") + " ");
			}

			exRichTextBox.Text = sbCode.ToString().Trim();
		}
	}
}
