using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AtomosZ.Dragonaid.ItemEditor;
using static AtomosZ.DragonAid.MonsterEditor.MonsterConsts;

namespace AtomosZ.DragonAid.MonsterEditor
{
	public partial class MonsterEditorView : UserControl
	{
		private List<Label> actionChanceLabels;
		private List<ComboBox> actionComboBoxes;
		private byte[] data;
		private MonsterStatBlock statBlock;
		private List<ComboBox> resistanceComboBoxes;

		public MonsterEditorView()
		{
			InitializeComponent();

			aiType_comboBox.DataSource = Enum.GetValues(typeof(AIType));

			actionChanceLabels = new List<Label>()
			{
				chance0_label, chance1_label, chance2_label, chance3_label,
				chance4_label, chance5_label, chance6_label, chance7_label,
			};

			actionComboBoxes = new List<ComboBox>()
			{
				action1_comboBox, action2_comboBox, action3_comboBox, action4_comboBox,
				action5_comboBox, action6_comboBox, action7_comboBox, action0_comboBox,
			};

			foreach (var combobox in actionComboBoxes)
			{
				var bs = new BindingSource();
				bs.DataSource = MonsterConsts.actions;
				combobox.DataSource = bs;
			}

			resistanceComboBoxes = new List<ComboBox>()
			{
				fireResitance_comboBox, iceResitance_comboBox, windResitance_comboBox,
				lightningResitance_comboBox, beat_comboBox, sacrifice_comboBox,
				sleep_comboBox, stopspell_comboBox, sap_comboBox,
				surround_comboBox, robMagic_comboBox, chaos_comboBox,
				limbo_comboBox, expel_comboBox,
			};

			foreach (var combobox in resistanceComboBoxes)
			{
				var bs = new BindingSource();
				bs.DataSource = MonsterConsts.resistanceDescriptions;
				combobox.DataSource = bs;
			}

			itemDrop_comboBox.DataSource = ItemConsts.itemBasicList;
			itemDropChance_comboBox.DataSource = MonsterConsts.itemDropChances;
		}

		public void LoadMonsterStats(byte[] byteData, int monsterIndex)
		{
			data = byteData;
			statBlock = new MonsterStatBlock(byteData, 0);

			index_label.Text = statBlock.index.ToString("X2") + ":";
			level_spinner.Value = statBlock.level;
			evade_spinner.Value = statBlock.evade;
			exp_spinner.Value = statBlock.exp;

			agi_spinner.Value = statBlock.agility;
			gold_spinner.Value = statBlock.gold;
			atk_spinner.Value = statBlock.attackPower;
			def_spinner.Value = statBlock.defensePower;
			hp_spinner.Value = statBlock.hp;
			mp_spinner.Value = statBlock.mp;

			for (int i = 0; i < actionComboBoxes.Count; ++i)
			{
				actionComboBoxes[i].SelectedIndex = statBlock.actions[i];
			}

			aiType_comboBox.SelectedIndex = statBlock.aiType;

			actionChanceType_spinner.Value = statBlock.actionChance;
			ActionChanceType_spinner_ValueChanged(null, null);

			actionCount_Spinner.Value = statBlock.actionCountType;
			ActionCount_Spinner_ValueChanged(null, null);

			regen_spinner.Value = statBlock.regeneration;
			Regen_spinner_ValueChanged(null, null);

			for (int i = 0; i < resistanceComboBoxes.Count; ++i)
			{
				resistanceComboBoxes[i].SelectedIndex = statBlock.resistances[i];
			}

			focusFire_checkBox.Checked = statBlock.focusFire;

			itemDrop_comboBox.SelectedIndex = statBlock.itemDrop;
			itemDropChance_comboBox.SelectedIndex = statBlock.itemDropChance;
		}

		private void Regen_spinner_ValueChanged(object sender, EventArgs e)
		{
			statBlock.regeneration = (int) regen_spinner.Value;
			switch (statBlock.regeneration)
			{
				case 0:
					regen_label.Text = "No regeneration";
					break;
				default:
					int i = statBlock.regeneration - 1;
					i = i << 1;
					int baseHP = data[RegenerationAddress + 0x10 + i];
					int range = data[RegenerationAddress + 0x10 + i + 1] - 1;
					regen_label.Text = $"{baseHP}-{baseHP + range} HP/turn";
					break;
			}
		}

		private void AiType_comboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (statBlock == null)
				return;
			statBlock.aiType = aiType_comboBox.SelectedIndex;
			aiDesc_label.Text = aiDescriptions[statBlock.aiType];
			ActionCount_Spinner_ValueChanged(null, null);
		}

		private void ActionChanceType_spinner_ValueChanged(object sender, EventArgs e)
		{
			int chanceType = (int) actionChanceType_spinner.Value;

			statBlock.actionChance = chanceType;
			if (chanceType == 3)
				chanceOrSeq0_label.Text = chanceOrSeq1_label.Text = "Order";
			else
				chanceOrSeq0_label.Text = chanceOrSeq1_label.Text = "%";

			switch (chanceType)
			{
				case 0:
					actionChanceDesc_label.Text = "Equal chance for all actions";
					for (int i = 0; i < actionChanceLabels.Count; ++i)
						actionChanceLabels[i].Text = "12%";
					break;

				case 1:
					actionChanceDesc_label.Text = "Type 1";
					for (int i = 0; i < actionChanceLabels.Count; ++i)
						actionChanceLabels[i].Text = (data[ActionChancesType1Address + 0x10 + i] / 2.56f).ToString("0.00") + "%";
					break;

				case 2:
					actionChanceDesc_label.Text = "Type 2";
					for (int i = 0; i < actionChanceLabels.Count; ++i)
						actionChanceLabels[i].Text = (data[ActionChancesType2Address + 0x10 + i] / 2.56f).ToString("0.00") + "%";
					break;

				case 3:
					actionChanceDesc_label.Text = "Fixed sequence";
					for (int i = 0; i < actionChanceLabels.Count; ++i)
						actionChanceLabels[i].Text = $"{i + 1}";
					break;
			}
		}

		private void ActionCount_Spinner_ValueChanged(object sender, EventArgs e)
		{
			int actionCount = (int) actionCount_Spinner.Value;
			statBlock.actionCountType = actionCount;

			switch (actionCount)
			{
				case 0:
					actionCount_label.Text = "1";
					break;

				case 1:
					actionCount_label.Text = "1-2";
					break;

				case 2:
					if (statBlock.aiType == 2)
						actionCount_label.Text = "1-2";
					else
						actionCount_label.Text = "2";
					break;

				case 3:
					if (statBlock.aiType == 2)
						actionCount_label.Text = "2";
					else
						actionCount_label.Text = "1-3";
					break;
			}
		}
	}
}
