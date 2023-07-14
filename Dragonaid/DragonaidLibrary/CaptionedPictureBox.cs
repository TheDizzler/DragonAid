using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace AtomosZ.DragonAid.Libraries
{
	public partial class CaptionedPictureBox : UserControl
	{
		public new MouseEventHandler OnMouseClick;

		public CaptionedPictureBox()
		{
			InitializeComponent();

			pictureBox.MouseClick += OnMouseClicked;
			caption_label.MouseClick += OnMouseClicked;
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


		private void OnMouseClicked(object sender, MouseEventArgs e)
		{
			var a = sender as PictureBox;
			if (a != null)
				Console.WriteLine(a.GetHashCode());

			if (OnMouseClick != null)
				OnMouseClick((CaptionedPictureBox)this, e);
		}
	}
}