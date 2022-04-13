namespace AtomosZ.DragonAid.Libraries
{
	partial class AddressView
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
			this.address_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.address_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.name_textBox = new System.Windows.Forms.TextBox();
			this.length_label = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.length_spinner = new System.Windows.Forms.NumericUpDown();
			this.offset_textBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.address_textBox = new System.Windows.Forms.TextBox();
			this.notes_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.label4 = new System.Windows.Forms.Label();
			this.notes_richTextBox = new System.Windows.Forms.RichTextBox();
			this.address_flowLayoutPanel.SuspendLayout();
			this.address_tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.length_spinner)).BeginInit();
			this.notes_flowLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// address_flowLayoutPanel
			// 
			this.address_flowLayoutPanel.AutoSize = true;
			this.address_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.address_flowLayoutPanel.Controls.Add(this.address_tableLayoutPanel);
			this.address_flowLayoutPanel.Controls.Add(this.notes_flowLayoutPanel);
			this.address_flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.address_flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.address_flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.address_flowLayoutPanel.Name = "address_flowLayoutPanel";
			this.address_flowLayoutPanel.Size = new System.Drawing.Size(490, 376);
			this.address_flowLayoutPanel.TabIndex = 0;
			this.address_flowLayoutPanel.WrapContents = false;
			// 
			// address_tableLayoutPanel
			// 
			this.address_tableLayoutPanel.AutoSize = true;
			this.address_tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.address_tableLayoutPanel.ColumnCount = 2;
			this.address_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.address_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.address_tableLayoutPanel.Controls.Add(this.name_textBox, 1, 0);
			this.address_tableLayoutPanel.Controls.Add(this.length_label, 0, 3);
			this.address_tableLayoutPanel.Controls.Add(this.label2, 0, 2);
			this.address_tableLayoutPanel.Controls.Add(this.label1, 0, 1);
			this.address_tableLayoutPanel.Controls.Add(this.length_spinner, 1, 3);
			this.address_tableLayoutPanel.Controls.Add(this.offset_textBox, 1, 2);
			this.address_tableLayoutPanel.Controls.Add(this.label5, 0, 0);
			this.address_tableLayoutPanel.Controls.Add(this.address_textBox, 1, 1);
			this.address_tableLayoutPanel.Location = new System.Drawing.Point(3, 3);
			this.address_tableLayoutPanel.Name = "address_tableLayoutPanel";
			this.address_tableLayoutPanel.RowCount = 4;
			this.address_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.address_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.address_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.address_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.address_tableLayoutPanel.Size = new System.Drawing.Size(484, 128);
			this.address_tableLayoutPanel.TabIndex = 4;
			this.address_tableLayoutPanel.Click += new System.EventHandler(this.Defocus_Click);
			// 
			// name_textBox
			// 
			this.name_textBox.Location = new System.Drawing.Point(115, 3);
			this.name_textBox.Name = "name_textBox";
			this.name_textBox.Size = new System.Drawing.Size(366, 26);
			this.name_textBox.TabIndex = 1;
			this.name_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
			// 
			// length_label
			// 
			this.length_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.length_label.AutoSize = true;
			this.length_label.Location = new System.Drawing.Point(3, 102);
			this.length_label.Name = "length_label";
			this.length_label.Size = new System.Drawing.Size(106, 20);
			this.length_label.TabIndex = 2;
			this.length_label.Text = "Length";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 70);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(106, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "iNES address";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 38);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(106, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Address";
			// 
			// length_spinner
			// 
			this.length_spinner.Location = new System.Drawing.Point(115, 99);
			this.length_spinner.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.length_spinner.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.length_spinner.Name = "length_spinner";
			this.length_spinner.Size = new System.Drawing.Size(100, 26);
			this.length_spinner.TabIndex = 4;
			this.length_spinner.Value = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.length_spinner.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
			// 
			// offset_textBox
			// 
			this.offset_textBox.Enabled = false;
			this.offset_textBox.Location = new System.Drawing.Point(115, 67);
			this.offset_textBox.MaxLength = 7;
			this.offset_textBox.Name = "offset_textBox";
			this.offset_textBox.Size = new System.Drawing.Size(100, 26);
			this.offset_textBox.TabIndex = 3;
			this.offset_textBox.Text = "0x00000";
			this.offset_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 6);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(106, 20);
			this.label5.TabIndex = 4;
			this.label5.Text = "Name";
			// 
			// address_textBox
			// 
			this.address_textBox.Enabled = false;
			this.address_textBox.Location = new System.Drawing.Point(115, 35);
			this.address_textBox.MaxLength = 7;
			this.address_textBox.Name = "address_textBox";
			this.address_textBox.Size = new System.Drawing.Size(100, 26);
			this.address_textBox.TabIndex = 2;
			this.address_textBox.Text = "0x00000";
			this.address_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
			// 
			// notes_flowLayoutPanel
			// 
			this.notes_flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.notes_flowLayoutPanel.AutoSize = true;
			this.notes_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.notes_flowLayoutPanel.Controls.Add(this.label4);
			this.notes_flowLayoutPanel.Controls.Add(this.notes_richTextBox);
			this.notes_flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.notes_flowLayoutPanel.Location = new System.Drawing.Point(3, 137);
			this.notes_flowLayoutPanel.Name = "notes_flowLayoutPanel";
			this.notes_flowLayoutPanel.Size = new System.Drawing.Size(484, 236);
			this.notes_flowLayoutPanel.TabIndex = 3;
			this.notes_flowLayoutPanel.WrapContents = false;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(55, 20);
			this.label4.TabIndex = 0;
			this.label4.Text = "Notes:";
			// 
			// notes_richTextBox
			// 
			this.notes_richTextBox.Location = new System.Drawing.Point(3, 23);
			this.notes_richTextBox.Name = "notes_richTextBox";
			this.notes_richTextBox.Size = new System.Drawing.Size(475, 210);
			this.notes_richTextBox.TabIndex = 5;
			this.notes_richTextBox.Text = "";
			// 
			// AddressView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.address_flowLayoutPanel);
			this.Name = "AddressView";
			this.Size = new System.Drawing.Size(490, 376);
			this.Click += new System.EventHandler(this.Defocus_Click);
			this.address_flowLayoutPanel.ResumeLayout(false);
			this.address_flowLayoutPanel.PerformLayout();
			this.address_tableLayoutPanel.ResumeLayout(false);
			this.address_tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.length_spinner)).EndInit();
			this.notes_flowLayoutPanel.ResumeLayout(false);
			this.notes_flowLayoutPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox name_textBox;
		private System.Windows.Forms.FlowLayoutPanel address_flowLayoutPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox address_textBox;
		private System.Windows.Forms.TableLayoutPanel address_tableLayoutPanel;
		private System.Windows.Forms.Label length_label;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown length_spinner;
		private System.Windows.Forms.TextBox offset_textBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.FlowLayoutPanel notes_flowLayoutPanel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RichTextBox notes_richTextBox;
	}
}
