﻿using System;

using AtomosZ.DragonAid.Libraries;

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


		public MonsterStatBlock(byte[] romData, byte monsterIndex)
		{
			index = monsterIndex;
			int monsterStart = PointerList.MonsterStatBlockAddress.offset + index * PointerList.MonsterStatBlockAddress.length;

			name = Names.GetMonsterName(romData, monsterIndex);
			level = romData[monsterStart + Level] & 0x3F;
			evade = romData[monsterStart + Evade1] & 0xC0;
			evade = evade >> 3;
			int evade2 = romData[monsterStart + Evade2] & 0x80;
			evade2 = evade2 >> 5;
			evade = evade | evade2;
			exp = (romData[monsterStart + Exp2] << 8) | romData[monsterStart + Exp1];

			agility = romData[monsterStart + Agility];

			gold = romData[monsterStart + Gold1] | ((romData[monsterStart + Gold2] & 0x03) << 8);

			attackPower = romData[monsterStart + Attack1] | (romData[monsterStart + Attack2] & 0x03) << 8;
			defensePower = romData[monsterStart + Defense1] | (romData[monsterStart + Defense2] & 0x03) << 8;
			hp = romData[monsterStart + HP1] | (romData[monsterStart + HP2] & 0x03) << 8;

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

			aiType = ((romData[monsterStart + AISelector1] & 0x80) >> 7)
				+ ((romData[monsterStart + AISelector2] & 0x80) >> 7);

			actionChance = ((romData[monsterStart + ActionChance1] & 0x80) >> 7)
				| ((romData[monsterStart + ActionChance2] & 0x80) >> 6);
			actionCountType = ((romData[monsterStart + ActionCount1] & 0x80) >> 7)
				| ((romData[monsterStart + ActionCount2] & 0x80) >> 6);
			regeneration = ((romData[monsterStart + Regeneration1] & 0x80) >> 7)
				| ((romData[monsterStart + Regeneration2] & 0x80) >> 6);

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




			{
			}


		}
	}
}
