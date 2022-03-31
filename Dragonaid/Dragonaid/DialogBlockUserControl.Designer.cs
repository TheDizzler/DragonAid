namespace Dragonaid
{
	partial class DialogBlockUserControl
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
			this.label1 = new System.Windows.Forms.Label();
			this.address_groupBox = new System.Windows.Forms.GroupBox();
			this.address_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.pointer_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.pointerAddr_label = new System.Windows.Forms.Label();
			this.offset_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.offsetAddr_label = new System.Windows.Forms.Label();
			this.dialog_textBox = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.address_groupBox.SuspendLayout();
			this.address_flowLayoutPanel.SuspendLayout();
			this.pointer_flowLayoutPanel.SuspendLayout();
			this.offset_flowLayoutPanel.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Pointer:";
			// 
			// address_groupBox
			// 
			this.address_groupBox.AutoSize = true;
			this.address_groupBox.Controls.Add(this.address_flowLayoutPanel);
			this.address_groupBox.Location = new System.Drawing.Point(3, 3);
			this.address_groupBox.Name = "address_groupBox";
			this.address_groupBox.Size = new System.Drawing.Size(175, 81);
			this.address_groupBox.TabIndex = 1;
			this.address_groupBox.TabStop = false;
			this.address_groupBox.Text = "Address";
			// 
			// address_flowLayoutPanel
			// 
			this.address_flowLayoutPanel.Controls.Add(this.pointer_flowLayoutPanel);
			this.address_flowLayoutPanel.Controls.Add(this.offset_flowLayoutPanel);
			this.address_flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.address_flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.address_flowLayoutPanel.Location = new System.Drawing.Point(3, 22);
			this.address_flowLayoutPanel.Name = "address_flowLayoutPanel";
			this.address_flowLayoutPanel.Size = new System.Drawing.Size(169, 56);
			this.address_flowLayoutPanel.TabIndex = 0;
			// 
			// pointer_flowLayoutPanel
			// 
			this.pointer_flowLayoutPanel.AutoSize = true;
			this.pointer_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pointer_flowLayoutPanel.Controls.Add(this.label1);
			this.pointer_flowLayoutPanel.Controls.Add(this.pointerAddr_label);
			this.pointer_flowLayoutPanel.Location = new System.Drawing.Point(3, 3);
			this.pointer_flowLayoutPanel.Name = "pointer_flowLayoutPanel";
			this.pointer_flowLayoutPanel.Size = new System.Drawing.Size(147, 20);
			this.pointer_flowLayoutPanel.TabIndex = 3;
			// 
			// pointerAddr_label
			// 
			this.pointerAddr_label.AutoSize = true;
			this.pointerAddr_label.Location = new System.Drawing.Point(72, 0);
			this.pointerAddr_label.Name = "pointerAddr_label";
			this.pointerAddr_label.Size = new System.Drawing.Size(72, 20);
			this.pointerAddr_label.TabIndex = 1;
			this.pointerAddr_label.Text = "$000000";
			// 
			// offset_flowLayoutPanel
			// 
			this.offset_flowLayoutPanel.AutoSize = true;
			this.offset_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.offset_flowLayoutPanel.Controls.Add(this.label2);
			this.offset_flowLayoutPanel.Controls.Add(this.offsetAddr_label);
			this.offset_flowLayoutPanel.Location = new System.Drawing.Point(3, 29);
			this.offset_flowLayoutPanel.Name = "offset_flowLayoutPanel";
			this.offset_flowLayoutPanel.Size = new System.Drawing.Size(148, 20);
			this.offset_flowLayoutPanel.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "Offset:";
			// 
			// offsetAddr_label
			// 
			this.offsetAddr_label.AutoSize = true;
			this.offsetAddr_label.Location = new System.Drawing.Point(66, 0);
			this.offsetAddr_label.Name = "offsetAddr_label";
			this.offsetAddr_label.Size = new System.Drawing.Size(79, 20);
			this.offsetAddr_label.TabIndex = 3;
			this.offsetAddr_label.Text = "0x000000";
			// 
			// dialog_textBox
			// 
			this.dialog_textBox.Location = new System.Drawing.Point(181, 17);
			this.dialog_textBox.Multiline = true;
			this.dialog_textBox.Name = "dialog_textBox";
			this.dialog_textBox.Size = new System.Drawing.Size(528, 67);
			this.dialog_textBox.TabIndex = 2;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.flowLayoutPanel1);
			this.groupBox1.Location = new System.Drawing.Point(715, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(211, 84);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Character Length";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
			this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel3);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 22);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(205, 59);
			this.flowLayoutPanel1.TabIndex = 1;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.AutoSize = true;
			this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel2.Controls.Add(this.label3);
			this.flowLayoutPanel2.Controls.Add(this.label4);
			this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(114, 20);
			this.flowLayoutPanel2.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(66, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "Current:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(75, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(36, 20);
			this.label4.TabIndex = 1;
			this.label4.Text = "000";
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.AutoSize = true;
			this.flowLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel3.Controls.Add(this.label5);
			this.flowLayoutPanel3.Controls.Add(this.label6);
			this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 29);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(114, 20);
			this.flowLayoutPanel3.TabIndex = 3;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(66, 20);
			this.label5.TabIndex = 2;
			this.label5.Text = "Max:      ";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(75, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(36, 20);
			this.label6.TabIndex = 3;
			this.label6.Text = "000";
			// 
			// DialogBlockUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.dialog_textBox);
			this.Controls.Add(this.address_groupBox);
			this.Name = "DialogBlockUserControl";
			this.Size = new System.Drawing.Size(929, 87);
			this.address_groupBox.ResumeLayout(false);
			this.address_flowLayoutPanel.ResumeLayout(false);
			this.address_flowLayoutPanel.PerformLayout();
			this.pointer_flowLayoutPanel.ResumeLayout(false);
			this.pointer_flowLayoutPanel.PerformLayout();
			this.offset_flowLayoutPanel.ResumeLayout(false);
			this.offset_flowLayoutPanel.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox address_groupBox;
		private System.Windows.Forms.FlowLayoutPanel address_flowLayoutPanel;
		private System.Windows.Forms.FlowLayoutPanel pointer_flowLayoutPanel;
		private System.Windows.Forms.Label pointerAddr_label;
		private System.Windows.Forms.FlowLayoutPanel offset_flowLayoutPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label offsetAddr_label;
		private System.Windows.Forms.TextBox dialog_textBox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
	}
}
