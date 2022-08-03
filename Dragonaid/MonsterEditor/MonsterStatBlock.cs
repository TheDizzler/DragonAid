using System;

using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.Pointers;
using static AtomosZ.DragonAid.MonsterAid.MonsterConsts;

namespace AtomosZ.DragonAid.MonsterAid
{
	public class MonsterStatBlock
	{
		public int index;
		public string name;
		/// <summary>
		/// [Level]: Bits 0-5
		/// </summary>
		public int level;

		/// <summary>
		/// 4 bits, multiples of 4.
		/// [Evade1]: bits 6-7: bits 3-4 of evade rate (X,Y)
		/// [Evade2]: bit 7: bit 2 of evade rate (Z)
		/// 
		/// [$00] AND #$C0 >> 3 
		/// ORA
		/// [$01] AND #$80 >> 5
		/// 000X YZ00
		/// </summary>
		public int evade;

		/// <summary>
		/// 16 bits.
		/// high byte [Exp2] low byte [Exp1] 
		/// </summary>
		public int exp;

		/// <summary>
		/// 8 bits.
		/// [Agility]
		/// </summary>
		public int agility;

		/// <summary>
		/// 10 bits.
		/// [Gold1] + ([Gold2] AND #$03 << 8)
		/// </summary>
		public int gold;

		/// <summary>
		/// 10 bits.
		/// [Attack1] + [Attack2] AND #$03 left shift 8
		/// </summary>
		public int attackPower;

		/// <summary>
		/// 10 bits.
		/// [Defense1] + [Defense2] AND #$03 left shift 8
		/// </summary>
		public int defensePower;

		/// <summary>
		/// 10 bits.
		/// [HP1] + [HP2] AND 0x03 left shift 8 
		/// </summary>
		public int hp;

		/// <summary>
		/// 8 bits.
		/// [MP]
		/// 255 sets to infinity?
		/// </summary>
		public int mp;

		/// <summary>
		/// 7 bits.
		/// [ItemDrop] & #$7F
		/// </summary>
		public int itemDrop;

		/// <summary>
		/// 6  bits x 8.
		/// 
		/// [Action0] & 0x3F
		/// [Action1] & 0x3F
		/// etc...
		/// </summary>
		public int[] actions = new int[8];

		/// <summary>
		/// 2 bits added together == 3 selections.
		/// 
		/// [AISelector1] AND 0x80 right shift 7 + [AISelector2] AND 0x80 right shift 7
		/// </summary>
		public int aiType;

		/// <summary>
		/// 2 bits = 4 selections.
		/// Which one is the high byte?
		/// [ActionChance1] AND 0x80 right shift 7? | [ActionChance2] AND 0x80 right shift 6?
		/// </summary>
		public int actionChance;

		/// <summary>
		/// 2 bits = 4 selections.
		/// Which one is the high byte?
		/// [ActionCount1] AND 0x80 right shift 7? | [ActionCount2] AND 0x80 right shift 6?
		/// 
		/// For	AI type 2:		1, 1-2, 1-2, 2
		///	For other AI types:	1, 1-2, 2,   1-3
		/// </summary>
		public int actionCountType;

		/// <summary>
		/// 2 bits = 4 selections.
		/// Which one is the high byte?
		/// [Regeneration1] AND 0x80 right shift 7? | [Regeneration2] AND 0x80 right shift 6?
		/// Type 0: no regeneration
		///	Type 1: 16-23 HP/turn
		///	Type 2: 44-55 HP/turn
		///	Type 3: 90-109 HP/turn
		/// </summary>
		public int regeneration;

		public int[] resistances = new int[14];

		/// <summary>
		/// if set, the enemy continually attacks the same character until they die
		/// </summary>
		public bool focusFire;
		public int itemDropChance;

		/// <summary>
		/// Required for Json reconstruction.
		/// </summary>
		public MonsterStatBlock()
		{

		}

		public MonsterStatBlock(byte[] romData, byte monsterIndex)
		{
			index = monsterIndex;
			int monsterStart = ROMPointers.MonsterStatBlockAddress.offset + index * ROMPointers.MonsterStatBlockAddress.length;

			name = Names.GetMonsterName(romData, monsterIndex);
			level = romData[monsterStart + Level] & 0x3F;
			evade = romData[monsterStart + Evade0] & 0xC0;
			evade = evade >> 3;
			int evade2 = romData[monsterStart + Evade1] & 0x80;
			evade2 = evade2 >> 5;
			evade = evade | evade2;
			exp = (romData[monsterStart + Exp1] << 8) | romData[monsterStart + Exp0];

			agility = romData[monsterStart + Agility];

			gold = romData[monsterStart + Gold0] | ((romData[monsterStart + Gold1] & 0x03) << 8);

			attackPower = romData[monsterStart + Attack0] | (romData[monsterStart + Attack1] & 0x03) << 8;
			defensePower = romData[monsterStart + Defense0] | (romData[monsterStart + Defense1] & 0x03) << 8;
			hp = romData[monsterStart + HP0] | (romData[monsterStart + HP1] & 0x03) << 8;

			mp = romData[monsterStart + MP];

			itemDrop = romData[monsterStart + ItemDrop] & 0x7F;

			actions[0] = (romData[monsterStart + Action0] & 0x3F);
			actions[1] = (romData[monsterStart + Action1] & 0x3F);
			actions[2] = (romData[monsterStart + Action2] & 0x3F);
			actions[3] = (romData[monsterStart + Action3] & 0x3F);
			actions[4] = (romData[monsterStart + Action4] & 0x3F);
			actions[5] = (romData[monsterStart + Action5] & 0x3F);
			actions[6] = (romData[monsterStart + Action6] & 0x3F);
			actions[7] = (romData[monsterStart + Action7] & 0x3F);

			aiType = ((romData[monsterStart + AISelector0] & 0x80) >> 7)
				+ ((romData[monsterStart + AISelector1] & 0x80) >> 7);

			actionChance = ((romData[monsterStart + ActionChance0] & 0x80) >> 7)
				| ((romData[monsterStart + ActionChance1] & 0x80) >> 6);
			actionCountType = ((romData[monsterStart + ActionCount0] & 0x80) >> 7)
				| ((romData[monsterStart + ActionCount1] & 0x80) >> 6);
			regeneration = ((romData[monsterStart + Regeneration0] & 0x80) >> 7)
				| ((romData[monsterStart + Regeneration1] & 0x80) >> 6);

			resistances[0] = (romData[monsterStart + Resistance + 0] & 0xC0) >> 6;
			resistances[1] = (romData[monsterStart + Resistance + 0] & 0x30) >> 4;
			resistances[2] = (romData[monsterStart + Resistance + 0] & 0x0C) >> 2;
			resistances[3] = (romData[monsterStart + Resistance + 1] & 0xC0) >> 6;
			resistances[4] = (romData[monsterStart + Resistance + 1] & 0x30) >> 4;
			resistances[5] = (romData[monsterStart + Resistance + 1] & 0x0C) >> 2;
			resistances[6] = (romData[monsterStart + Resistance + 2] & 0xC0) >> 6;
			resistances[7] = (romData[monsterStart + Resistance + 2] & 0x30) >> 4;
			resistances[8] = (romData[monsterStart + Resistance + 2] & 0x0C) >> 2;
			resistances[9] = (romData[monsterStart + Resistance + 3] & 0xC0) >> 6;
			resistances[10] = (romData[monsterStart + Resistance + 3] & 0x30) >> 4;
			resistances[11] = (romData[monsterStart + Resistance + 3] & 0x0C) >> 2;
			resistances[12] = (romData[monsterStart + Resistance + 4] & 0xC0) >> 6;
			resistances[13] = (romData[monsterStart + Resistance + 4] & 0x30) >> 4;

			focusFire = (romData[monsterStart + FocusFire] & 0x08) == 0x08;

			itemDropChance = romData[monsterStart + ItemDropChance] & 0x07;
		}


		public int[] ConvertStatBlockToBytes()
		{
			int[] statData = new int[0x17];

			statData[Level] |= level;

			statData[Evade0] |= (evade & 0x18) << 3;
			statData[Evade1] |= (evade & 0x04) << 5;

			statData[Exp0] |= exp & 0xFF;
			statData[Exp1] |= exp >> 8;

			statData[Agility] |= agility;

			statData[Gold0] |= gold & 0xFF;
			statData[Gold1] |= gold >> 8;

			statData[Attack0] |= attackPower & 0xFF;
			statData[Attack1] |= attackPower >> 8;

			statData[Defense0] |= defensePower & 0xFF;
			statData[Defense1] |= defensePower >> 8;

			statData[HP0] |= hp & 0xFF;
			statData[HP1] |= hp >> 8;

			statData[MP] |= mp;

			statData[ItemDrop] |= itemDrop;

			statData[Action0] |= actions[0];
			statData[Action1] |= actions[1];
			statData[Action2] |= actions[2];
			statData[Action3] |= actions[3];
			statData[Action4] |= actions[4];
			statData[Action5] |= actions[5];
			statData[Action6] |= actions[6];
			statData[Action7] |= actions[7];

			switch (aiType)
			{
				case 0:
					statData[AISelector0] |= 0x00;
					statData[AISelector1] |= 0x00;
					break;
				case 1: // does this need to be reversed?
					statData[AISelector0] |= 0x80;
					statData[AISelector1] |= 0x00;
					break;
				case 2:
					statData[AISelector0] |= 0x80;
					statData[AISelector1] |= 0x80;
					break;
			}

			statData[ActionChance0] |= (actionChance & 0x01) << 7; // these might be backwards
			statData[ActionChance1] |= (actionChance & 0x02) << 6;

			statData[ActionCount0] |= (actionCountType & 0x01) << 7; // these might be backwards
			statData[ActionCount1] |= (actionCountType & 0x02) << 6;

			statData[Regeneration0] |= (regeneration & 0x01) << 7;
			statData[Regeneration1] |= (regeneration & 0x02) << 6;

			statData[Resistance] |= resistances[0] << 6;
			statData[Resistance] |= resistances[1] << 4;
			statData[Resistance] |= resistances[2] << 2;
			statData[Resistance + 1] |= resistances[3] << 6;
			statData[Resistance + 1] |= resistances[4] << 4;
			statData[Resistance + 1] |= resistances[5] << 2;
			statData[Resistance + 2] |= resistances[6] << 6;
			statData[Resistance + 2] |= resistances[7] << 4;
			statData[Resistance + 2] |= resistances[8] << 2;
			statData[Resistance + 3] |= resistances[9] << 6;
			statData[Resistance + 3] |= resistances[10] << 4;
			statData[Resistance + 3] |= resistances[11] << 2;
			statData[Resistance + 4] |= resistances[12] << 6;
			statData[Resistance + 4] |= resistances[13] << 4;

			statData[FocusFire] |= focusFire ? 0x08 : 0x00;

			statData[ItemDropChance] |= itemDropChance;

			return statData;
		}
	}
}
