namespace AtomosZ.DragonAid.PointerAid
{
	partial class MultipleChoiceMessageBox
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
			this.checkBox_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ds07_checkBox = new System.Windows.Forms.CheckBox();
			this.ds17_checkBox = new System.Windows.Forms.CheckBox();
			this.lp_checkBox = new System.Windows.Forms.CheckBox();
			this.ok_button = new System.Windows.Forms.Button();
			this.cancel_button = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.checkBox_flowLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkBox_flowLayoutPanel
			// 
			this.checkBox_flowLayoutPanel.AutoSize = true;
			this.checkBox_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.checkBox_flowLayoutPanel.Controls.Add(this.ds07_checkBox);
			this.checkBox_flowLayoutPanel.Controls.Add(this.ds17_checkBox);
			this.checkBox_flowLayoutPanel.Controls.Add(this.lp_checkBox);
			this.checkBox_flowLayoutPanel.Location = new System.Drawing.Point(10, 12);
			this.checkBox_flowLayoutPanel.Name = "checkBox_flowLayoutPanel";
			this.checkBox_flowLayoutPanel.Size = new System.Drawing.Size(569, 30);
			this.checkBox_flowLayoutPanel.TabIndex = 0;
			this.checkBox_flowLayoutPanel.WrapContents = false;
			// 
			// ds07_checkBox
			// 
			this.ds07_checkBox.AutoSize = true;
			this.ds07_checkBox.Location = new System.Drawing.Point(3, 3);
			this.ds07_checkBox.Name = "ds07_checkBox";
			this.ds07_checkBox.Size = new System.Drawing.Size(208, 24);
			this.ds07_checkBox.TabIndex = 0;
			this.ds07_checkBox.Text = "Dynamic Subroutines 07";
			this.ds07_checkBox.UseVisualStyleBackColor = true;
			// 
			// ds17_checkBox
			// 
			this.ds17_checkBox.AutoSize = true;
			this.ds17_checkBox.Location = new System.Drawing.Point(217, 3);
			this.ds17_checkBox.Name = "ds17_checkBox";
			this.ds17_checkBox.Size = new System.Drawing.Size(208, 24);
			this.ds17_checkBox.TabIndex = 1;
			this.ds17_checkBox.Text = "Dynamic Subroutines 17";
			this.ds17_checkBox.UseVisualStyleBackColor = true;
			// 
			// lp_checkBox
			// 
			this.lp_checkBox.AutoSize = true;
			this.lp_checkBox.Location = new System.Drawing.Point(431, 3);
			this.lp_checkBox.Name = "lp_checkBox";
			this.lp_checkBox.Size = new System.Drawing.Size(135, 24);
			this.lp_checkBox.TabIndex = 2;
			this.lp_checkBox.Text = "Local Pointers";
			this.lp_checkBox.UseVisualStyleBackColor = true;
			// 
			// ok_button
			// 
			this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ok_button.Location = new System.Drawing.Point(10, 88);
			this.ok_button.Name = "ok_button";
			this.ok_button.Size = new System.Drawing.Size(115, 38);
			this.ok_button.TabIndex = 1;
			this.ok_button.Text = "OK";
			this.ok_button.UseVisualStyleBackColor = true;
			// 
			// cancel_button
			// 
			this.cancel_button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel_button.Location = new System.Drawing.Point(464, 88);
			this.cancel_button.Name = "cancel_button";
			this.cancel_button.Size = new System.Drawing.Size(115, 38);
			this.cancel_button.TabIndex = 2;
			this.cancel_button.Text = "Cancel";
			this.cancel_button.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(81, 54);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(388, 20);
			this.label1.TabIndex = 3;
			this.label1.Text = "WARNING: Selecting a box will overwrite current data!";
			// 
			// MultipleChoiceMessageBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.cancel_button;
			this.ClientSize = new System.Drawing.Size(591, 144);
			this.ControlBox = false;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cancel_button);
			this.Controls.Add(this.ok_button);
			this.Controls.Add(this.checkBox_flowLayoutPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MultipleChoiceMessageBox";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Which Pointers to Reload?";
			this.TopMost = true;
			this.checkBox_flowLayoutPanel.ResumeLayout(false);
			this.checkBox_flowLayoutPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel checkBox_flowLayoutPanel;
		private System.Windows.Forms.Button ok_button;
		private System.Windows.Forms.Button cancel_button;
		public System.Windows.Forms.CheckBox ds07_checkBox;
		public System.Windows.Forms.CheckBox ds17_checkBox;
		public System.Windows.Forms.CheckBox lp_checkBox;
		private System.Windows.Forms.Label label1;
	}
}