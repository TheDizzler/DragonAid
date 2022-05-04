namespace AtomosZ.DragonAid.PointerAid
{
	partial class SearchMessageBox
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
			this.subroutines_listBox = new System.Windows.Forms.ListBox();
			this.search_label = new System.Windows.Forms.Label();
			this.searchBox_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ok_button = new System.Windows.Forms.Button();
			this.cancel_button = new System.Windows.Forms.Button();
			this.searchBox_flowLayoutPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// subroutines_listBox
			// 
			this.subroutines_listBox.FormattingEnabled = true;
			this.subroutines_listBox.ItemHeight = 20;
			this.subroutines_listBox.Location = new System.Drawing.Point(3, 25);
			this.subroutines_listBox.Name = "subroutines_listBox";
			this.subroutines_listBox.Size = new System.Drawing.Size(486, 124);
			this.subroutines_listBox.TabIndex = 0;
			// 
			// search_label
			// 
			this.search_label.AutoSize = true;
			this.search_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.search_label.Location = new System.Drawing.Point(3, 0);
			this.search_label.Name = "search_label";
			this.search_label.Size = new System.Drawing.Size(346, 22);
			this.search_label.TabIndex = 1;
			this.search_label.Text = "Dynamic Subroutines found with address: ";
			// 
			// searchBox_flowLayoutPanel
			// 
			this.searchBox_flowLayoutPanel.AutoSize = true;
			this.searchBox_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.searchBox_flowLayoutPanel.Controls.Add(this.search_label);
			this.searchBox_flowLayoutPanel.Controls.Add(this.subroutines_listBox);
			this.searchBox_flowLayoutPanel.Controls.Add(this.panel1);
			this.searchBox_flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.searchBox_flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.searchBox_flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.searchBox_flowLayoutPanel.Name = "searchBox_flowLayoutPanel";
			this.searchBox_flowLayoutPanel.Size = new System.Drawing.Size(496, 198);
			this.searchBox_flowLayoutPanel.TabIndex = 2;
			this.searchBox_flowLayoutPanel.WrapContents = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.ok_button);
			this.panel1.Controls.Add(this.cancel_button);
			this.panel1.Location = new System.Drawing.Point(3, 155);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(486, 40);
			this.panel1.TabIndex = 5;
			// 
			// ok_button
			// 
			this.ok_button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ok_button.Dock = System.Windows.Forms.DockStyle.Left;
			this.ok_button.Location = new System.Drawing.Point(0, 0);
			this.ok_button.Name = "ok_button";
			this.ok_button.Size = new System.Drawing.Size(145, 40);
			this.ok_button.TabIndex = 2;
			this.ok_button.Text = "OK";
			this.ok_button.UseVisualStyleBackColor = true;
			// 
			// cancel_button
			// 
			this.cancel_button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel_button.Dock = System.Windows.Forms.DockStyle.Right;
			this.cancel_button.Location = new System.Drawing.Point(341, 0);
			this.cancel_button.Name = "cancel_button";
			this.cancel_button.Size = new System.Drawing.Size(145, 40);
			this.cancel_button.TabIndex = 3;
			this.cancel_button.Text = "Cancel";
			this.cancel_button.UseVisualStyleBackColor = true;
			// 
			// SearchMessageBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.cancel_button;
			this.ClientSize = new System.Drawing.Size(496, 198);
			this.ControlBox = false;
			this.Controls.Add(this.searchBox_flowLayoutPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SearchMessageBox";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Multiple Dynamic Subroutines found";
			this.TopMost = true;
			this.searchBox_flowLayoutPanel.ResumeLayout(false);
			this.searchBox_flowLayoutPanel.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox subroutines_listBox;
		private System.Windows.Forms.Label search_label;
		private System.Windows.Forms.FlowLayoutPanel searchBox_flowLayoutPanel;
		private System.Windows.Forms.Button ok_button;
		private System.Windows.Forms.Button cancel_button;
		private System.Windows.Forms.Panel panel1;
	}
}