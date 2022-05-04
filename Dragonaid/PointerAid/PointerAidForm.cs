﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using AtomosZ.DragonAid.Libraries;
using Newtonsoft.Json;
using static AtomosZ.DragonAid.Libraries.DynamicSubroutine;

namespace AtomosZ.DragonAid.PointerAid
{
	public partial class PointerAidForm : Form, UserControlParent
	{
		private PointerAidSettingsData pointerAidUserSettings;
		private byte[] gameData;

		private DynamicPointerData pointerData;

		public PointerAidForm()
		{
			InitializeComponent();

			if (!File.Exists(AidUserSettings.pointerAidFormUserSettingsFile))
			{
				while (!AidUserSettings.InitializeUserSettings(out pointerAidUserSettings))
				{ }

				ExtractDynamicPointers(gameData);
			}
			else
			{
				pointerAidUserSettings = JsonConvert.DeserializeObject<PointerAidSettingsData>(
					File.ReadAllText(AidUserSettings.pointerAidFormUserSettingsFile));
				gameData = File.ReadAllBytes(pointerAidUserSettings.romFile);
				if (string.IsNullOrEmpty(pointerAidUserSettings.dynamicPointersJsonFile))
					ExtractDynamicPointers(gameData);
				else
				{
					if (!File.Exists(pointerAidUserSettings.dynamicPointersJsonFile))
						PointerFileNotFound();
					else
						pointerData = JsonConvert.DeserializeObject<DynamicPointerData>(
							File.ReadAllText(pointerAidUserSettings.dynamicPointersJsonFile));
				}
			}

			Initialize();
		}

		private void PointerFileNotFound()
		{
			var result = MessageBox.Show("Yes: Find Pointer file manually.\nNo: Extract pointers from ROM.\nCancel: Exit program.",
							"Could not find dynamic pointer file", MessageBoxButtons.YesNoCancel);
			switch (result)
			{
				case DialogResult.Cancel:
					Environment.Exit(0);
					break;
				case DialogResult.Yes:
					if (!FindPointerFile())
						PointerFileNotFound();
					break;
				case DialogResult.No:
					RestoreFromROM_ToolStripMenuItem_Click(null, null);
					break;
			}
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


		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.Control | Keys.S))
			{
				Save(null, null);
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
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

			SaveNeeded(true);
		}


		private void SaveNeeded(bool needed)
		{
			saveStatus_label.Visible = true;
			saveStatus_label.Text = (needed ? "Not " : "") + "Saved";
			this.Text = "PointerAid" + (needed ? "*" : "");
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
			Initialize();

			this.Text = "PointerAid*";
			SaveNeeded(true);
		}


		private void SaveAs_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!ChooseSaveFile())
				return;

			SaveData();
		}

		public void Save(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(pointerAidUserSettings.dynamicPointersJsonFile))
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
			File.WriteAllText(pointerAidUserSettings.dynamicPointersJsonFile,
					JsonConvert.SerializeObject(pointerData, Formatting.Indented));

			File.WriteAllText(AidUserSettings.pointerAidFormUserSettingsFile,
				JsonConvert.SerializeObject(pointerAidUserSettings, Formatting.Indented));

			SaveNeeded(false);
		}

		private bool ChooseSaveFile()
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.AddExtension = true;
			dialog.DefaultExt = "*" + AidUserSettings.pointerAidExtension;
			dialog.Filter = $"DragonAid Pointer file (dialog.DefaultExt)|{dialog.DefaultExt}";

			var result = dialog.ShowDialog();
			if (result != DialogResult.OK)
			{
				return false;
			}

			pointerAidUserSettings.dynamicPointersJsonFile = dialog.FileName;

			return true;
		}


		private void Load_ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FindPointerFile();
		}

		private bool FindPointerFile()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.AddExtension = true;
			dialog.DefaultExt = "*" + AidUserSettings.pointerAidExtension;
			dialog.Filter = $"DragonAid Pointer file (dialog.DefaultExt)|{dialog.DefaultExt}";

			var result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				pointerAidUserSettings.dynamicPointersJsonFile = dialog.FileName;
				pointerData = JsonConvert.DeserializeObject<DynamicPointerData>(
						File.ReadAllText(pointerAidUserSettings.dynamicPointersJsonFile));
				Initialize();

				SaveNeeded(false);

				AidUserSettings.SaveUserSettings(pointerAidUserSettings);
				return true;
			}

			return false;
		}

		private void SearchAddress_spinner_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				SearchAddr_button_Click(null, null);
				e.Handled = true;
				e.SuppressKeyPress = true;
				addressView.Defocus_Click(null, null);
			}
		}

		private void SearchAddr_button_Click(object sender, EventArgs e)
		{
			var find = (int)searchAddress_spinner.Value;
			var results = pointerData.FindAddress(find);
			if (results.Count == 1)
			{
				ShowSearchResult(results[0]);
			}
			else if (results.Count > 1)
			{
				var searchBox = new SearchMessageBox();
				searchBox.AddResults(results);
				var result = searchBox.ShowDialog();
				if (result == DialogResult.OK)
				{
					ShowSearchResult(searchBox.selectedSubroutine);
				}
			}
		}

		private void ShowSearchResult(DynamicSubroutine dynamicSubroutine)
		{
			if (dynamicSubroutine.loader == DynamicLoader.Load07)
			{
				subroutine_tabControl.SelectedIndex = 0;
				pointer07_listBox.SelectedItem = dynamicSubroutine;
			}
			else
			{
				subroutine_tabControl.SelectedIndex = 1;
				pointer17_listBox.SelectedItem = dynamicSubroutine;
			}
		}
	}
}
