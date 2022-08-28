﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.ASM;
using AtomosZ.MiNesEmulator.Compiler;


namespace AtomosZ.MiNesEmulator
{
	public partial class MiNesForm : Form, UserControlParent
	{
		private CPU cpu;
		private CPU.ControlUnit cu;
		private byte[] byteStream;

		private string asmFilepath = @"D:\github\RomHacking\Working ROMs\ROM writing\unit test code.asm";
		private List<Instruction> allInstructions;

		public MiNesForm()
		{
			InitializeComponent();

			cpu = new CPU();
			cu = cpu.controlUnit;

			ram_memoryScrollView.Initialize(0x8000);
			rom_memoryScrollView.Initialize(0x8000, 0x8000);



			AssembleAndLoadRom();

			// start from RESET_IRQ
			var instrAndBranchPointers = cpu.GetInstructionBlock(cpu.GetPointerAt(UniversalConsts.RESET_Pointer));

			allInstructions =  instrAndBranchPointers.Item1; 

			code_listView.RetrieveVirtualItem += Code_listView_RetrieveVirtualItem;
			code_listView.VirtualListSize = allInstructions.Count;

			UpdateDisplay();


			Defocus(null, null);
		}

		private void Code_listView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			//{
			//}
				e.Item = new ListViewItem();
				var instr = allInstructions[e.ItemIndex];
				e.Item.SubItems.Add(instr.address.ToString("X4"));
				e.Item.SubItems.Add(allInstructions[e.ItemIndex].byteCode);
				e.Item.SubItems.Add(acc.PeekNextInstruction(instr));
				e.Item.SubItems[1].BackColor = SystemColors.ControlLight;
				e.Item.SubItems[2].BackColor = SystemColors.Menu;
				e.Item.UseItemStyleForSubItems = false;
		}


		private void AssembleAndLoadRom()
		{
			var byteCode = ASMCompiler.Compile(asmFilepath);
			File.WriteAllBytes(asmFilepath.Replace(".asm", ".nes"), byteCode);
			cpu.LoadRom(byteCode);
		}

		private void UpdateDisplay()
		{
			a_numberBox.Value = cu.a;
			x_numberBox.Value = cu.x;
			y_numberBox.Value = cu.y;
			pc_numberBox.Value = cu.pc;
			sp_numberBox.Value = cu.sp;
			ps_numberBox.Value = cu.ps;

			stack_textBox.Text = "";
			for (int i = cu.sp + 1; i <= 0xFF; ++i)
			{
				var s = cpu.stack;
				var b = s[i];
				stack_textBox.Text += b.ToString("X2") + ", ";
			}

			carry_checkBox.Checked = cu.carry;
			zero_checkBox.Checked = cu.zero;
			interrupt_checkBox.Checked = cu.interrupt;
			overflow_checkBox.Checked = cu.overflow;
			negative_checkBox.Checked = cu.negative;

			ram_memoryScrollView.SetMemory(cpu[0, 0x8000]);
			rom_memoryScrollView.SetMemory(cpu[0x8000, 0x8000]);


			pc_label.Text = "$" + cu.pc.ToString("X4");
			nextLine_textBox.Text = cpu.ViewNextInstruction();


		}

		private void NextLine_button_Click(object sender, EventArgs e)
		{
			cpu.ParseNextInstruction();
			UpdateDisplay();
		}

		private void Run_button_Click(object sender, EventArgs e)
		{

		}

		private void Reset_button_Click(object sender, EventArgs e)
		{
			cpu.Reset();
			AssembleAndLoadRom();
			UpdateDisplay();
		}



		public void Defocus(object sender, EventArgs e)
		{
			this.ActiveControl = null;
		}

		public void Save(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		public void UpdateView()
		{
			throw new NotImplementedException();
		}
	}
}
