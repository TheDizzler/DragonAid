using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtomosZ.MiNesEmulator
{
	public partial class ExRichTextBox : RichTextBox
	{
		const int WM_SETCURSOR = 0x0020;

		public ExRichTextBox()
		{
			InitializeComponent();
		}

		/// <summary>
		/// This prevents the cursor from flickering.
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_SETCURSOR)
				Cursor.Current = this.Cursor;
			else
				base.WndProc(ref m);
		}

		private void ScrollTextBox_Click(object sender, EventArgs e)
		{
			var charIndex = this.SelectionStart;
			var line = this.GetLineFromCharIndex(charIndex);
			var firstCharIndex = this.GetFirstCharIndexFromLine(line);
			var diff = (charIndex - firstCharIndex);
			var remain = diff % 3;
			this.SelectionStart = charIndex - remain;
			this.SelectionLength = 2;
		}
	}
}
