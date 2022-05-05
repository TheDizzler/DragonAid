using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AtomosZ.DragonAid.Libraries
{
	public partial class CaptionedPictureBox : UserControl
	{
		public CaptionedPictureBox()
		{
			InitializeComponent();
		}


		public override string Text
		{
			get { return caption_label.Text; }
			set { caption_label.Text = value; }
		}

		public Image Image
		{
			get { return pictureBox.Image; }
			set { pictureBox.Image = value; }
		}

		public new Size Size
		{
			get { return pictureBox.Size; }
			set { pictureBox.Size = value; }
		}
	}
}