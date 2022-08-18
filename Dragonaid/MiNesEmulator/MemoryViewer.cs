using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static AtomosZ.MiNesEmulator.MemLabel;

namespace AtomosZ.MiNesEmulator
{
	public partial class MemoryViewer : UserControl
	{
		public MemoryViewer()
		{
			InitializeComponent();

			this.SuspendLayout();
			rowHeader_flowLayoutPanel.Controls.Clear();
			code_flowLayoutPanel.Controls.Clear();
			for (int i = 0; i <= 0x1FF; ++i)
			{
				var codeLbl = new MemLabel();
				codeLbl.LabelState = State.Focus;
				codeLbl.Text = 0.ToString("X2");
				code_flowLayoutPanel.Controls.Add(codeLbl);
				if (i % 0x10 == 0)
				{
					var label = new MemLabel();
					label.LabelState = State.Normal;
					int row = i & 0xFFF0;
					label.Text = row.ToString("X4");
					rowHeader_flowLayoutPanel.Controls.Add(label);
				}
			}

			code_flowLayoutPanel.Size = new Size(colHeader_flowLayoutPanel.Size.Width, code_flowLayoutPanel.Size.Height);

			this.ResumeLayout();
		}

		public void SetZeroPages(byte[] zeroPages)
		{
			this.SuspendLayout();
			for (int i = 0; i < zeroPages.Length; ++i)
			{
				var mem = (MemLabel)code_flowLayoutPanel.Controls[i];
				mem.Text = zeroPages[i].ToString("X2");
			}

			this.ResumeLayout();
		}

		public void SetStack(byte[] theStack)
		{
			this.SuspendLayout();
			for (int i = 0; i < theStack.Length; ++i)
			{
				var mem = (MemLabel)code_flowLayoutPanel.Controls[i + CPU.Stack.stackStart];
				mem.Text = theStack[i].ToString("X2");
			}

			this.ResumeLayout();
		}
	}
}
