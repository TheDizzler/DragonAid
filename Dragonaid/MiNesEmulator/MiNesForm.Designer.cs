namespace AtomosZ.MiNesEmulator
{
	partial class MiNesForm
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
			this.cpu_groupBox = new System.Windows.Forms.GroupBox();
			this.cpuStatus_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.cpu_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.acc_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.a_numberBox = new AtomosZ.DragonAid.Libraries.NumberBox();
			this.label2 = new System.Windows.Forms.Label();
			this.x_numberBox = new AtomosZ.DragonAid.Libraries.NumberBox();
			this.label3 = new System.Windows.Forms.Label();
			this.y_numberBox = new AtomosZ.DragonAid.Libraries.NumberBox();
			this.label4 = new System.Windows.Forms.Label();
			this.pc_numberBox = new AtomosZ.DragonAid.Libraries.NumberBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cycles_numberBox = new AtomosZ.DragonAid.Libraries.NumberBox();
			this.flag_groupBox = new System.Windows.Forms.GroupBox();
			this.flags_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.flags_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.ps_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.label6 = new System.Windows.Forms.Label();
			this.ps_numberBox = new AtomosZ.DragonAid.Libraries.NumberBox();
			this.negative_checkBox = new System.Windows.Forms.CheckBox();
			this.overflow_checkBox = new System.Windows.Forms.CheckBox();
			this.reserved_checkBox = new System.Windows.Forms.CheckBox();
			this.break_checkBox = new System.Windows.Forms.CheckBox();
			this.unused_checkBox = new System.Windows.Forms.CheckBox();
			this.interrupt_checkBox = new System.Windows.Forms.CheckBox();
			this.zero_checkBox = new System.Windows.Forms.CheckBox();
			this.carry_checkBox = new System.Windows.Forms.CheckBox();
			this.stack_groupBox = new System.Windows.Forms.GroupBox();
			this.stack_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.sp_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.label7 = new System.Windows.Forms.Label();
			this.sp_numberBox = new AtomosZ.DragonAid.Libraries.NumberBox();
			this.stack_textBox = new System.Windows.Forms.TextBox();
			this.nextLine_button = new System.Windows.Forms.Button();
			this.code_listBox = new System.Windows.Forms.ListBox();
			this.nextLine_textBox = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.pc_label = new System.Windows.Forms.Label();
			this.run_button = new System.Windows.Forms.Button();
			this.reset_button = new System.Windows.Forms.Button();
			this.memoryViewer = new AtomosZ.MiNesEmulator.MemoryViewer();
			this.cpu_groupBox.SuspendLayout();
			this.cpuStatus_flowLayoutPanel.SuspendLayout();
			this.cpu_flowLayoutPanel.SuspendLayout();
			this.acc_flowLayoutPanel.SuspendLayout();
			this.flag_groupBox.SuspendLayout();
			this.flags_flowLayoutPanel.SuspendLayout();
			this.flags_tableLayoutPanel.SuspendLayout();
			this.ps_flowLayoutPanel.SuspendLayout();
			this.stack_groupBox.SuspendLayout();
			this.stack_flowLayoutPanel.SuspendLayout();
			this.sp_flowLayoutPanel.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// cpu_groupBox
			// 
			this.cpu_groupBox.Controls.Add(this.cpuStatus_flowLayoutPanel);
			this.cpu_groupBox.Location = new System.Drawing.Point(3, 3);
			this.cpu_groupBox.Name = "cpu_groupBox";
			this.cpu_groupBox.Size = new System.Drawing.Size(750, 241);
			this.cpu_groupBox.TabIndex = 0;
			this.cpu_groupBox.TabStop = false;
			this.cpu_groupBox.Text = "CPU Status";
			// 
			// cpuStatus_flowLayoutPanel
			// 
			this.cpuStatus_flowLayoutPanel.AutoSize = true;
			this.cpuStatus_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.cpuStatus_flowLayoutPanel.Controls.Add(this.cpu_flowLayoutPanel);
			this.cpuStatus_flowLayoutPanel.Controls.Add(this.stack_groupBox);
			this.cpuStatus_flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cpuStatus_flowLayoutPanel.Location = new System.Drawing.Point(3, 22);
			this.cpuStatus_flowLayoutPanel.Name = "cpuStatus_flowLayoutPanel";
			this.cpuStatus_flowLayoutPanel.Size = new System.Drawing.Size(744, 216);
			this.cpuStatus_flowLayoutPanel.TabIndex = 4;
			this.cpuStatus_flowLayoutPanel.WrapContents = false;
			// 
			// cpu_flowLayoutPanel
			// 
			this.cpu_flowLayoutPanel.AutoSize = true;
			this.cpu_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.cpu_flowLayoutPanel.Controls.Add(this.acc_flowLayoutPanel);
			this.cpu_flowLayoutPanel.Controls.Add(this.flag_groupBox);
			this.cpu_flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.cpu_flowLayoutPanel.Location = new System.Drawing.Point(3, 3);
			this.cpu_flowLayoutPanel.Name = "cpu_flowLayoutPanel";
			this.cpu_flowLayoutPanel.Size = new System.Drawing.Size(575, 146);
			this.cpu_flowLayoutPanel.TabIndex = 1;
			this.cpu_flowLayoutPanel.WrapContents = false;
			// 
			// acc_flowLayoutPanel
			// 
			this.acc_flowLayoutPanel.AutoSize = true;
			this.acc_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.acc_flowLayoutPanel.Controls.Add(this.label1);
			this.acc_flowLayoutPanel.Controls.Add(this.a_numberBox);
			this.acc_flowLayoutPanel.Controls.Add(this.label2);
			this.acc_flowLayoutPanel.Controls.Add(this.x_numberBox);
			this.acc_flowLayoutPanel.Controls.Add(this.label3);
			this.acc_flowLayoutPanel.Controls.Add(this.y_numberBox);
			this.acc_flowLayoutPanel.Controls.Add(this.label4);
			this.acc_flowLayoutPanel.Controls.Add(this.pc_numberBox);
			this.acc_flowLayoutPanel.Controls.Add(this.label5);
			this.acc_flowLayoutPanel.Controls.Add(this.cycles_numberBox);
			this.acc_flowLayoutPanel.Location = new System.Drawing.Point(3, 3);
			this.acc_flowLayoutPanel.Name = "acc_flowLayoutPanel";
			this.acc_flowLayoutPanel.Size = new System.Drawing.Size(510, 32);
			this.acc_flowLayoutPanel.TabIndex = 0;
			this.acc_flowLayoutPanel.WrapContents = false;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(24, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "A:";
			// 
			// a_numberBox
			// 
			this.a_numberBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.a_numberBox.Hexadecimal = true;
			this.a_numberBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.a_numberBox.Location = new System.Drawing.Point(33, 3);
			this.a_numberBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.a_numberBox.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.a_numberBox.Name = "a_numberBox";
			this.a_numberBox.Size = new System.Drawing.Size(39, 26);
			this.a_numberBox.TabIndex = 1;
			this.a_numberBox.Value = 255;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(78, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(24, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "X:";
			// 
			// x_numberBox
			// 
			this.x_numberBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.x_numberBox.Hexadecimal = true;
			this.x_numberBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.x_numberBox.Location = new System.Drawing.Point(108, 3);
			this.x_numberBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.x_numberBox.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.x_numberBox.Name = "x_numberBox";
			this.x_numberBox.Size = new System.Drawing.Size(39, 26);
			this.x_numberBox.TabIndex = 3;
			this.x_numberBox.Value = 255;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(153, 6);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(24, 20);
			this.label3.TabIndex = 4;
			this.label3.Text = "Y:";
			// 
			// y_numberBox
			// 
			this.y_numberBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.y_numberBox.Hexadecimal = true;
			this.y_numberBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.y_numberBox.Location = new System.Drawing.Point(183, 3);
			this.y_numberBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.y_numberBox.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.y_numberBox.Name = "y_numberBox";
			this.y_numberBox.Size = new System.Drawing.Size(39, 26);
			this.y_numberBox.TabIndex = 5;
			this.y_numberBox.Value = 255;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(228, 6);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(34, 20);
			this.label4.TabIndex = 6;
			this.label4.Text = "PC:";
			// 
			// pc_numberBox
			// 
			this.pc_numberBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pc_numberBox.Hexadecimal = true;
			this.pc_numberBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.pc_numberBox.Location = new System.Drawing.Point(268, 3);
			this.pc_numberBox.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.pc_numberBox.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.pc_numberBox.Name = "pc_numberBox";
			this.pc_numberBox.Size = new System.Drawing.Size(49, 26);
			this.pc_numberBox.TabIndex = 7;
			this.pc_numberBox.Value = 65535;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(323, 6);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(59, 20);
			this.label5.TabIndex = 8;
			this.label5.Text = "Cycles:";
			// 
			// cycles_numberBox
			// 
			this.cycles_numberBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cycles_numberBox.Hexadecimal = false;
			this.cycles_numberBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.cycles_numberBox.Location = new System.Drawing.Point(388, 3);
			this.cycles_numberBox.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.cycles_numberBox.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.cycles_numberBox.Name = "cycles_numberBox";
			this.cycles_numberBox.Size = new System.Drawing.Size(119, 26);
			this.cycles_numberBox.TabIndex = 9;
			this.cycles_numberBox.Value = 0;
			// 
			// flag_groupBox
			// 
			this.flag_groupBox.Controls.Add(this.flags_flowLayoutPanel);
			this.flag_groupBox.Location = new System.Drawing.Point(3, 41);
			this.flag_groupBox.Name = "flag_groupBox";
			this.flag_groupBox.Size = new System.Drawing.Size(569, 102);
			this.flag_groupBox.TabIndex = 2;
			this.flag_groupBox.TabStop = false;
			this.flag_groupBox.Text = "Flags";
			// 
			// flags_flowLayoutPanel
			// 
			this.flags_flowLayoutPanel.Controls.Add(this.flags_tableLayoutPanel);
			this.flags_flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flags_flowLayoutPanel.Location = new System.Drawing.Point(3, 22);
			this.flags_flowLayoutPanel.Name = "flags_flowLayoutPanel";
			this.flags_flowLayoutPanel.Size = new System.Drawing.Size(563, 77);
			this.flags_flowLayoutPanel.TabIndex = 1;
			// 
			// flags_tableLayoutPanel
			// 
			this.flags_tableLayoutPanel.AutoSize = true;
			this.flags_tableLayoutPanel.ColumnCount = 5;
			this.flags_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
			this.flags_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
			this.flags_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
			this.flags_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
			this.flags_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
			this.flags_tableLayoutPanel.Controls.Add(this.ps_flowLayoutPanel, 0, 0);
			this.flags_tableLayoutPanel.Controls.Add(this.negative_checkBox, 4, 1);
			this.flags_tableLayoutPanel.Controls.Add(this.overflow_checkBox, 3, 1);
			this.flags_tableLayoutPanel.Controls.Add(this.reserved_checkBox, 2, 1);
			this.flags_tableLayoutPanel.Controls.Add(this.break_checkBox, 1, 1);
			this.flags_tableLayoutPanel.Controls.Add(this.unused_checkBox, 4, 0);
			this.flags_tableLayoutPanel.Controls.Add(this.interrupt_checkBox, 3, 0);
			this.flags_tableLayoutPanel.Controls.Add(this.zero_checkBox, 2, 0);
			this.flags_tableLayoutPanel.Controls.Add(this.carry_checkBox, 1, 0);
			this.flags_tableLayoutPanel.Location = new System.Drawing.Point(3, 3);
			this.flags_tableLayoutPanel.Name = "flags_tableLayoutPanel";
			this.flags_tableLayoutPanel.RowCount = 2;
			this.flags_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.flags_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.flags_tableLayoutPanel.Size = new System.Drawing.Size(556, 60);
			this.flags_tableLayoutPanel.TabIndex = 2;
			// 
			// ps_flowLayoutPanel
			// 
			this.ps_flowLayoutPanel.Controls.Add(this.label6);
			this.ps_flowLayoutPanel.Controls.Add(this.ps_numberBox);
			this.ps_flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ps_flowLayoutPanel.Location = new System.Drawing.Point(3, 3);
			this.ps_flowLayoutPanel.Name = "ps_flowLayoutPanel";
			this.flags_tableLayoutPanel.SetRowSpan(this.ps_flowLayoutPanel, 2);
			this.ps_flowLayoutPanel.Size = new System.Drawing.Size(90, 54);
			this.ps_flowLayoutPanel.TabIndex = 0;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(3, 6);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(34, 20);
			this.label6.TabIndex = 2;
			this.label6.Text = "PS:";
			// 
			// ps_numberBox
			// 
			this.ps_numberBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ps_numberBox.Hexadecimal = true;
			this.ps_numberBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.ps_numberBox.Location = new System.Drawing.Point(43, 3);
			this.ps_numberBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.ps_numberBox.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.ps_numberBox.Name = "ps_numberBox";
			this.ps_numberBox.Size = new System.Drawing.Size(39, 26);
			this.ps_numberBox.TabIndex = 3;
			this.ps_numberBox.Value = 255;
			// 
			// negative_checkBox
			// 
			this.negative_checkBox.AutoSize = true;
			this.negative_checkBox.Location = new System.Drawing.Point(444, 33);
			this.negative_checkBox.Name = "negative_checkBox";
			this.negative_checkBox.Size = new System.Drawing.Size(97, 24);
			this.negative_checkBox.TabIndex = 7;
			this.negative_checkBox.Text = "Negative";
			this.negative_checkBox.UseVisualStyleBackColor = true;
			// 
			// overflow_checkBox
			// 
			this.overflow_checkBox.AutoSize = true;
			this.overflow_checkBox.Location = new System.Drawing.Point(329, 33);
			this.overflow_checkBox.Name = "overflow_checkBox";
			this.overflow_checkBox.Size = new System.Drawing.Size(96, 24);
			this.overflow_checkBox.TabIndex = 6;
			this.overflow_checkBox.Text = "Overflow";
			this.overflow_checkBox.UseVisualStyleBackColor = true;
			// 
			// reserved_checkBox
			// 
			this.reserved_checkBox.AutoSize = true;
			this.reserved_checkBox.Enabled = false;
			this.reserved_checkBox.Location = new System.Drawing.Point(214, 33);
			this.reserved_checkBox.Name = "reserved_checkBox";
			this.reserved_checkBox.Size = new System.Drawing.Size(103, 24);
			this.reserved_checkBox.TabIndex = 5;
			this.reserved_checkBox.Text = "Reserved";
			this.reserved_checkBox.UseVisualStyleBackColor = true;
			// 
			// break_checkBox
			// 
			this.break_checkBox.AutoSize = true;
			this.break_checkBox.Enabled = false;
			this.break_checkBox.Location = new System.Drawing.Point(99, 33);
			this.break_checkBox.Name = "break_checkBox";
			this.break_checkBox.Size = new System.Drawing.Size(77, 24);
			this.break_checkBox.TabIndex = 4;
			this.break_checkBox.Text = "Break";
			this.break_checkBox.UseVisualStyleBackColor = true;
			// 
			// unused_checkBox
			// 
			this.unused_checkBox.AutoSize = true;
			this.unused_checkBox.Enabled = false;
			this.unused_checkBox.Location = new System.Drawing.Point(444, 3);
			this.unused_checkBox.Name = "unused_checkBox";
			this.unused_checkBox.Size = new System.Drawing.Size(91, 24);
			this.unused_checkBox.TabIndex = 3;
			this.unused_checkBox.Text = "Unused";
			this.unused_checkBox.UseVisualStyleBackColor = true;
			// 
			// interrupt_checkBox
			// 
			this.interrupt_checkBox.AutoSize = true;
			this.interrupt_checkBox.Location = new System.Drawing.Point(329, 3);
			this.interrupt_checkBox.Name = "interrupt_checkBox";
			this.interrupt_checkBox.Size = new System.Drawing.Size(96, 24);
			this.interrupt_checkBox.TabIndex = 2;
			this.interrupt_checkBox.Text = "Interrupt";
			this.interrupt_checkBox.UseVisualStyleBackColor = true;
			// 
			// zero_checkBox
			// 
			this.zero_checkBox.AutoSize = true;
			this.zero_checkBox.Location = new System.Drawing.Point(214, 3);
			this.zero_checkBox.Name = "zero_checkBox";
			this.zero_checkBox.Size = new System.Drawing.Size(68, 24);
			this.zero_checkBox.TabIndex = 1;
			this.zero_checkBox.Text = "Zero";
			this.zero_checkBox.UseVisualStyleBackColor = true;
			// 
			// carry_checkBox
			// 
			this.carry_checkBox.AutoSize = true;
			this.carry_checkBox.Location = new System.Drawing.Point(99, 3);
			this.carry_checkBox.Name = "carry_checkBox";
			this.carry_checkBox.Size = new System.Drawing.Size(72, 24);
			this.carry_checkBox.TabIndex = 0;
			this.carry_checkBox.Text = "Carry";
			this.carry_checkBox.UseVisualStyleBackColor = true;
			// 
			// stack_groupBox
			// 
			this.stack_groupBox.Controls.Add(this.stack_flowLayoutPanel);
			this.stack_groupBox.Location = new System.Drawing.Point(584, 3);
			this.stack_groupBox.Name = "stack_groupBox";
			this.stack_groupBox.Size = new System.Drawing.Size(153, 182);
			this.stack_groupBox.TabIndex = 3;
			this.stack_groupBox.TabStop = false;
			this.stack_groupBox.Text = "Stack";
			// 
			// stack_flowLayoutPanel
			// 
			this.stack_flowLayoutPanel.AutoSize = true;
			this.stack_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.stack_flowLayoutPanel.Controls.Add(this.sp_flowLayoutPanel);
			this.stack_flowLayoutPanel.Controls.Add(this.stack_textBox);
			this.stack_flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.stack_flowLayoutPanel.Location = new System.Drawing.Point(3, 22);
			this.stack_flowLayoutPanel.Name = "stack_flowLayoutPanel";
			this.stack_flowLayoutPanel.Size = new System.Drawing.Size(140, 159);
			this.stack_flowLayoutPanel.TabIndex = 0;
			this.stack_flowLayoutPanel.WrapContents = false;
			// 
			// sp_flowLayoutPanel
			// 
			this.sp_flowLayoutPanel.Controls.Add(this.label7);
			this.sp_flowLayoutPanel.Controls.Add(this.sp_numberBox);
			this.sp_flowLayoutPanel.Location = new System.Drawing.Point(3, 3);
			this.sp_flowLayoutPanel.Name = "sp_flowLayoutPanel";
			this.sp_flowLayoutPanel.Size = new System.Drawing.Size(102, 33);
			this.sp_flowLayoutPanel.TabIndex = 0;
			this.sp_flowLayoutPanel.WrapContents = false;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(3, 6);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(34, 20);
			this.label7.TabIndex = 2;
			this.label7.Text = "SP:";
			// 
			// sp_numberBox
			// 
			this.sp_numberBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.sp_numberBox.Hexadecimal = true;
			this.sp_numberBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.sp_numberBox.Location = new System.Drawing.Point(43, 3);
			this.sp_numberBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.sp_numberBox.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.sp_numberBox.Name = "sp_numberBox";
			this.sp_numberBox.Size = new System.Drawing.Size(39, 26);
			this.sp_numberBox.TabIndex = 3;
			this.sp_numberBox.Value = 255;
			// 
			// stack_textBox
			// 
			this.stack_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.stack_textBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.stack_textBox.Location = new System.Drawing.Point(3, 42);
			this.stack_textBox.Multiline = true;
			this.stack_textBox.Name = "stack_textBox";
			this.stack_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.stack_textBox.ShortcutsEnabled = false;
			this.stack_textBox.Size = new System.Drawing.Size(134, 114);
			this.stack_textBox.TabIndex = 1;
			this.stack_textBox.Tag = "";
			this.stack_textBox.Text = "$00, $00, $00, $00";
			// 
			// nextLine_button
			// 
			this.nextLine_button.AutoSize = true;
			this.nextLine_button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.nextLine_button.Location = new System.Drawing.Point(7, 595);
			this.nextLine_button.Name = "nextLine_button";
			this.nextLine_button.Size = new System.Drawing.Size(53, 30);
			this.nextLine_button.TabIndex = 2;
			this.nextLine_button.Text = "Step";
			this.nextLine_button.UseVisualStyleBackColor = true;
			this.nextLine_button.Click += new System.EventHandler(this.NextLine_button_Click);
			// 
			// code_listBox
			// 
			this.code_listBox.FormattingEnabled = true;
			this.code_listBox.ItemHeight = 20;
			this.code_listBox.Location = new System.Drawing.Point(6, 285);
			this.code_listBox.Name = "code_listBox";
			this.code_listBox.Size = new System.Drawing.Size(747, 304);
			this.code_listBox.TabIndex = 4;
			// 
			// nextLine_textBox
			// 
			this.nextLine_textBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nextLine_textBox.Location = new System.Drawing.Point(63, 3);
			this.nextLine_textBox.Name = "nextLine_textBox";
			this.nextLine_textBox.Size = new System.Drawing.Size(681, 26);
			this.nextLine_textBox.TabIndex = 5;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.pc_label);
			this.flowLayoutPanel1.Controls.Add(this.nextLine_textBox);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 247);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(747, 32);
			this.flowLayoutPanel1.TabIndex = 6;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// pc_label
			// 
			this.pc_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.pc_label.AutoSize = true;
			this.pc_label.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pc_label.Location = new System.Drawing.Point(3, 6);
			this.pc_label.Name = "pc_label";
			this.pc_label.Size = new System.Drawing.Size(54, 19);
			this.pc_label.TabIndex = 0;
			this.pc_label.Text = "$0000";
			// 
			// run_button
			// 
			this.run_button.AutoSize = true;
			this.run_button.Location = new System.Drawing.Point(94, 595);
			this.run_button.Name = "run_button";
			this.run_button.Size = new System.Drawing.Size(75, 30);
			this.run_button.TabIndex = 7;
			this.run_button.Text = "Run";
			this.run_button.UseVisualStyleBackColor = true;
			this.run_button.Click += new System.EventHandler(this.Run_button_Click);
			// 
			// reset_button
			// 
			this.reset_button.AutoSize = true;
			this.reset_button.Location = new System.Drawing.Point(212, 599);
			this.reset_button.Name = "reset_button";
			this.reset_button.Size = new System.Drawing.Size(75, 30);
			this.reset_button.TabIndex = 9;
			this.reset_button.Text = "Reset";
			this.reset_button.UseVisualStyleBackColor = true;
			this.reset_button.Click += new System.EventHandler(this.Reset_button_Click);
			// 
			// memoryViewer
			// 
			this.memoryViewer.AutoScroll = true;
			this.memoryViewer.AutoScrollMinSize = new System.Drawing.Size(27, 0);
			this.memoryViewer.Location = new System.Drawing.Point(759, 12);
			this.memoryViewer.Name = "memoryViewer";
			this.memoryViewer.Size = new System.Drawing.Size(658, 613);
			this.memoryViewer.TabIndex = 8;
			// 
			// MiNesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(1687, 905);
			this.Controls.Add(this.reset_button);
			this.Controls.Add(this.memoryViewer);
			this.Controls.Add(this.run_button);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.code_listBox);
			this.Controls.Add(this.nextLine_button);
			this.Controls.Add(this.cpu_groupBox);
			this.Name = "MiNesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MiNes Emulator";
			this.cpu_groupBox.ResumeLayout(false);
			this.cpu_groupBox.PerformLayout();
			this.cpuStatus_flowLayoutPanel.ResumeLayout(false);
			this.cpuStatus_flowLayoutPanel.PerformLayout();
			this.cpu_flowLayoutPanel.ResumeLayout(false);
			this.cpu_flowLayoutPanel.PerformLayout();
			this.acc_flowLayoutPanel.ResumeLayout(false);
			this.acc_flowLayoutPanel.PerformLayout();
			this.flag_groupBox.ResumeLayout(false);
			this.flags_flowLayoutPanel.ResumeLayout(false);
			this.flags_flowLayoutPanel.PerformLayout();
			this.flags_tableLayoutPanel.ResumeLayout(false);
			this.flags_tableLayoutPanel.PerformLayout();
			this.ps_flowLayoutPanel.ResumeLayout(false);
			this.ps_flowLayoutPanel.PerformLayout();
			this.stack_groupBox.ResumeLayout(false);
			this.stack_groupBox.PerformLayout();
			this.stack_flowLayoutPanel.ResumeLayout(false);
			this.stack_flowLayoutPanel.PerformLayout();
			this.sp_flowLayoutPanel.ResumeLayout(false);
			this.sp_flowLayoutPanel.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox cpu_groupBox;
		private System.Windows.Forms.FlowLayoutPanel acc_flowLayoutPanel;
		private System.Windows.Forms.Label label1;
		private AtomosZ.DragonAid.Libraries.NumberBox a_numberBox;
		private System.Windows.Forms.Label label2;
		private AtomosZ.DragonAid.Libraries.NumberBox x_numberBox;
		private System.Windows.Forms.Label label3;
		private AtomosZ.DragonAid.Libraries.NumberBox y_numberBox;
		private System.Windows.Forms.Label label4;
		private AtomosZ.DragonAid.Libraries.NumberBox pc_numberBox;
		private System.Windows.Forms.Label label5;
		private AtomosZ.DragonAid.Libraries.NumberBox cycles_numberBox;
		private System.Windows.Forms.FlowLayoutPanel cpu_flowLayoutPanel;
		private System.Windows.Forms.GroupBox flag_groupBox;
		private System.Windows.Forms.FlowLayoutPanel flags_flowLayoutPanel;
		private System.Windows.Forms.TableLayoutPanel flags_tableLayoutPanel;
		private System.Windows.Forms.FlowLayoutPanel ps_flowLayoutPanel;
		private System.Windows.Forms.Label label6;
		private AtomosZ.DragonAid.Libraries.NumberBox ps_numberBox;
		private System.Windows.Forms.CheckBox negative_checkBox;
		private System.Windows.Forms.CheckBox overflow_checkBox;
		private System.Windows.Forms.CheckBox reserved_checkBox;
		private System.Windows.Forms.CheckBox break_checkBox;
		private System.Windows.Forms.CheckBox unused_checkBox;
		private System.Windows.Forms.CheckBox interrupt_checkBox;
		private System.Windows.Forms.CheckBox zero_checkBox;
		private System.Windows.Forms.CheckBox carry_checkBox;
		private System.Windows.Forms.GroupBox stack_groupBox;
		private System.Windows.Forms.FlowLayoutPanel stack_flowLayoutPanel;
		private System.Windows.Forms.FlowLayoutPanel sp_flowLayoutPanel;
		private System.Windows.Forms.Label label7;
		private AtomosZ.DragonAid.Libraries.NumberBox sp_numberBox;
		private System.Windows.Forms.TextBox stack_textBox;
		private System.Windows.Forms.FlowLayoutPanel cpuStatus_flowLayoutPanel;
		private System.Windows.Forms.Button nextLine_button;
		private System.Windows.Forms.ListBox code_listBox;
		private System.Windows.Forms.TextBox nextLine_textBox;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label pc_label;
		private System.Windows.Forms.Button run_button;
		private MemoryViewer memoryViewer;
		private System.Windows.Forms.Button reset_button;
	}
}

