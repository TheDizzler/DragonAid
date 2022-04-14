using System;

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
		/// 2 bits.
		/// 
		/// [ActionChance1] AND 0x80 right shift 7 | [ActionChance2] AND 0x80 right shift 7
		/// </summary>
		public int actionChance;

		/// <summary>
		/// 2 bits.
		/// [ActionCount1] AND 0x80 right shift 7 | [ActionCount2] AND 0x80 right shift 7
		/// 
		/// For	AI type 2:		1, 1-2, 1-2, 2
		///	For other AI types:	1, 1-2, 2,   1-3
		/// </summary>
		public int actionCountType;

		/// <summary>
		/// 2 bits.
		/// 
		/// [Regeneration1] AND 0x80 right shift 7 | [Regeneration2] AND 0x80 right shift 7
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
			level = romData[monsterStart + MonsterConsts.Level] & 0x3F;
			evade = romData[monsterStart + MonsterConsts.Evade1] & 0xB0;
			evade = evade >> 3;
			int evade2 = romData[monsterStart + MonsterConsts.Evade2] & 0x80;
			evade2 = evade2 >> 5;
			evade = evade | evade2;
			exp = (romData[monsterStart + MonsterConsts.Exp2] << 8) | romData[monsterStart + Exp1];

			agility = romData[monsterStart + MonsterConsts.Agility];

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
				| ((romData[monsterStart + ActionChance2] & 0x80) >> 7);

			actionCountType = ((romData[monsterStart + ActionCount1] & 0x80)
				| (romData[monsterStart + ActionCount2] & 0x80)) >> 7;

			regeneration = ((romData[monsterStart + Regeneration1] & 0x80)
					| (romData[monsterStart + Regeneration2] & 0x80)) >> 7;

			
			int byteOffset = -1;
			int shift = 6;
			int mask = 0xC0;
			for (int i = 0; i < resistances.Length; ++i)
			{
				if (i % 3 == 0)
					++byteOffset;
				resistances[i] = (romData[monsterStart + Resistance + byteOffset] & mask) >> shift;
				shift -= 2;
				if (shift == 0)
					shift = 6;
				mask = mask >> 2;
				if (mask == 0x03)
					mask = 0xC0;
			}

			focusFire = (romData[monsterStart + FocusFire] & 0x02) == 0x08;

			itemDropChance = romData[monsterStart + ItemDropChance] & 0x07;
		}
	}
}
