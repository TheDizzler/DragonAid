using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AtomosZ.DragonAid.Libraries;
using static AtomosZ.DragonAid.Libraries.Opcode;
using AtomosZ.DragonAid.Libraries.ASM;

namespace AtomosZ.MiNesEmulator
{
	public partial class MiNesForm : Form
	{
		private CPU cpu;
		private CPU.ControlUnit acc;
		private byte[] byteStream;

		private string asmFilepath = @"D:\github\RomHacking\Working ROMs\ROM writing\unit test code.asm";


		private Dictionary<string, int> labels = new Dictionary<string, int>()
		{
			["NMI"] = 0xFFDF,
			["Reset"] = 0x8010,
		};

		public MiNesForm()
		{
			InitializeComponent();

			cpu = new CPU();
			acc = cpu.cu;

			FillCPU();
			UpdateDisplay();
		}

		private void FillCPU()
		{
			//var code = File.ReadLines(asmFilepath).ToArray();

			//var bList = new Dictionary<int, byte[]>();
			//for (int i = 0; i < code.Length; ++i)
			//{
			//	if (string.IsNullOrEmpty(code[i]))
			//		continue;
			//	if (!code[i].StartsWith("."))
			//		throw new Exception($"Fix yo shit; {code[i]}");

			//	int addr;
			//	if (char.IsLetter(code[i][1]))
			//	{
			//		var lbl = code[i].Substring(1);
			//		addr = labels[lbl];
			//		if (lbl.ToUpper() == "RESET")
			//			cpu[UniversalConsts.RESET_Pointer] = new byte[] { (byte)(addr), (byte)(addr >> 8) };
			//		else if (lbl.ToUpper() == "NMI")
			//			cpu[UniversalConsts.NMI_Pointer] = new byte[] { (byte)(addr), (byte)(addr >> 8) };
			//		else if (lbl.ToUpper() == "IRQ")
			//			cpu[UniversalConsts.IRQBRK_Pointer] = new byte[] { (byte)(addr), (byte)(addr >> 8) };
			//	}
			//	else
			//		addr = int.Parse(code[i].Substring(1), NumberStyles.HexNumber, CultureInfo.CurrentCulture);

			//	var bStream = new List<byte>();
			//	while (!code[++i].StartsWith(".") && !string.IsNullOrEmpty(code[i]))
			//		bStream.AddRange(ParseByteStream(code[i]));

			//	bList[addr] = bStream.ToArray();
			//}

			//foreach (var bStream in bList)
			//{
			//	cpu[bStream.Key] = bStream.Value.ToArray();
			//}

			var machineCode = ASMCompiler.Compile(asmFilepath);
			File.WriteAllBytes(asmFilepath.Replace(".asm", ".bin"), machineCode);

			byte[] bank = new byte[0x4000];
			Array.Copy(machineCode, 0x0010, bank, 0, 0x4000);
			cpu[0x8000] = bank;
			Array.Copy(machineCode, 0x4010, bank, 0, 0x4000);
			cpu[0xC000] = bank;
			cpu.Initialize();
		}

		private List<byte> ParseByteStream(string line)
		{
			var stream = new List<byte>();
			for (int c = 0; c < line.Length; ++c)
			{
				if (char.IsWhiteSpace(line[c]))
					continue;
				if (line[c] == '#')
					break;
				if (!byte.TryParse(line.Substring(c, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out byte b))
					throw new Exception($"Garbage in {line.Substring(c, 2)}, exception out");
				stream.Add(b);
				++c;
			}

			return stream;
		}

		private void UpdateDisplay()
		{
			a_numberBox.Value = acc.a;
			x_numberBox.Value = acc.x;
			y_numberBox.Value = acc.y;
			pc_numberBox.Value = acc.pc;
			sp_numberBox.Value = acc.sp;
			ps_numberBox.Value = acc.ps;

			stack_textBox.Text = "";
			for (int i = acc.sp + 1; i <= 0xFF; ++i)
			{
				var s = cpu.stack;
				var b = s[i];
				stack_textBox.Text += b.ToString("X2") + ", ";
			}

			carry_checkBox.Checked = acc.carry;
			zero_checkBox.Checked = acc.zero;
			interrupt_checkBox.Checked = acc.interrupt;
			overflow_checkBox.Checked = acc.overflow;
			negative_checkBox.Checked = acc.negative;

			memoryViewer.SetZeroPages(cpu.zeroPages);
			memoryViewer.SetStack(cpu.stack);

			pc_label.Text = "$" + acc.pc.ToString("X4");
			nextLine_textBox.Text = cpu.GetASM(acc.pc);


			//code_listBox.Items.Add();
		}

		private void NextLine_button_Click(object sender, EventArgs e)
		{
			acc.ParseNextInstruction();
			UpdateDisplay();
		}

		private void Run_button_Click(object sender, EventArgs e)
		{

		}

		private void Reset_button_Click(object sender, EventArgs e)
		{
			cpu.Reset();
			FillCPU();
			UpdateDisplay();
		}

		private void MemoryViewer_Resize(object sender, EventArgs e)
		{
			memoryViewer.SetSize();
		}

		private void MemoryViewer_SizeChanged(object sender, EventArgs e)
		{
			memoryViewer.SetSize();
		}

		private void MemoryViewer_AutoSizeChanged(object sender, EventArgs e)
		{
			memoryViewer.SetSize();
		}

	}
}
