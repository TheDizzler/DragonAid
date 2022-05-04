using System.Collections.Generic;
using System.Windows.Forms;
using AtomosZ.DragonAid.Libraries;

namespace AtomosZ.DragonAid.PointerAid
{
	public partial class SearchMessageBox : Form
	{
		public SearchMessageBox()
		{
			InitializeComponent();
		}

		public DynamicSubroutine selectedSubroutine
		{
			get { return (DynamicSubroutine)subroutines_listBox.SelectedItem; }
		}

		public void AddResults(List<DynamicSubroutine> subroutines)
		{
			search_label.Text = "Dynamic Subroutines found with address: 0x" + subroutines[0].prgAddress.pointer.ToString("X5");
			foreach (var ds in subroutines)
				subroutines_listBox.Items.Add(ds);
			subroutines_listBox.SelectedIndex = 0;
		}
	}
}
