using System;
using System.Collections.Generic;

namespace AtomosZ.DragonAid.Libraries.PointerList
{
	public static class Pointers
	{
		/// <summary>
		/// <para>256 bytes from 0x00 to 0xFF of NES RAM.</para>
		/// </summary>
		public static class ZeroPages
		{
			/// <summary>
			/// 0x14
			/// <para>
			/// <br>$01 A</br><br>$02 B</br><br>$04 Select</br><br>$08 Start</br>
			/// <br>$10 Up</br><br>$20 Down</br><br>$40 Left</br><br>$80 Right</br>
			/// </para>
			/// </summary>
			public static byte controller1_ButtonStore = 0x14;
			/// <summary>
			/// 0x15
			/// </summary>
			public static byte controller2_ButtonStore = 0x15;
			/// <summary>
			/// <para>zeroPages 0x1B
			/// <br>NMI_VBlank_3C000 - if == 10, skip PPU_Update</br>
			/// </para>
			/// </summary>
			public static byte loopTrap_flag = 0x1B;
			/// <summary>
			/// 0x1C
			/// </summary>
			public static byte currentRNGSeed = 0x1C;
			/// <summary>
			/// DynamicSubroutine address: 0x21 (zeroPages).
			/// 2 bytes.
			/// </summary>
			public static byte dynamicSubroutineAddr = 0x21;
			/// <summary>
			/// <para>zeroPages 0x28</para>
			/// </summary>
			public static byte PPU_renderStart_flag = 0x28;
			/// <summary>
			/// <para>zeroPages 0x29</para>
			/// </summary>
			public static byte PPU_render_flag = 0x29;
			/// <summary>
			///<para>zeroPages 0x2A</para>
			/// </summary>
			public static byte map_WorldPosition_X = 0x2A;
			/// <summary>
			/// <para>zeroPages 0x2B</para>
			/// </summary>
			public static byte map_WorldPosition_Y = 0x2B;
			/// <summary>
			/// <para>zeroPages 0x2C</para>
			/// </summary>
			public static byte encounterVariable_A = 0x2C;
			/// <summary>
			/// <para>zeroPages 0x2F</para>
			/// bit 1 seems to indicate whether in light (0) or dark (1) world
			/// </summary>
			public static byte lightOrDarkWorld = 0x2F;
			/// <summary>
			/// 0x7E
			/// </summary>
			public static byte menu_PointerIndex = 0x7E;
			/// <summary>
			/// <para>2 bytes starting at 0x86.
			/// <br>[1] if bit 3 || 4 == 1, scroll horizontal</br>
			/// <br>[1] if bit 0-3 == 1, scroll vertical</br></para>
			/// </summary>
			public static byte mapScrollCheck = 0x86;
			/// <summary>
			/// 0x92
			/// </summary>
			public static byte currentTileType = 0x92;
			/// <summary>
			/// 0xB0
			/// <para>investigation needed</para>
			/// </summary>
			public static byte characterStatusRelated_Vector = 0xB0;
			/// <summary>
			/// 0xCA
			/// </summary>
			public static int controllerInputStore = 0xCA;
			/// <summary>
			/// 0xCE
			/// </summary>
			public static byte character_FormationIndex = 0xCE;
			/// <summary>
			/// 0xCF
			/// </summary>
			public static byte Item_Check_IsEquipped = 0xCF;
			/// <summary>
			/// 0xD6
			/// <para>
			/// Array of addresses (2 bytes) to next byte data of a track.
			/// <br>8 bytes total</br>
			/// </para>
			/// </summary>
			public static byte APU_TrackPosition = 0xD6;
			/// <summary>
			/// 0xDE
			/// </summary>
			public static int SFXPointer = 0xDE;
			/// <summary>
			/// 0xE0
			/// <para>An array of instructions per track.
			/// <br>+0 and +1 are probably temp variables for current track.</br>
			/// <br>0x10 is probably for DMC.</br>
			/// <br>if (x + 2 != 0) APU_UpdateTrack()</br>
			/// <br>+0 set SqxTimer(4002/4006), +1 sets SqxLength(4003/4007)</br>
			/// <br>x + 2, x + 3, x + 12, x + 13 may be duty cycle related</br>
			/// <br>when (x == 2 || x == 4): {x + 3 = x + 1; x + 13 = x + 11;}</br>
			/// </para>
			/// </summary>
			public static byte APU_TrackInstructions = 0xE0;
			/// <summary>
			/// 0xF6 2 bytes
			/// <para>
			/// <br>[0]: offset to APU_DutySetup_Addresses where duty settings (?) are stored.</br></para>
			/// </summary>
			public static byte APU_TempDutySettings = 0xF6;
		}

		/// <summary>
		/// <para>
		/// <br>2048 bytes from 0x00 to 0x07FF. (mirrors from 0x800 to 0x1FFF)</br>
		/// <br>Technically includes zeroPages but for simplicity we are keeping these seperate.</br></para>
		/// </summary>
		public static class NESRAM
		{
			/// <summary>
			/// <para>NES RAM 0x0200
			/// <br>This is where sprites are pre-loaded. During NMI, these get copied to the PPU OAM through RegisterPointers.SpriteDMA_4014</br></para>
			/// </summary>
			public static int PPU_SpriteDMA = 0x0200;
			/// <summary>
			/// <para>NES RAM 0x300</para>
			/// </summary>
			public static int PPU_StagingArea = 0x0300;
			/// <summary>
			/// 0x0400
			/// <para>96 byte length array</para>
			/// </summary>
			public static int menu_WriteBlock = 0x0400;
			/// <summary>
			/// 0x0470
			/// </summary>
			public static int menu_PositionA = 0x0470;
			/// <summary>
			/// 0x0471
			/// </summary>
			public static int menu_WriteDimensions = 0x0471;
			/// <summary>
			/// 0x047C
			/// <para>it this is  >= 6A, run track updates</para>
			/// </summary>
			public static int updateTracks = 0x047C;
			/// <summary>
			/// <para>NES RAM 0x0644</para>
			/// 0 - Up
			/// 1 - Right
			/// 2 - Down
			/// 3 - Left
			/// </summary>
			public static int walkDirection = 0x0644;
			public static int PPUScroll_X = 0x06D0;
			public static int PPUScroll_Y = 0x06D1;
			/// <summary>
			/// <para>NES RAM 0x06D2</para>
			/// INC in NMI to signify NMI completed.
			/// </summary>
			public static int waitForNMI_flag = 0x06D2;
			/// <summary>
			/// <para>NES RAM 0x06D3</para>
			/// </summary>
			public static int PPUControl_2000_Settings = 0x06D3;
			/// <summary>
			/// <para>NES RAM 0x06D4</para>
			/// </summary>
			public static int PPUMask_2001_Settings = 0x06D4;
			/// <summary>
			/// Either the current bank or bank to switch back to.
			/// </summary>
			public static int bankSwitch_CurrentBankId = 0x06D5;
			/// <summary>
			/// <para> NES RAM 0x06D6</para>
			/// </summary>
			public static int caretUpdate_flag = 0x06D6;
			/// <summary>
			/// <para>NES RAM 0x06D9
			/// <br>amount of lines to write to PPU from staging area</br></para>
			/// </summary>
			public static int PPU_DrawBackgrounLineCount = 0x06D9;
			/// <summary>
			/// 0x06DE
			/// </summary>
			public static int timeSubValue = 0x06DE;
			/// <summary>
			/// <para>0x06DF
			/// <br>In-game day/night counter</br></para>
			/// </summary>
			public static int timeOfDay = 0x06DF;
			/// <summary>
			/// <para>NES RAM 0x06E1</para>
			/// Check for encounter if == 0
			/// </summary>
			public static int encounterCheckRequired_A = 0x06E1;
			/// <summary>
			/// 0x06F0
			/// <para>
			/// <br>The current sequence playing.</br>
			/// <br>if bit 0 == 1, skip APU updates.</br>
			/// </para>
			/// </summary>
			public static int APU_SequenceID = 0x6F0;
			/// <summary>
			/// <para>NesRam 0x073C.
			/// <br></br>2 bytes per character:
			/// <br></br>$8080 is normal, anything else is dead?</para>
			/// </summary>
			public static int Character_Statuses = 0x073C;

		}

		/// <summary>
		/// <para>
		/// <br>0x2000 - 0x2007 PPU registers (mirrors from 0x2008 to 0x3FFF)</br>
		/// <br>0x4000 - 0x4017 APU and I/O registers</br>
		/// <br>0x4018 - 0x401F APU and I/O functionality that is normally disabled.</br>
		/// </para>
		/// </summary>
		public static class Registers
		{
			/// <summary>
			/// <para>0x2000
			/// <br>Access: write</br>
			/// <br>VPHB SINN</br>
			/// <br>NMI enable (V), PPU master/slave (P), sprite height (H), background tile select (B), sprite tile select (S), increment mode (I), nametable select (NN) </br>
			/// </para>
			/// </summary>
			public static int PPU_Control = 0x2000;
			/// <summary>
			/// <para> 0x2001
			/// <br>Access: write</br>
			/// <br>BGRs bMmG</br>
			/// <br>color emphasis (BGR), sprite enable (s), background enable (b), sprite left column enable (M), background left column enable (m), greyscale (G) </br>
			/// </para>
			/// </summary>
			public static int PPU_Mask = 0x2001;
			/// <summary>
			/// <para><br>0x2002</br>
			/// <br>Reading this register will clear bit 7 and the address latches PPU_Scroll and PPU_Addr.</br>
			/// <br>Race Condition Warning: Reading PPU_Status within two cycles of the start of vertical
			/// blank will return 0 in bit 7 but clear the latch anyway, causing NMI to not occur that frame.</br>
			/// </para>
			/// </summary>
			public static int PPU_Status = 0x2002;
			/// <summary>
			/// <para>0x2005
			/// <br>Access: write twice (x, then y)</br>
			/// <br>fine scroll position (two writes: X scroll, Y scroll)</br>
			/// <br></br>
			/// </para>
			/// </summary>
			public static int PPU_Scroll = 0x2005;

			/// <summary>
			/// <para> 0x4000
			/// <br>DDLC VVVV</br>
			/// <br>Duty (D), envelope loop / length counter halt (L), constant volume (C), volume/envelope (V)</br>
			/// <br>Duty Cycles - 0: 12.5%; 1: 25%; 2: 50%; 3: 25% negated</br>
			/// </para>
			/// </summary>
			public static int SQ0Duty = 0x4000;
			/// <summary>
			/// <para>0x4001
			/// <br>EPPP NSSS</br>
			/// <br>Sweep unit: enabled (E), period (P), negate (N), shift (S)</br>
			/// <br>E--- ---- (bit 7) Enabled flag </br>
			/// <br>-PPP ---- (bits 6-4) The divider's period is P + 1 half-frames </br>
			/// <br>---- N--- (bit 3) 0: sweeping down 1: sweeping up</br>
			/// <br>---- -SSS (bits 2-0) Shift count (number of bits) </br>
			/// </para>
			/// </summary>
			public static int SQ0Sweep = 0x4001;
			/// <summary>
			/// <para> 0x4002
			/// <br>TTTT TTTT</br>
			/// <br>Timer low bytes(T)</br>
			/// <br>High 3 bytes are in 0x4003</br>
			/// </para>
			/// </summary>
			public static int SQ0Timer = 0x4002;
			/// <summary>
			/// <para>0x4003
			/// <br>LLLL LTTT</br>
			/// <br>Length counter load (L), timer high bytes(T)</br>
			/// <br>Low 8 bytes of timer are in 0x4002</br>
			/// </para>
			/// </summary>
			public static int SQ0Length = 0x4003;

			/// <summary>
			/// <para> 0x4004
			/// <br>DDLC VVVV</br>
			/// <br>Duty (D), envelope loop / length counter halt (L), constant volume (C), volume/envelope (V)</br>
			/// </para>
			/// </summary>
			public static int SQ1Duty = 0x4004;
			/// <summary>
			/// <para>0x4005
			/// <br>EPPP NSSS</br>
			/// <br>Sweep unit: enabled (E), period (P), negate (N), shift (S)</br>
			/// <br>E--- ---- (bit 7) Enabled flag </br>
			/// <br>-PPP ---- (bits 6-4) The divider's period is P + 1 half-frames </br>
			/// <br>---- N--- (bit 3) 0: sweeping down 1: sweeping up</br>
			/// <br>---- -SSS (bits 2-0) Shift count (number of bits) </br>
			/// </para>
			/// </summary>
			public static int SQ1Sweep = 0x4005;
			/// <summary>
			/// <para> 0x4006
			/// <br>TTTT TTTT</br>
			/// <br>Timer low bytes(T)</br>
			/// <br>High 3 bytes are in 0x4007</br>
			/// </para>
			/// </summary>
			public static int SQ1Timer = 0x4006;
			/// <summary>
			/// <para>0x4007
			/// <br>LLLL LTTT</br>
			/// <br>Length counter load (L), timer high bytes(T)</br>
			/// <br>Low 8 bytes of timer are in 0x4006</br>
			/// </para>
			/// </summary>
			public static int SQ1Length = 0x4007;

			/// <summary>
			/// <para>0x4008</para>
			/// </summary>
			public static int TriangleLinear = 0x4008;
			/// <summary>
			/// <para>0x4009</para>
			/// </summary>
			public static int TriangleUnused = 0x4009;
			/// <summary>
			/// <para>0x400A
			/// <br>TTTT TTTT</br>
			/// <br>Timer low bytes(T)</br>
			/// <br>High 3 bytes are in 0x4007</br>
			/// </para>
			/// </summary>
			public static int TriangleTimer = 0x400A;
			/// <summary>
			/// <para>0x400B
			/// <br>LLLL LTTT</br>
			/// <br>Length counter load (L), timer high bytes(T)</br>
			/// <br>Low 8 bytes of timer are in 0x400A</br>
			/// </para>
			/// </summary>
			public static int TriangleLength = 0x400B;


			/// <summary>
			/// <para>0x4011
			/// <br>-DDD DDDD</br>
			/// <br>Load counter (D)</br></para>
			/// </summary>
			public static int DmcCounter = 0x4011;
			/// <summary>
			/// <para><br>0x4014</br>
			/// <br>Writing $XX will upload 256 bytes of data from CPU page $XX00-$XXFF to the internal PPU OAM.</br>
			/// </para>
			/// </summary>
			/// 

			public static int SpriteDMA = 0x4014;
			/// <summary>
			/// <para>0x4015
			/// <br>Used to enable and disable individual channels, control the DMC, and can read the status of length counters and APU interrupts. </br>
			/// </para>
			/// <para>
			/// <br>WRITE</br>
			/// <br>---D NT21</br>
			/// <br>Enable DMC (D), noise (N), triangle (T), and pulse channels (2/1) </br>
			/// </para>
			/// <para>
			/// <br>READ</br>
			/// <br>IF-D NT21</br>
			/// <br>DMC interrupt (I), frame interrupt (F), DMC active (D), length counter > 0 (N/T/2/1) </br>
			/// </para>
			/// </summary>
			public static int APUStatus = 0x4015;
			/// <summary>
			/// 0x4016
			/// <para>
			/// <br>WRITE</br>
			/// <br>---- ---A</br>
			/// <br>Output data (strobe) to both controllers.</br>
			/// </para>
			/// </summary>
			public static int Ctrl1 = 0x4016;
			/// <summary>
			/// 0x4017
			/// <para>
			/// <br></br>
			/// </para>
			/// </summary>
			public static int Ctrl2_FrameCounter = 0x4017;
		}

		/// <summary>
		/// 
		/// </summary>
		public static class SRAM
		{
			/// <summary>
			/// 0x60C5
			/// </summary>
			public static int encounterCheckRequired_b = 0x60C5;
			/// <summary>
			/// 0x60CB
			/// </summary>
			public static int encounterCheckRequired_c = 0x60CB;
			/// <summary>
			/// 0x67C1
			/// </summary>
			public static int Character_NameIndices = 0x67C1;
			/// <summary>
			/// 0x6A3D
			/// <para>when not 0, signals APU to turn on the DMC</para>
			/// </summary>
			public static int APU_enableDMC = 0x6A3D;
			/// <summary>
			/// 0x6C1A
			/// <para>Points JSR C4A0 (LoadDynamicSubroutine_From_InstructionByte)</para>
			/// </summary>
			public static int LoadDynamicSubroutine_GetCharacterCountAndNameIndices = 0x6C1A;
		}


		/// <summary>
		/// <para>Pointers in raw ROM. 
		/// <br></br>Use .offset for address to make edits to ROM (includes iNES header).
		/// <br></br>Use .pointer for referencing address while games is RUNNING (no iNES header)</para>
		/// </summary>
		public static class ROM
		{
			/// <summary>
			/// Locations where hardcoded day-to-night time checked
			/// Night starts at 0x78.
			/// Timer Resets at 0xCC
			/// </summary>
			public static Address[] NightTimeCheckAddresses = new Address[]
			{
			new Address("GetEncounterRate", 0x003BD, 1),
			new Address("SetMenuColor", 0x35067, 1),
			};



			/* Bank 0 $00000 */
			/// <summary>
			/// Index derived from EncounterMonsterLists ( >>= 5)
			/// </summary>
			public static Address EncounterRateMultipliers = new Address("Unknown", 0x00628, 4);
			/// <summary>
			/// $DB8A
			/// </summary>
			public static Address EncounterMonsterListPointer = new Address("Points to EncounterMonsterLists (0x00ADB)", 0x00648, 2);
			public static Address EncounterRates = new Address("Encounter Rates per Tile", 0x00934, 18)
			{
				notes = "0-8 Day encounter rates\n"
					+ "9-16 Night encounter rates\n\n"
					+ "day\n"
					+ "[0]: 04 - water\n"
					+ "[1]: 0F - woods/desert/ice\n"
					+ "[2]: 0A - grass\n"
					+ "[3]: 0F - woods/desert/ice\n"
					+ "[4]: 12 - heavy forest\n"
					+ "[5]: 19 - hills\n"
					+ "[6]: 54 - dungeon after room change\n"
					+ "[7]: 12 - swamp\n"
					+ "[8]: 0A - dungeon\n\n"
					+ "night\n"
					+ "[9]: 05 - water\n"
					+ "[10]: 13 - woods/desert/ice\n"
					+ "[11]: 0D - grass\n"
					+ "[12]: 0F - woods/desert/ice\n"
					+ "[13]: 16 - heavy forest\n"
					+ "[14]: 1F - hills\n"
					+ "[15]: 54 - dungeon after room change\n"
					+ "[16]: 16 - swamp\n"
					+ "[17]: 0A - dungeon\n"
			};
			/// <summary>
			/// Index is derived from position on map. 
			/// (XXXX 0000 >> 4) + YYYY 0000 = YYYY XXXX
			/// </summary>
			public static Address LightWorldEncounterZones = new Address("Encounter zones on the Light World Map", 0x00946, 256);
			public static Address DarkWorldEncounterZones = new Address("Encounter zones on the Dark World Map", 0x00A46, 64);
			public static Address EncounterMonsterLists = new Address("Monster lists for encounter areas", 0x00ADB, 76);

			/// <summary>
			/// [0,1] Experience
			/// [2,3] Strength
			/// [4,5] Agility
			/// [6,7] Vitality
			/// </summary>
			public static Address CharacterStatPointers = new Address("This requires a better name", 0x027F4, 8);
			public static Address CharacterStatsPointer = new Address("This requires a better name too", 0x02800);
			/// <summary>
			/// Theses probably point to stat baselines.
			/// </summary>
			public static Address CharacterLevelUpPointers = new Address("This requires a better name three", 0x02802);
			public static Address MonsterStatBlockAddress = new Address("Monster Data 1", 0x0032D3, UniversalConsts.MonsterStatLength);


			/* Bank 1 $04000 */
			public static Address MonsterRegeneration = new Address("Monster Regeneration", 0x047EF);


			/* Bank 2 $08000 */
			/// <summary>
			/// [0,1] 1st part of item name
			/// [2,3] 2nd part of item name
			/// [4,5] = $B3BC monster name list 1
			/// [6,7] = $B8BF End of MonsterName_List
			/// [$0A] = $AA28 Class name list
			/// </summary>
			public static Address NamePointers = new Address("Item, Monster, Class name pointers", 0x0AA0E);

			/* Bank 4 $10000 */
			public static Address MonsterActionChancesType1 = new Address("ActionChancesType1", 0x1342A);
			public static Address MonsterActionChancesType2 = new Address("ActionChancesType2", 0x13432);

			/* Bank 5 $14000 */
			/// <summary>
			/// 0x14018
			/// </summary>
			public static Address Map_LightWorld_Data_A = new Address("The difference between this and Data_B?", 0x14018, 256);
			/// <summary>
			/// 0x14118
			/// </summary>
			public static Address Map_LightWorld_Data_B = new Address("The difference between this and Data_A?", 0x14118, 217);
			/// <summary>
			/// 0x15A95
			/// </summary>
			public static Address Map_DarkWorld = new Address("The difference between this and Data_A?", 0x15A95, 256);
			/// <summary>
			/// 0x16DD0
			/// </summary>
			public static Address TileBatchSomethingPointerA = new Address("Unclear what this vector does", 0x16DD0, 2);
			/// <summary>
			/// 016DD2
			/// </summary>
			public static Address TileBatchSomethingPointerB = new Address("Unclear what this vector does", 0x16DD2, 2);
			/// <summary>
			/// 0x16DD4
			/// </summary>
			public static Address TileBatches_Characters = new Address("Tile batch instructions for character sprites", 0x16DD4);

			public static Address PPUAddressTable = new Address("Unclear what this vector does", 0x17438, 15)
			{
				notes = "Entry 1 (map tiles):"
					+ "[$00] - [$01]:$18D0 -> STA $2006 PPUWrite address "
					+ "[$02] - [$03]: $7200 -> CHR_TileIndex_Vector are tiles are stored (for reference later?)"
					+ "[$04]: $8D -> offset to write location in PPU\n"
					+ "Entry 2 (bird):"
					+ "[$05] - [$06]: $0800 -> STA $2006 PPUWrite address "
					+ "[$07] - [$08]: $6F00 -> Sprite_Index_Vector "
					+ "[$09]: $80 -> offset to write location in PPU\n"
					+ "Entry 3 (PC sprite):"
					+ "[$10]: $0000 "
					+ "[$12]: $6E00 -> staging area for sprites before passing to PPU"
					+ "[$13]: $00\n",
			};
			public static Address OffsetsToNextSpriteInTileBatch = new Address("", 0x17456, 88)
			{
				notes = "The value at nextSpriteAddressOffset is added to instructionByte[1] "
					+ "to get the address (before << 4) of the sprite to load. "
					+ "The Address is saved in zeroPages[0x04 + x].",
			};
			public static Address TileBatchVectorA = new Address("Unclear what this vector does", 0x174AD, 88);
			public static Address TileDynamicOffsets = new Address("Tile dynamic offsets", 0x174BC, 5)
			{
				notes = "paired bytes:\n"
					+ "1st byte is added to dynamicSubroutine (pointer to instruction bytes)\n"
					+ "2nd byte is tileSomethingVectorCOffset",
			};
			public static Address TileBatchSpriteOrder = new Address("Order sprites are parsed from zeroPages[0x04]", 0x174CC, 5)
			{
				notes = "Order sprites are parsed from zeroPages[0x04 + x] where x is from one of the lists below:\n"
					+ "Standard order: 00 02 04 06\n"
					+ "Other orders not observed yet",
			};
			public static Address MapScrollVector = new Address("", 0x17A72, 4)
			{
				notes = "[$00]: (moving up) $BB13\n"
					+ "[$02]: (moving right) $BAEC\n"
					+ "[$04]: (moving down) $BAF8\n"
					+ "[$06]: (moving left) $BAE5",
			};

			/* Bank 6 $18000 */
			/// <summary>
			/// 0x18000
			/// </summary>
			public static Address LocalPointers_18000 = new Address("Pointer table for $18000 bank", 0x18000, 24)
			{
				notes = "[$0A]: $B387 \n"
					+ "[$16]: $B755 (Day/Night Palettes)\n",
			};
			/// <summary>
			/// 0x1B755
			/// </summary>
			public static Address DayNightPalettes = new Address("Only day/night palettes?", 0x1B755, 84)
			{
				notes = "Clock 00: $00-$0B; Clock 1E: $0C-$17; Clock 3C: $18-$23; Clock 5A: $24-$29; "
					+ "Clock 78 (night start): $30-$3B; Clock 96: $3C-$47; Clock B4: $48-$53",
			};
			/// <summary>
			/// 0x1B7B0
			/// </summary>
			public static Address DayNightTransitionTimes = new Address("Day night cycle transition times", 0x1B7B0, 7)
			{
				notes = "0x00, 0x1E, 0x3C, 0x5A, 0x78, 0x96, 0xB4",
			};

			/* Bank 9 $24000 */
			public static Address WeaponPowers = new Address("Weapon Powers", 0x027990, UniversalConsts.WeaponCount);
			public static Address ArmorPowers = new Address("Armor Powers", 0x0279B0, UniversalConsts.ArmorCount);
			public static Address ShieldPowers = new Address("Shield Powers", 0x0279C8, UniversalConsts.ShieldCount);
			public static Address HelmetPowers = new Address("Helmet Powers", 0x0279CF, UniversalConsts.HelmetCount);

			/* Bank A $28000 */
			/* Bank B $2C000 */
			/* Bank C $30000 */
			/// <summary>
			/// 0x32FC7
			/// </summary>
			public static Address Map_Scroll_Finish_Vector = new Address("Unknown", 0x32FC7, 16);

			/* Bank D $34000 */
			/// <summary>
			/// 0x355F9
			/// </summary>
			public static Address MenuColorQuarterHP = new Address("Color used when character's HP below quarter max", 0x355F9, 1)
			{
				notes = "#$2A (green) by default",
			};
			public static Address MenuColorStatusNormal = new Address("Color used when status normal", 0x3560B, 1)
			{
				notes = "#$30 (white) by default",
			};
			public static Address MenuColorNightTime = new Address("Color used at night time", 0x3560E, 1)
			{
				notes = "#$21 (light blue) by default",
			};
			public static Address MenuColorQCharacterDead = new Address("Color used when character dead", 0x35613, 1)
			{
				notes = "#$27 (orange) by default",
			};
			public static Address MenuColorUnknown = new Address("Color used when value at 6A58 (Save RAM) is 0", 0x3561A, 1)
			{
				notes = "#$30 (white) by default",
			};

			/* Bank E $38000 */
			/// <summary>
			/// Add 0x23 and you get the DynamicSubroutine07 Index
			/// 
			/// [$00]: Item Command ($2F + $23 => $52)
			/// [$01]: ?	($26 + $23 => $49?)
			/// [$02]: ?	($1D + $23 => $40?)
			/// [$03]: ?	($1F + $23 => $42?)
			/// [$04]: ?	($1E + $23 => $41?)
			///	[$05]: ?	($20 + $23 => $43?)
			///	[$06]: ?	($2F + $23 => $52)
			///	AE EB 6A D0 04 C9 79 F0 03 9D 00 04
			/// 
			/// $52 -> $9976 Character_GetCountAndNameIndices
			/// </summary>
			public static Address AdjustableHeightInstructions = new Address("Menu adjustable height instructions", 0x38DB2)
			{
				length = 7, // at least
				notes = "Add 0x23 and you get the DynamicSubroutine07 Index" +
				"[$00]: Item Command ($2F + $23 => $52) "
					+ "$23-> Character_GetCurrentHP"
					+ "$27-> $932C Character_GetCurrentMP"
					+ "$2D-> $93D0 Character_CheckStatus"
					+ "$3A-> $9586 TransferXPToQuickStorage"
					+ "$3C-> $95ED Character_GetTotalGold "
					+ "$4E-> $9944 GetCharacterClass_JMP"
					+ "$52-> $9976 Character_GetCountAndNameIndices"
				,
			};
			/// <summary>
			/// [$02]:$9074							[$0A]:$910E -> Status 2 Characters
			/// [$18]:$9CFE -> (#69)				[$26]:$93C7 ->
			/// [$28]:$93F1 -> Menu_MainCommand		[$2A]:$9420 -> (#15) [$34]:$94BB -> 
			/// [$2C]: Item character select
			/// [$36]:$94DB -> FIGHT.PARRY.ITEM.	[$69]: Item window	[$72]:$96A9 -> Enemy Display, Interactive
			/// [$7A]:$96DD -> Dialog Text			[$7C]:$96E9 -> (Someone died) Redirects to $96DD
			/// [$7E]:$96ED -> Enemy Display, Non-Interactive
			/// </summary>
			public static Address MenuPointers = new Address("Menu Adresses", 0x38F84)
			{
				notes = "There are at least 2 type of pointer in this list: "
				+ "1) normal instructions and 2) redirect to normal instructions.\n"
				+ "If byte 0 is >= $80, then it is case 2). Bytes 2 and 3 is the new menu pointer, "
				+ "and byte 1 is menuPositionA & B.\n"
				+ "How to read normal instructions:\n"
				+ "[$00]: "
				+ "[$01]: if >= 10, the height is dynamic. Otherwise, this number *2 == height of menu in 8 pixel lines.",
			};
			/// <summary>
			/// 0x39D2B
			/// </summary>
			public static Address MenuTitles = new Address("Menu titles", 0x39D2B);
			/// <summary>
			/// 0x3A346
			/// </summary>
			public static Address Menu_CloseAllMenus_Vector = new Address("Unknown", 0x3A346, 10);

			/// <summary>
			/// 0x3BA34
			/// </summary>
			public static Address LoadSpritesVector = new Address("Unknown", 0x3BA34);


			/* Bank F $3C000 (low default) */
			/// <summary>
			/// 0x3C000
			/// </summary>
			public static Address MainLowBank = new Address("", 0x3C000, 0x4000);
			/// <summary>
			/// 0x3C2AB
			/// </summary>
			public static Address MapScrollVectorB = new Address("Unknown", 0x3C2AB, 5)
			{
				notes = "Used in character sprite parser so...what is this?",
			};

			/// <summary>
			/// 0x3E917
			/// </summary>
			public static Address Load07BankIds_3C000 = new Address("DynamicSubroutine_BankIds_07", 0x3E917);
			/// <summary>
			/// 0x3E997
			/// </summary>
			public static Address Load17BankIds_3C000 = new Address("DynamicSubroutine_BankIds_17", 0x3E997);
			/// <summary>
			/// 0x3E9ED
			/// </summary>
			public static Address Load07PointerIndices_3C000 = new Address("DynamicSubroutine_PointerIndex_07", 0x3E9ED, 256);
			/// <summary>
			/// 0x3EAED
			/// </summary>
			public static Address Load17PointerIndices_3C000 = new Address("DynamicSubroutine_PointerIndex_17", 0x3EAED, 256);
			/// <summary>
			/// 0x3FFFA
			/// </summary>
			public static Address NMIPointer_3C000 = new Address("RESET address", 0x3FFFA, 2);
			/// <summary>
			/// 0x3FFFC
			/// </summary>
			public static Address RESETPointer_3C000 = new Address("RESET address", 0x3FFFC, 2);
			/// <summary>
			/// 0x3FFFE
			/// </summary>
			public static Address IRQBRKPointer_3C000 = new Address("IRQ/BRK address", 0x3FFFE, 2);





			/* Bank 16 $58000 */
			public static Address PaletterStoreOffsets = new Address(
				"Order in which palettes are written to $03E7 (PPU_BGPaletteColor_Store)", 0x58152);
			/// <summary>
			/// offsets to palettes for different times of day in DayNightPalettes vector (0x1B755)
			/// </summary>
			public static Address TimeOfDayDayNightPalettesIndices = new Address("", 0x583A3, 7);
			public static Address TimeOfDayChangeTimes = new Address("Time-of-Day Change Clock Values", 0x583AA, 7)
			{
				notes = "Clock Value: 00, 1E, 3C, 5A, 78 (night start), 96, B4",
			};

			/* Bank 1E $78000 */
			/// <summary>
			/// 0x78000
			/// <para>appear to be all APU related.</para>
			/// </summary>
			public static Address LocalPointers_78000 = new Address("", 0x78000, 16)
			{
				notes = "[$00]: $B365 APU_RunEngine\n"
					+ "[$04]: $B8C5 ResetAPU\n"
					+ "[$06]: $B904 APU_StartNewSequence\n"
					+ "[$0E]: $B35A\n",
			};
			/// <summary>
			/// 0x7816B
			/// </summary>
			public static Address APU_DutySetup_Addresses = new Address("Addresses to APU_DutySettings_Vector", 0x7816B, 52);
			/// <summary>
			/// 0x7835B
			/// </summary>
			public static Address APU_DutySettings_Vector = new Address("Duty setting data?", 0x7835B, 12);
			/// <summary>
			/// 0x7863E
			/// </summary>
			public static Address SequenceData = new Address("Start of music sequence data", 0x7863E);
			/// <summary>
			/// 0x79064
			/// </summary>
			public static Address SFX_Pointers = new Address("SFX pointers", 0x79064);


			/* Bank 1F $7C000 (high default) */
			/// <summary>
			/// 0x7C000
			/// </summary>
			public static Address MainHighBank = new Address("", 0x7C000, 0x4000);

			/// <summary>
			/// 0x7E917
			/// </summary>
			public static Address Load07BankIds_7C000 = new Address("DynamicSubroutine_BankIds_07", 0x7E917);
			/// <summary>
			/// 0x7E997
			/// </summary>
			public static Address Load17BankIds_7C000 = new Address("DynamicSubroutine_BankIds_17", 0x7E997);
			/// <summary>
			/// 0x7E9ED
			/// </summary>
			public static Address Load07PointerIndices_7C000 = new Address("DynamicSubroutine_PointerIndex_07", 0x7E9ED, 256);
			/// <summary>
			/// 0x7EAED
			/// </summary>
			public static Address Load17PointerIndices_7C000 = new Address("DynamicSubroutine_PointerIndex_17", 0x7EAED, 256);

			/// <summary>
			/// 0x7EC99. 4842c bytes that differ from LowMainBank ($1F).
			/// A lot of empty space (0xFF and 0xAA) but there is code in there too (APU related?).
			/// </summary>
			public static Address DiffFromBank0F = new Address(
				"This block is different from Bank 1F (0x3C000)", 0x7EC99, 4842);


			/// <summary>
			/// 0x7FFFA
			/// </summary>
			public static Address NMIPointer_7C000 = new Address("RESET address", 0x7FFFA, 2);
			/// <summary>
			/// 0x7FFFC
			/// </summary>
			public static Address RESETPointer_7C000 = new Address("RESET address", 0x7FFFC, 2);
			/// <summary>
			/// 0x7FFFE
			/// </summary>
			public static Address IRQBRKPointer_7C000 = new Address("IRQ/BRK address", 0x7FFFE, 2);






			public static Address GetBankAddress(byte bankId)
			{
				int address = bankId * 0x4000;
				return new Address(address.ToString(), address, 0x4000);
			}

			/// <summary>
			/// Does not include iNES header.
			/// </summary>
			/// <param name="bankId"></param>
			/// <returns>address of bank without iNES header</returns>
			public static int GetBankPointer(byte bankId)
			{
				return bankId * 0x4000;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="pointer">ROM-relative pointer (with iNES header)</param>
			/// <returns></returns>
			public static byte GetBankIdFromPointer(int pointer)
			{
				return (byte)((pointer - Address.iNESHeaderLength) / 0x4000);
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="romData"></param>
			/// <param name="bankId"></param>
			/// <param name="pointerAddress">rom address (with iNES header)</param>
			/// <returns>null if invalid address (ie. below 0x8000 or over 0xFFFF)
			/// or invalid bankId (ie. above 0x1F) or combination of both 
			/// (ie. bankId 0xXF and address below 0xC000)</returns>
			public static Address GetAddressAt(byte[] romData, byte bankId, int pointerAddress)
			{
				var gameRelativeAddr = GetPointerAt(romData, bankId, pointerAddress);
				if (gameRelativeAddr == -1)
					return null;
				var romRelativeAddr = gameRelativeAddr - Address.iNESHeaderLength;
				return new Address(romRelativeAddr.ToString(), romRelativeAddr);
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="romData"></param>
			/// <param name="bankId"></param>
			/// <param name="pointerAddress">address of low byte of pointer</param>
			/// <returns>ROM-relative address, or -1 if invalid ROM address (ie. below 0x8000 or over 0xFFFF)
			/// or invalid bankId (ie. above 0x1F) or combination of both 
			/// (ie. bankId 0xXF and address below 0xC000).</returns>
			public static int GetPointerAt(byte[] romData, byte bankId, int pointerAddress)
			{
				if (bankId > 0x1F)
					return -1;
				byte lowByte = romData[pointerAddress];
				int highByte = romData[pointerAddress + 1] << 8;
				int cpuAddr = highByte + lowByte;
				int cpuRelativeAddr;
				if (cpuAddr < 0x8000) // in RAM and not rom-relative address.
					return -1;
				else if (cpuAddr > 0xFFFF)
					return -1;
				else if (cpuAddr < 0xC000 && (bankId & 0x0F) == 0x0F) // this COULD be valid but I pray to all 
					return -1; // the gods of ASM programming that it is never the case
				else if (cpuAddr >= 0xC000) // in main bank (3C000 or 7C000)
					cpuRelativeAddr = (cpuAddr & 0x3FFF) + GetBankPointer((byte)(bankId | 0x0F));
				else
					cpuRelativeAddr = (cpuAddr & 0x3FFF) + GetBankPointer(bankId);
				return cpuRelativeAddr + Address.iNESHeaderLength;
			}

			public static int GetPointerAt(byte[] romData, int pointerAddress)
			{
				return (romData[pointerAddress + 1] << 8) + romData[pointerAddress];
			}
		}
	}
}
