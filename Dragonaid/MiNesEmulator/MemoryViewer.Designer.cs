namespace AtomosZ.MiNesEmulator
{
	partial class MemoryViewer
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
			this.rowHeader_textBox = new System.Windows.Forms.TextBox();
			this.colHeader_textBox = new System.Windows.Forms.TextBox();
			this.scrollTextBox = new AtomosZ.MiNesEmulator.ScrollTextBox();
			this.SuspendLayout();
			// 
			// rowHeader_textBox
			// 
			this.rowHeader_textBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rowHeader_textBox.Location = new System.Drawing.Point(0, 31);
			this.rowHeader_textBox.Multiline = true;
			this.rowHeader_textBox.Name = "rowHeader_textBox";
			this.rowHeader_textBox.ReadOnly = true;
			this.rowHeader_textBox.Size = new System.Drawing.Size(61, 389);
			this.rowHeader_textBox.TabIndex = 29;
			this.rowHeader_textBox.Text = "$0000\r\n$0010";
			// 
			// colHeader_textBox
			// 
			this.colHeader_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.colHeader_textBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.colHeader_textBox.Location = new System.Drawing.Point(71, 3);
			this.colHeader_textBox.Multiline = true;
			this.colHeader_textBox.Name = "colHeader_textBox";
			this.colHeader_textBox.ReadOnly = true;
			this.colHeader_textBox.ShortcutsEnabled = false;
			this.colHeader_textBox.Size = new System.Drawing.Size(449, 26);
			this.colHeader_textBox.TabIndex = 30;
			this.colHeader_textBox.TabStop = false;
			this.colHeader_textBox.Text = "00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F";
			// 
			// scrollTextBox
			// 
			this.scrollTextBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.scrollTextBox.Location = new System.Drawing.Point(68, 31);
			this.scrollTextBox.MaxLength = 196608;
			this.scrollTextBox.Multiline = true;
			this.scrollTextBox.Name = "scrollTextBox";
			this.scrollTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.scrollTextBox.ShortcutsEnabled = false;
			this.scrollTextBox.Size = new System.Drawing.Size(474, 389);
			this.scrollTextBox.TabIndex = 1;
			this.scrollTextBox.TabStop = false;
			this.scrollTextBox.Text = "00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F";
			// 
			// MemoryViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.scrollTextBox);
			this.Controls.Add(this.colHeader_textBox);
			this.Controls.Add(this.rowHeader_textBox);
			this.Name = "MemoryViewer";
			this.Size = new System.Drawing.Size(542, 423);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox rowHeader_textBox;
		private System.Windows.Forms.TextBox colHeader_textBox;
		private ScrollTextBox scrollTextBox;
	}
}
