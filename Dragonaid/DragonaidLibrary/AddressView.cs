using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtomosZ.DragonAid.Libraries
{
	public partial class AddressView : UserControl
	{
		private DynamicSubroutine selected;

		public AddressView()
		{
			InitializeComponent();

			name_textBox.LostFocus += Name_textBox_LostFocus;
			notes_richTextBox.LostFocus += Name_textBox_LostFocus;
		}

		public void LoadDynamicSubroutine(DynamicSubroutine selectedItem)
		{
			selected = selectedItem;

			length_label.Visible = false;
			length_spinner.Visible = false;

			if (selected == null)
			{
				name_textBox.Text = "Null Pointer";
				address_textBox.Text = null;
				offset_textBox.Text = null;
				notes_richTextBox.Text = null;

				name_textBox.Enabled = false;
				address_textBox.Enabled = false;
				offset_textBox.Enabled = false;
				notes_richTextBox.Enabled = false;
			}
			else
			{
				name_textBox.Text = selectedItem.name;
				address_textBox.Text = "0x" + selectedItem.prgAddress.pointer.ToString("X5");
				offset_textBox.Text = "0x" + selectedItem.prgAddress.offset.ToString("X5");
				//length_spinner.Value = selectedItem.prgAddress.length;
				notes_richTextBox.Text = selectedItem.prgAddress.notes;

				name_textBox.Enabled = true;
				address_textBox.Enabled = false;
				offset_textBox.Enabled = false;
				notes_richTextBox.Enabled = true;
			}
		}


		private void Name_textBox_LostFocus(object sender, EventArgs e)
		{
			if (sender is TextBox)
			{
				TextBox tb = (TextBox)sender;
				switch (tb.Name)
				{
					case "name_textBox":
						if (selected.name != name_textBox.Text)
						{
							selected.name = name_textBox.Text;
							((UserControlParent)ParentForm).UpdateView();
						}
						break;

					case "address_textBox":
						//address_textBox.Text = "0x" + selected.prgAddress.pointer.ToString("X5");
						selected.prgAddress.ValidateAddress(address_textBox.Text);
						break;

					case "offset_textBox":
						//offset_textBox.Text = "0x" + selected.prgAddress.offset.ToString("X5");
						break;

					default:
						Debug.WriteLine($"Wut? {tb.Name}");
						break;
				}
			}
			else if (sender is RichTextBox)
			{
				var rtb = (RichTextBox)sender;
				if (selected.prgAddress.notes != rtb.Text)
				{
					selected.prgAddress.notes = rtb.Text;
					((UserControlParent)ParentForm).UpdateView();
				}
			}
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox tb = (TextBox)sender;
			if (e.KeyCode == Keys.Escape)
			{
				switch (tb.Name)
				{
					case "name_textBox":
						name_textBox.Text = selected.name;
						break;

					case "address_textBox":
						address_textBox.Text = "0x" + selected.prgAddress.pointer.ToString("X5");
						break;

					case "offset_textBox":
						offset_textBox.Text = "0x" + selected.prgAddress.offset.ToString("X5");
						break;

					default:
						Debug.WriteLine($"Wut? {tb.Name}");
						break;
				}

				e.Handled = true;
				e.SuppressKeyPress = true;
				tb.SelectionLength = 0;
				Defocus_Click(null, null);

				return;
			}


			switch (tb.Name)
			{
				case "name_textBox":
				case "notes_richTextBox":

					break;
				case "address_textBox":
				case "offset_textBox":
					if ((e.KeyValue >= '0' && e.KeyValue <= '9')
						|| (e.KeyValue >= 'a' && e.KeyValue <= 'f') || (e.KeyValue >= 'A' && e.KeyValue <= 'F')
						|| (e.KeyCode == Keys.Back))
					{

					}
					else
					{
						e.SuppressKeyPress = true;
					}
					break;
			}

			e.Handled = true;
		}

		public void Defocus_Click(object sender, EventArgs e)
		{
			this.ActiveControl = null;
			((UserControlParent)ParentForm).Defocus(sender, e);
		}
	}
}
