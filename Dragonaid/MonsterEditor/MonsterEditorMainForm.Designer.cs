namespace AtomosZ.Dragonaid.MonsterEditor
{
	partial class MonsterEditorMainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonsterEditorMainForm));
			this.monsterEditorView = new AtomosZ.Dragonaid.MonsterEditor.MonsterEditorView();
			this.SuspendLayout();
			// 
			// monsterEditorView
			// 
			this.monsterEditorView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.monsterEditorView.Location = new System.Drawing.Point(0, 0);
			this.monsterEditorView.Name = "monsterEditorView";
			this.monsterEditorView.Size = new System.Drawing.Size(851, 933);
			this.monsterEditorView.TabIndex = 14;
			// 
			// MonsterEditorMainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(849, 945);
			this.Controls.Add(this.monsterEditorView);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MonsterEditorMainForm";
			this.Text = "DW3 Monster Editor";
			this.ResumeLayout(false);

		}

		#endregion
		private MonsterEditorView monsterEditorView;
	}
}

