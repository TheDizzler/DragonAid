namespace AtomosZ.MiNesEmulator
{
	partial class MemLabel
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
			this.mem_label = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// mem_label
			// 
			this.mem_label.AutoSize = true;
			this.mem_label.BackColor = System.Drawing.SystemColors.Window;
			this.mem_label.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mem_label.Location = new System.Drawing.Point(0, 0);
			this.mem_label.Margin = new System.Windows.Forms.Padding(0);
			this.mem_label.MinimumSize = new System.Drawing.Size(27, 19);
			this.mem_label.Name = "mem_label";
			this.mem_label.Size = new System.Drawing.Size(27, 19);
			this.mem_label.TabIndex = 1;
			this.mem_label.Text = "00";
			// 
			// MemLabel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.mem_label);
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.Name = "MemLabel";
			this.Size = new System.Drawing.Size(27, 19);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label mem_label;
	}
}
