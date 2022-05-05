namespace AtomosZ.DragonAid.Libraries
{
	partial class CaptionedPictureBox
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaptionedPictureBox));
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.caption_label = new AtomosZ.DragonAid.Libraries.TransparentLabel();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox.InitialImage")));
			this.pictureBox.Location = new System.Drawing.Point(0, 0);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(150, 150);
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			// 
			// caption_label
			// 
			this.caption_label.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.caption_label.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.caption_label.Location = new System.Drawing.Point(0, 125);
			this.caption_label.Name = "caption_label";
			this.caption_label.Size = new System.Drawing.Size(150, 25);
			this.caption_label.TabIndex = 1;
			this.caption_label.Text = "Caption";
			this.caption_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// CaptionedPictureBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.caption_label);
			this.Controls.Add(this.pictureBox);
			this.Name = "CaptionedPictureBox";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		public TransparentLabel caption_label;
		public System.Windows.Forms.PictureBox pictureBox;
	}
}
