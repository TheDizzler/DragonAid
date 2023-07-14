using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.MiNesEmulator.Compiler;
using AtomosZ.MiNesEmulator.CPU2A03;
using AtomosZ.MiNesEmulator.PPU2C02;
using static AtomosZ.MiNesEmulator.CPU2A03.VirtualCPU;

namespace AtomosZ.MiNesEmulator
{
	public partial class MiNesForm : Form, UserControlParent
	{

		
		//private byte[] byteStream;

		private string asmFilepath = @"D:\github\RomHacking\Working ROMs\ROM writing\unit test code.asm";
		private List<VirtualCPU.VirtualInstruction> allInstructions;
		
		private MiNes miNes;
		private VirtualCPU cpu;

		public MiNesForm()
		{
			InitializeComponent();

			miNes = new MiNes();

			//AssembleAndLoadRom();
			miNes.LoadRom(@"D:\github\RomHacking\Working ROMs\Dragon Warrior 3 (U).nes");
			cpu = miNes.cpu;
			ram_memoryScrollView.Initialize(0x8000);
			rom_memoryScrollView.Initialize(0x8000, 0x8000);





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

			miNes.Start();
		}


		private void Code_listView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			//if (instructionCache != null && e.ItemIndex >= firstItem && e.ItemIndex < firstItem + instructionCache.Count)
			//{
			//	//A cache hit, so get the ListViewItem from the cache instead of making a new one.
			//	e.Item = instructionCache[e.ItemIndex - firstItem];
			//}
			//else
			{
				//A cache miss, so create a new ListViewItem and pass it back.
				var vInstr = allInstructions[e.ItemIndex];
				e.Item = new ListViewItem(vInstr.instruction.address.ToString("X4"));
				e.Item.SubItems.Add(allInstructions[e.ItemIndex].instruction.byteCode);
				e.Item.SubItems.Add(cpu.ViewNextInstruction(vInstr.instruction));
				e.Item.SubItems[1].BackColor = SystemColors.ControlLight;
				e.Item.SubItems[2].BackColor = SystemColors.Menu;
				e.Item.UseItemStyleForSubItems = false;
				e.Item.StateImageIndex = 0;

				//instructionCache.Add(e.Item);
			}
		}


		private void AssembleAndLoadRom()
		{
			var byteCode = ASMCompiler.Compile(asmFilepath);
			string newFile = asmFilepath.Replace(".asm", ".nes");
			File.WriteAllBytes(newFile, byteCode);
			miNes.LoadRom(newFile);
		}

		private void UpdateDisplay()
		{
			ControlUnit6502 controlUnit = miNes.controlUnit;
			a_numberBox.Value = controlUnit.a;
			x_numberBox.Value = controlUnit.x;
			y_numberBox.Value = controlUnit.y;
			pc_numberBox.Value = controlUnit.pc;
			cycles_numberBox.Value = controlUnit.cycleCount;
			sp_numberBox.Value = controlUnit.sp;
			ps_numberBox.Value = controlUnit.ps;

			stack_textBox.Text = "";
			for (int i = controlUnit.sp + 1; i <= 0xFF; ++i)
			{
				var b = miNes.Memory(0x100 + i);
				stack_textBox.Text += b.ToString("X2") + ", ";
			}

			carry_checkBox.Checked = controlUnit.carry;
			zero_checkBox.Checked = controlUnit.zero;
			interrupt_checkBox.Checked = controlUnit.interrupt;
			overflow_checkBox.Checked = controlUnit.overflow;
			negative_checkBox.Checked = controlUnit.negative;

			ram_memoryScrollView.SetMemory(miNes.Memory(0, 0x2000));
			rom_memoryScrollView.SetMemory(miNes.Memory(0x8000, 0x8000));


			pc_label.Text = "$" + controlUnit.pc.ToString("X4");
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
			miNes.Reset();
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

		private void MiNesForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			miNes.Stop();
		}
	}
}
