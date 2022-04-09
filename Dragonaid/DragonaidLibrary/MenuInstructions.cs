using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomosZ.Dragonaid.Libraries
{
	public class MenuInstructions
	{
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
		public static Address adjustableHeightInstructions = new Address("Menu adjustable height instructions", 0x38DB2)
		{
			length = 7, // at least
			notes = "Add 0x23 and you get the DynamicSubroutine07 Index" +
			"[$00]: Item Command ($2F + $23 => $52) "
				+ "$23 -> Character_GetCurrentHP"
				+ "$27-> $932C Character_GetCurrentMP"
				+ "$2D-> $93D0 Character_CheckStatus"
				+ "$3A-> $9586 TransferXPToQuickStorage"
				+ "$3C-> $95ED Character_GetTotalGold "
				+ "$4E- > $9944 GetCharacterClass_JMP"
				+ "$52-> $9976 Character_GetCountAndNameIndices"
			,
		};

		// Variables that are set outside the scope of the menu subroutine
		public byte[] _0647 = new byte[7];
		/// <summary>
		/// when does 100-10F get written to? before/during menu construction?
		/// </summary>
		public byte[] theStack = new byte[0xFF];

		public byte selectedCharacterFormationIndex;

		public byte map_WorldPosition_X;
		public byte map_WorldPosition_Y;
		public byte _75;
		public byte _2D;
		public byte _2E;
		public byte _9B;
		public byte _9C;
		public byte _9E;
		public byte _9F;

		/// <summary>
		/// for F385BE() points to low stack (0x100)
		/// _Menu_Subroutine_() used to hold menu address
		/// </summary>
		private byte[] dynamicPointerSpace = new byte[2];
		private byte[] quickStorage04 = new byte[2];


		private byte positionA;
		private byte positionB;
		private byte widthCode;
		private int writeDimensions;
		/// <summary>
		/// # of characters in party + 1
		/// </summary>
		private int writeToCounter;
		/// <summary>
		/// first character index
		/// </summary>
		private int _7D;
		private int _8E;
		private int _7F;
		private int nextCharPosition;
		private int textRowCounter;
		private int titleHasArrow;
		private int _6ACA;
		private int instructionIndex;
		private int titleCode;
		private int writeNextLine;
		private int halfWidth;
		/// <summary>
		/// # of characters in party
		/// </summary>
		private int _74;
		private int positionLowNibble;
		private int positionHighNibble;
		private byte _78;
		private byte _2F;
		private byte _AC;
		/// <summary>
		/// map_WorldPosition_X - 0x08
		/// </summary>
		private byte _06;
		/// <summary>
		/// map_WorldPosition_Y - 0x07;
		/// </summary>
		private byte _07;
		private byte[] romData;
		private Address menuPointer;
		/// <summary>
		/// selectedCharacterFormationIndex
		/// </summary>
		private byte _6AC7;


		public void MenuSubroutineExtended(byte[] romData, Address menu)
		{
			this.romData = romData;
			menuPointer = menu;

			MenuSubroutine();
			F385BE();
			_Menu_Subroutine_();
		}

		private void _Menu_Subroutine_()
		{
			// Menu_WriteToScreen()
			GetMenuDimensions();
			ClearWriteBlock();
			WritetopBorder();
			TitleAndText();

			// Menu_CheckForInput()
			//
		}

		private void GetMenuDimensions()
		{
			ParseDimensions();

		}

		private void F385BE()
		{
			// F385C4
			if ((_2F & 0x01) != 0)
			{
				//L385CD();
			}
			else
				ReadLowStack_CheckWaitForNMI();
		}

		private void ReadLowStack_CheckWaitForNMI()
		{
			// F38666
			{
				// Menu_SetDynamicPointerSpaceVector
				dynamicPointerSpace[1] = 0x01;
				if ((_AC & 0x1F) == 0)
					dynamicPointerSpace[0] = 0x00;
				else
					dynamicPointerSpace[0] = 0x10;
			}

			_06 = (byte)(map_WorldPosition_X - 0x08);
			_07 = (byte)(map_WorldPosition_Y - 0x07);

			Menu_ReadLowStack();

			//F386C5				// F386D5			//F386CE
			if (L386D9(_2D, _2E) && L386D9(_9B, _9C) && L386D9(_9E, _9F))
			{
				// Menu_WaitFor_NMI
			}
			else
			{
				DynamicSubroutine_38000_B();
			}
		}


		private bool L386D9(byte x, byte y)
		{
			byte a = (byte)(x - _06);
			if (a >= 0x10)
				return true;

			quickStorage04[0] = a;
			a = (byte)(y - _07);
			quickStorage04[1] = a;
			return a >= 0x0f;
		}

		private void Menu_ReadLowStack()
		{
			if (theStack[dynamicPointerSpace[0] + 2] != 0xFF)
			{   // L385DF
				var y = 0x00;
				byte a = (byte)(theStack[dynamicPointerSpace[0] + y] - _06);
				if (a < 0x10)
				{ // L385F4
					quickStorage04[0] = a;
					++y;
					a = (byte)(theStack[dynamicPointerSpace[0] + y] - _07);
					if (a < 0x0F)
					{
						quickStorage04[1] = a;
						if (!DynamicSubroutine_38000_B())
						{
							if ((dynamicPointerSpace[0] += 0x04) < 80)
								Menu_ReadLowStack();
						}
						else
						{ // $860A

						}
					}
					else if ((dynamicPointerSpace[0] += 0x04) < 80)
						Menu_ReadLowStack();
				}
				else if ((dynamicPointerSpace[0] += 0x04) < 80)
					Menu_ReadLowStack();
			}
		}


		public bool DynamicSubroutine_38000_B()
		{
			var x = quickStorage04[1] << 1; // index for _647[]
			var y = ++quickStorage04[0]; // how many times to right bitshift 0x80 (divide by 2^y)
			if (y >= 0x09)
			{
				y -= 0x08;
				++x;
			}

			int a = 0x80;
			bool hasCarry = false;
			do
			{
				hasCarry = (a & 0x01) == 1;
				a >>= 1;
			} while (--y > 0);

			a <<= 1;
			if (hasCarry)
				a += 1;
			a &= _0647[x];
			return (a != 0);
		}


		public void MenuSubroutine()
		{// $9E55
			ParseDimensions();

			// Menu_ParseWidthAndPosition
			halfWidth = widthCode >> 1;
			_74 = writeToCounter;
			positionLowNibble = positionA & 0x0F;
			positionHighNibble = positionA >> 4;

			if (positionLowNibble >= 0x08)
			{
				PositionLowNibble_08OrGreater();
			}
			else
			{
				{// $9EAA no name function
					_78 = 0x80;
					for (int i = positionLowNibble; i > 0; --i)
					{
						_78 >>= 1;
					}

					byte z = 0;
					for (int i = halfWidth; i > 0; --i)
					{
						z |= _78;
						_78 >>= 1;
					}

					_78 = z;
				}

				{// $9EC5
					var x = positionHighNibble << 1;
					if (x == 0) // L39ECB
					{
						L39ECB();
					}
					else
						L39ED0(x);
				}

				var decCount = 0x08 - positionLowNibble;

				while (--decCount != 0)
				{
					--_75;
				}

				if (_75 != 0)
				{
					//_9E9D()
					positionLowNibble = 0x08;
					PositionLowNibble_08OrGreater();
				}
				// RTS
			}
		}

		private void ParseDimensions()
		{
			_6AC7 = selectedCharacterFormationIndex;
			if (romData[menuPointer.offset] >= 0x80)
			{ // GetNewMenuAddress
				positionA = romData[menuPointer.offset + 1];
				positionB = positionA;

				var newLowByte = romData[menuPointer.offset + 2];
				var newHighByte = romData[menuPointer.offset + 3];

				menuPointer = new Address($"Actual menu address. Was {menuPointer.pointer}.", (newHighByte << 8) + newLowByte);
			}
			else
			{
				positionA = romData[menuPointer.offset + 2];
				positionB = positionA;
			}

			widthCode = romData[menuPointer.offset];
			writeDimensions = (widthCode >> 1) | 0x10;

			var height = romData[menuPointer.offset + 1];
			if (height >= 0x10)
			{
				GetAdjustableHeight(romData, menuPointer, height);
			}

			_8E = 0xFF;
			_7F = 0xFF;

			byte titleInstruction = romData[menuPointer.offset + 4];

			bool carry = false;
			titleInstruction = ASMHelper.ROL(titleInstruction, 3, ref carry);

			titleInstruction &= 0x03;

			titleCode = titleInstruction;

			var width = widthCode << 1;
			width += nextCharPosition;
			writeNextLine = width;
			++writeNextLine;
			++titleCode;

			nextCharPosition = 0;
			textRowCounter = 0;
			titleHasArrow = 0;
			_6ACA = 0;

			instructionIndex = 0x0B;
		}

		private void L39ECB()
		{
			var x = positionHighNibble << 1;
			L39ED0(++x);
		}

		private void L39ED0(int x)
		{
			var y = _74;

			do
			{
				_0647[x] = (byte)(_78 | _0647[x]);
				x += 2;
			}
			while (--y > 0);
		}

		private void PositionLowNibble_08OrGreater()
		{
			// F9EA7
			var x = positionLowNibble - 8;
			// L9EAA
			_78 = 0x80;
			while (x != 0)
			{
				_78 >>= 1;
				--x;
			}

			x = _75;
			byte a = 0;

			do
			{ // $9EBB
				a = (byte)(_78 | a);
				_78 >>= 1;
			}
			while (--x != 0);

			_78 = a;

			// L39ECB
			L39ECB();
		}

		private void GetAdjustableHeight(byte[] romData, Address menu, byte heightCode)
		{
			heightCode -= 0x10;
			switch (heightCode)// LoadSubroutine_A_Plus23
			{
				case 7:
					//??
					break;
				case 8:
					//GetMonsterGroupCount
					break;
				case 9:
					// ??
					break;
				case 0x0A:
					// ??
					break;
				default:
					AdjustHeightFromDynamicSubroutine(romData, heightCode);
					break;
			}
		}

		/// <summary>
		/// LoadSubroutine_A_Plus23
		/// </summary>
		/// <param name="romData"></param>
		/// <param name="heightCode"> 0 to 6, and possibly >= B</param>
		private void AdjustHeightFromDynamicSubroutine(byte[] romData, byte heightCode)
		{
			var adjHeightInstruction = romData[adjustableHeightInstructions.offset + heightCode];
			if (adjHeightInstruction == 0x66) // this is probably not relevant here
			{
				// Execute Subroutine 0x66
				return;
			}

			int[] quickStorage = new int[5];

			heightCode += 23; // this is the 07 Subroutine to launch
			switch (heightCode)
			{
				case 52:// Character_GetCountAndNameIndices
					quickStorage[0] = 2; // character count
					quickStorage[1] = 0; // first character index
					quickStorage[1] = 1; // second character index
					quickStorage[1] = 0xFF; // third character index
					quickStorage[1] = 0xFF; // fourth character index
					break;
			}

			writeToCounter = quickStorage[0];
			_7D = quickStorage[0];
			++writeToCounter;
		}
	}
}
