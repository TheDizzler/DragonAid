using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtomosZ.DragonAid.PointerAid
{
	public partial class MultipleChoiceMessageBox : Form
	{
		public MultipleChoiceMessageBox()
		{
			InitializeComponent();
		}

		public bool[] GetResults()
		{
			return new bool[3] { ds07_checkBox.Checked, ds17_checkBox.Checked, lp_checkBox.Checked};
		}
	}
}
