using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Dragonaid.TextToHex;

namespace AtomosZ.Dragonaid
{
	/// <summary>
	/// PRG ROM 32 x 16KB = 512KB
	/// CHR ROM: 0 x 8KB = 0KB
	/// ROM CRC32: 0xa86a5318
	/// ROM MD5:  0x16a03048ce659d3d733026b6b72f2470
	/// Mapper #: 1
	/// Mapper name: MMC1
	/// Mirroring: Horizontal
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Offset 0x0032E3 (0x032D3)<para>
		/// Pointer $32D3
		/// blue slime</para>
		/// 
		/// EELL LLLL
		/// E - bits 3-4 of evade rate
		/// L - level
		/// </summary>
		public int ENEMY_DATA_START = 0x0032E3;
		/// <summary>
		/// 0x17
		/// blue slime to black raven
		/// </summary>
		public const int ENEMY_DATA_LENGTH = 23;
		/// <summary>
		/// Not sure if these values are accurate because the game seems to add
		/// a random amount to the total exp, but these should give a ballpark figure.
		/// 0x00 + 0x00 == 0
		/// 0xff + 0x00 == 255
		/// 0x00 + 0x01 == 256 // 352???
		/// 0x00 + 0x03 == 768
		/// 0x00 + 0x11 == 4352
		/// 0x01 + 0x11 == 4353
		/// 0x01 + 0x12 == 4609
		/// 0x00 + 0x13 == 4864
		/// 0x00 + 0xff == 65280
		/// </summary>
		public const int ENEMY_EXP_OFFSET = 1;
		public const int ENEMY_AGI_OFFSET = 3;
		/// <summary>
		/// Low byte. For gold 255+, use byte 18.
		/// </summary>
		public const int ENEMY_GOLD_OFFSET = 4;
		/// <summary>
		/// Low byte. For attack 255+, use byte 19.
		/// </summary>
		public const int ENEMY_ATTACK_OFFSET = 5;
		/// <summary>
		/// Low byte. For defence 255+, use byte 20.
		/// </summary>
		public const int ENEMY_DEFENSE_OFFSET = 6;
		/// <summary>
		/// Low byte. For HP 255+, use byte 21.
		/// </summary>
		public const int ENEMY_HP_OFFSET = 7;
		/// <summary>
		/// 255 sets to infinity.
		/// </summary>
		public const int ENEMY_MP_OFFSET = 8;

		/// <summary>
		/// EIII IIII
		/// E - bit 2 of Evade Rate (Evade rate is a multiple of 4)
		/// I - Item Drop ($7F == none)
		/// </summary>
		public const int ENEMY_DROP_OFFSET = 9;
		/// <summary>
		/// 8 Bytes of behavior patterns
		/// 
		/// Bytes 10 & 11
		/// TxAA AAAA
		/// T - AI type selector (10 low bit, 11 high bit)
		/// A - Actions
		/// 
		/// Bytes 12 & 13
		/// CxAA AAAA
		/// C - Action chance selector
		/// A - Actions
		/// 
		/// Bytes 14 & 15
		/// NxAA AAAA
		/// N - # of actions per turn
		/// A - Actions
		/// 
		/// Bytes 16 & 17
		/// RxAA AAAA
		/// R - Regeneration type
		/// 
		/// T - AI Types { random target choice, smart target choice, smart target choice and enemy chooses action on turn}
		/// C - Type 0: equal chance for all 8 actions
		///		Type 1: $12, $16, $1A, $1E, $22, $26, $2A, $2E (/256)
		///		Type 2: $02, $04, $06, $08, $0A, $0C, $0E, $C8 (/256)
		///		Type 3: fixed action sequence
		///	N - For	AI type 2:		1, 1-2, 1-2, 2
		///		For other AI types:	1, 1-2, 2,   1-3
		///	R - Type 0: no regeneration
		///		Type 1: 16-23 HP/turn
		///		Type 2: 44-55 HP/turn
		///		Type 3: 90-109 HP/turn
		/// </summary>
		public const int ENEMY_BEHAVIOR_OFFSET = 10;
		public const int ENEMY_BEHAVIOR_LENGTH = 8;

		/// <summary>
		/// FFII WWGG
		/// F - fire resistance (Blaze&Fireball&Bang)
		/// I - ice resistance (Icebolt)
		/// W - wind resistance (Infernos)
		/// G - gold (high bits)
		/// </summary>
		public const int ENEMY_EFFECTS1_OFFSET = 18;
		/// <summary>
		/// LLDD SSPP
		/// L - lightning resistance 
		/// D - death resistance (Beat)
		/// S - sacrifice resistance (Sacrifice)
		/// P - attack power (high bits)
		/// </summary>
		public const int ENEMY_EFFECTS2_OFFSET = 19;
		/// <summary>
		/// SSMM ssDD
		/// S - sleep resistance
		/// M - stopspell resistance
		/// s - sap resistance
		/// D - defense power (high bits)
		/// </summary>
		public const int ENEMY_EFFECTS3_OFFSET = 20;
		/// <summary>
		/// SSRR CCHH
		/// S - surround resistance
		/// R - RobMagic resistance
		/// C - Chaos resistance (Confuse)
		/// H - HP (high bits)
		/// </summary>
		public const int ENEMY_EFFECTS4_OFFSET = 21;
		/// <summary>
		/// LLEE FIII
		/// L - Limbo/Slow resistance
		/// E - Expel/Fairy Water/Zombie Slasher resistance
		/// F - Focus-fire flag (if set, the enemy continually attacks the same character until they die)
		/// I - Item drop chance 
		/// 
		/// 00 = 1/1	100 percent			000
		/// 01 = 1/8	12.5 percent		001
		/// 02 = 1/16	6.25 percent		010
		/// 03 = 1/32	3.13 percent		011
		/// 04 = 1/64	1.56% percent		100
		/// 05 = 1/128	0.78% percent		101
		/// 06 = 1/256	0.39% percent		110
		/// 07 = 1/2048	0.049% percent		111
		/// </summary>
		public const int ENEMY_DROP_PERCENT = 22; // 0x0032F9

		/// <summary>
		/// Offset 0x00AA22
		/// Pointer $AA12
		/// 2 bytes.
		/// </summary>
		public int MONSTER_NAME_POINTER = 0x00AA22;
		/// <summary>
		/// 0x000B3BC
		/// Name deliminater = FF
		/// </summary>
		public int MONSTER_NAME_START = 0x000B3BC;


		public MainForm()
		{
			InitializeComponent();

			byte[] byteData = File.ReadAllBytes(@"D:\github\RomHacking\Working ROMs\Dragon Warrior 3 (U).nes");
			//byteData[ENEMY_DATA_START + ENEMY_EXP_OFFSET] = 0x04;
			//byteData[ENEMY_DATA_START + ENEMY_EXP_OFFSET + 1] = 0x00;
			//byteData[ENEMY_DATA_START + ENEMY_HP_OFFSET] = 0x08;
			//byteData[ENEMY_DATA_START + ENEMY_MP_OFFSET] = 0x00;
			//File.WriteAllBytes(@"D:\github\RomHacking\Working ROMs\Dragon Warrior 3 (U) Test.nes", byteData);


			bytes_textBox.AppendText("Level:    " + byteData[ENEMY_DATA_START].ToString("X2"));
			bytes_textBox.AppendText("\r\nExp:      " + (byteData[ENEMY_DATA_START + ENEMY_EXP_OFFSET]
				+ byteData[ENEMY_DATA_START + ENEMY_EXP_OFFSET + 1] * 256)); // byteData[0x0032E5] is for > 255 exp
			bytes_textBox.AppendText("\r\nAgi:      " + byteData[ENEMY_DATA_START + ENEMY_AGI_OFFSET].ToString("X2"));
			bytes_textBox.AppendText("\r\nGP:       " + byteData[ENEMY_DATA_START + ENEMY_GOLD_OFFSET].ToString("X2"));
			bytes_textBox.AppendText("\r\nAttack:   " + byteData[ENEMY_DATA_START + ENEMY_ATTACK_OFFSET].ToString("X2"));
			bytes_textBox.AppendText("\r\nDefense:  " + byteData[ENEMY_DATA_START + ENEMY_DEFENSE_OFFSET].ToString("X2"));
			bytes_textBox.AppendText("\r\nHP:       " + byteData[ENEMY_DATA_START + ENEMY_HP_OFFSET].ToString("X2"));
			bytes_textBox.AppendText("\r\nMP:       " + byteData[ENEMY_DATA_START + ENEMY_MP_OFFSET].ToString("X2"));
			bytes_textBox.AppendText("\r\nItemDrop%: " + byteData[ENEMY_DATA_START + ENEMY_DROP_PERCENT].ToString("X2"));
			bytes_textBox.AppendText(" ItemDrop: " + byteData[ENEMY_DATA_START + ENEMY_DATA_LENGTH * 0x12 + ENEMY_DROP_OFFSET].ToString("X2"));
			bytes_textBox.AppendText("\r\nAdded FX1: " + byteData[ENEMY_DATA_START + ENEMY_EFFECTS1_OFFSET].ToString("X2"));
			bytes_textBox.AppendText("\r\nAdded FX2: " + byteData[ENEMY_DATA_START + ENEMY_EFFECTS2_OFFSET].ToString("X2"));
			bytes_textBox.AppendText("\r\nAdded FX3: " + byteData[ENEMY_DATA_START + ENEMY_EFFECTS3_OFFSET].ToString("X2"));
			bytes_textBox.AppendText("\r\nAdded FX4: " + byteData[ENEMY_DATA_START + ENEMY_EFFECTS4_OFFSET].ToString("X2"));

			var byte1 = byteData[MONSTER_NAME_POINTER];
			var byte2 = byteData[MONSTER_NAME_POINTER + 1];
			int pointer = OffsetToPointer(byte2, byte1);
			bytes_textBox.AppendText("\r\n" + pointer.ToString("X2"));
			bytes_textBox.AppendText("\r\n" + pointer);
			bytes_textBox.AppendText("\r\n" + Tables.textTable[byteData[pointer]] + Tables.textTable[byteData[pointer + 1]]
				 + Tables.textTable[byteData[pointer + 2]] + Tables.textTable[byteData[pointer + 3]] + Tables.textTable[byteData[pointer + 4]]);

			Debug.WriteLine(PointerToOffset(0x005c55f).ToString("X2"));
			Debug.WriteLine(PointerToOffset(0x005c567).ToString("X2"));

			//for (int i = 0x0032E3; i < 0x0032FA; ++i)
			//{
			//	if (i % 16 == 0)
			//		bytes_textBox.AppendText("\r\n");
			//	bytes_textBox.AppendText(byteData[i].ToString("X2"));
			//	if ((i+1) % 2 == 0)
			//		bytes_textBox.AppendText(" ");
			//}

		}



		/// <summary>
		/// Offset is the address in the .NES file. Pointer is the logical address (i.e. where it would be in the actual cartridge)
		/// </summary>
		/// <param name="hiByte">8bit int. By convention, the second byte.</param>
		/// <param name="loByte">8bit int.</param>
		/// <returns>little endien 16bit int</returns>
		public int OffsetToPointer(int hiByte, byte loByte)
		{
			int newHi = hiByte << 8;
			int sum = newHi | loByte;
			return sum + 0x10;
		}

		/// <summary>
		/// Offset is the address in the .NES file. Pointer is the logical address (i.e. where it would be in the actual cartridge)
		/// </summary>
		/// <param name="offset">big endian 16bit int</param>
		/// <returns></returns>
		public int OffsetToPointer(int offset)
		{
			var nibs = SwapNibbles(offset);
			return nibs + 0x10;
		}

		public int PointerToOffset(int pointer)
		{
			return SwapNibbles(pointer - 0x10);
		}

		public int SwapNibbles(int x)
		{
			var hi = (x & 0x00ff) << 8; // gets last 4 bits and shifts them left 4 bits
			var lo = (x & 0xff00) >> 8; // gets first 4 bit and shifts them right 4 bits
			return hi | lo;
		}
	}
}
