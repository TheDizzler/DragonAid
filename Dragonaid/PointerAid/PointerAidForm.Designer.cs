namespace AtomosZ.DragonAid.PointerAid
{
	partial class PointerAidForm
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
			this.pointer07_listBox = new System.Windows.Forms.ListBox();
			this.pointerAndAddress_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.subroutine_tabControl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.pointer17_listBox = new System.Windows.Forms.ListBox();
			this.addressView = new AtomosZ.DragonAid.Libraries.AddressView();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.save_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.load_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.restoreFromROM_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.info_panel = new System.Windows.Forms.Panel();
			this.saveStatus_label = new System.Windows.Forms.Label();
			this.pointerAndAddress_flowLayoutPanel.SuspendLayout();
			this.subroutine_tabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.info_panel.SuspendLayout();
			this.SuspendLayout();
			// 
			// pointer07_listBox
			// 
			this.pointer07_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pointer07_listBox.FormattingEnabled = true;
			this.pointer07_listBox.ItemHeight = 20;
			this.pointer07_listBox.Location = new System.Drawing.Point(3, 3);
			this.pointer07_listBox.Name = "pointer07_listBox";
			this.pointer07_listBox.Size = new System.Drawing.Size(387, 337);
			this.pointer07_listBox.TabIndex = 2;
			this.pointer07_listBox.SelectedIndexChanged += new System.EventHandler(this.Pointer07_listBox_SelectedIndexChanged);
			// 
			// pointerAndAddress_flowLayoutPanel
			// 
			this.pointerAndAddress_flowLayoutPanel.AutoSize = true;
			this.pointerAndAddress_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pointerAndAddress_flowLayoutPanel.Controls.Add(this.subroutine_tabControl);
			this.pointerAndAddress_flowLayoutPanel.Controls.Add(this.addressView);
			this.pointerAndAddress_flowLayoutPanel.Location = new System.Drawing.Point(0, 36);
			this.pointerAndAddress_flowLayoutPanel.Name = "pointerAndAddress_flowLayoutPanel";
			this.pointerAndAddress_flowLayoutPanel.Size = new System.Drawing.Size(903, 382);
			this.pointerAndAddress_flowLayoutPanel.TabIndex = 3;
			this.pointerAndAddress_flowLayoutPanel.WrapContents = false;
			// 
			// subroutine_tabControl
			// 
			this.subroutine_tabControl.Controls.Add(this.tabPage1);
			this.subroutine_tabControl.Controls.Add(this.tabPage2);
			this.subroutine_tabControl.HotTrack = true;
			this.subroutine_tabControl.Location = new System.Drawing.Point(3, 3);
			this.subroutine_tabControl.Name = "subroutine_tabControl";
			this.subroutine_tabControl.SelectedIndex = 0;
			this.subroutine_tabControl.Size = new System.Drawing.Size(401, 376);
			this.subroutine_tabControl.TabIndex = 3;
			this.subroutine_tabControl.SelectedIndexChanged += new System.EventHandler(this.Subroutine_tabControl_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.pointer07_listBox);
			this.tabPage1.Location = new System.Drawing.Point(4, 29);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(393, 343);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Dynamic Subs 07";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.pointer17_listBox);
			this.tabPage2.Location = new System.Drawing.Point(4, 29);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(393, 343);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Dynamic Subs 17";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// pointer17_listBox
			// 
			this.pointer17_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pointer17_listBox.FormattingEnabled = true;
			this.pointer17_listBox.ItemHeight = 20;
			this.pointer17_listBox.Location = new System.Drawing.Point(3, 3);
			this.pointer17_listBox.Name = "pointer17_listBox";
			this.pointer17_listBox.Size = new System.Drawing.Size(387, 337);
			this.pointer17_listBox.TabIndex = 3;
			this.pointer17_listBox.SelectedIndexChanged += new System.EventHandler(this.Pointer17_listBox_SelectedIndexChanged);
			// 
			// addressView
			// 
			this.addressView.AutoSize = true;
			this.addressView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.addressView.Location = new System.Drawing.Point(410, 3);
			this.addressView.Name = "addressView";
			this.addressView.Size = new System.Drawing.Size(490, 376);
			this.addressView.TabIndex = 1;
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(914, 33);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.save_ToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.load_ToolStripMenuItem,
            this.restoreFromROM_ToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// save_ToolStripMenuItem
			// 
			this.save_ToolStripMenuItem.Name = "save_ToolStripMenuItem";
			this.save_ToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
			this.save_ToolStripMenuItem.Text = "Save";
			this.save_ToolStripMenuItem.Click += new System.EventHandler(this.Save);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
			this.saveAsToolStripMenuItem.Text = "Save As";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAs_ToolStripMenuItem_Click);
			// 
			// load_ToolStripMenuItem
			// 
			this.load_ToolStripMenuItem.Name = "load_ToolStripMenuItem";
			this.load_ToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
			this.load_ToolStripMenuItem.Text = "Load";
			this.load_ToolStripMenuItem.Click += new System.EventHandler(this.Load_ToolStripMenuItem_Click);
			// 
			// restoreFromROM_ToolStripMenuItem
			// 
			this.restoreFromROM_ToolStripMenuItem.Name = "restoreFromROM_ToolStripMenuItem";
			this.restoreFromROM_ToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
			this.restoreFromROM_ToolStripMenuItem.Text = "Restore From ROM";
			this.restoreFromROM_ToolStripMenuItem.Click += new System.EventHandler(this.RestoreFromROM_ToolStripMenuItem_Click);
			// 
			// info_panel
			// 
			this.info_panel.Controls.Add(this.saveStatus_label);
			this.info_panel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.info_panel.Location = new System.Drawing.Point(0, 429);
			this.info_panel.Name = "info_panel";
			this.info_panel.Size = new System.Drawing.Size(914, 27);
			this.info_panel.TabIndex = 6;
			// 
			// saveStatus_label
			// 
			this.saveStatus_label.AutoSize = true;
			this.saveStatus_label.Dock = System.Windows.Forms.DockStyle.Right;
			this.saveStatus_label.Location = new System.Drawing.Point(818, 0);
			this.saveStatus_label.Name = "saveStatus_label";
			this.saveStatus_label.Size = new System.Drawing.Size(96, 20);
			this.saveStatus_label.TabIndex = 0;
			this.saveStatus_label.Text = "Save Status";
			this.saveStatus_label.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.saveStatus_label.Visible = false;
			// 
			// PointerAidForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(914, 456);
			this.Controls.Add(this.info_panel);
			this.Controls.Add(this.pointerAndAddress_flowLayoutPanel);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "PointerAidForm";
			this.Text = "PointerAid - DragonAid";
			this.Click += new System.EventHandler(this.Defocus);
			this.pointerAndAddress_flowLayoutPanel.ResumeLayout(false);
			this.pointerAndAddress_flowLayoutPanel.PerformLayout();
			this.subroutine_tabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.info_panel.ResumeLayout(false);
			this.info_panel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private AtomosZ.DragonAid.Libraries.AddressView addressView;
		private System.Windows.Forms.ListBox pointer07_listBox;
		private System.Windows.Forms.FlowLayoutPanel pointerAndAddress_flowLayoutPanel;
		private System.Windows.Forms.TabControl subroutine_tabControl;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.ListBox pointer17_listBox;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem save_ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem load_ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem restoreFromROM_ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.Panel info_panel;
		private System.Windows.Forms.Label saveStatus_label;
	}
}

