namespace AtomosZ.DragonAid.MonsterAid
{
	partial class MonsterAidMainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonsterAidMainForm));
			this.nextMonster_button = new System.Windows.Forms.Button();
			this.prevMonster_button = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadROMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.insertIntoROMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.info_transparentPanel = new AtomosZ.DragonAid.Libraries.TransparentPanel();
			this.saveStatus_label = new System.Windows.Forms.Label();
			this.monsterAidView = new AtomosZ.DragonAid.MonsterAid.MonsterAidView();
			this.menuStrip1.SuspendLayout();
			this.info_transparentPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// nextMonster_button
			// 
			this.nextMonster_button.AutoSize = true;
			this.nextMonster_button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.nextMonster_button.Location = new System.Drawing.Point(611, 55);
			this.nextMonster_button.Name = "nextMonster_button";
			this.nextMonster_button.Size = new System.Drawing.Size(37, 30);
			this.nextMonster_button.TabIndex = 15;
			this.nextMonster_button.Text = ">>";
			this.nextMonster_button.UseVisualStyleBackColor = true;
			this.nextMonster_button.Click += new System.EventHandler(this.NextMonster_button_Click);
			// 
			// prevMonster_button
			// 
			this.prevMonster_button.AutoSize = true;
			this.prevMonster_button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.prevMonster_button.Location = new System.Drawing.Point(268, 55);
			this.prevMonster_button.Name = "prevMonster_button";
			this.prevMonster_button.Size = new System.Drawing.Size(37, 30);
			this.prevMonster_button.TabIndex = 16;
			this.prevMonster_button.Text = "<<";
			this.prevMonster_button.UseVisualStyleBackColor = true;
			this.prevMonster_button.Click += new System.EventHandler(this.PrevMonster_button_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(849, 33);
			this.menuStrip1.TabIndex = 17;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadROMToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.restoreToolStripMenuItem,
            this.insertIntoROMToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// loadROMToolStripMenuItem
			// 
			this.loadROMToolStripMenuItem.Name = "loadROMToolStripMenuItem";
			this.loadROMToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
			this.loadROMToolStripMenuItem.Text = "Load MonsterAid File";
			this.loadROMToolStripMenuItem.Click += new System.EventHandler(this.LoadMonsterAidFileToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
			this.saveToolStripMenuItem.Text = "Save MonsterAid File";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
			this.saveAsToolStripMenuItem.Text = "Save As";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAs_ToolStripMenuItem_Click);
			// 
			// restoreToolStripMenuItem
			// 
			this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
			this.restoreToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
			this.restoreToolStripMenuItem.Text = "Restore from ROM";
			this.restoreToolStripMenuItem.Click += new System.EventHandler(this.RestoreToolStripMenuItem_Click);
			// 
			// insertIntoROMToolStripMenuItem
			// 
			this.insertIntoROMToolStripMenuItem.Name = "insertIntoROMToolStripMenuItem";
			this.insertIntoROMToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
			this.insertIntoROMToolStripMenuItem.Text = "Insert Into ROM";
			this.insertIntoROMToolStripMenuItem.Click += new System.EventHandler(this.InsertIntoROMToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
			// 
			// info_transparentPanel
			// 
			this.info_transparentPanel.Controls.Add(this.saveStatus_label);
			this.info_transparentPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.info_transparentPanel.Location = new System.Drawing.Point(0, 1052);
			this.info_transparentPanel.Name = "info_transparentPanel";
			this.info_transparentPanel.Size = new System.Drawing.Size(849, 30);
			this.info_transparentPanel.TabIndex = 19;
			// 
			// saveStatus_label
			// 
			this.saveStatus_label.AutoSize = true;
			this.saveStatus_label.Dock = System.Windows.Forms.DockStyle.Right;
			this.saveStatus_label.Location = new System.Drawing.Point(753, 0);
			this.saveStatus_label.Name = "saveStatus_label";
			this.saveStatus_label.Size = new System.Drawing.Size(96, 20);
			this.saveStatus_label.TabIndex = 0;
			this.saveStatus_label.Text = "Save Status";
			this.saveStatus_label.Visible = false;
			// 
			// monsterAidView
			// 
			this.monsterAidView.AutoSize = true;
			this.monsterAidView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.monsterAidView.Location = new System.Drawing.Point(0, 45);
			this.monsterAidView.Name = "monsterAidView";
			this.monsterAidView.Size = new System.Drawing.Size(847, 947);
			this.monsterAidView.TabIndex = 14;
			// 
			// MonsterAidMainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(849, 1082);
			this.Controls.Add(this.info_transparentPanel);
			this.Controls.Add(this.prevMonster_button);
			this.Controls.Add(this.nextMonster_button);
			this.Controls.Add(this.monsterAidView);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MonsterAidMainForm";
			this.Text = "MonsterAid - DragonAid Monster Editor";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.info_transparentPanel.ResumeLayout(false);
			this.info_transparentPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private MonsterAidView monsterAidView;
		private System.Windows.Forms.Button nextMonster_button;
		private System.Windows.Forms.Button prevMonster_button;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadROMToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem insertIntoROMToolStripMenuItem;
		private Libraries.TransparentPanel info_transparentPanel;
		private System.Windows.Forms.Label saveStatus_label;
	}
}

