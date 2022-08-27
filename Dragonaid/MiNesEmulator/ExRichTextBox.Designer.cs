namespace AtomosZ.MiNesEmulator
{
	partial class ExRichTextBox
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
			this.SuspendLayout();
			// 
			// ExRichTextBox
			// 
			this.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.DetectUrls = false;
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HideSelection = false;
			this.Margin = new System.Windows.Forms.Padding(0);
			this.MaxLength = 32768;
			this.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.ShortcutsEnabled = false;
			this.TabStop = false;
			this.WordWrap = false;
			this.Click += new System.EventHandler(this.ScrollTextBox_Click);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
