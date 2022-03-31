using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using AtomosZ.Dragonaid.Libraries;
using AtomosZ.Dragonaid.PointerAid;

using Newtonsoft.Json;

using static AtomosZ.Dragonaid.Libraries.DynamicSubroutine;
using static AtomosZ.Dragonaid.PointerAid.PointerAidSettingsData;

namespace PointerAid
{
	public partial class PointerAidForm : Form, UserControlParent
	{
		private PointerAidSettingsData pointerAid;
		private byte[] gameData;

		private DynamicPointerData pointerData;

		public PointerAidForm()
		{
			InitializeComponent();

			if (!File.Exists(pointerAidFormUserSettings))
			{
				while (!InitializeUserSettings())
				{ }

				ExtractDynamicPointers(gameData);
			}
			else
			{
				pointerAid = JsonConvert.DeserializeObject<PointerAidSettingsData>(
					File.ReadAllText(pointerAidFormUserSettings));
				gameData = File.ReadAllBytes(pointerAid.romFile);
				if (string.IsNullOrEmpty(pointerAid.dynamicPointersJsonFile))
				{
					ExtractDynamicPointers(gameData);
				}
				else
				{
					pointerData = JsonConvert.DeserializeObject<DynamicPointerData>(
						File.ReadAllText(pointerAid.dynamicPointersJsonFile));
				}
			}

			Initialize();
		}

		private void Initialize()
		{

			BindingSource source = new BindingSource();
			source.DataSource = pointerData.subroutines17;
			pointer17_listBox.DataSource = source;

			source = new BindingSource();
			source.DataSource = pointerData.subroutines07;
			pointer07_listBox.DataSource = source;
		}

		private bool InitializeUserSettings()
		{
			FileDialog fileDialog = new OpenFileDialog();
			fileDialog.Filter = "iNES files (*.nes)|*.nes";
			fileDialog.CheckFileExists = true;

			var result = fileDialog.ShowDialog();
			if (result == DialogResult.Cancel)
			{
				Environment.Exit(0);
			}
			else if (result == DialogResult.OK)
			{
				var file = fileDialog.FileName;
				gameData = File.ReadAllBytes(file);
				if (gameData[0] != 0x4E && gameData[1] != 0x45 && gameData[2] != 0x53 && gameData[3] != 0x1A)
				{
					MessageBox.Show("Invalid iNES file");
					return false;
				}

				pointerAid = new PointerAidSettingsData();
				pointerAid.romFile = file;

				File.WriteAllText(pointerAidFormUserSettings,
					JsonConvert.SerializeObject(pointerAid, Formatting.Indented));
			}

			return true;
		}


		public void Defocus(object sender, EventArgs e)
		{
			this.ActiveControl = null;
		}

		public void UpdateView()
		{
			if (subroutine_tabControl.SelectedIndex == 0)
			{
				var index = pointer07_listBox.SelectedIndex;
				BindingSource source = new BindingSource();
				source.DataSource = pointer07_listBox.DataSource;
				pointer07_listBox.DataSource = source;
				pointer07_listBox.SelectedIndex = index;
			}
			else
			{
				var index = pointer17_listBox.SelectedIndex;
				BindingSource source = new BindingSource();
				source.DataSource = pointer17_listBox.DataSource;
				pointer17_listBox.DataSource = source;
				pointer17_listBox.SelectedIndex = index;
			}
		}

		/// <summary>
		/// Used to extract data from ROM.
		/// </summary>
		/// <param name="data"></param>
		private void ExtractDynamicPointers(byte[] data)
		{
			var subroutines07 = new List<DynamicSubroutine>();
			var subroutines17 = new List<DynamicSubroutine>();
			for (int i = 0; i < Load07PointerIndices.length; ++i)
			{
				if (i == 145)
					Console.WriteLine("");
				byte index = data[DynamicSubroutine.Load07PointerIndices.offset + i];
				byte bankId = data[DynamicSubroutine.Load07BankIds.offset + (i / 2)];
				int bank;
				if (i % 2 == 0)
					bank = bankId >> 4;
				else
					bank = bankId & 0x0F;

				Address bankPointer = DynamicSubroutineBankPointers[bank];
				if (bankPointer.pointer == -1)
				{
					Debug.WriteLine($"No Addresses for {i.ToString("X2")} => {bankPointer.name}");
					continue;
				}

				int subroutine = data[bankPointer.offset + index * 2]
					+ (data[bankPointer.offset + (index * 2) + 1] << 8);
				//Debug.WriteLine($"ID: {i.ToString("X2")} BankId: {bank.ToString("X2")} Bank: {bankPointer.name} Index: {index.ToString("X2")} DS: ${subroutine.ToString("X2")}");

				DynamicSubroutine ds = new DynamicSubroutine()
				{
					code = i,
					bankId = bank,
					dsIndex = index,
					loader = DynamicLoader.Load07,
					address = subroutine,
					prgAddress = new Address("Full address", Math.Max(0, subroutine - 0x8000) + bankPointer.pointer, 2),
					notes = bankPointer.name,
				};

				subroutines07.Add(ds);
			}


			for (int i = 0; i < Load17PointerIndices.length; ++i)
			{
				byte index = data[DynamicSubroutine.Load17PointerIndices.offset + i];
				byte bankId = data[DynamicSubroutine.Load17BankIds.offset + (i / 2)];
				int bank;
				if (i % 2 == 0)
					bank = bankId >> 4;
				else
					bank = bankId & 0x0F;

				Address bankPointer = DynamicSubroutineBankPointers[bank];
				if (bankPointer.pointer == -1)
				{
					Debug.WriteLine($"No Addresses for {i.ToString("X2")} => {bankPointer.name}");
					continue;
				}

				int subroutine = data[bankPointer.offset + index * 2]
					+ (data[bankPointer.offset + (index * 2) + 1] << 8);
				//Debug.WriteLine($"ID: {i.ToString("X2")} BankId: {bank.ToString("X2")} Bank: {bankPointer.name} Index: {index.ToString("X2")} DS: ${subroutine.ToString("X2")}");

				DynamicSubroutine ds = new DynamicSubroutine()
				{
					code = i,
					bankId = bank,
					dsIndex = index,
					loader = DynamicLoader.Load17,
					address = subroutine,
					prgAddress = new Address("Full address", Math.Max(0, subroutine - 0x8000) + bankPointer.pointer, 2),
					notes = bankPointer.name,
				};

				subroutines17.Add(ds);
			}

			pointerData = new DynamicPointerData();
			pointerData.subroutines07 = subroutines07;
			pointerData.subroutines17 = subroutines17;
		}


		private void Pointer07_listBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			addressView.LoadDynamicSubroutine((DynamicSubroutine)pointer07_listBox.SelectedItem);
		}

		private void Pointer17_listBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			addressView.LoadDynamicSubroutine((DynamicSubroutine)pointer17_listBox.SelectedItem);
		}

		private void Subroutine_tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (subroutine_tabControl.SelectedIndex == 0)
				addressView.LoadDynamicSubroutine((DynamicSubroutine)pointer07_listBox.SelectedItem);
			else
				addressView.LoadDynamicSubroutine((DynamicSubroutine)pointer17_listBox.SelectedItem);
		}

		private void RestoreFromROM_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ExtractDynamicPointers(gameData);
			//dgdfg
		}

		private void SaveAs_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!ChooseSaveFile())
				return;

			SaveData();
		}

		public void Save(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(pointerAid.dynamicPointersJsonFile))
			{
				if (!ChooseSaveFile())
				{
					MessageBox.Show("Must choose a file");
					return;
				}
			}

			SaveData();
		}

		private void SaveData()
		{
			File.WriteAllText(pointerAid.dynamicPointersJsonFile,
					JsonConvert.SerializeObject(pointerData, Formatting.Indented));

			File.WriteAllText(pointerAidFormUserSettings,
				JsonConvert.SerializeObject(pointerAid, Formatting.Indented));
		}

		private bool ChooseSaveFile()
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.AddExtension = true;
			dialog.DefaultExt = "*.da";
			dialog.Filter = "DragonAid file (*.da)|*.da";

			var result = dialog.ShowDialog();
			if (result != DialogResult.OK)
			{
				return false;
			}

			pointerAid.dynamicPointersJsonFile = dialog.FileName;

			return true;
		}


		private void Load_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.AddExtension = true;
			dialog.DefaultExt = "*.da";
			dialog.Filter = "DragonAid file (*.da)|*.da";

			var result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				pointerAid.dynamicPointersJsonFile = dialog.FileName;
				pointerData = JsonConvert.DeserializeObject<DynamicPointerData>(
						File.ReadAllText(pointerAid.dynamicPointersJsonFile));
				Initialize();
			}
		}
	}
}
