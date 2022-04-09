namespace AtomosZ.DragonAid.TextToHex
{
	partial class Text2HexForm
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
			this.warning_label = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.copy_button = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.input_richTextBox = new System.Windows.Forms.RichTextBox();
			this.hex_richTextBox = new System.Windows.Forms.RichTextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// warning_label
			// 
			this.warning_label.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.warning_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.warning_label.ForeColor = System.Drawing.Color.Brown;
			this.warning_label.Location = new System.Drawing.Point(0, 119);
			this.warning_label.Name = "warning_label";
			this.warning_label.Size = new System.Drawing.Size(661, 33);
			this.warning_label.TabIndex = 4;
			this.warning_label.Text = "Warnings";
			this.warning_label.UseMnemonic = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 50);
			this.label1.TabIndex = 2;
			this.label1.Text = "Text";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(3, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 50);
			this.label2.TabIndex = 3;
			this.label2.Text = "Hex";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// copy_button
			// 
			this.copy_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.copy_button.Location = new System.Drawing.Point(568, 44);
			this.copy_button.Name = "copy_button";
			this.copy_button.Size = new System.Drawing.Size(93, 72);
			this.copy_button.TabIndex = 5;
			this.copy_button.Text = "Copy to Clipboard";
			this.copy_button.UseVisualStyleBackColor = true;
			this.copy_button.Click += new System.EventHandler(this.Copy_button_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.input_richTextBox, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.hex_richTextBox, 1, 1);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(557, 100);
			this.tableLayoutPanel1.TabIndex = 6;
			// 
			// input_richTextBox
			// 
			this.input_richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.input_richTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.input_richTextBox.Location = new System.Drawing.Point(62, 3);
			this.input_richTextBox.Name = "input_richTextBox";
			this.input_richTextBox.ShowSelectionMargin = true;
			this.input_richTextBox.Size = new System.Drawing.Size(492, 44);
			this.input_richTextBox.TabIndex = 4;
			this.input_richTextBox.Text = "";
			this.input_richTextBox.TextChanged += new System.EventHandler(this.Input_textBox_TextChanged);
			// 
			// hex_richTextBox
			// 
			this.hex_richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hex_richTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.hex_richTextBox.Location = new System.Drawing.Point(62, 53);
			this.hex_richTextBox.Name = "hex_richTextBox";
			this.hex_richTextBox.ReadOnly = true;
			this.hex_richTextBox.ShortcutsEnabled = false;
			this.hex_richTextBox.ShowSelectionMargin = true;
			this.hex_richTextBox.Size = new System.Drawing.Size(492, 44);
			this.hex_richTextBox.TabIndex = 5;
			this.hex_richTextBox.Text = "";
			// 
			// Text2HexForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(661, 152);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.copy_button);
			this.Controls.Add(this.warning_label);
			this.MaximizeBox = false;
			this.Name = "Text2HexForm";
			this.ShowIcon = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Text-2-Hext Converter";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Label warning_label;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button copy_button;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.RichTextBox input_richTextBox;
		private System.Windows.Forms.RichTextBox hex_richTextBox;
	}
}

