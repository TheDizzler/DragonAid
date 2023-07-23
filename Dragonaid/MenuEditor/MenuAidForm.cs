using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.PointerList;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AtomosZ.DragonAid.MenuAid
{
	public partial class MenuAidForm : Form
	{
		private byte[] romData;

		//private byte[] zeroPage = new byte[256];
		private byte[] ram = new byte[0x8000];

		private class PPUState
		{
			private bool addressLatch;
			private int writeAddress;

			/// <summary>
			/// $0000 Pattern table 0
			/// $1000 Pattern table 1
			/// 
			/// $2000 Name table 0
			/// $23C0 Attribute table 0
			/// $2400 Name table 1
			/// $27C0 Attribute table 1
			/// $2800 Name table 2
			/// $2BC0 Attribute table 2
			/// $2C00 Name table 3
			/// $2FC0 Attribute table 3
			/// 
			/// $3F00 BG palette
			/// $3F20 Sprite palette
			/// </summary>
			private byte[] memory = new byte[0x4000];
			private PictureBox picBox;
			private byte[] romData;
			private byte status = 0x00;

			public PPUState(byte[] romData, PictureBox image, int scale = 2)
			{
				this.romData = romData;

				this.picBox = image;
				picBox.Image = SpriteParser.GetSolidColor(0x10, new Size(32 * 8 * scale, 30 * 8 * scale));
				picBox.Size = picBox.Image.Size;
			}


			/// <summary>
			/// This controls which nametable/sprite table/BG pattern table to use
			/// as well as turning NMI on/off and some other things.<br/>
			/// Dummied for now.
			/// </summary>
			public byte ppuControl_2000 { get; set; }

			public byte ppuStatus_2002
			{
				get
				{
					addressLatch = false;
					return status;
				}
				set { status = value; }
			}

			public byte ppuAddr_2006
			{
				get
				{
					if (addressLatch)
						return (byte)(writeAddress << 8);
					return (byte)writeAddress;
				}
				set
				{
					if (!addressLatch)
					{
						writeAddress = value << 8;
					}
					else
					{
						writeAddress &= 0xFF00;
						writeAddress |= value;

						Debug.WriteLine("Write to: " + writeAddress.ToString("X4"));
					}
					addressLatch = !addressLatch;
				}
			}


			public byte ppuData_2007
			{
				get { throw new Exception(); }

				set
				{
					memory[writeAddress] = value;

					if (writeAddress >= 0x3F00)
					{   // palette
						Debug.WriteLine("palette " + writeAddress.ToString("X4") + " : " + value);

					}
					else if (writeAddress >= 0x2000)
					{
						if (writeAddress < 0x2020)
							Debug.WriteLine("" + writeAddress.ToString("X4") + " : " + value);
						if ((writeAddress & 0x03C0) >= 0x03C0)
						{ // attribute
						  //Debug.WriteLine("attribute");
						}
						else
						{   // name table
							// get position relative to nametable image topleft
							//byte xPos = (byte)(writeAddress & 0x1F);            // $2914 & $1F					= $14
							//byte yPos = (byte)((writeAddress & 0x03FF) >> 5);   // $2914 & $3FF = $100 LSR 5	= $8

							//// get palette data
							//byte attributeIndex = (byte)((yPos / 4 * 8) + (xPos / 4));    // 4 tiles per attribute, 8 attributes per line
							//int baseNametableAddress = writeAddress & 0xFC00;
							//int atttributeTableAddress = baseNametableAddress + 0x03C0;
						}
					}
					else
					{   // pattern table
						Debug.WriteLine("pattern table " + writeAddress.ToString("X4") + " : " + value);
					}

					++writeAddress;
				}
			}

			public void Render()
			{
				// nametable
				var baseAddr = 0x2000;
				for (int i = 0; i < 0x3C0; ++i)
				{
					var addr = baseAddr + i;
					if (addr == 0x21C8)
						Debug.WriteLine("nametable " + writeAddress.ToString("X4"));
					byte yPos = (byte)(i / 32);
					byte xPos = (byte)(i & 0x1F);

					// get palette data
					byte attributeIndex = (byte)((yPos / 4 * 8) + (xPos / 4));    // 4 tiles per attribute, 8 attributes per line
					int atttributeTableAddress = baseAddr + 0x03C0;
					var attributeAddress = atttributeTableAddress + attributeIndex;
					byte attributeData = memory[attributeAddress];

					byte xQuadrant = (byte)(xPos & 0x02);
					byte yQuadrant = (byte)((yPos >> 1) & 0x01);
					byte attributeQuadrant = (byte)(xQuadrant + yQuadrant);
					byte attribute = (byte)((attributeData >> (attributeQuadrant << 1)) & 0x03);
					//byte attr0 = (byte)((attributeData >> 0) & 0x03);	// lowest 2 bits are top-left quadrant
					//byte attr1 = (byte)((attributeData >> 2) & 0x03);	// top right
					//byte attr2 = (byte)((attributeData >> 4) & 0x03);	// bottom left
					//byte attr3 = (byte)((attributeData >> 6) & 0x03);	// hightest 2 bits are bottom-right quadrant

					byte x = (byte)(attribute << 2);

					var palette = new byte[4]
					{
								memory[0x3F00 + x],
								memory[0x3F01 + x],
								memory[0x3F02 + x],
								memory[0x3F03 + x],
					};

					var value = memory[addr];
					var chrTile = SpriteParser.GetCHR(romData, value, palette, 2);
					SpriteParser.AddToImage(picBox.Image, chrTile, xPos, yPos);
				}

				picBox.Refresh();
			}

			public int nextRenderAddress = 0x2000;
			public void RenderLine()
			{
				var baseAddress = nextRenderAddress & 0xFC00;
				for (int i = 0; i < 0x20; ++i)
				{
					if (nextRenderAddress == 0x21C8)
						Debug.WriteLine("nametable " + writeAddress.ToString("X4"));
					//var addr = baseAddr + i;
					//if (addr == 0x21C8)
					//Debug.WriteLine("nametable " + writeAddress.ToString("X4"));
					var offset = nextRenderAddress - baseAddress;
					byte yPos = (byte)((offset) / 32);
					byte xPos = (byte)(offset & 0x1F);

					// get palette data
					byte attributeIndex = (byte)((yPos / 4 * 8) + (xPos / 4));    // 4 tiles per attribute, 8 attributes per line
					int atttributeTableAddress = nextRenderAddress + 0x03C0;
					var attributeAddress = atttributeTableAddress + attributeIndex;
					byte attributeData = memory[attributeAddress];

					byte xQuadrant = (byte)(xPos & 0x02);
					byte yQuadrant = (byte)((yPos >> 1) & 0x01);
					byte attributeQuadrant = (byte)(xQuadrant + yQuadrant);
					byte attribute = (byte)((attributeData >> (attributeQuadrant << 1)) & 0x03);
					//byte attr0 = (byte)((attributeData >> 0) & 0x03);	// lowest 2 bits are top-left quadrant
					//byte attr1 = (byte)((attributeData >> 2) & 0x03);	// top right
					//byte attr2 = (byte)((attributeData >> 4) & 0x03);	// bottom left
					//byte attr3 = (byte)((attributeData >> 6) & 0x03);	// hightest 2 bits are bottom-right quadrant

					byte x = (byte)(attribute << 2);

					var palette = new byte[4]
					{
								memory[0x3F00 + x],
								memory[0x3F01 + x],
								memory[0x3F02 + x],
								memory[0x3F03 + x],
					};

					var value = memory[nextRenderAddress];
					var chrTile = SpriteParser.GetCHR(romData, (byte)(value), palette, 2);
					SpriteParser.AddToImage(picBox.Image, chrTile, xPos, yPos);
					++nextRenderAddress;
				}


				picBox.Refresh();
			}

		}

		private PPUState ppuState;


		public MenuAidForm()
		{
			InitializeComponent();

			romData = File.ReadAllBytes(@"D:\github\RomHacking\Working ROMs\Dragon Warrior 3 (U).nes");

			dialog_pictureBox.Image = SpriteParser.CreateAlphanumericTileSheet(romData);
			dialog_pictureBox.Size = dialog_pictureBox.Image.Size;

			ram[0xCE] = 0x02;
			ParseMenu(0x57);
		}

		public void ParseMenu(byte menuIndex)
		{
			dialog_pictureBox.Image.Dispose();
			dialog_pictureBox.Image = null;
			dialog_pictureBox.Update();

			ppuState = new PPUState(romData, dialog_pictureBox);

			// Menu_WriteToScreen
			// Menu_GetDimensions
			// Menu_GetDimensions_Parse

			int romMenuPointer = Pointers.ROM.GetPointerAt(romData, Pointers.ROM.MenuPointers.iNESAddress + menuIndex * 2);
			byte bankId = Pointers.ROM.GetBankIdFromPointer(Pointers.ROM.MenuPointers.iNESAddress);
			int prgMenuAddress = Pointers.ROM.ConvertCPUPointerToPRGAddress(romMenuPointer, bankId);

			ram[0x6AC7] = ram[0xCE];// character_FormationIndex_CE;
			ram[0x6D3] = 0x90; // PPUControl_2000_Settings_06D3 => 0x90 = nametable $2000, sprite table $0000, BG table $1000

			byte posData_470 = romData[prgMenuAddress + 2];
			byte posData_6AC8 = posData_470;

			if (romData[prgMenuAddress + 0] >= 0x80)
			{
				throw new Exception("Variable width menus not supported yet");
			}

			byte widthInSprites_6AE9 = romData[prgMenuAddress + 0];
			byte widthData_471 = (byte)((widthInSprites_6AE9 >> 1) | 0x10); // total sprites on a line + 1 ?

			byte heightData = romData[prgMenuAddress + 1];
			if (heightData >= 0x10)
			{
				throw new Exception("Variable height menus not supported yet");
			}

			byte writeToCounter_6AEA = heightData;
			ram[0x7D] = heightData;
			--ram[0x7D];
			//

			ram[0x8E] = 0xFF;
			ram[0x7F] = 0xFF;

			byte titleData = romData[prgMenuAddress + 4];
			bool c = false;
			byte titleCode_6AC6 = ASMHelper.ROL(titleData, 3, ref c);
			titleCode_6AC6 &= 0x03;


			byte lineLength_6AC2 = (byte)((widthInSprites_6AE9 << 1) + titleCode_6AC6 + 1);

			++titleCode_6AC6; // spaces between first letter of line and side border?
			byte nextPrintPos_6AEB = 0;
			byte textRowCounter_6AC1 = 0;
			ram[0x6AC9] = 0;
			ram[0x6ACA] = 0;
			byte instructionIndex_6AC3 = 0x0B;
			//

			if (menuIndex == 0x3D || menuIndex == 0x3E)
			{
				throw new Exception("combat text and 0x3E menus not supported yet");
			}
			//

			// Menu_ClearWriteBlock
			ClearLines(0x6F, 0);

			// WriteTopBorder
			ram[0x400 + 0] = 0x7A;
			int x = 1;
			for (; x < widthInSprites_6AE9 - 1; ++x)
			{
				ram[0x400 + x] = 0x78;
			}

			ram[0x400 + x] = 0x7D;
			++x;
			ram[0x400 + x] = 0x77;
			x += widthInSprites_6AE9;
			--x;
			ram[0x400 + x] = 0x7C;




			// Write Borders
			//dialog_pictureBox.Image = SpriteParser.GetSolidColor(0x0F, new Size(8 * widthInSprites_6AE9, 8 * heightData));
			//dialog_pictureBox.Size = dialog_pictureBox.Image.Size;

			//var topLeftCornerSprite = SpriteParser.GetCHR(romData, 0x7A);
			//SpriteParser.AddToImage(dialog_pictureBox.Image, topLeftCornerSprite, 0, 0);
			//var topRightCornerSprite = SpriteParser.GetCHR(romData, 0x7D);
			//SpriteParser.AddToImage(dialog_pictureBox.Image, topRightCornerSprite, widthInSprites_6AE9 - 1, 0);
			//var topBarSprite = SpriteParser.GetCHR(romData, 0x78);
			//for (int i = widthInSprites_6AE9 - 2; i > 0; --i)
			//	SpriteParser.AddToImage(dialog_pictureBox.Image, topBarSprite, i, 0);

			//var leftVerticalBarSprite = SpriteParser.GetCHR(romData, 0x77);
			//var rightVerticalBarSprite = SpriteParser.GetCHR(romData, 0x7C);
			//for (int i = 1; i < heightData - 1; ++i)
			//{
			//	SpriteParser.AddToImage(dialog_pictureBox.Image, leftVerticalBarSprite, 0, i);
			//	SpriteParser.AddToImage(dialog_pictureBox.Image, rightVerticalBarSprite, widthInSprites_6AE9 - 1, i);
			//}


			//var bottomLeftCornerSprite = SpriteParser.GetCHR(romData, 0x7B);
			//SpriteParser.AddToImage(dialog_pictureBox.Image, bottomLeftCornerSprite, 0, heightData - 1);
			//var bottomRightCornerSprite = SpriteParser.GetCHR(romData, 0x7F);
			//SpriteParser.AddToImage(dialog_pictureBox.Image, bottomRightCornerSprite, widthInSprites_6AE9 - 1, heightData - 1);
			//var bottomBarSprite = SpriteParser.GetCHR(romData, 0x7E);
			//for (int i = widthInSprites_6AE9 - 2; i > 0; --i)
			//	SpriteParser.AddToImage(dialog_pictureBox.Image, bottomBarSprite, i, heightData - 1);


			// Title and text
			byte instr3 = romData[prgMenuAddress + 3];
			if (instr3 >= 0x80)
			{
				throw new Exception("What ever this is it is not implemented yet");
			}

			if (menuIndex == 0x34 || menuIndex == 0x3B)
			{
				throw new Exception("Menus 0x34 and 0x3B not supported yet");
			}

			// Menu_ReadInstruction05
			byte instr5 = romData[prgMenuAddress + 5];
			if ((instr5 & 0x40) == 0x40)
			{
				ParseInstruction05((byte)(titleCode_6AC6 + widthInSprites_6AE9), prgMenuAddress, ref instructionIndex_6AC3,
					ref nextPrintPos_6AEB, ref textRowCounter_6AC1, widthInSprites_6AE9);
			}
			else
			{
				Debug.WriteLine("what does this mean?"); // main menu skips above code but Select Message Speed does not
			}

			do
			{
				// clear 2 lines
				Clear2Lines(widthInSprites_6AE9);
				// write vertical bars
				WriteBorder(widthInSprites_6AE9, writeToCounter_6AEA);

				WriteNextLine(writeToCounter_6AEA, lineLength_6AC2, prgMenuAddress, ref instructionIndex_6AC3,
					ref nextPrintPos_6AEB, ref textRowCounter_6AC1, widthInSprites_6AE9, titleCode_6AC6);

				Copy2LinesToStagingArea(nextPrintPos_6AEB, posData_470, widthData_471);
				// slide lines up
				SlideUp2Lines(ref posData_470, widthInSprites_6AE9);
				//return;
			} while (--writeToCounter_6AEA > 0);

			//ppuState.Render();
		}


		private void Copy2LinesToStagingArea(byte nextPrintPos_6AEB, byte posData_470, byte widthData_471)
		{
			byte pha = 0;

			//GetPPUPos
			ram[0x24] = 0;


			byte xPos_04 = ASMHelper.ASL(posData_470, 1, out bool c);
			xPos_04 &= 0x1E;
			byte yPos_05 = ASMHelper.LSR(posData_470, 3, out c);
			yPos_05 &= 0x1E;

			// setPPUAddress
			byte highBytePosInPixels_20 = 0x20;
			byte lowBytePosInPixels_1F = ASMHelper.ASL(xPos_04, 3, out c);
			// add lowBytePosInPixels_1F with x scroll. With menus this will always be 0, so no C

			// _L3C2ED_
			if (xPos_04 == 0x08)
				lowBytePosInPixels_1F = lowBytePosInPixels_1F;
			byte a = yPos_05;
			ASMHelper.CMP(yPos_05, 0x1E, out c, out bool z, out bool n);
			if (c)
				a = ASMHelper.SBC(a, 0x1E, ref c);
			a = ASMHelper.LSR(a, 1, out c);
			lowBytePosInPixels_1F = ASMHelper.ROR(lowBytePosInPixels_1F, 1, ref c);
			a = ASMHelper.LSR(a, 1, out c);
			lowBytePosInPixels_1F = ASMHelper.ROR(lowBytePosInPixels_1F, 1, ref c);
			a = ASMHelper.LSR(a, 1, out c);
			lowBytePosInPixels_1F = ASMHelper.ROR(lowBytePosInPixels_1F, 1, ref c);

			a |= highBytePosInPixels_20;
			highBytePosInPixels_20 = a;

			if (highBytePosInPixels_20 == 0x20)
				//if (lowBytePosInPixels_1F < 0x20)
				lowBytePosInPixels_1F = lowBytePosInPixels_1F;
			//

			if (pha != 0)
			{
				throw new Exception("Whatever this is it's not implemented yet");
				return;
			}

			ram[0x25] = 0;
			ram[0x26] = 0;

			pha = widthData_471;

			byte lineCount_22 = widthData_471;
			lineCount_22 &= 0xF0;
			lineCount_22 = ASMHelper.LSR(lineCount_22, 3, out c);

			byte lineLength_23 = pha;
			lineLength_23 &= 0x0F;
			lineLength_23 = ASMHelper.ASL(lineLength_23, 1, out c);
			ram[0x27] = lineLength_23;

			int count = 0;

			byte x = ram[0x6D8];// should be 0 at this point, yeah?
			while (lineCount_22 > 0)
			{
				byte ppuAddressHighByte_09 = highBytePosInPixels_20;
				byte ppuAddressLowByte_08 = lowBytePosInPixels_1F;
				xPos_04 = (byte)(lowBytePosInPixels_1F & 0x1F);
				byte numSpritesOnRow_06 = (byte)(0x20 - xPos_04);
				c = true;
				byte _07 = ASMHelper.SBC(lineLength_23, numSpritesOnRow_06, ref c, out n, out z);
				if (z || !c)
				{   // _L3C7A2
					numSpritesOnRow_06 = lineLength_23;
				}
				else if (c)
				{
					// _L3C7A9_
				}


				// _Menu_CopyLine_AndPrepNextLine_
				// Menu_CopyLineAndAttributesTo_StagingArea
				if (++ram[0x24] >= 0x08)
				{
					ram[0x24] = 0;
					// WaitForPPUToLoadFromStagingArea
				}

				a = (byte)(ppuAddressHighByte_09 | 0x80);
				ram[0x300 + x] = a;
				ram[0x301 + x] = (byte)(numSpritesOnRow_06);
				ram[0x302 + x] = ppuAddressLowByte_08;
				x += 3;
				pha = numSpritesOnRow_06;
				byte y = ram[0x25];
				for (; numSpritesOnRow_06 > 0; --numSpritesOnRow_06)
				{
					ram[0x300 + x] = ram[0x400 + y];
					++x;
					++y;
				}

				ram[0x25] = y;

				numSpritesOnRow_06 = ASMHelper.LSR(pha, 1, out c);
				if ((lineCount_22 & 0x01) != 0)
				{
					y = ram[0x26];
					byte ppuAddressHighByte_11 = ppuAddressHighByte_09;
					byte ppuAddressLowByte_10 = ppuAddressLowByte_08;


					for (; numSpritesOnRow_06 > 0; --numSpritesOnRow_06)
					{
						byte pha_x = x;
						byte pha_y = y;
						byte pha_09 = ppuAddressHighByte_09;
						byte _0F = ram[0x460];
						byte _13;
						byte attributeAddressLowByte_12;
						// GetAttributeValue()
						{
							ppuAddressHighByte_09 = ASMHelper.LSR((byte)(0x1F & ppuAddressLowByte_08), 2, out c);
							ppuAddressHighByte_09 |= ASMHelper.LSR((byte)(0x80 & ppuAddressLowByte_08), 4, out c);
							a = 0x03;
							a &= ppuAddressHighByte_11;
							a = ASMHelper.ASL(a, 4, out c);
							a |= 0xC0;
							a |= ppuAddressHighByte_09;
							attributeAddressLowByte_12 = a;

							if (ppuAddressHighByte_11 >= 0x24)
								_13 = 0x27; // attribute table 1
							else
								_13 = 0x23; // attribute table 0

							byte _0C = ASMHelper.LSR((byte)(ppuAddressLowByte_10 & 0x40), 4, out c);
							_0C |= (byte)(ppuAddressLowByte_10 & 0x02);
							c = true;
							x = ASMHelper.SBC(attributeAddressLowByte_12, 0xC0, ref c);

							if (_13 != 0x23)
							{
								x += 0x40;
							}

							byte _0D = ram[0x480 + x];
							a = 0x03;
							y = _0C;

							while (y > 0)
							{
								a = ASMHelper.ASL(a, 1, out c);
								_0F = ASMHelper.ASL(_0F, 1, out c);
								--y;
							}

							a ^= 0xFF;
							a &= _0D;
							a |= _0F;
							ram[0x480 + x] = a;
						}

						xPos_04 = a; // attribute value
						ppuAddressHighByte_09 = pha_09;
						y = pha_y;
						x = pha_x;

						ram[0x300 + x] = _13; // high byte attribute address
						++x;
						ram[0x300 + x] = attributeAddressLowByte_12; // low byte attribute address
						++x;
						ram[0x300 + x] = xPos_04;
						++x;
						++y;
						ppuAddressLowByte_10 += 2;
						++ram[0x6D9];
						++count;
					}

					ram[0x26] = y;
				}
				//_Menu_CopyLineTo_StagingArea_end_
				++ram[0x6D9];

				if (highBytePosInPixels_20 == 0x23)
					a = a;
				ASMHelper.CMP((byte)(highBytePosInPixels_20 & 0xFB), 0x23, out c, out z, out n);
				if (c)
				{
					ASMHelper.CMP(lowBytePosInPixels_1F, 0xA0, out c, out z, out n);
					if (!c)
					{
						// do stuff that is probably not relevant
						lowBytePosInPixels_1F &= 0x1F;
						a = (byte)(highBytePosInPixels_20 & 0xFC);
					}
				}
				else
				{
					// _L3C7DF_
					c = false;
					lowBytePosInPixels_1F = ASMHelper.ADC(lowBytePosInPixels_1F, 0x20, ref c);
					a = ASMHelper.ADC(highBytePosInPixels_20, 0x00, ref c);
				}

				highBytePosInPixels_20 = a;

				--lineCount_22;
			}

			ram[0x6D8] = x;

			// WaitForNMI
			NMI();
		}

		private void NMI()
		{
			byte x = 0;
			byte a = ppuState.ppuStatus_2002;
			a = ram[0x6D9];

			while (ram[0x6D9] > 0) // if 0, jump ahead to palette setup
			{ // draw one line
				byte numTiles = 0x01;
				a = ram[0x300 + x];
				if (a >= 0x80)
				{
					ppuState.ppuControl_2000 = (byte)((ASMHelper.LSR(a, 4, out bool c) & 0x40) | ram[0x6D3]);
					++x;

					numTiles = ram[0x300 + x];
					a &= 0x3F;
				}
				//_L3CACC_
				if (a == 0x20)
					x = x;
				++x;
				ppuState.ppuAddr_2006 = a; // write high-byte
				a = ram[0x300 + x++];
				ppuState.ppuAddr_2006 = a; // write low-byte

				while (numTiles > 0) // draw each tile on line
				{
					a = ram[0x300 + x];
					++x;
					ppuState.ppuData_2007 = a;
					--numTiles;
				}
				--ram[0x6D9];
			}

			// 3CAE6 Setup Palettes
			ppuState.ppuAddr_2006 = 0x3F;
			ppuState.ppuAddr_2006 = 0x00;
			ppuState.ppuData_2007 = 0x0F; // black
			ppuState.ppuAddr_2006 = 0x00;
			ppuState.ppuAddr_2006 = 0x00;
			ram[0x6D8] = 0x00;
			ram[0x6DA] = 0x00;


		}

		private void WriteBorder(byte widthInSprites_6AE9, byte writeToCounter_6AEA)
		{
			byte x = ASMHelper.ASL(widthInSprites_6AE9, 1, out bool c);
			WriteVerticalBars(ref x, widthInSprites_6AE9);
			++x;
			if (writeToCounter_6AEA != 0x02)
			{
				WriteVerticalBars(ref x, widthInSprites_6AE9);
				return;
			}

			// Close off border
			ram[0x400 + x] = 0x7B;  // bottom left border
			x += widthInSprites_6AE9;
			--x;
			ram[0x400 + x] = 0x7F;  // bottom right border
			--x;
			byte y = widthInSprites_6AE9;
			y -= 2;

			for (; y > 0; --y, --x)
				ram[0x400 + x] = 0x7E; // bottom border
		}

		private void WriteVerticalBars(ref byte x, byte widthInSprites_6AE9)
		{
			ram[0x400 + x] = 0x77;  // left vertical border
			x += widthInSprites_6AE9;
			--x;
			ram[0x400 + x] = 0x7C;  // right vertical border
		}

		/// <summary>
		/// $38DDD
		/// </summary>
		/// <param name="widthInSprites_6AE9"></param>
		private void Clear2Lines(byte widthInSprites_6AE9)
		{
			byte x = ASMHelper.ASL(widthInSprites_6AE9, 1, out bool c);
			ClearLines((byte)(x - 1), x);
		}

		private void ClearLines(byte totalBytesToClear, byte offsetFrom400)
		{
			for (int x = 0; x <= totalBytesToClear; ++x)
			{
				ram[0x400 + offsetFrom400 + x] = 0;
			}
		}

		private void SlideUp2Lines(ref byte posData_470, byte widthInSprites_6AE9)
		{
			posData_470 += 0x10;
			byte y = ASMHelper.ASL(widthInSprites_6AE9, 1, out bool c);
			byte x = ASMHelper.ASL(y, 1, out c);

			for (; y > 0; --y, --x)
			{
				ram[0x3FF + y] = ram[0x3FF + x];
			}
		}

		private void WriteNextLine(byte writeToCounter_6AEA, byte lineLength_6AC2, int prgMenuAddress,
			ref byte instructionIndex_6AC3, ref byte nextPrintPos_6AEB, ref byte textRowCounter_6AC1,
			byte widthInSprites_6AE9, byte titleCode)
		{
			if (writeToCounter_6AEA == 0x01)
				return;

			ParseInstruction05(lineLength_6AC2, prgMenuAddress, ref instructionIndex_6AC3,
					ref nextPrintPos_6AEB, ref textRowCounter_6AC1, widthInSprites_6AE9);

			if (writeToCounter_6AEA == 0x02)
			{
				return;
			}

			if ((romData[prgMenuAddress + 5] & 0x40) == 0)
			{
				return;
			}

			// not sure the significance of this since it gets reset on the next line
			// move next char pos to next line, creating a space between lines ?
			nextPrintPos_6AEB = (byte)(lineLength_6AC2 + widthInSprites_6AE9);

			ParseNextInstructionByte(prgMenuAddress, ref instructionIndex_6AC3,
				ref nextPrintPos_6AEB, ref textRowCounter_6AC1, widthInSprites_6AE9);
		}

		private void ParseInstruction05(byte a, int prgMenuAddress, ref byte instructionIndex_6AC3,
			ref byte nextPrintPos_6AEB, ref byte textRowCounter_6AC1, byte widthInSprites_6AE9)
		{
			nextPrintPos_6AEB = a;
			if (romData[prgMenuAddress + 5] >= 0x80)
			{
				throw new Exception("What ever this is, it is not implemented yet");
			}

			ParseNextInstructionByte(prgMenuAddress, ref instructionIndex_6AC3,
				ref nextPrintPos_6AEB, ref textRowCounter_6AC1, widthInSprites_6AE9);
		}


		private void ParseNextInstructionByte(int prgMenuAddress, ref byte instructionIndex_6AC3,
			ref byte nextPrintPos_6AEB, ref byte textRowCounter_6AC1, byte widthInSprites_6AE9)
		{
			byte nextInstr_6AC4 = GetNextInstructionByte(prgMenuAddress, ref instructionIndex_6AC3);
			while (nextInstr_6AC4 < 0x80)
			{ // printable character!
				WriteNextChar(nextInstr_6AC4, widthInSprites_6AE9, ref nextPrintPos_6AEB);
				nextInstr_6AC4 = GetNextInstructionByte(prgMenuAddress, ref instructionIndex_6AC3);
			}

			if (nextInstr_6AC4 == 0xFF)
			{
				++textRowCounter_6AC1;
				return;
			}

			nextInstr_6AC4 &= 0x1F;
			switch (nextInstr_6AC4)
			{
				case 0x19:
				{
					// Get Save Game name
					var saveGameIndex = GetIndex(ParseIndex(nextInstr_6AC4), prgMenuAddress);
					var quickStorage04 = new byte[8] // name = "NotRoto" + saveGameIndex;
					{
						0x32,
						0x19,
						0x1E,
						0x36,
						0x19,
						0x1E,
						0x19,
						(byte)(saveGameIndex + 2)
					};

					// F38B52
					nextInstr_6AC4 = GetNextInstructionByte(prgMenuAddress, ref instructionIndex_6AC3);
					WriteTextFromStorage(widthInSprites_6AE9, ref nextPrintPos_6AEB, ref nextInstr_6AC4, quickStorage04);
					ParseNextInstructionByte(prgMenuAddress, ref instructionIndex_6AC3,
						ref nextPrintPos_6AEB, ref textRowCounter_6AC1, widthInSprites_6AE9); // this should always be 0xFF, right?
				}
				break;


			}
		}

		private void WriteNextChar(byte nextInstr_6AC4, byte widthInSprites_6AE9, ref byte nextPrintPos_6AEB)
		{
			if (nextPrintPos_6AEB == 0 && nextInstr_6AC4 == 0x79)
			{
				throw new Exception("What ever this is is not implemented yet");
				return;
			}

			//var charSprite = SpriteParser.GetCHR(romData, nextInstr_6AC4);
			//byte xPos = (byte)(nextPrintPos_6AEB % widthInSprites_6AE9);
			//byte yPos = (byte)(nextPrintPos_6AEB / widthInSprites_6AE9); // could use textRowCounter_6AC1 +1 as well?
			//SpriteParser.AddToImage(dialog_pictureBox.Image, charSprite, xPos, yPos);

			ram[0x400 + nextPrintPos_6AEB] = nextInstr_6AC4;
			++nextPrintPos_6AEB;

			if (nextInstr_6AC4 == 0x64)
			{
				throw new Exception("What ever this is is not implemented yet");
			}
		}


		private void WriteTextFromStorage(byte widthInSprites_6AE9, ref byte nextPrintPos_6AEB,
			ref byte nextInstr_6AC4, byte[] quickStorage04)
		{
			nextInstr_6AC4 = ASMHelper.LSR(nextInstr_6AC4, 4, out bool c);
			for (int i = 0; i < nextInstr_6AC4; ++i)
			{
				if ((quickStorage04[i] & 0xF0) == 0xA0)
				{
					// special endings (ex: plural)
					return;
				}

				WriteNextChar(nextInstr_6AC4, widthInSprites_6AE9, ref nextPrintPos_6AEB);
			}
		}


		private byte GetIndex(byte transformedByte, int prgMenuAddress)
		{
			byte _CE = transformedByte;
			byte instr8 = romData[prgMenuAddress + 8];
			if ((instr8 &= 0x40) == 0x40)
			{
				//_CE = _6AC7
			}

			return _CE;
		}

		/// <summary>
		/// Set _CE to current value A UNLESS bit 6 is set, then store _6AC7 in _CE.
		/// </summary>
		/// <param name="nextInstr_6AC4"></param>
		/// <returns></returns>
		private byte ParseIndex(byte nextInstr_6AC4)
		{
			byte result = (byte)(nextInstr_6AC4 & 0x60);
			bool c = false;
			result = ASMHelper.ROL(result, 4, ref c);
			return result;
		}


		private byte GetNextInstructionByte(int prgMenuAddress, ref byte instructionIndex_6AC3)
		{
			byte result = romData[prgMenuAddress + instructionIndex_6AC3++];
			return result;
		}

		private void renderLine_button_Click(object sender, EventArgs e)
		{
			ppuState.RenderLine();
			renderAddr_label.Text = ppuState.nextRenderAddress.ToString("X4");
		}

		private void render_button_Click(object sender, EventArgs e)
		{
			ppuState.Render();
		}
	}
}
