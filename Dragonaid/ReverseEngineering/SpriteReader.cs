using System;
using System.Collections.Generic;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.PointerList;
using AtomosZ.DragonAid.ReverseEngineering;

namespace AtomosZ.DragonAid.SpriteAid
{
	public class SpriteReader
	{
		public byte PPUStatus_2002;
		public byte[] zeroPages = new byte[0xFF];
		public byte[] theStack = new byte[0xFF];
		public byte[] loadedSpritesVectorCopy = new byte[0x0C];
		public byte[] characterStatus = new byte[0x08];
		private byte characterClassAndSex;
		private bool isCatSuitEquipped;
		private bool isSwimwearEquipped;

		private byte[] romData;

		/// <summary>
		/// 0x0300 to 0x03FF
		/// </summary>
		private byte[] ppuStagingArea = new byte[0xFF];

		/// <summary>
		/// 0x0200 to 0x02FF
		/// </summary>
		private byte[] ppuSpriteDMA = new byte[0xFF];

		public byte _06D7;
		public byte _06E1;
		/// <summary>
		/// menu_NumTitlesToWrite
		/// </summary>
		public byte _6ABE;
		private byte _0645;
		private byte _60B8;
		private byte _60C9;
		private byte _06D9;
		/// <summary>
		/// $6ABE
		/// </summary>
		private byte characterCount;
		/// <summary>
		/// 0x6ABF
		/// </summary>
		private byte classSpriteIndex;
		/// <summary>
		/// title_InstructionCode
		/// </summary>
		private byte _6AC0;
		/// <summary>
		/// menuTextRowCounter
		/// </summary>
		private byte _6AC1;
		/// <summary>
		/// menu_WriteNextLine_Variable
		/// </summary>
		private byte _6AC2;

		public byte Post_NMI_Const { get; private set; }


		/// <summary>
		/// Player Character sprites:	$6E00 (+$20 per character)
		/// ??? sprites:				$6F00
		/// Map Tiles:					$7300
		/// </summary>
		private Queue<byte> ppuInstructions = new Queue<byte>();

		private byte PPUAddr_2006;
		private byte PPUData_2007;

		private Address dynamicSubroutine
		{
			get { return new Address("Dynamic Subroutine", zeroPages[0x21] + (zeroPages[0x22] << 8)); }
		}
		/// <summary>
		/// zeroPages[0x25]
		/// </summary>
		private byte tileDynamicOffsetsOffset
		{
			get { return zeroPages[0x25]; }
			set { zeroPages[0x25] = value; }
		}
		/// <summary>
		/// zeroPages[0x4A]
		/// </summary>
		private byte tileBatchSpriteOrderIndex
		{
			get { return zeroPages[0x4A]; }
			set { zeroPages[0x4A] = value; }
		}
		/// <summary>
		/// zeroPages[0x4D] + zeroPages[0x4E].
		/// </summary>
		private int spritePointer
		{
			get { return zeroPages[0x4D] + (zeroPages[0x4E] << 8); }
			set
			{
				zeroPages[0x4D] = (byte)(value & 0xFF);
				zeroPages[0x4E] = (byte)(value >> 8);
			}
		}
		/// <summary>
		/// new byte[] { zeroPages[0x4F], zeroPages[0x50], zeroPages[0x51] };
		/// </summary>
		private byte[] instructionBytes
		{
			get { return new byte[] { zeroPages[0x4F], zeroPages[0x50], zeroPages[0x51] }; }
		}
		/// <summary>
		/// zeroPages[0x4F], zeroPages[0x50], zeroPages[0x51]
		/// </summary>
		/// <param name="i"></param>
		/// <param name="value"></param>
		private void SetInstructionByte(int i, byte value)
		{
			zeroPages[0x4F + i] = value;
		}
		/// <summary>
		/// zeroPages[0x52]
		/// </summary>
		private byte nextSpriteAddressOffset
		{
			get { return zeroPages[0x52]; }
			set { zeroPages[0x52] = value; }
		}
		/// <summary>
		/// num tiles written to PPU.
		/// zeroPages[0x53]
		/// </summary>
		private byte writeTileCount
		{
			get { return zeroPages[0x53]; }
			set { zeroPages[0x53] = value; }
		}
		/// <summary>
		/// zeroPages[0x55]
		/// </summary>
		private byte tileBankId
		{
			get { return zeroPages[0x55]; }
			set { zeroPages[0x55] = value; }
		}
		/// <summary>
		/// zeroPages[0x56]
		/// $00 CHR tile  $20, $80 sprites ($20 for player?)
		/// </summary>
		private byte baseTileIndex
		{
			get { return zeroPages[0x56]; }
			set { zeroPages[0x56] = value; }
		}
		/// <summary>
		/// 0x60? I had 0x59 before, length: 8 bytes
		/// </summary>
		private byte readTileAddressVector = 0x60;


		public void LoadSprites(byte[] romData)
		{ // BA16
			this.romData = romData;
			// wait for vblank and hide sprites
			F3E285();
			F3E299(0x12);
			F3E299(0x1F);

			for (int i = 0; i < 0x0C; ++i)
				loadedSpritesVectorCopy[i] = romData[Pointers.ROM.LoadSpritesVector.offset];
		}

		/// <summary>
		/// a == 0xFF ($B91A from a battle) or 0x00 ($B91E character died on map)
		/// </summary>
		/// <param name="romData"></param>
		/// <param name="a"></param>
		public void LoadCharacterSprites(byte[] romData, byte a, byte characterCount, byte characterClassAndSex,
			byte _60B8, byte _60C9, byte Post_NMI_Const, byte[] characterStatus, bool isCatSuitEquipped, bool isSwimwearEquipped)
		{ // B91A
			this.romData = romData;
			_6AC2 = a;
			this.characterCount = characterCount;
			_6AC0 = 0;
			_6AC1 = 0;
			this._60B8 = _60B8;
			this._60C9 = _60C9;
			this.Post_NMI_Const = Post_NMI_Const;

			this.characterStatus = characterStatus;
			this.characterClassAndSex = characterClassAndSex;
			this.isCatSuitEquipped = isCatSuitEquipped;
			this.isSwimwearEquipped = isSwimwearEquipped;
			GetCharacterSprite_Loop();
		}


		private void GetCharacterSprite_Loop()
		{
			zeroPages[0xCE] = _6AC1;
			if (characterStatus[0] < 0x80 || characterStatus[1] < 0x80)
			{
				GetCharacterSprite_Loop_ContinueOrBreak();
				return;
			}

			byte a;
			if ((_60C9 & 0x3F) != 0)
			{
				a = (byte)((_60B8 & 0x3F) + 0x0F);
				// B952
				classSpriteIndex = a;
			}
			// B957
			a = 0x0F;
			if ((Post_NMI_Const & 0x40) == 0x40)
			{
				//L3B952();
				// this is probably to wait for NMI to finish
			}

			// get character class
			classSpriteIndex = (byte)(characterClassAndSex & UniversalConsts.ClassMask);
			// get equipped items
			if (isCatSuitEquipped)
			{
				classSpriteIndex = UniversalConsts.AnimalSuitSpriteIndex;
			}
			else if ((characterClassAndSex * UniversalConsts.SexMask) == UniversalConsts.SexMask)
			{ // is female
				if (isSwimwearEquipped)
					classSpriteIndex = UniversalConsts.SwimwearSpriteIndex;
				else
					classSpriteIndex += UniversalConsts.FemaleClassSpriteOffset;
			}

			LoadSprites();
		}


		private void LoadSprites()
		{
			if ((_6AC2 & 0x40) == 0x40) // if bit 6 (overflow flag) is set
			{ // B9B0
			  //FetchSpriteDataFromBank05(classSpriteIndex, _6AC0);
			  // E2A8
				tileBatchSpriteOrderIndex = classSpriteIndex;
				zeroPages[0x4D] = _6AC0; // meta batch count?

				// LoadBank05_ResetPPULatch()
				byte x = (byte)(zeroPages[0x4D] * 5 + 0x0A);
				PPU_SetAddressForWrite_loadSomething(x);
				// L3E2F4()
				PrepTileBatchInstructions(tileBatchSpriteOrderIndex);
				// return bank switch

				F3B9FA_BRK_1725();
				GetCharacterSprite_Loop_ContinueOrBreak();
			}
			else
			{
				// E2C3();
			}
		}



		private void GetCharacterSprite_Loop_ContinueOrBreak()
		{
			++_6AC1;
			if (_6AC1 != _6ABE)
			{
				GetCharacterSprite_Loop();
			}
			else
			{
				// a character died on map screen
				// B9C1
				_6AC1 = 0;
				L3B9C6();
			}
		}

		private void L3B9C6()
		{
			zeroPages[0xCE] = _6AC1;
			// Get Character Statuses
			if (characterStatus[0] >= 0x80 && characterStatus[1] < 0x80)
			{
				byte a = 0x31;
				byte y = _6AC0;
				ASMHelper.BIT(a, _6AC2, out bool n, out bool v, out bool z);
				if (v)
				{
					FetchSpriteDataFromBank05();
				}
				else
				{ // B9E0
				  // F3E2C3(); 
					F3E2C9();
					// wait for NMI
				}
				//L3B9E9();
				F3B9FA_BRK_1725();
				L3B9EC();
			}
			// L3B9EC();
		}

		private void F3E285()
		{ // E285
		  // LoadBank05_ResetPPULatch()
			ppuSpriteDMA[0] = 0xFF;
			PPU_SetAddressForWrite_loadSomething(0x05);
			// return bank switch
		}

		private void PPU_SetAddressForWrite_loadSomething(byte x)
		{
			PPUAddr_2006 = romData[Pointers.ROM.PPUAddressTable.offset + x + 0]; // high byte of PPUAddress to write to next
			PPUAddr_2006 = romData[Pointers.ROM.PPUAddressTable.offset + x + 1]; // low byte ($0800)
			zeroPages[readTileAddressVector + 7] = 0;
			zeroPages[0x24] = romData[Pointers.ROM.PPUAddressTable.offset + x + 2];
			zeroPages[0x23] = romData[Pointers.ROM.PPUAddressTable.offset + x + 3]; // ($6E00, $6F00 for sprites, $7200 for CHR)
			baseTileIndex = romData[Pointers.ROM.PPUAddressTable.offset + x + 4]; // ($20,$80 for sprites, $00 for CHR)
		}

		private void F3E299(byte a)
		{
			tileBatchSpriteOrderIndex = (byte)(a & 0x0F);
			// LoadBank05_ResetPPULatch()
			// reset ppu latch
			a = PPUStatus_2002;
			// L3E2F4()
			// L3E2FA()
			PrepTileBatchInstructions(tileBatchSpriteOrderIndex);
			// ReturnBankSwitchFromStack();
		}


		private void PrepTileBatchInstructions(byte spriteIndex)
		{
			tileDynamicOffsetsOffset = spriteIndex;
			spriteIndex &= 0x3F;
			SetDynamicSubroutineToSpriteAddress(spriteIndex);
			byte a = 0x04;
			zeroPages[0x54] = a;
			tileBankId = (byte)(a << 1);
			a = romData[Pointers.ROM.TileBatchSomethingPointerA.offset]; // $ADD4
			byte y = romData[Pointers.ROM.TileBatchSomethingPointerA.offset + 1];
			ASMHelper.IncrementValueAtXBy_AandY(zeroPages, a, 0x21, y); // incrementing the dynamic subroutine
			a = tileDynamicOffsetsOffset;
			zeroPages[readTileAddressVector + 6] = 0;
			if ((a & 0x80) != 0x80)
				tileDynamicOffsetsOffset = 0x08;
			else
				tileDynamicOffsetsOffset = 0x00;
			// E4AF
			while (zeroPages[0x54] != 0)
			{
				y = 0;
				a = romData[Pointers.ROM.TileDynamicOffsets.offset + tileDynamicOffsetsOffset];
				if (a >= 0x80)
					--y;
				// E4B9
				++tileDynamicOffsetsOffset;
				ASMHelper.IncrementValueAtXBy_AandY(zeroPages, a, 0x21, y); // add a and y to dynamic subroutine
				PrepAndFetchNextTileBatch();

				if (tileDynamicOffsetsOffset >= 0x08)
				{
					PrepAndFetchNextTileBatch();
				}
				// E4CC
				ASMHelper.Add16Bit(a, ref zeroPages[0x21], ref zeroPages[0x22]); // add a to dynamic subroutine pointer

				if (tileDynamicOffsetsOffset < 0x08)
				{
					PrepAndFetchNextTileBatch();
				}

				++tileDynamicOffsetsOffset;
				--zeroPages[0x54];
			}
		}

		private void SetDynamicSubroutineToSpriteAddress(byte spriteIndex)
		{ // E46E
			zeroPages[0x21] = spriteIndex;
			zeroPages[0x22] = 0;
			ASMHelper.Add16Bit(spriteIndex, ref zeroPages[0x21], ref zeroPages[0x22]);
			ASMHelper.Add16Bit(spriteIndex, ref zeroPages[0x21], ref zeroPages[0x22]);
			// ^ sprite index * 3, basically
			for (int i = 0; i < 3; ++i)
			{ // roll into address space
				zeroPages[0x21] = ASMHelper.ASL(zeroPages[0x21], 1, out bool hasCarry);
				zeroPages[0x22] = ASMHelper.ROL(zeroPages[0x22], 1, ref hasCarry);
			}
		}

		private void PrepAndFetchNextTileBatch()
		{ // E4E3
			tileBatchSpriteOrderIndex = romData[Pointers.ROM.TileDynamicOffsets.offset + tileDynamicOffsetsOffset];
			GetTileBatchInstructions();
		}


		private void GetTileBatchInstructions()
		{
			for (int i = 0; i < 3; ++i) // get instruction bytes
				SetInstructionByte(i, romData[0x14000 + dynamicSubroutine.offset - 0x8000 + i]);
			byte a = (byte)(instructionBytes[0] >> 4); // roll high nibble down to low nibble
			if (a != 0x0F) // E549
			{
				if (tileBankId != 0x09)
					a += 0x0F;
				nextSpriteAddressOffset = (byte)(a * 3);
				byte y = (byte)((instructionBytes[0] & 0x03) | tileBatchSpriteOrderIndex);
				byte x = 0;
				while (x < 8) // set PPU addresses for sprites in batch
				{ // E561
					zeroPages[0x4D] = x; // low byte of spritePointer
					zeroPages[0x04 + x + 0] = instructionBytes[1]; // part of high and low address to sprite
					zeroPages[0x04 + x + 1] = y;
					a = romData[Pointers.ROM.OffsetsToNextSpriteInTileBatch.offset + nextSpriteAddressOffset];
					// E56E -- this is a simplified version of the jumping around in the ROM. It's right, don't worry.
					bool hasCarry = false;
					a = ASMHelper.ADC(a, instructionBytes[1], ref hasCarry);
					if (a < 0x80 && hasCarry) // these seems like a complicated way to do 16bit math
						++y;
					else if (a >= 0x80 && !hasCarry)
						--y;
					// E576
					SetInstructionByte(1, a);
					++nextSpriteAddressOffset;
					x += 2;
				}
			}
			else
			{ // E58E
				zeroPages[0x42] = instructionBytes[1];
				zeroPages[0x43] = 0;
				zeroPages[0x42] = ASMHelper.ASL(zeroPages[0x42], 2, out bool hasCarry);
				zeroPages[0x43] = ASMHelper.ROL(zeroPages[0x43], 1, ref hasCarry);
				// basically instructionBytes[1] * 3 stored as 16 bit int
				ASMHelper.Add16Bit(instructionBytes[1], ref zeroPages[0x42], ref zeroPages[0x43]);
				a = romData[Pointers.ROM.TileBatchSomethingPointerB.offset];
				byte y = romData[Pointers.ROM.TileBatchSomethingPointerB.offset + 1];
				ASMHelper.IncrementValueAtXBy_AandY(zeroPages, a, 0x42, y);

				int romAddress = 0x14000 + zeroPages[0x42] + (zeroPages[0x43] << 8) - 0x8000 + Address.iNESHeaderLength;
				nextSpriteAddressOffset = romData[romAddress];
				byte x = 0;
				y = 1;
				while (x < 8)
				{
					zeroPages[0x04 + x] = romData[romAddress + y];
					zeroPages[0x05 + x] = (byte)((nextSpriteAddressOffset & 0x03) | tileBatchSpriteOrderIndex);
					nextSpriteAddressOffset >>= 2;
					x += 2;
					++y;
				}
			}

			ParseTileBatch();
		}

		private void ParseTileBatch()
		{ // E5CA
			byte y = 0;
			byte a = 0;
			while (y < 0x08)
			{
				byte spriteAddressIndex = romData[Pointers.ROM.TileBatchSpriteOrder.offset + tileBatchSpriteOrderIndex];
				zeroPages[0x57 + y] = zeroPages[0x04 + spriteAddressIndex + 0]; // part of high and low address to sprite
				zeroPages[0x58 + y] = zeroPages[0x04 + spriteAddressIndex + 1]; // part of high address to sprite
				y >>= 1;
				a = instructionBytes[2];
				while (spriteAddressIndex > 0) // E5E5
				{
					a <<= 1;
					--spriteAddressIndex;
				}
				// E5E9
				zeroPages[0x44 + y] = (byte)(a & 0xC0); // results of instructionBytes[2]
				++y;
				y <<= 1; // this efficitively makes ++y equal y += 2
				++tileBatchSpriteOrderIndex;
			}
			// E5F8
			a = instructionBytes[0];
			bool hasCarry = true;
			a = ASMHelper.ROR(a, 2, ref hasCarry);
			SetInstructionByte(0, (byte)(a & 0x03)); // bits 2 and 3 slid to bits 0 and 1

			// E600
			nextSpriteAddressOffset = 0;
			writeTileCount = 0;
			while (writeTileCount < 0x08)
			{
				a = FindNextTileAddressSlotAndWriteToPPU(); // E606

				a += baseTileIndex;
				ppuInstructions.Enqueue(a);
				if (tileBankId == 0x08)
				{
					byte x = writeTileCount;
					x >>= 1;
					a = zeroPages[0x44 + x]; // results of 3rd instruction byte.

					x = tileBatchSpriteOrderIndex;
					if (x == 0x08 || x == 0x0C) // E61E
					{
						a = ASMHelper.ASL(a, 1, out hasCarry);
						if (hasCarry)
							a |= 0x40;
					}

					// E62B
					a |= instructionBytes[0]; // instructionBytes[0] bits 2 and 3 slid to bits 0 and 1
					ppuInstructions.Enqueue(a);
					++nextSpriteAddressOffset;
				}

				++nextSpriteAddressOffset;
				// next sprite address
				zeroPages[0x57] = zeroPages[readTileAddressVector + writeTileCount++]; // high nibble == low nibble of high byte, low nibble == high nibble of low byte
				zeroPages[0x58] = zeroPages[readTileAddressVector + writeTileCount++]; // low nibble == high nibble of high byte
			}

			if (tileBankId != 0x08)
				a = 0x04;
			else
				a = tileBankId;
			// add tileBankId to DynamicSubroutine+2 address
			ASMHelper.Add16Bit(a, ref zeroPages[0x23], ref zeroPages[0x24]); // calculate where to place sprite data in save ram
			if (romData[readTileAddressVector + 7] != 0)
			{
				L3E656();
			}
		}

		private void L3E656()
		{ // E656
			zeroPages[readTileAddressVector + 7] = 0x03;
			ppuStagingArea[0] = zeroPages[0x62];
			ppuStagingArea[2] = zeroPages[0x61];
			ppuStagingArea[1] = 0x40;
			_06D9 = 0x01;
			// Wait for PPU to load from staging area
			ASMHelper.Add16Bit(0x40, ref zeroPages[0x61], ref zeroPages[0x62]);
		}

		private byte FindNextTileAddressSlotAndWriteToPPU()
		{ // E678
			byte x;
			if (baseTileIndex < 0x80)
			{ // is first? is character sprite?
				zeroPages[0x4D] = zeroPages[0x57]; // high nibble == low nibble of high byte, low nibble == high nibble of low byte
				zeroPages[0x4E] = (byte)((zeroPages[0x58] & 0x03) | 0x08); // low nibble == high nibble of high byte
				x = zeroPages[readTileAddressVector + 6];
				zeroPages[readTileAddressVector + 6] += 2;
				PrepAddressAndWriteSpriteToPPU();
			}
			else // is CHR tile
			{ // E6961
				x = 0;
				do
				{
					byte a = ppuSpriteDMA[x];
					if (a >= 0x80)
					{ // a should equal 0xFF
						TransferTile(x);
						break;
					}
					if (a == zeroPages[0x58])
					{
						if (ppuSpriteDMA[x + 1] == zeroPages[0x57])
							break;
					}
					// E6A3
					x += 2;
				} while (x != 0);
			}

			return (byte)(x >> 1);
		}




		/// <summary>
		/// Only for CHR
		/// </summary>
		/// <param name="x"></param>
		private void TransferTile(int x)
		{ // E6AA
			byte a = zeroPages[0x58];
			ppuSpriteDMA[x] = a;
			// high byte of spritePointer before ASL x4
			zeroPages[0x4E] = (byte)((a & 0x03) | 0x08); // note the | 0x08 could be removed then the -0x8000 later would be unneccessary 
			ppuSpriteDMA[x + 1] = zeroPages[0x57];
			zeroPages[0x4D] = zeroPages[0x57]; // low byte of spritePointer before ROL x4
			ppuSpriteDMA[x + 2] = 0xFF;
			PrepAddressAndWriteSpriteToPPU();
		}

		private void PrepAddressAndWriteSpriteToPPU()
		{ // E6C1
			Address bankAddress = Pointers.ROM.GetBankAddressFromId(tileBankId);

			for (int i = 0; i < 4; ++i)
			{ // set spritePointer
				zeroPages[0x4D] = ASMHelper.ASL(zeroPages[0x4D], 1, out bool hasCarry);
				zeroPages[0x4E] = ASMHelper.ROL(zeroPages[0x4E], 1, ref hasCarry);
			}

			PPULoadPartialSprite(bankAddress, 0); // first 8 bytes of sprite
			PPULoadPartialSprite(bankAddress, 8); // second 8 bytes
		}

		private void PPULoadPartialSprite(Address bankAddress, byte baseIndex)
		{ // E6E2
			for (int i = 0; i < 8; ++i)
				zeroPages[0x04 + i] = romData[bankAddress.offset + spritePointer - 0x8000 + baseIndex + i];

			switch (zeroPages[0x58] >> 2)
			{
				case 0:
					WritePartialSpriteToPPU();
					return;

				// these seem rare
				case 0x01:
					// 36F8
					// E712()
					return;
				case 0x02:
					// E702();
					return;
				default:
					// C12CF();
					return;
			}
		}

		private void WritePartialSpriteToPPU()
		{
			if (zeroPages[readTileAddressVector + 7] != 0)
				CopyStoredPartialSpriteTo_PPUStagingArea(); // This happened when a character died on the map screen
			else // CopyStoredPartialSprite to PPU directily
				for (int i = 0; i < 8; ++i) // this happens going to map screen from battle
					PPUData_2007 = zeroPages[0x04 + i]; // write sprite data to PPU, 8bit line by 8bit line
		}


		private void CopyStoredPartialSpriteTo_PPUStagingArea()
		{
			byte x = 0;
			byte y = zeroPages[readTileAddressVector + 7];
			do
			{
				ppuStagingArea[y] = zeroPages[0x04 + x];
				++x;
				++y;
			} while (x != 0x7);

		}

		private void F3B9FA_BRK_1725()
		{
			// PHA zeroPages[0x90] // backup old value
			byte backup90 = zeroPages[0x90];
			zeroPages[0x90] = 0x01;
			Bank3C000.F3F116();
			++zeroPages[0x90];
			Bank3C000.F3F116();
			// WaitForNMI
			zeroPages[0x90] = backup90;
			// BRK 1725
			++_6AC0;
		}


		private void SetSegmentPointerToStack()
		{
			byte a = (byte)(zeroPages[0x90] & 0x0F);
			byte x = 0x08;
			if (a == 0x01)
			{
				x = 0x00;
			}
			else if (a != 0x02)
			{
				return;
			}

			zeroPages[0x72] = x;
			zeroPages[0x73] = 0x01;
			if ((zeroPages[0xAC] & 0x1F) != 0x00)
				L3F295();
			else
			{ // F109
				L3F1A4();
				L3F1A4();
				L3F295();
			}

		}

		private void L3F295()
		{
			if (zeroPages[0x2C] != 0x01)
			{
				byte x = zeroPages[0x2D];
				byte y = zeroPages[0x2E];
				zeroPages[0x80] = x;
				zeroPages[0x81] = y;

				// STA_WorldMapCoordsTo_06
				STAWorldMapCoordsAndDoSomething(x, y, out bool hasCarry);


				if (!hasCarry)
				{ // L3F2C3();
					F3F2F4(out x, out y);
					F3F2CC();
					F3F305(ref x, ref y);
					F3F2CC();
					return;
				}
			}


		}

		private void F3F305(ref byte x, ref byte y)
		{
			zeroPages[0x3C] = 0x05;
			x = 0x18;
			if (zeroPages[0x2C] == 0x02)
				x = 0;
			zeroPages[0x3D] = x;
			x = zeroPages[0x9E];
			y = zeroPages[0x9F];
			zeroPages[0x3E] = zeroPages[0xA0];
		}

		private void F3F2CC()
		{
			if ((zeroPages[0x2F] & 0x02) != 0)
				return;
			if (zeroPages[0x2C] == 0)
			{
				STAWorldMapCoordsAndDoSomething(zeroPages[0x80], zeroPages[0x81], out bool hasCarry);
				if (!hasCarry)
					return;
			}
			// L3F2DF()
		}

		private void F3F2F4(out byte x, out byte y)
		{
			x = zeroPages[0x9B];
			y = zeroPages[0x9C];
			zeroPages[0x3C] = 0x04;
			zeroPages[0x3D] = 0x14;
			zeroPages[0x9D] = zeroPages[0x3E];
		}

		private void STAWorldMapCoordsAndDoSomething(byte x, byte y, out bool hasCarry)
		{
			zeroPages[0x06] = zeroPages[0x2A]; // world X pos
			zeroPages[0x07] = zeroPages[0x2B]; // world Y pos
			L3F7E0(x, y, out hasCarry);
		}
		private void L3F7E0(byte x, byte y, out bool hasCarry)
		{
			zeroPages[0x08] = 0x08;
			zeroPages[0x09] = 0x07;
			zeroPages[0x04] = x;
			zeroPages[0x05] = y;

			hasCarry = false; // really?
			byte a = ASMHelper.SBC(zeroPages[0x06], zeroPages[0x08], ref hasCarry);
			if (!hasCarry)
				a = 0;
			if (a <= zeroPages[0x04])
			{
				//L3F7FD();
				hasCarry = false;
				a = ASMHelper.ADC(zeroPages[0x06], zeroPages[0x09], ref hasCarry);
				if (hasCarry)
					a = 0xFF;
				if (a < zeroPages[0x04])
					return;
				hasCarry = true;
				a = ASMHelper.SBC(zeroPages[0x07], zeroPages[0x09], ref hasCarry);
				if (!hasCarry)
					a = 0;
				if (a <= zeroPages[0x05])
				{
					//F81B();
				}
			}

		}

		private void L3F1A4()
		{ // F1A4
			byte x = (byte)(zeroPages[0x72] << 1);
			if (characterStatus[x] < 0x80)
				L3F1F8(); // character dead?
			else if ((characterStatus[x + 1] & 0x40) == 0)
				L3F1C5();
			else
			{ // character dead?
			  // PHA _0645 backup
				_0645 = 0x00;
				L3F1F5();
			}
		}

		private void L3F1C5()
		{
			if (zeroPages[0x72] < 0x10)
				L3F1F5();
			else
			{
				//EF34(zeroPages[0x80], zeroPages[0x81]);
			}
		}

		private void L3F1F5()
		{
			F3F259();
			L3F1F8();
		}

		private void F3F259()
		{ // F259
		  //int pointer = zeroPages[0x72] + (zeroPages[0x73] << 8); // this points to the stack
			byte a = (byte)(theStack[zeroPages[0x72] + 0x02] & 0x0F);

			// F3F8D7()
			byte backup = zeroPages[0x04];
			zeroPages[0x04] = 0;
			zeroPages[0x7C] = 0;
			zeroPages[0x7D] = 0x6E;
			for (int i = 0; i < 6; ++i)
			{
				a = ASMHelper.ASL(a, 1, out bool hasCarry);
				zeroPages[0x04] = ASMHelper.ROL(zeroPages[0x04], 1, ref hasCarry);
			}

			zeroPages[0x7C] += a;
			zeroPages[0x7D] += zeroPages[0x04];
			zeroPages[0x04] = backup;

			// F262
			backup = theStack[zeroPages[0x72] + 0x03];

			if (!(_06E1 < 0x07) && !(_06E1 >= 0x0A))
				a = 0x02;
			// F274
			// F3F909()
			a &= 0x03;
			a = ASMHelper.ASL(a, 4, out bool carry);
			if (_0645 >= 0x80)
			{
				a += 0x08;
			}

			//int pointer = zeroPages[0x72] + (zeroPages[0x73] << 8);
			//if (zeroPages[0x73] == 0x63)
			//{ // character sprites

			//}
			//else if (zeroPages[0x73] == 0x73)
			//{ // map tiles

			//}

			for (int i = 0; i < 8; ++i)
				zeroPages[0x74 + i] = ppuInstructions.Dequeue();

			// F277
			backup &= 0x3C;
			if (backup != 0 || zeroPages[0x72] == 0)
				L3F282(backup);
		}

		private void L3F282(byte y)
		{ // F282
			F3F385();
			L3F32F(y);
		}

		private void L3F32F(byte y)
		{ // F32F
			byte x = 0x04;
			L3C208(0x80, x, y); // compressed F3C1F9()

			x = (byte)(x + 0x04); // actually from L3C29B()
			y = zeroPages[0x21];
			if ((ppuSpriteDMA[4] & ppuSpriteDMA[7]) != 0xF8)
			{ // F340
				ppuSpriteDMA[5] = zeroPages[0x09];
				ppuSpriteDMA[6] = zeroPages[0x0A];
			}
			// F34A
			if ((ppuSpriteDMA[8] & ppuSpriteDMA[11]) != 0xF8)
			{ // F354
				ppuSpriteDMA[9] = zeroPages[0x0D];
				ppuSpriteDMA[10] = zeroPages[0x0E];
			}
			// F35E
			if ((ppuSpriteDMA[12] & ppuSpriteDMA[15]) != 0xF8)
			{ // F368
				ppuSpriteDMA[13] = zeroPages[0x11];
				ppuSpriteDMA[14] = zeroPages[0x12];
			}
		}

		/// <summary>
		/// This skips the jump to F3C1F9 (a = 0x80) or F3C1FD (a = 0xFF)
		/// </summary>
		/// <param name="a">F3C1F9 (a = 0x80) or F3C1FD (a = 0xFF)</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private void L3C208(byte a, byte x, byte y)
		{   // actually at C1FF
			zeroPages[0x22] = a;
			// C208
			zeroPages[0x1B] |= 0x01;
			a = y;
			zeroPages[0x21] = 0;
			y = 0x05;
			bool aSmallerThanMem = false;
			do
			{
				if (--y >= 80)
				{
					aSmallerThanMem = true;
					break;
				}
			} while (a < romData[Pointers.ROM.MapScrollVectorB.offset + y]);

			if (!aSmallerThanMem)
			{ // C222
				byte backupa = a; // PHA
				bool carrySet = true;
				y = ASMHelper.ROL(y, 1, ref carrySet);
				y -= _06D7;
				if (y > 0x80)
				{ // C22C
					carrySet = false;
					y = ASMHelper.ADC(y, 0x0A, ref carrySet);
				}
				// C22F
				y = ASMHelper.LSR(y, 1, out carrySet);
				if (!carrySet)
				{
					y ^= 0x7F;
					carrySet = false;
					y = ASMHelper.ADC(y, 0x85, ref carrySet);
				}
				// C237
				zeroPages[0x21] = y;
				y <<= 1;
				y += zeroPages[0x21];
				y = ASMHelper.ASL(y, 4, out carrySet);
				y += 0x10;
				zeroPages[0x21] = y;
				// PLA
				a = backupa;
				a -= 0x04;
				do
				{
					a = ASMHelper.SBC(a, 0x0C, ref carrySet);
				} while (carrySet);
			}
			// C251
			L3C251(a, x);
		}

		private void L3C251(byte a, byte x)
		{
			a = ASMHelper.ASL(a, 2, out bool hasCarry);
			hasCarry = false;
			a = ASMHelper.ADC(a, zeroPages[0x21], ref hasCarry);
			if (hasCarry)
				a += 0x0F;
			// C25A
			byte y = a;
			zeroPages[0x21] = y;
			if (zeroPages[0x22] == 0)
			{ // C28F
				do
				{
					ppuSpriteDMA[y++] = zeroPages[x++];
				} while ((y & 0x03) != 0);
			}
			else if (zeroPages[0x22] == 0x80)
			{
				// L3C274();
				a = ppuSpriteDMA[y];
				if ((a & ppuSpriteDMA[3 + y]) != 0xF8)
				{

					ppuSpriteDMA[++y] = zeroPages[++x];
					ppuSpriteDMA[++y] = zeroPages[++x];
				}
			}
			else
			{
				// C265
				do
				{
					zeroPages[x++] = ppuSpriteDMA[y++];
				} while ((y & 0x03) != 0);
			}
			// L3C29B()
			zeroPages[0x1B] &= 0x01 ^ 0xFF; // why not just 0xFE?
		}

		private void F3F385()
		{
			zeroPages[0x05] = zeroPages[0x74];
			zeroPages[0x06] = zeroPages[0x75];
			zeroPages[0x09] = zeroPages[0x76];
			zeroPages[0x0A] = zeroPages[0x77];
			zeroPages[0x0D] = zeroPages[0x78];
			zeroPages[0x0E] = zeroPages[0x79];
			zeroPages[0x11] = zeroPages[0x7A];
			zeroPages[0x12] = zeroPages[0x7B];
		}

		private void L3F1F8()
		{
			zeroPages[0x72] += 0x04;
		}

	}
}
