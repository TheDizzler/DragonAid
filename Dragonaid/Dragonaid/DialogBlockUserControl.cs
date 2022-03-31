using System.Windows.Forms;

namespace Dragonaid
{
	public partial class DialogBlockUserControl : UserControl
	{
		public DialogBlockUserControl()
		{
			InitializeComponent();
		}

		public void SetBlock(int pointer, int offset, string text)
		{
			pointerAddr_label.Text = "$" + pointer.ToString("000000");
			offsetAddr_label.Text = "0x" + offset;
			dialog_textBox.Text = text;
		}
	}
}
