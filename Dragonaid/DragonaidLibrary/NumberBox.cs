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
	public partial class NumberBox : UserControl
	{
		public NumberBox()
		{
			InitializeComponent();
			numericUpDown.Controls.RemoveAt(0);
		}


		public bool Hexadecimal
		{
			get { return numericUpDown.Hexadecimal; }
			set { numericUpDown.Hexadecimal = value; }
		}

		public decimal Maximum
		{
			get { return numericUpDown.Maximum; }
			set { numericUpDown.Maximum = value; }
		}

		public decimal Minimum
		{
			get { return numericUpDown.Minimum; }
			set { numericUpDown.Minimum = value; }
		}

		public decimal Increment
		{
			get { return numericUpDown.Increment; }
			set { numericUpDown.Increment = value; }
		}

		public int Value
		{
			get { return (int)numericUpDown.Value; }
			set { numericUpDown.Value = value; }
		}
	}
}
