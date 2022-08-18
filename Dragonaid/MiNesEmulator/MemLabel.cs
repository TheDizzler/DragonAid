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
	public partial class MemLabel : UserControl
	{
		public enum State
		{
			Normal, Focus
		}

		private State state;


		public MemLabel()
		{
			InitializeComponent();
		}

		public State LabelState
		{
			get { return state; }
			set
			{
				state = value;
				ChangeState();
			}
		}

		public override string Text
		{
			get { return mem_label.Text; }
			set { mem_label.Text = value; }
		}

		private void ChangeState()
		{
			switch (state)
			{
				case State.Focus:
					mem_label.BackColor = SystemColors.Window;
					break;
				case State.Normal:
					mem_label.BackColor = SystemColors.Control;
					break;
			}
		}
	}
}
