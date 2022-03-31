namespace Dragonaid
{
	partial class DialogViewUserControl
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
			this.dialogBlockUserControl1 = new Dragonaid.DialogBlockUserControl();
			this.SuspendLayout();
			// 
			// dialogBlockUserControl1
			// 
			this.dialogBlockUserControl1.AutoSize = true;
			this.dialogBlockUserControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.dialogBlockUserControl1.Location = new System.Drawing.Point(3, 3);
			this.dialogBlockUserControl1.Name = "dialogBlockUserControl1";
			this.dialogBlockUserControl1.Size = new System.Drawing.Size(929, 87);
			this.dialogBlockUserControl1.TabIndex = 0;
			// 
			// DialogViewUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dialogBlockUserControl1);
			this.Name = "DialogViewUserControl";
			this.Size = new System.Drawing.Size(1024, 150);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DialogBlockUserControl dialogBlockUserControl1;
	}
}
