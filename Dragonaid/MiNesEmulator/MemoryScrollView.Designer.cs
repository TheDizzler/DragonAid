namespace AtomosZ.MiNesEmulator
{
	partial class MemoryScrollView
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
			this.rowHeader_richTextBox = new System.Windows.Forms.RichTextBox();
			this.exRichTextBox = new AtomosZ.MiNesEmulator.ExRichTextBox();
			this.SuspendLayout();
			// 
			// rowHeader_richTextBox
			// 
			this.rowHeader_richTextBox.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.rowHeader_richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rowHeader_richTextBox.CausesValidation = false;
			this.rowHeader_richTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.rowHeader_richTextBox.DetectUrls = false;
			this.rowHeader_richTextBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rowHeader_richTextBox.HideSelection = false;
			this.rowHeader_richTextBox.Location = new System.Drawing.Point(0, 0);
			this.rowHeader_richTextBox.Margin = new System.Windows.Forms.Padding(0);
			this.rowHeader_richTextBox.Name = "rowHeader_richTextBox";
			this.rowHeader_richTextBox.ReadOnly = true;
			this.rowHeader_richTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.rowHeader_richTextBox.Size = new System.Drawing.Size(38, 417);
			this.rowHeader_richTextBox.TabIndex = 0;
			this.rowHeader_richTextBox.TabStop = false;
			this.rowHeader_richTextBox.Text = "0000\n0010\n";
			// 
			// exRichTextBox
			// 
			this.exRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.exRichTextBox.Cursor = System.Windows.Forms.Cursors.Default;
			this.exRichTextBox.DetectUrls = false;
			this.exRichTextBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.exRichTextBox.HideSelection = false;
			this.exRichTextBox.Location = new System.Drawing.Point(41, 0);
			this.exRichTextBox.Margin = new System.Windows.Forms.Padding(0);
			this.exRichTextBox.MaxLength = 32768;
			this.exRichTextBox.Name = "exRichTextBox";
			this.exRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.exRichTextBox.ShortcutsEnabled = false;
			this.exRichTextBox.Size = new System.Drawing.Size(436, 417);
			this.exRichTextBox.TabIndex = 1;
			this.exRichTextBox.TabStop = false;
			this.exRichTextBox.Text = "00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F";
			this.exRichTextBox.WordWrap = false;
			// 
			// MemoryScrollView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.exRichTextBox);
			this.Controls.Add(this.rowHeader_richTextBox);
			this.Name = "MemoryScrollView";
			this.Size = new System.Drawing.Size(477, 417);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox rowHeader_richTextBox;
		private ExRichTextBox exRichTextBox;
	}
}
