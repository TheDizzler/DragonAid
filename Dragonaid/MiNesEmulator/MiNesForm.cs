using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.ASM;
using AtomosZ.MiNesEmulator.Compiler;
using AtomosZ.MiNesEmulator.CPU2A03;
using static AtomosZ.MiNesEmulator.CPU2A03.VirtualCPU;

namespace AtomosZ.MiNesEmulator
{
	public partial class MiNesForm : Form, UserControlParent
	{
		private VirtualCPU cpu;
		private ControlUnit6502 cu;
		private byte[] byteStream;

		private string asmFilepath = @"D:\github\RomHacking\Working ROMs\ROM writing\unit test code.asm";
		private List<VirtualCPU.VirtualInstruction> allInstructions;


		public MiNesForm()
		{
			InitializeComponent();

			cpu = new VirtualCPU();
			cu = cpu.controlUnit;

			ram_memoryScrollView.Initialize(0x8000);
			rom_memoryScrollView.Initialize(0x8000, 0x8000);



			AssembleAndLoadRom();

			// start from RESET_IRQ
			
			var first = cpu.PeekNextInstruction();
			var vInstr = new VirtualInstruction();
			vInstr.instruction = first;
			vInstr.state = cpu.GetCurrentState();

			var instrAndBranchPointers =
				cpu.RunInstructionBlockVirtually(vInstr.state);

			allInstructions = instrAndBranchPointers;

			var imageList = new ImageList();
			Image img = (Image)Properties.Resources.Arrow;
			imageList.Images.Add("Selected", img);
			code_listView.StateImageList = imageList;

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
			cycles_numberBox.Value = cu.cycleCount;
			sp_numberBox.Value = cu.sp;
			ps_numberBox.Value = cu.ps;

			stack_textBox.Text = "";
			for (int i = cu.sp + 1; i <= 0xFF; ++i)
			{
				var b = cpu.memory[0x100 + i];
				stack_textBox.Text += b.ToString("X2") + ", ";
			}

			carry_checkBox.Checked = cu.carry;
			zero_checkBox.Checked = cu.zero;
			interrupt_checkBox.Checked = cu.interrupt;
			overflow_checkBox.Checked = cu.overflow;
			negative_checkBox.Checked = cu.negative;

			ram_memoryScrollView.SetMemory(cpu.memory[0, 0x8000]);
			rom_memoryScrollView.SetMemory(cpu.memory[0x8000, 0x8000]);


			pc_label.Text = "$" + cu.pc.ToString("X4");
			nextLine_textBox.Text = cpu.ViewNextInstruction();
		}

		private void NextLine_button_Click(object sender, EventArgs e)
		{
			cpu.ParseAndRunNextInstruction();
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
