using AtomosZ.DragonAid.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static AtomosZ.DragonAid.ReverseEngineering.ROMPlaceholders;

namespace AtomosZ.DragonAid.ReverseEngineering
{
	public static class Bank08000
	{
		/// <summary>
		/// $097AD <br/>
		/// First instructions bytes:<br/>
		///		$C9 - <br/>
		///		
		///	<br/>
		/// 2nd instruction bytes:<br/>
		///		$50 - <br/>
		///		$60 - <br/>
		///		
		/// <br/>
		/// </summary>
		/// <param name="bankId"></param>
		/// <returns></returns>
		private static byte SpriteDecoder_GetNextEncodedByte()
		{
			var a = Bank3C000.GetVariable_FromDynamic(0, 0xB4, zeroPage[0xB6]);

			Increment_AddressAt_B4();
			return a;
		}

		/// <summary>
		/// $097C2
		/// </summary>
		private static void Increment_AddressAt_B4()
		{
			ASMHelper.Add16Bit(1, ref zeroPage[0xB4], ref zeroPage[0xB5]);
		}

		private static void F97C9(byte x)
		{
			//theStack.Push(y);
			//theStack.Push(x);
			x >>= 3;
			var a = nesRam[0x56D + x];
			theStack.Push(a);
			a &= 0x07;
			x = a;
			a = theStack.Pop(out bool z, out bool n);
			a >>= 3;
			byte y = a;
			a = romData[0x9100 + Address.iNESHeaderLength]; // EncodedTable_03
			bool c;
			do
			{
				a = ASMHelper.ASL(a, 1, out c);
				--x;
			}
			while (x < 0x80);

			y = 0x02;
			if (c)
				++y;
			zeroPage[0xB6] = y;
			//x = theStack.Pop(out z, out n);
			//y = theStack.Pop(); // irrelevant for this context
		}

		/// <summary>
		/// $09C79
		/// </summary>
		private static void _SpriteDecoder_PrepareNextSprite_()
		{
			byte a;
			var _b4 = zeroPage[0xB4];
			var _b5 = zeroPage[0xB5];
			zeroPage[0xC5] = 0;
			zeroPage[0xC6] = 0xFF;
			zeroPage[0xC7] = 0xFF;

			// $09C89 __GetNextEncodedByte_loop
			do
			{
				a = Bank08000.SpriteDecoder_GetNextEncodedByte();
				if ((a & 0x40) == 0)
					Bank08000.Increment_AddressAt_B4();
				Bank08000.Increment_AddressAt_B4();
			}
			while (a <= 0x80);

			ASMHelper.LSR(a, 1, out bool c);
			a = ASMHelper.AND(a, 0x04, out bool z, out bool n);
			if (!z)
			{
				if (c)
				{
					a = Bank08000.SpriteDecoder_GetNextEncodedByte(); // get base byte
					zeroPage[0xC5] = a;
				}

				a = Bank08000.SpriteDecoder_GetNextEncodedByte(); // get low bit flag
				zeroPage[0xC6] = a;
				a = Bank08000.SpriteDecoder_GetNextEncodedByte(); // get high bit flag
				zeroPage[0xC7] = a;
			}

			Bank08000.SpriteDecoder(zeroPage[0xC5], zeroPage[0xC6], zeroPage[0xC7]);

			// _ReturnToFirstInstructionByte:
			zeroPage[0xB4] = _b4;
			zeroPage[0xB5] = _b5;
			zeroPage[0xC1] = 0;
			Bank08000.SpriteDecoder_GetNextSprite();
		}


		/// <summary>
		/// $09CB0
		/// </summary>
		/// <param name="baseByte">$C5</param>
		/// <param name="flagLow">$C6</param>
		/// <param name="flagHigh">$C7</param>
		private static void SpriteDecoder(byte baseByte, byte flagLow, byte flagHigh)
		{
			for (int x = 0; x < 0x10; ++x)
			{
				var a = baseByte;
				ASMHelper.ASL(flagLow, 1, out bool hasCarry);
				ASMHelper.ROL(flagHigh, 1, ref hasCarry);
				if (hasCarry)
					a = SpriteDecoder_GetNextEncodedByte();
				nesRam[0x400 + x] = a;  // this is the decoded sprite

				for (int y = 0; y < 8; ++y)
				{
					ASMHelper.ASL(a, 1, out hasCarry);
					ASMHelper.ROR(nesRam[0x410 + x], 1, ref hasCarry); // this is the reversed decoded sprite
				}
			}

			// copy the 32 bytes in 4 8-byte reversed chunks
			for (int x = 0; x < 0x20; ++x)
			{
				int y = x | 0x07;
				do
				{
					nesRam[0x420 + x] = nesRam[0x400 + x]; // copy 8 bytes in reverse
					--y;
				} while ((x & 0x07) != 0);
			}
		}

		private static void SpriteDecoder_GetNextSprite()
		{
			var a = Bank08000.SpriteDecoder_GetNextEncodedByte(); // get the 1st instruction byte of batch again
			zeroPage[0xC2] = a; // $C8
			a = ASMHelper.LSR(a, 1, out bool c); // $C8 >>1 = $64
			a = ASMHelper.AND(a, 0x03, out bool z, out bool n); //	$64 & 0000 0011 = $00 
			var x = a; // bit 1 and 2 of 1st instruction byte shifted right once
			++x;    // at most this can be 7
			a = ASMHelper.AND(zeroPage[0xC2], 0x40, out z, out n); // bit 6 of 1st instruction byte
			if (z)
			{
				// mystery stuff
			}

			// _SpriteDecoder_Parse2ndInstructionByte:
			a = Bank08000.SpriteDecoder_GetNextEncodedByte(); // 2nd instrucion byte
			a = ASMHelper.AND(a, 0x0F, out z, out n);
			ASMHelper.CMP(a, zeroPage[0xC4], out c, out z, out n);
			if (c)
				zeroPage[0xC4] = a;
			var y = x; // bit 1 and 2 of 1st instruction byte shifted right once
			++y;
			a = 0x20;
			a = ASMHelper.LSR(a, y, out c);
			y = a;
			a = ASMHelper.AND(a, zeroPage[0xC1], out z, out n);

			if (z)
			{
				a = y;
				zeroPage[0xC1] = ASMHelper.ORA(a, zeroPage[0xC1], out z, out n);
				y = 0x10;
				a = x;
				if (a != 0)
				{
					y = 0;
					++x; // at most this can be 8
				}

				zeroPage[ZeroPage.PPU_WriteAddress_1F + 1] = y; // either $10 or $00, depending on X. X = 0, then 10, otherwise 00. 
				a = --x; // at most this can be 7
				a = ASMHelper.ASL(a, 4, out c); // offset into PPU_WriteBlock_Address - this can be $70, $60, $50, $40, $30, $20, $10

				// set memory address to write next sprite to 
				c = false;
				zeroPage[ZeroPage.dynamicSubroutine_21 + 0] = ASMHelper.ADC(a, romData[ROM.PPU_WriteBlockAddress.pointer + 0], ref c); // extra bytes to transfer (over 0x10)
				a = 0;
				zeroPage[ZeroPage.dynamicSubroutine_21 + 1] = ASMHelper.ADC(a, romData[ROM.PPU_WriteBlockAddress.pointer + 1], ref c);

				a = 0x10; // base # of bytes to transfer
				zeroPage[ZeroPage.dynamicSubroutine_21 + 2] = ASMHelper.ADC(a, zeroPage[ZeroPage.dynamicSubroutine_21 + 2], ref c); // end low-byte address for read/write

				a = 0;
				zeroPage[ZeroPage.bankSwitch_LastBankIndex_24] = ASMHelper.ADC(a, zeroPage[ZeroPage.dynamicSubroutine_21 + 1], ref c);
				// $9D58
				zeroPage[ZeroPage.PPU_WriteAddress_1F + 0] = 0;
				a = zeroPage[ZeroPage.PPU_WriteAddress_1F + 1]; // either 00 or 10
				a = ASMHelper.ASL(a, 4, out c); // either 0 or 1
				a &= 0x01; // redundant but ok
				x = a;
				a = zeroPage[ZeroPage.PPU_NextTileIndex_BE + x]; // what is this and where is it from??
				zeroPage[ZeroPage.PPU_NextTileIndex_BE + x] = ASMHelper.INC(zeroPage[ZeroPage.PPU_NextTileIndex_BE + x], out z, out n);
				if (z)
				{
					// over?
					return;
				}

				// $9D69 _GetPPUAddressFromTileIndex:
				for (y = 4; y > 0; --y)
				{
					a = ASMHelper.LSR(a, 1, out c);
					zeroPage[ZeroPage.PPU_WriteAddress_1F + 0] = ASMHelper.ROR(zeroPage[ZeroPage.PPU_WriteAddress_1F + 0], 1, ref c);
				}

				zeroPage[ZeroPage.PPU_WriteAddress_1F + 1] = ASMHelper.ORA(a, zeroPage[ZeroPage.PPU_WriteAddress_1F + 1], out z, out n);

				a = zeroPage[ZeroPage.post_NMI_store_32];
				ASMHelper.CMP(a, 0xFD, out c, out z, out n);
				if (!z)
					Bank3C000.BattleInit_WriteToPPU();
			}

			_Check1stInstructionByte_();
		}

		/// <summary>
		/// $9D7E
		/// </summary>
		private static void _Check1stInstructionByte_(ref bool c)
		{
			byte a = zeroPage[0xC2]; // 1st instruction byte of batch
			if (a < 0x80)
			{
				SpriteDecoder_GetNextSprite();
				return;
			}

			MoveToNextDecodingFlags(a);
			zeroPage[ZeroPage.MonsterSpriteCount_C0] = ASMHelper.DEC(zeroPage[ZeroPage.MonsterSpriteCount_C0], out bool z, out bool n); // sprites remaining to be decoded
			if (!z)
			{
				_SpriteDecoder_PrepareNextSprite_(); // get next sprite
				return;
			}

			++zeroPage[0xC4];
			c = false;
			return; // exit sprite decoder here
		}

		/// <summary>
		/// 0x098B3<br/>
		/// </summary>
		private static void F98B3()
		{
			byte a = 0;
			byte x;
			for (x = 0; x < 0x70; ++x)
			{
				nesRam[NESRAM.PPU_WriteBlock_400 + x] = 0;
			}

			for (x = 0x0C; x > 0; --x)
			{
				nesRam[0x4C2] = a;
			}
		}

		/// <summary>
		/// $098C7<br/>
		/// Moves the address at B4 to the next encoding flags.
		/// </summary>
		private static void MoveToNextDecodingFlags(byte a)
		{
			a = ASMHelper.LSR(a, 1, out bool c);
			a = ASMHelper.AND(a, 0x04, out bool z, out bool n);

			if (z)
			{   // $98CC

				return;
			}

			if (!c)
			{
				Increment_AddressAt_B4();
			}

			// _GetPreviousBitFlags_()
			a = SpriteDecoder_GetNextEncodedByte(); // 1st bit flag
			var y = SpriteDecoder_GetNextEncodedByte(); // 2nd bit flag

			_MoveToNextDecodingFlags_(a);
			a = y;
			_MoveToNextDecodingFlags_(a);
		}

		private static void _MoveToNextDecodingFlags_(byte a)
		{
			do
			{
				a = ASMHelper.ASL(a, 1, out bool c);
				if (!c)
				{
					continue;
				}

				Increment_AddressAt_B4();
			}
			while (a != 0);
		}

		/// <summary>
		/// $9972
		/// </summary>
		private static void F9972(out bool c)
		{
			var x = zeroPage[0xB8];
			var y = zeroPage[0xB9];
			++y;
			F99A4(ref x, out c);
			if (c)
				return;
			var a = nesRam[0x697 + x];
			a |= 0x80;
			nesRam[0x697 + x] = a;
			F9E9E();
			c = false;
		}

		/// <summary>
		/// 0x09989<br/>
		/// DynamicSub 5F: 0x09989 (Bank 2)
		/// </summary>
		public static void DynamicSubroutine_09989()
		{
			Bank3C000.CopyStagedPalettesToPPU();
			byte x;

			// 998E
			for (x = 0; x < 0x20; ++x)
			{
				zeroPage[0xBA] = x;
				byte a = nesRam[0x697 + x];
				if (a >= 0x80)
				{
					F9E9E();
				}
				// L999A;
			}
		}

		/// <summary>
		/// $99CD
		/// Control flow should go directly to F9972 when this returns
		/// </summary>
		private static void LL99CD()
		{
			F99EC(ref c);
			if (c)
				return;

			var x = zeroPage[0xBA]; // what is this?
			var y = zeroPage[0xBB]; // sprite monster index?
			nesRam[0x699 + x] = nesRam[0x4CF + y]; // the low nybble, plus 1, of the last 2nd instruction byte in the encoded sprite
			var a = nesRam[0x4C0]; // what is this?
			if (a >= 0x80)
				return;
			F9D93(ref a, ref c);
			if (!c)
			{
				x = zeroPage[0xBA];
				nesRam[0x698 + x] = a;
			}
		}


		private static bool L99F8()
		{
			...
			bool c = ;
			var a = F9AA9(ref c);
			// 9A14
			var y = zeroPage[0xBB];
			var x = zeroPage[0xBA]; // what is this?
			if (c)
				return;
			//L9A1B
			nesRam[0x699 + x] = a; // the low nybble, plus 1, of the last 2nd instruction byte in the encoded sprite
			a = nesRam[0x4D2 + y];
			c = false;
			if (a == 0)
				return c;
			// L9A25
			...
			return c;
		}

		/// <summary>
		/// $9B6F
		/// </summary>
		private static void _Post_SpriteDecoder_(ref bool c)
		{
			var x = zeroPage[0xBB]; // sprite monster index?
			nesRam[0x4F5] = zeroPage[0xBD]; // what is this?? It's set to 0x02 in L9E08
			nesRam[0x4F6] = zeroPage[0xBC]; // what is this??
			nesRam[0x4CF + x] = zeroPage[0xC4]; // the low nybble, plus 1, of the last 2nd instruction byte in the encoded sprite
			nesRam[NESRAM.SpriteStart_PPUAddress_Table_4D0 + 1 + x] = nesRam[NESRAM.NextMonsterStart_PPUAddress_4F3 + 0];
			nesRam[NESRAM.SpriteStart_PPUAddress_Table_4D0 + 0 + x] = nesRam[NESRAM.NextMonsterStart_PPUAddress_4F3 + 1];
			nesRam[0x4D2 + x] = zeroPage[0xC3]; // what is this??
			nesRam[NESRAM.NextMonsterStart_PPUAddress_4F3 + 0] = zeroPage[ZeroPage.PPU_NextTileIndex_BE + 0];
			nesRam[NESRAM.NextMonsterStart_PPUAddress_4F3 + 1] = zeroPage[ZeroPage.PPU_NextTileIndex_BE + 1];
			c = false;
		}

		/// <summary>
		/// $9D93<br/>
		/// a is important when C:0<br/>
		/// Possible end values for a are:<br/>
		/// 0x4CF,X (C:1)
		/// 0x4C0
		/// ????
		/// 0xBD + 0x699,X (C:1)
		/// 0xBD
		/// </summary>
		private static void F9D93(ref byte a, ref bool c)
		{
			var x = zeroPage[0xBB]; // sprite monster index?
			a = nesRam[0x4CF + x]; // the low nybble, plus 1, of the last 2nd instruction byte in the encoded sprite
			if (a == 0)
			{
				c = true;
				return;
			}

			// L9D9C
			zeroPage[ZeroPage.MonsterSpriteCount_C0] = a;
			a = nesRam[0x4C0];
			if (a != 0)
			{
				if (a >= 0x80)
				{
					c = false;
					return;
				}
				....
			}

			// L9E08
			bool z, n;
			// iterates over the monster sprite meta-data and changes 0xBD if something something
			zeroPage[0xBD] = 0x02;
			for (x = 0; x < 0x20;)
			{
				if (nesRam[0x697 + x] >= 0x80) // what is this?
				{
					a = nesRam[0x698 + x]; // what is this? this gets set after this routine finishes
					c = true;
					a = ASMHelper.ADC(a, nesRam[0x699 + x], ref c); // 0x699 the low nybble, plus 1, of the last 2nd instruction byte in the encoded sprite
					ASMHelper.CMP(a, zeroPage[0xBD], out c, out z, out n);
					if (c)
						zeroPage[0xBD] = a;
				}

				x += 4;
			}

			a = zeroPage[0xBD];
			x = zeroPage[0xBA]; // what is this?
			a += nesRam[0x699 + x];
			ASMHelper.CMP(a, 0x1F, out c, out z, out n);

			if (c)
				return;

			a = zeroPage[0xBD];
			c = false;
		}

		/// <summary>
		/// 0x09E39
		/// </summary>
		private static void F9E39()
		{
			byte a = zeroPage[0xC1];
			a = ASMHelper.LSR(a, 1, out bool c);
			a |= 0x50;
			nesRam[NESRAM.menu_PositionA_470] = a;
			a = 0;
			a = ASMHelper.ROL(a, 1, ref c);
			c = true;
			byte x = zeroPage[0xBB];
			a = ASMHelper.ADC(a, nesRam[0x4CF + x], ref c);
			a = ASMHelper.LSR(a, 1, out c);
			a |= 0x40;
			nesRam[NESRAM.menu_WriteDimensions_471] = a;
		}

		/// <summary>
		/// 0xF9E51
		/// </summary>
		private static void F9E51()
		{
			byte a = zeroPage[0x471];
			a &= 0x0F;
			theStack.Push(a);
			a |= 0x20;
			nesRam[0x471] = a;
			Bank3C000.PPU_Copy2LinesAndAttributesTo_StagingArea();
		}

		/// <summary>
		/// 0x09E9E
		/// </summary>
		private static void F9E9E()
		{
			var y = zeroPage[0xBA];
			var a = nesRam[0x697 + y];
			a &= 0x03;
			zeroPage[0xBB] = a;
			a <<= 3;
			a += zeroPage[0xBB];
			zeroPage[0xBB] = a;
			var x = a;
			zeroPage[0xC5] = nesRam[0x4C1]; // what is this?
			zeroPage[0xB4] = nesRam[0x4D5 + 0 + x]; // address where variable is to be fetched
			zeroPage[0xB5] = nesRam[0x4D5 + 1 + x];
			zeroPage[ZeroPage.MonsterSpriteCount_C0] = nesRam[0x4D5 + 2 + x]; // sprite count for this monster?

			F97C9(x);
			a = nesRam[0x4C0];
			if (a >= 0x80)
			{
				L9E9C();
			}
			else if (a != 0)
			{ // used when doing palette stuff but not Slime sprite
				L9EF9(x, y);
			}
			else
				_Check1stInstructionByte_ForBit6_loop_();
		}

		/// <summary>
		/// 0x09ECE<br/>
		/// checks 1st instruction byte of every compressed sprite for bit 6 = 1<br/>
		/// for reasons still unknown
		/// </summary>
		private static void _Check1stInstructionByte_ForBit6_loop_()
		{
			bool z;
			byte a;
			do
			{
				a = SpriteDecoder_GetNextEncodedByte();
				theStack.Push(a);
				Increment_AddressAt_B4();
				a &= 0x40;
				if (a == 0)
				{
					a = zeroPage[0xC5];
					a <<= 2;
					var x = a;
					a = SpriteDecoder_GetNextEncodedByte();
					....
				}
				// L9EE9;
				a = theStack.Pop(out z, out bool n);
				if (!n)
					continue;
				MoveToNextDecodingFlags(a);
				ASMHelper.DEC(zeroPage[ZeroPage.MonsterSpriteCount_C0], out z, out n); // sprite count for this monster?
			} while (!z);

			a = zeroPage[0xC5];
			nesRam[0x4C1] = a; // what is this?
		}

		/// <summary>
		/// 0x09EF9
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private static void L9EF9(byte x, byte y)
		{
			byte a = nesRam[0x698 + y];
			zeroPage[0xC1] = a;
			zeroPage[0xC7] = nesRam[0x4D0 + 0 + x];
			++zeroPage[0xC7];
			zeroPage[0xC6] = nesRam[0x4D0 + 1 + x];
			++zeroPage[0xC6];
			F9E39();
			F98B3();
			Bank3C000.WaitForNMI_3C000();
			// L9F15:
			while (zeroPage[0xC0] > 0)
			{
				a = 0;
				zeroPage[0xC2] = a;
				L9F19();
				--zeroPage[0xC0];
			}
			// LA042:
			Bank3C000.WaitForNMI_3C000();
			F9E51();
		}


		/// <summary>
		/// 0x09F19
		/// </summary>
		private static void L9F19()
		{
			byte a = SpriteDecoder_GetNextEncodedByte();
			zeroPage[0xC3] = a;
			a &= 0x40;
			if (a != 0)
			{
				L9F7F();
				return;
			}

			...
		}



		private static void L9F7F()
		{
			byte a = zeroPage[0xC3]; // 1st instruction byte of compressed sprite
			a = ASMHelper.LSR(a, 4, out bool c);
			a &= 0x03;
			byte y = a;
			byte x = zeroPage[0xBB];
			a = nesRam[0x4D4 + x];
			++y;
			while (true)
			{
				if (--y == 0)
					break;
				a = ASMHelper.LSR(a, 2, out c);
			}

			a &= 0x03;
			zeroPage[0x1E] = a;
			a = SpriteDecoder_GetNextEncodedByte();
			theStack.Push(a);
			a &= 0x0F;
			zeroPage[0x04 + 0] = a;
			a = nesRam[NESRAM.menu_WriteDimensions_471]; // set back in F9E39
			a &= 0x0F;
			zeroPage[0x04 + 1] = a;
			a = theStack.Pop(out bool z, out bool n);
			a = ASMHelper.LSR(a, 4, out c);
			a &= 0x07;
			x = a;
			if (x != 0)
			{
				a = 0;
				while (x > 0) // multiply x by value at 0x05
				{
					c = false;
					a = ASMHelper.ADC(a, zeroPage[0x05], ref c);
					--x;
				}

				a = ASMHelper.ASL(a, 1, out c);
				c = false;
				zeroPage[0x04] = ASMHelper.ADC(a, zeroPage[0x04], ref c);
			}
			// L9FC1:
			a = zeroPage[0xC1];
			a &= 0x01;
			c = false;
			nesRam[0x4C2] = ASMHelper.ADC(a, zeroPage[0x04 + 0], ref c);
			a = zeroPage[0x05];
			zeroPage[0x06] = ASMHelper.ASL(a, 2, out c);
			a = nesRam[0x4C2]; // index to write next byte in PPU_WriteBlock_400

			x = 0;
			c = true;
			while (c) // subtract 0x06 from A until A goes negative
			{
				zeroPage[0x07] = a;
				++x;
				a = ASMHelper.SBC(a, zeroPage[0x06], ref c);
			}

			--x;
			a = 0;
			do
			{
				c = false;
				a += zeroPage[0x05];
			} while (--x > 0);

			zeroPage[0x06] = a; // remainder?
			zeroPage[0x05] <<= 1;
			a = zeroPage[0x07];
			c = true;
			a = ASMHelper.SBC(a, zeroPage[0x05], ref c);
			if (c)
				zeroPage[0x07] = a;
			a = zeroPage[0x07];
			a >>= 1;
			a += zeroPage[0x06];
			x = a;
			a = zeroPage[0x1E];
			ASMHelper.CMP(x, 0x0C, out c, out z, out n);
			if (!c)
				nesRam[0x460 + x] = a;
			else
				nesRam[0x4B7 + x] = a;
			// LA009:
			a = zeroPage[0xC3];
			a >>= 1;
			a &= 0x03;
			x = y = a;
			++y;
			a = 0x10;

			while (y > 0)
			{
				a >>= 1;
				--y;
			}

			zeroPage[0xC4] = a;
			a = ASMHelper.AND(a, zeroPage[0xC2], out z, out n);
			if (z)
			{
				a = zeroPage[0xC2];
				a |= zeroPage[0xC3];
				zeroPage[0xC2] = a;
				zeroPage[0xBC + x] = ++zeroPage[0xC7];
			}
			// LA029:
			a = zeroPage[0xBC + x];
			x = nesRam[0x4C2]; // index to write next byte in PPU_WriteBlock_400
			nesRam[NESRAM.PPU_WriteBlock_400 + x] = a;
			// LA031:
			a = zeroPage[0xC3];
			if (a < 0x80)
			{
				L9F19();
				return;
			}
			// LA038:
			MoveToNextDecodingFlags(a);
			ASMHelper.DEC(zeroPage[ZeroPage.MonsterSpriteCount_C0], out z, out n);
		}

		/// <summary>
		/// DynamicSub 5F: 0A13A (Bank 2)
		/// </summary>
		public static void BattleInit_0A13A()
		{
			byte y = zeroPage[0x62];
			if (++y != 0)
				CheckForMonsterReinforcements();
			Bank3C000.WaitForVBlank_HideSprites_SetLoopTrap();
			zeroPage[0x32] = 0xFD;
			nesRam[0x6B8] = 0;
			ClearMemAndInitPalette();
			BattleInit_BattleVariables_0();
			Bank3C000.Negate_LoopTragFlag();
		}

		/// <summary>
		/// 0x0A157<br/>
		/// </summary>
		private static void BattleInit_BattleVariables_0()
		{
			bool c;
			byte x;
			byte a = 0;
			nesRam[0x4C0] = a;
			nesRam[0x6B7] = a;
			for (x = 0; x < 0x04; ++x)
			{
				zeroPage[0xB8] = x;
				zeroPage[0xB9] = 0;
				a = nesRam[0x56D];
				if (a == 0xFF)
					continue;

				a = nesRam[0x571 + x];
				if (a == 0)
				{
					...
				}

				// __LA1A8_loop
				while (true)
				{
					F9972(out c);
					x = zeroPage[0xB8];
					if (c)
						break;
					a = ++zeroPage[0xB9];
					ASMHelper.CMP(a, nesRam[0x571 + x], out c, out bool z, out bool n);
					if (!c)
					{
						break;
					}
				}

				// LA1BA
			}

			x = 1;
			nesRam[0x4C1] = x; // what is this?
			if (nesRam[0x6B8] != 0)
			{
				LA25E();
				return;
			}

			byte a = 0xF0;
			--x;

			do // __SpriteDMA_SetEvery4thTo_F0_loop
			{
				nesRam[NESRAM.PPU_SpriteDMA_200 + x] = a;
				x += 4;
			} while (x != 0);

			// clear PPU WriteBlock and PaletteStore
			a = 0;
			x = 0x40;
			while (x != 0)
			{
				nesRam[NESRAM.PPU_SpritePalette_Store_3F4 + x] = a;
				--x;
			}

			zeroPage[0xBA] = 0;
			bool c;
			byte y;
			for (x = 0; x != 0x20; x += 4)
			{
				a = nesRam[0x697 + x];
				if (a < 0x80)
					continue;
				zeroPage[0xBB] = a;
				a &= 0x7F;
				nesRam[0x697 + x] = a;
				a &= 0x03;
				a <<= 3;
				zeroPage[0xBC] = a;
				a = zeroPage[0xBB];
				a >>= 2;
				a &= 0x07; // bits 4, 3, 2 of value at 0xBB, 0x697
				a |= zeroPage[0xBC];
				y = a;
				a = nesRam[0x699 + x];
				c = true;
				a = ASMHelper.ADC(a, zeroPage[0xBA], ref c);
				if (a >= 0x1E)
					continue;
				zeroPage[0xBA] = a;
				a = x;
				a |= 0x80;
				nesRam[NESRAM.PPU_WriteBlock_400 + y] = a;
			}

			// A232
			--zeroPage[0xBA];
			a = 0x20;
			c = true;
			ASMHelper.SBC(a, zeroPage[0xBA], ref c);
			a >>= 1;
			zeroPage[0xBA] = a;

			for (y = 0; y < 0x20; ++y)
			{
				x = nesRam[NESRAM.PPU_WriteBlock_400 + y];
				if (x < 0x80)
					continue;
				x &= 0x1F;
				a = nesRam[0x697 + x];
				a |= 0x80;
				nesRam[0x697 + x] = a;
				a = zeroPage[0xBA];
				nesRam[0x698 + x] = a;
				c = true;
				a = ASMHelper.ADC(a, nesRam[0x699 + x], ref c);
				nesRam[0xBA] = a;
			}

			a = 0x01;
			nesRam[0x4C0] = a;
			nesRam[0x6B7] = ASMHelper.LSR(nesRam[0x6B7], 1, out c);
		}


		/// <summary>
		/// 0x0A1D6<br/>
		/// </summary>
		private static void LA1D6(byte x)
		{

		}
	}
}