namespace AtomosZ.DragonAid.Libraries
{
	partial class NumberBox
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
			this.textBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox.Location = new System.Drawing.Point(3, 3);
			this.textBox.Margin = new System.Windows.Forms.Padding(0);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(117, 26);
			this.textBox.TabIndex = 0;
			this.textBox.SizeChanged += new System.EventHandler(this.TextBox_SizeChanged);
			this.textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumberOnly_TextBox_KeyDown);
			this.textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumberOnly_TextBox_KeyPress);
			// 
			// NumberBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.textBox);
			this.Name = "NumberBox";
			this.Size = new System.Drawing.Size(123, 32);
			this.ClientSizeChanged += new System.EventHandler(this.TextBox_SizeChanged);
			this.SizeChanged += new System.EventHandler(this.TextBox_SizeChanged);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox;
	}
}
