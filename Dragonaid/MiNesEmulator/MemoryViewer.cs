using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace AtomosZ.MiNesEmulator
{
	public partial class MemoryViewer : UserControl
	{
		public MemoryViewer()
		{
			InitializeComponent();

			scrollTextBox.Clear();
			rowHeader_textBox.Clear();

			StringBuilder sb = new StringBuilder();
			int i;
			for (i = 0; i <= 0xFFFF; ++i)
			{
				if (i % 0x0010 == 0)
				{
					if (i != 0)
					{
						sb.Remove(sb.Length - 1, 1);
						sb.Append(Environment.NewLine);
					}
					rowHeader_textBox.Text += (i & 0xFFF0).ToString("X4") + Environment.NewLine;
				}

				sb.Append(/*"00 "*/ (i & 0xFF).ToString("X2") + " ");
			}
			rowHeader_textBox.Text = rowHeader_textBox.Text.Trim();

			scrollTextBox.Text = sb.ToString();
			scrollTextBox.row_textBox = rowHeader_textBox;

			scrollTextBox.Size = new Size(this.Size.Width - scrollTextBox.Location.X - 50, this.Size.Height - scrollTextBox.Location.Y - scrollTextBox.Margin.Vertical);
		}


		public void SetSize()
		{
			scrollTextBox.Size = new Size(this.Size.Width - scrollTextBox.Location.X - 50, this.Size.Height - scrollTextBox.Location.Y - scrollTextBox.Margin.Vertical);

			rowHeader_textBox.Size = new Size(rowHeader_textBox.Size.Width, scrollTextBox.Size.Height);
		}

		public void SetMemory(CPU cpu)
		{
			var mem = cpu.GetMemory();
			for (int i = 0; i < mem.Length; ++i)
			{
				//scrollTextBox.Text[i * 3] = mem[i] + " ";
			}
		}

	}
}
