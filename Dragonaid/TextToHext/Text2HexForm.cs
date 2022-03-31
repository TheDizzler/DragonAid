using System;
using System.Windows.Forms;
using Dragonaid.TextToHex;

namespace DragonAid.TextToHex
{
	public partial class Text2HexForm : Form
	{
		public Text2HexForm()
		{
			InitializeComponent();
			warning_label.Text = "";
			input_richTextBox.AutoWordSelection = false;
		}

		private void Input_textBox_TextChanged(object sender, EventArgs e)
		{
			hex_richTextBox.Text = "";
			string warningText = "";
			foreach (char c in input_richTextBox.Text)
			{
				var check = c;
				if (check == ' ')
					check = '∙';
				if (!Tables.textToHexDict.ContainsKey(check.ToString()))
				{
					warningText = c + " is not a valid character\n";
					hex_richTextBox.Text += " ¿";
				}
				else
				{
					hex_richTextBox.Text += Tables.textToHexDict[check.ToString()].ToString("X2") + " ";
				}
			}

			warning_label.Text = warningText;
		}

		//private void MirrorSelection(object sender, MouseEventArgs  e)
		//{
		//	int charStart = input_textBox.SelectionStart;
		//	int charLength = input_textBox.SelectionLength;

		//	//hex_textBox.Select(charStart * 3, charLength * 3);
		//	hex_textBox.SelectionStart = charStart * 3;
		//	hex_textBox.SelectionLength = charLength * 3;
		//	hex_textBox.Select();
		//	input_textBox.Focus();
		//	Debug.WriteLine(hex_textBox.SelectedText);
		//}

		private void Copy_button_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(hex_richTextBox.Text);
		}
	}
}
