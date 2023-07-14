namespace AtomosZ.DragonAid.SpriteAid
{
	partial class SpriteAidForm
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
			this.sprite_pictureBox = new System.Windows.Forms.PictureBox();
			this.address_Spinner = new System.Windows.Forms.NumericUpDown();
			this.bgPalettes_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.paletteTime_spinner = new System.Windows.Forms.NumericUpDown();
			this.timeControl_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.time_label = new System.Windows.Forms.Label();
			this.paletteChangeTimes_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.morning_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.morningStart_spinner = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.label3 = new System.Windows.Forms.Label();
			this.lateMorning_spinner = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.label4 = new System.Windows.Forms.Label();
			this.afternoon_spinner = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.label5 = new System.Windows.Forms.Label();
			this.evening_spinner = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.label6 = new System.Windows.Forms.Label();
			this.dusk_spinner = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.label7 = new System.Windows.Forms.Label();
			this.night_spinner = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.label8 = new System.Windows.Forms.Label();
			this.lateNight_spinner = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
			this.label9 = new System.Windows.Forms.Label();
			this.dayCycleLength_spinner = new System.Windows.Forms.NumericUpDown();
			this.palette_comboBox = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.sprite_pictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.address_Spinner)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.paletteTime_spinner)).BeginInit();
			this.timeControl_flowLayoutPanel.SuspendLayout();
			this.paletteChangeTimes_flowLayoutPanel.SuspendLayout();
			this.morning_flowLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.morningStart_spinner)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.lateMorning_spinner)).BeginInit();
			this.flowLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.afternoon_spinner)).BeginInit();
			this.flowLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.evening_spinner)).BeginInit();
			this.flowLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dusk_spinner)).BeginInit();
			this.flowLayoutPanel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.night_spinner)).BeginInit();
			this.flowLayoutPanel6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.lateNight_spinner)).BeginInit();
			this.flowLayoutPanel7.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dayCycleLength_spinner)).BeginInit();
			this.SuspendLayout();
			// 
			// sprite_pictureBox
			// 
			this.sprite_pictureBox.Location = new System.Drawing.Point(12, 45);
			this.sprite_pictureBox.Name = "sprite_pictureBox";
			this.sprite_pictureBox.Size = new System.Drawing.Size(128, 128);
			this.sprite_pictureBox.TabIndex = 0;
			this.sprite_pictureBox.TabStop = false;
			// 
			// address_Spinner
			// 
			this.address_Spinner.Hexadecimal = true;
			this.address_Spinner.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
			this.address_Spinner.Location = new System.Drawing.Point(13, 13);
			this.address_Spinner.Maximum = new decimal(new int[] {
            524287,
            0,
            0,
            0});
			this.address_Spinner.Name = "address_Spinner";
			this.address_Spinner.Size = new System.Drawing.Size(120, 26);
			this.address_Spinner.TabIndex = 1;
			this.address_Spinner.Value = new decimal(new int[] {
            131088,
            0,
            0,
            0});
			this.address_Spinner.ValueChanged += new System.EventHandler(this.Address_Spinner_ValueChanged);
			// 
			// bgPalettes_tableLayoutPanel
			// 
			this.bgPalettes_tableLayoutPanel.AutoSize = true;
			this.bgPalettes_tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.bgPalettes_tableLayoutPanel.ColumnCount = 4;
			this.bgPalettes_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
			this.bgPalettes_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
			this.bgPalettes_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
			this.bgPalettes_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
			this.bgPalettes_tableLayoutPanel.Location = new System.Drawing.Point(373, 45);
			this.bgPalettes_tableLayoutPanel.Name = "bgPalettes_tableLayoutPanel";
			this.bgPalettes_tableLayoutPanel.RowCount = 4;
			this.bgPalettes_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
			this.bgPalettes_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
			this.bgPalettes_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
			this.bgPalettes_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
			this.bgPalettes_tableLayoutPanel.Size = new System.Drawing.Size(256, 256);
			this.bgPalettes_tableLayoutPanel.TabIndex = 2;
			// 
			// paletteTime_spinner
			// 
			this.paletteTime_spinner.Location = new System.Drawing.Point(106, 3);
			this.paletteTime_spinner.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
			this.paletteTime_spinner.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.paletteTime_spinner.Name = "paletteTime_spinner";
			this.paletteTime_spinner.Size = new System.Drawing.Size(57, 26);
			this.paletteTime_spinner.TabIndex = 3;
			this.paletteTime_spinner.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.paletteTime_spinner.ValueChanged += new System.EventHandler(this.PaletteTime_spinner_ValueChanged);
			// 
			// timeControl_flowLayoutPanel
			// 
			this.timeControl_flowLayoutPanel.AutoSize = true;
			this.timeControl_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.timeControl_flowLayoutPanel.Controls.Add(this.label1);
			this.timeControl_flowLayoutPanel.Controls.Add(this.paletteTime_spinner);
			this.timeControl_flowLayoutPanel.Controls.Add(this.time_label);
			this.timeControl_flowLayoutPanel.Location = new System.Drawing.Point(376, 7);
			this.timeControl_flowLayoutPanel.Name = "timeControl_flowLayoutPanel";
			this.timeControl_flowLayoutPanel.Size = new System.Drawing.Size(208, 32);
			this.timeControl_flowLayoutPanel.TabIndex = 4;
			this.timeControl_flowLayoutPanel.WrapContents = false;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Time of Day:";
			// 
			// time_label
			// 
			this.time_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.time_label.AutoSize = true;
			this.time_label.Location = new System.Drawing.Point(169, 6);
			this.time_label.Name = "time_label";
			this.time_label.Size = new System.Drawing.Size(36, 20);
			this.time_label.TabIndex = 4;
			this.time_label.Text = "$00";
			// 
			// paletteChangeTimes_flowLayoutPanel
			// 
			this.paletteChangeTimes_flowLayoutPanel.AutoSize = true;
			this.paletteChangeTimes_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.paletteChangeTimes_flowLayoutPanel.Controls.Add(this.morning_flowLayoutPanel);
			this.paletteChangeTimes_flowLayoutPanel.Controls.Add(this.flowLayoutPanel1);
			this.paletteChangeTimes_flowLayoutPanel.Controls.Add(this.flowLayoutPanel2);
			this.paletteChangeTimes_flowLayoutPanel.Controls.Add(this.flowLayoutPanel3);
			this.paletteChangeTimes_flowLayoutPanel.Controls.Add(this.flowLayoutPanel4);
			this.paletteChangeTimes_flowLayoutPanel.Controls.Add(this.flowLayoutPanel5);
			this.paletteChangeTimes_flowLayoutPanel.Controls.Add(this.flowLayoutPanel6);
			this.paletteChangeTimes_flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.paletteChangeTimes_flowLayoutPanel.Location = new System.Drawing.Point(373, 342);
			this.paletteChangeTimes_flowLayoutPanel.Name = "paletteChangeTimes_flowLayoutPanel";
			this.paletteChangeTimes_flowLayoutPanel.Size = new System.Drawing.Size(198, 266);
			this.paletteChangeTimes_flowLayoutPanel.TabIndex = 5;
			this.paletteChangeTimes_flowLayoutPanel.WrapContents = false;
			// 
			// morning_flowLayoutPanel
			// 
			this.morning_flowLayoutPanel.AutoSize = true;
			this.morning_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.morning_flowLayoutPanel.Controls.Add(this.label2);
			this.morning_flowLayoutPanel.Controls.Add(this.morningStart_spinner);
			this.morning_flowLayoutPanel.Location = new System.Drawing.Point(3, 3);
			this.morning_flowLayoutPanel.Name = "morning_flowLayoutPanel";
			this.morning_flowLayoutPanel.Size = new System.Drawing.Size(192, 32);
			this.morning_flowLayoutPanel.TabIndex = 4;
			this.morning_flowLayoutPanel.WrapContents = false;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 6);
			this.label2.MinimumSize = new System.Drawing.Size(112, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Morning:";
			// 
			// morningStart_spinner
			// 
			this.morningStart_spinner.Hexadecimal = true;
			this.morningStart_spinner.Location = new System.Drawing.Point(121, 3);
			this.morningStart_spinner.Maximum = new decimal(new int[] {
            203,
            0,
            0,
            0});
			this.morningStart_spinner.Name = "morningStart_spinner";
			this.morningStart_spinner.Size = new System.Drawing.Size(68, 26);
			this.morningStart_spinner.TabIndex = 3;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.label3);
			this.flowLayoutPanel1.Controls.Add(this.lateMorning_spinner);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 41);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(192, 32);
			this.flowLayoutPanel1.TabIndex = 5;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 6);
			this.label3.MinimumSize = new System.Drawing.Size(112, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "Late Morning:";
			// 
			// lateMorning_spinner
			// 
			this.lateMorning_spinner.Hexadecimal = true;
			this.lateMorning_spinner.Location = new System.Drawing.Point(121, 3);
			this.lateMorning_spinner.Maximum = new decimal(new int[] {
            203,
            0,
            0,
            0});
			this.lateMorning_spinner.Name = "lateMorning_spinner";
			this.lateMorning_spinner.Size = new System.Drawing.Size(68, 26);
			this.lateMorning_spinner.TabIndex = 3;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.AutoSize = true;
			this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel2.Controls.Add(this.label4);
			this.flowLayoutPanel2.Controls.Add(this.afternoon_spinner);
			this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 79);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(192, 32);
			this.flowLayoutPanel2.TabIndex = 6;
			this.flowLayoutPanel2.WrapContents = false;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 6);
			this.label4.MinimumSize = new System.Drawing.Size(112, 20);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 20);
			this.label4.TabIndex = 0;
			this.label4.Text = "Afternoon:";
			// 
			// afternoon_spinner
			// 
			this.afternoon_spinner.Hexadecimal = true;
			this.afternoon_spinner.Location = new System.Drawing.Point(121, 3);
			this.afternoon_spinner.Maximum = new decimal(new int[] {
            203,
            0,
            0,
            0});
			this.afternoon_spinner.Name = "afternoon_spinner";
			this.afternoon_spinner.Size = new System.Drawing.Size(68, 26);
			this.afternoon_spinner.TabIndex = 3;
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.AutoSize = true;
			this.flowLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel3.Controls.Add(this.label5);
			this.flowLayoutPanel3.Controls.Add(this.evening_spinner);
			this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 117);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(192, 32);
			this.flowLayoutPanel3.TabIndex = 7;
			this.flowLayoutPanel3.WrapContents = false;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 6);
			this.label5.MinimumSize = new System.Drawing.Size(112, 20);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(112, 20);
			this.label5.TabIndex = 0;
			this.label5.Text = "Early Evening:";
			// 
			// evening_spinner
			// 
			this.evening_spinner.Hexadecimal = true;
			this.evening_spinner.Location = new System.Drawing.Point(121, 3);
			this.evening_spinner.Maximum = new decimal(new int[] {
            203,
            0,
            0,
            0});
			this.evening_spinner.Name = "evening_spinner";
			this.evening_spinner.Size = new System.Drawing.Size(68, 26);
			this.evening_spinner.TabIndex = 3;
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.AutoSize = true;
			this.flowLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel4.Controls.Add(this.label6);
			this.flowLayoutPanel4.Controls.Add(this.dusk_spinner);
			this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 155);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(192, 32);
			this.flowLayoutPanel4.TabIndex = 8;
			this.flowLayoutPanel4.WrapContents = false;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(3, 6);
			this.label6.MinimumSize = new System.Drawing.Size(112, 20);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(112, 20);
			this.label6.TabIndex = 0;
			this.label6.Text = "Dusk:";
			// 
			// dusk_spinner
			// 
			this.dusk_spinner.Hexadecimal = true;
			this.dusk_spinner.Location = new System.Drawing.Point(121, 3);
			this.dusk_spinner.Maximum = new decimal(new int[] {
            203,
            0,
            0,
            0});
			this.dusk_spinner.Name = "dusk_spinner";
			this.dusk_spinner.Size = new System.Drawing.Size(68, 26);
			this.dusk_spinner.TabIndex = 3;
			// 
			// flowLayoutPanel5
			// 
			this.flowLayoutPanel5.AutoSize = true;
			this.flowLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel5.Controls.Add(this.label7);
			this.flowLayoutPanel5.Controls.Add(this.night_spinner);
			this.flowLayoutPanel5.Location = new System.Drawing.Point(3, 193);
			this.flowLayoutPanel5.Name = "flowLayoutPanel5";
			this.flowLayoutPanel5.Size = new System.Drawing.Size(192, 32);
			this.flowLayoutPanel5.TabIndex = 9;
			this.flowLayoutPanel5.WrapContents = false;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(3, 6);
			this.label7.MinimumSize = new System.Drawing.Size(112, 20);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(112, 20);
			this.label7.TabIndex = 0;
			this.label7.Text = "Night:";
			// 
			// night_spinner
			// 
			this.night_spinner.Hexadecimal = true;
			this.night_spinner.Location = new System.Drawing.Point(121, 3);
			this.night_spinner.Maximum = new decimal(new int[] {
            203,
            0,
            0,
            0});
			this.night_spinner.Name = "night_spinner";
			this.night_spinner.Size = new System.Drawing.Size(68, 26);
			this.night_spinner.TabIndex = 3;
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.AutoSize = true;
			this.flowLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel6.Controls.Add(this.label8);
			this.flowLayoutPanel6.Controls.Add(this.lateNight_spinner);
			this.flowLayoutPanel6.Location = new System.Drawing.Point(3, 231);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(192, 32);
			this.flowLayoutPanel6.TabIndex = 10;
			this.flowLayoutPanel6.WrapContents = false;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(3, 6);
			this.label8.MinimumSize = new System.Drawing.Size(112, 20);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(112, 20);
			this.label8.TabIndex = 0;
			this.label8.Text = "Late Night?:";
			// 
			// lateNight_spinner
			// 
			this.lateNight_spinner.Hexadecimal = true;
			this.lateNight_spinner.Location = new System.Drawing.Point(121, 3);
			this.lateNight_spinner.Maximum = new decimal(new int[] {
            203,
            0,
            0,
            0});
			this.lateNight_spinner.Name = "lateNight_spinner";
			this.lateNight_spinner.Size = new System.Drawing.Size(68, 26);
			this.lateNight_spinner.TabIndex = 3;
			// 
			// flowLayoutPanel7
			// 
			this.flowLayoutPanel7.AutoSize = true;
			this.flowLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel7.Controls.Add(this.label9);
			this.flowLayoutPanel7.Controls.Add(this.dayCycleLength_spinner);
			this.flowLayoutPanel7.Location = new System.Drawing.Point(373, 307);
			this.flowLayoutPanel7.Name = "flowLayoutPanel7";
			this.flowLayoutPanel7.Size = new System.Drawing.Size(224, 32);
			this.flowLayoutPanel7.TabIndex = 11;
			this.flowLayoutPanel7.WrapContents = false;
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(3, 6);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(144, 20);
			this.label9.TabIndex = 0;
			this.label9.Text = "Ticks per day cycle:";
			// 
			// dayCycleLength_spinner
			// 
			this.dayCycleLength_spinner.Hexadecimal = true;
			this.dayCycleLength_spinner.Location = new System.Drawing.Point(153, 3);
			this.dayCycleLength_spinner.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.dayCycleLength_spinner.Minimum = new decimal(new int[] {
            7,
            0,
            0,
            0});
			this.dayCycleLength_spinner.Name = "dayCycleLength_spinner";
			this.dayCycleLength_spinner.Size = new System.Drawing.Size(68, 26);
			this.dayCycleLength_spinner.TabIndex = 3;
			this.dayCycleLength_spinner.Value = new decimal(new int[] {
            203,
            0,
            0,
            0});
			this.dayCycleLength_spinner.ValueChanged += new System.EventHandler(this.DayCycleLength_spinner_ValueChanged);
			// 
			// palette_comboBox
			// 
			this.palette_comboBox.FormattingEnabled = true;
			this.palette_comboBox.Location = new System.Drawing.Point(178, 10);
			this.palette_comboBox.Name = "palette_comboBox";
			this.palette_comboBox.Size = new System.Drawing.Size(121, 28);
			this.palette_comboBox.TabIndex = 12;
			// 
			// SpriteAidForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 653);
			this.Controls.Add(this.palette_comboBox);
			this.Controls.Add(this.flowLayoutPanel7);
			this.Controls.Add(this.timeControl_flowLayoutPanel);
			this.Controls.Add(this.bgPalettes_tableLayoutPanel);
			this.Controls.Add(this.paletteChangeTimes_flowLayoutPanel);
			this.Controls.Add(this.address_Spinner);
			this.Controls.Add(this.sprite_pictureBox);
			this.Name = "SpriteAidForm";
			this.Text = "DraongAid - SpriteAid";
			((System.ComponentModel.ISupportInitialize)(this.sprite_pictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.address_Spinner)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.paletteTime_spinner)).EndInit();
			this.timeControl_flowLayoutPanel.ResumeLayout(false);
			this.timeControl_flowLayoutPanel.PerformLayout();
			this.paletteChangeTimes_flowLayoutPanel.ResumeLayout(false);
			this.paletteChangeTimes_flowLayoutPanel.PerformLayout();
			this.morning_flowLayoutPanel.ResumeLayout(false);
			this.morning_flowLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.morningStart_spinner)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.lateMorning_spinner)).EndInit();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.afternoon_spinner)).EndInit();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.evening_spinner)).EndInit();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dusk_spinner)).EndInit();
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.night_spinner)).EndInit();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel6.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.lateNight_spinner)).EndInit();
			this.flowLayoutPanel7.ResumeLayout(false);
			this.flowLayoutPanel7.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dayCycleLength_spinner)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox sprite_pictureBox;
		private System.Windows.Forms.NumericUpDown address_Spinner;
		private System.Windows.Forms.TableLayoutPanel bgPalettes_tableLayoutPanel;
		private System.Windows.Forms.NumericUpDown paletteTime_spinner;
		private System.Windows.Forms.FlowLayoutPanel timeControl_flowLayoutPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.FlowLayoutPanel paletteChangeTimes_flowLayoutPanel;
		private System.Windows.Forms.FlowLayoutPanel morning_flowLayoutPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown morningStart_spinner;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown lateMorning_spinner;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown afternoon_spinner;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown evening_spinner;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown dusk_spinner;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown night_spinner;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown lateNight_spinner;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.NumericUpDown dayCycleLength_spinner;
		private System.Windows.Forms.Label time_label;
		private System.Windows.Forms.ComboBox palette_comboBox;
	}
}

