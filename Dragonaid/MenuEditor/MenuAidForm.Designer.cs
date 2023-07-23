namespace AtomosZ.DragonAid.MenuAid
{
	partial class MenuAidForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dialog_pictureBox = new System.Windows.Forms.PictureBox();
			this.renderLine_button = new System.Windows.Forms.Button();
			this.renderAddr_label = new System.Windows.Forms.Label();
			this.render_button = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dialog_pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// dialog_pictureBox
			// 
			this.dialog_pictureBox.Location = new System.Drawing.Point(12, 48);
			this.dialog_pictureBox.Name = "dialog_pictureBox";
			this.dialog_pictureBox.Size = new System.Drawing.Size(79, 57);
			this.dialog_pictureBox.TabIndex = 1;
			this.dialog_pictureBox.TabStop = false;
			// 
			// renderLine_button
			// 
			this.renderLine_button.AutoSize = true;
			this.renderLine_button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.renderLine_button.Location = new System.Drawing.Point(12, 12);
			this.renderLine_button.Name = "renderLine_button";
			this.renderLine_button.Size = new System.Drawing.Size(106, 30);
			this.renderLine_button.TabIndex = 2;
			this.renderLine_button.Text = "Render Line";
			this.renderLine_button.UseVisualStyleBackColor = true;
			this.renderLine_button.Click += new System.EventHandler(this.renderLine_button_Click);
			// 
			// renderAddr_label
			// 
			this.renderAddr_label.AutoSize = true;
			this.renderAddr_label.Location = new System.Drawing.Point(135, 17);
			this.renderAddr_label.Name = "renderAddr_label";
			this.renderAddr_label.Size = new System.Drawing.Size(61, 20);
			this.renderAddr_label.TabIndex = 3;
			this.renderAddr_label.Text = "0x2000";
			// 
			// render_button
			// 
			this.render_button.AutoSize = true;
			this.render_button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.render_button.Location = new System.Drawing.Point(217, 12);
			this.render_button.Name = "render_button";
			this.render_button.Size = new System.Drawing.Size(93, 30);
			this.render_button.TabIndex = 4;
			this.render_button.Text = "Render All";
			this.render_button.UseVisualStyleBackColor = true;
			this.render_button.Click += new System.EventHandler(this.render_button_Click);
			// 
			// MenuAidForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.render_button);
			this.Controls.Add(this.renderAddr_label);
			this.Controls.Add(this.renderLine_button);
			this.Controls.Add(this.dialog_pictureBox);
			this.Name = "MenuAidForm";
			this.Text = "DragonAid - Menu Editor";
			((System.ComponentModel.ISupportInitialize)(this.dialog_pictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox dialog_pictureBox;
		private System.Windows.Forms.Button renderLine_button;
		private System.Windows.Forms.Label renderAddr_label;
		private System.Windows.Forms.Button render_button;
	}
}

