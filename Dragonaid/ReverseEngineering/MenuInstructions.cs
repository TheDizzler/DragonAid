using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.PointerList;
using AtomosZ.DragonAid.TextToHex;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;

namespace AtomosZ.DragonAid.Menu
{
	public class MenuInstructions
	{
		/// <summary>
		/// Set outside of scope:
		/// 0x2A - map x pos
		/// 0x2B - map y pos
		/// 0x2D
		/// 0x2E
		/// 0x2F
		/// 0x9B
		/// 0x9C
		/// 0x9E
		/// 0x9F
		/// </summary>
		public byte[] zeroPages = new byte[0xFF];

		/* Variables that are set outside the scope of the menu subroutine */
		/// <summary>
		/// typically these are zero'd in a function call
		/// Clear_0647to0664
		/// </summary>
		public byte[] _0647 = new byte[0x1C];
		/// <summary>
		/// when does 100-10F get written to? before/during menu construction?
		/// </summary>
		public byte[] theStack = new byte[0xFF];
		public byte[] characterStatuses = new byte[8];

		public byte selectedCharacterFormationIndex;
		public byte ppuAddress;

		/// <summary>
		/// 0x2A
		/// </summary>
		public byte mapWorldPositionX = 0x2A;
		/// <summary>
		/// 0x2B
		/// </summary>
		public byte mapWorldPositionY = 0x2B;
		//public byte _75;

		/// <summary>
		/// for F385BE() points to low stack (0x100)
		/// _Menu_Subroutine_() used to hold menu address
		/// </summary>
		//private byte[] dynamicPointerSpace = new byte[2];
		/// <summary>
		/// From $0400 to $046F
		/// </summary>
		private string[] writeBlock = new string[0x6F];

		private byte[] romData;
		private byte menuPointerIndex;
		private Address menuPointer;

		/// <summary>
		/// 0x0470
		/// byte 2 or
		/// byte 1 from redirected menu
		/// </summary>
		private byte menuPositionA;
		private byte menuPositionB;
		/// <summary>
		/// menuWidth
		/// </summary>
		private byte _6AE9;
		/// <summary>
		/// 0x0471
		/// </summary>
		private int writeDimensions;
		/// <summary>
		/// menu_WriteTo_Counter
		/// if not set in DynamicHeight then set outside scope of instructions?
		/// </summary>
		public byte _6AEA;
		/// <summary>
		/// first character index
		/// </summary>
		//private int _7D;


		/// <summary>
		/// menu_TextRowCounter
		/// </summary>
		private byte _6AC1;
		/// <summary>
		/// writeNextLineVariable
		/// </summary>
		private byte _6AC2;
		/// <summary>
		/// menu_InstructionIndex
		/// </summary>
		private byte _6AC3;
		/// <summary>
		/// titleCode
		/// </summary>
		private byte _6AC6;
		/// <summary>
		/// menu_TitleHasArrow64
		/// </summary>
		private byte _6AC9;
		private byte _6ACA;
		/// <summary>
		/// menu_NextCharPosition
		/// </summary>
		private byte _6AEB;

		//private byte halfWidth;
		/// <summary>
		/// # of characters in party
		/// </summary>
		//private int _74;
		//private byte positionLowNibble;
		//private byte positionHighNibble;
		/// <summary>
		/// map_WorldPosition_X - 0x08
		/// </summary>
		//private byte _06;
		/// <summary>
		/// map_WorldPosition_Y - 0x07;
		/// </summary>
		//private byte _07;


		/// <summary>
		/// selectedCharacterFormationIndex
		/// </summary>
		private byte _6AC7;

		private byte titleInstructionCode;
		private byte numTitlesToWrite;
		private byte nextMenuInstructionIndex;
		private byte menuInstructionByte;
		private byte characterFormationIndex;
		/// <summary>
		/// 0x6C
		/// </summary>
		private byte dynamicPointerSpace = 0x6C;


		public void MenuSubroutine(byte[] romData, byte menuPointerIndex)
		{
			this.romData = romData;
			this.menuPointerIndex = menuPointerIndex;
			menuPointer = new Address("Pointer to menu instructions to create",
				romData[ROM.MenuPointers.offset + menuPointerIndex]
				+ (romData[ROM.MenuPointers.offset + menuPointerIndex + 1] << 8));

			Menu_Initialization();
			F385BE();
			_Menu_Subroutine_();
		}

		private void _Menu_Subroutine_()
		{
			// Menu_WriteToScreen()
			GetMenuDimensions();
			ClearWriteBlock();
			WriteTopBorder();
			TitleAndText();

			// Menu_WriteToScreen_Loop
			while (_6AEA-- > 0)
			{
				Clear2Lines();
				WriteVerticalBarsAndClose();
				WriteNextLine();
				CopyLinesToStagingArea();
				//SlideLines
			}

			// Menu_CheckForInput()
			//
		}


		private void WriteNextLine()
		{
			if (_6AEA == 1)
				return;

			ParseInstruction05(_6AC2);
			if (_6AEA != 2 && (romData[menuPointer.offset + 5] & 0x40) != 0)
				L3880D();
		}

		private void ParseInstruction05(byte nextCharPos)
		{
			this._6AEB = nextCharPos;
			if (romData[menuPointer.offset + 5] < 0x80)
			{
				ParseNextInstruction();
			}
			else
			{
				if (_6AC1 != 0 || (romData[menuPointer.offset + 5] & 0x40) == 0)
				{
					ParseInstructionIndex0B();
				}
				else
				{
					SkipInstructionsLoop();
				}
			}
		}

		private void ParseInstructionIndex0B()
		{
			_6AC3 = 0x0B;
			ParseNextInstruction();
		}

		private void ParseNextInstruction()
		{
			if (GetNextInstruction() < 0x80)
			{
				InstructionIsPrintChar();
				return;
			}

			if (menuInstructionByte == 0xFF)
			{
				EoT();
				return;
			}

			var a = menuInstructionByte & 0x1F;
			if (a < 0x10)
			{

			}
			else if (a == 10)
			{ // 893F
				var previousInstruction = menuInstructionByte;
				GetNextInstruction();

				if ((GetNextInstruction() & 0x01) == 0x01)// high byte == num characters to write
				{ // 8951
				  // a = textRowCounter
					SetCharacterIndex(_6AC1);
					TransferCharacterNameToQuickStorage(); // probably not relevant to what trying to achieve
					var x = characterFormationIndex << 1;
					if (characterStatuses[x + 1] < 80 && menuPointerIndex == 0x4E)
						ppuAddress = 0x5A;
					else
						ppuAddress = 0;
					WriteCharsAndGetNextInstruction();
				}
				else
				{
					ParseNextCharacterIndex();
				}
			}
			else if (a == 11)
			{

			}
			else if (a < 0x14)
			{

			}
			else if (a < 0x16)
			{

			}
			else if (a < 0x17)
			{

			}
			else if (a < 0x18)
			{

			}
			else if (a < 0x19)
			{

			}
			else if (a < 0x1A)
			{

			}
			else if (a < 0x1B)
			{

			}
			else if (a < 0x1F)
			{

			}
			else
				EoT();
		}

		private void EoT()
		{
			++_6AC1;
		}

		private void TransferCharacterNameToQuickStorage()
		{
			zeroPages[0x04] = Tables.textToHexDict["D"];
			zeroPages[0x05] = Tables.textToHexDict["u"];
			zeroPages[0x06] = Tables.textToHexDict["m"];
			zeroPages[0x07] = Tables.textToHexDict["m"];
			zeroPages[0x08] = Tables.textToHexDict["y"];
			zeroPages[0x09] = 0xFF;
		}

		private void WriteCharsAndGetNextInstruction()
		{ // 8B55
			WriteLine();
			ParseNextInstruction();
		}

		private void WriteLine()
		{ // 8B5B
			menuInstructionByte >>= 4;
			for (int i = 0; i < menuInstructionByte; ++i)
			{
				var a = zeroPages[0x04];
				if ((a & 0xF0) == 0xA0)
					WriteSingularChars();
				else
				{
					WriteNextChar(a);
				}
			}
		}

		private void SetCharacterIndex(byte a)
		{ // 8C89
			characterFormationIndex = a;
			// PHA <- Y; // oh boy, what's this now? Last\next menu instruction parsed, I believe?
			if ((byte)(romData[menuPointer.offset + 8] & 0x40) != 0)
			{
				characterFormationIndex = _6AC7;
			}
			//PLA -> Y
		}

		private byte GetNextInstruction()
		{
			menuInstructionByte = romData[menuPointer.offset + _6AC3++];
			return menuInstructionByte;
		}

		private void WriteVerticalBarsAndClose()
		{
			byte x = (byte)(_6AE9 << 1);
			WriteVerticalBars(x++);
			if (_6AEA != 2)
				WriteVerticalBars(x);
			else
			{
				writeBlock[x] = Tables.textTable[0x7B]; // bottom left corner
				x += _6AE9;
				writeBlock[--x] = Tables.textTable[0x7F]; // bottom right corner
				--x;
				var y = _6AE9 - 2;
				while (y-- != 0)
				{
					writeBlock[x--] = Tables.textTable[0x78];// bottom aligned horizontal bar
				}
			}
		}

		private void Clear2Lines()
		{
			var x = _6AE9 << 1;
			var y = x;

			while (--y >= 0)
			{
				writeBlock[x++] = Tables.textTable[0];
			}
		}

		private void TitleAndText()
		{
			byte a = romData[menuPointer.offset + 3];
			if (a < 0x80)
			{ // 8C01
				if (menuPointerIndex == 0x34)
					menuIndex34_Instruction03();
				else if (menuPointerIndex == 0x3B)
					menuIndex3B_Instruction03();
				else
					ReadInstruction05(); // 8BEE
			}
			else
			{
				titleInstructionCode = a;
				if ((titleInstructionCode & 0x07) == 0x07)
					a = 0x08;
				_6AEB = a;
				numTitlesToWrite = (byte)(((titleInstructionCode >> 5) & 0x03) + 1);

				nextMenuInstructionIndex = 0x04;

				while (numTitlesToWrite-- > 0)
				{
					CheckForTitleAndWrite();

					a = (byte)((titleInstructionCode & 0x18) >> 3);
					_6AEB = (byte)(a + _6AEB);

					++nextMenuInstructionIndex;
				}

				ReadInstruction05();
			}
		}

		private void ReadInstruction05()
		{
			if ((romData[menuPointer.offset + 5] & 0x40) != 0)
				ParseInstruction05((byte)(_6AC6 + _6AE9));
		}

		private void CheckForTitleAndWrite()
		{
			var a = romData[menuPointer.offset + nextMenuInstructionIndex];
			a &= 0x3F;
			if (a < 0x20)
				FindTitleAndWrite(a);
			else if (a < 0x24)
				StatusTitles();
			else if (a == 0x25)
				WriteTitle(0x04);
			else if (a == 0x26)
				WriteTitle(0x08);
		}

		private void FindTitleAndWrite(byte x)
		{
			var y = 0;
			while (x != 0)
			{ // find correct title index
				if (romData[ROM.MenuTitles.offset + y++] == 0xFF)
					--x;
			}

			WriteNextChar(0x79);// top vertical bar with space for title

			byte a;
			while ((a = romData[ROM.MenuTitles.offset + y++]) != 0xFF)
				WriteNextChar(a);
		}

		private void WriteNextChar(byte nextChar)
		{
			if (_6AEB != 0 || nextChar != 0x79)
				writeBlock[_6AEB] = Tables.textTable[nextChar];
			if (++_6AEB == 0x64) // arrow
			{
				// stuff that's probably not relevant to what trying to achieve with this program
			}
		}

		private void WriteTopBorder()
		{
			writeBlock[0] = Tables.textTable[0x7A]; // top-left corner
			int i = _6AE9 - 1;
			writeBlock[i] = Tables.textTable[0x7D]; // top-right corner
			while (--i > 0)
			{
				writeBlock[i] = Tables.textTable[0x78]; // top horizontal bar
			}

			WriteVerticalBars(_6AE9);
		}

		private void WriteVerticalBars(byte writeStart)
		{
			writeBlock[writeStart] = Tables.textTable[0x77]; // left vertical bar, 2nd row
			writeBlock[(writeStart << 1) - 1] = Tables.textTable[0x7C]; // right vertical bar, 2nd row
		}

		private void GetMenuDimensions()
		{
			ParseDimensions();
			switch (menuPointerIndex)
			{
				case 0x3D: // combat text menu
				case 0x3E:
					// Do stuff
					break;
			}
		}

		private void ClearWriteBlock()
		{
			for (int i = 0; i < writeBlock.Length; ++i)
				writeBlock[i] = Tables.textTable[0x00];
		}

		private void F385BE()
		{
			// F385C4
			if ((zeroPages[0x2F] & 0x01) != 0)
			{
				//L385CD();
			}
			else
				ReadLowStack_CheckWaitForNMI();
		}

		private void ReadLowStack_CheckWaitForNMI()
		{
			// F38666
			{ // $8655
			  // Menu_SetDynamicPointerSpaceVector
				zeroPages[dynamicPointerSpace + 1] = 0x01; // pointer to The Stack
				if ((zeroPages[0xAC] & 0x1F) == 0)
					zeroPages[dynamicPointerSpace + 0] = 0x00; // first row of stack
				else
					zeroPages[dynamicPointerSpace + 0] = 0x10; // or second row
			}

			zeroPages[0x06] = (byte)(zeroPages[mapWorldPositionX] - 0x08);
			zeroPages[0x07] = (byte)(zeroPages[mapWorldPositionY] - 0x07);

			Menu_ReadLowStack();

										//F386CE
			if (L386D9(zeroPages[0x2D], zeroPages[0x2E]) //F386C5
				&& L386D9(zeroPages[0x9B], zeroPages[0x9C]) // F386D5
				&& L386D9(zeroPages[0x9E], zeroPages[0x9F]))
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
			byte a = (byte)(x - zeroPages[0x06]); // FF - world X Pos - 8
			if (a >= 0x10)
				return true;

			zeroPages[0x04] = a;
			a = (byte)(y - zeroPages[0x07]);
			zeroPages[0x05] = a;
			return a >= 0x0f;
		}

		/// <summary>
		/// Is this checking for map wrap-around?
		/// </summary>
		private void Menu_ReadLowStack()
		{
			while (theStack[zeroPages[dynamicPointerSpace] + 2] != 0xFF)
			{ // L385DF
				var y = 0x00;
				// current world X position (or last) - (world X position - 8)
				byte a = (byte)(theStack[zeroPages[dynamicPointerSpace] + y] - zeroPages[0x06]); 
				if (a >= 0x10)
				{
					if ((zeroPages[dynamicPointerSpace] += 0x04) >= 80)
						break;
					continue;
				}
				// L385F4
				zeroPages[0x04] = a;
				++y;
				// current world Y Position (or last) - (world Y Position - 7)
				a = (byte)(theStack[zeroPages[dynamicPointerSpace] + y] - zeroPages[0x07]); 
				if (a >= 0x0F)
				{
					if ((zeroPages[dynamicPointerSpace] += 0x04) >= 80)
						break;
					continue;
				}
				// $8603
				zeroPages[0x05] = a;
				if (!DynamicSubroutine_38000_B())
				{
					if ((zeroPages[dynamicPointerSpace] += 0x04) >= 80)
						break;
					continue;
				}

				{ // $860A

				}

			}
		}

		/// <summary>
		/// divide 0x80 by 2^Y
		/// if result & 0x0647+X == 0
		///		return CLC
		///	else SEC
		///	
		/// Y = quickstorage04 + 1 (- 8 if >= 9)
		/// X = quickstorage05 * 2 (+1 if y >=9)
		/// </summary>
		/// <returns>Carry Flag set</returns>
		public bool DynamicSubroutine_38000_B()
		{
			byte x = (byte)(zeroPages[0x05] << 1); // index for _647[]
			byte y = ++zeroPages[0x04]; // how many times to right bitshift 0x80 (divide by 2^y)
			if (y >= 0x09)
			{
				y -= 0x08;
				++x;
			}

			byte a = 0x80;
			bool hasCarry = false;
			do
			{
				a = ASMHelper.LSR(a, 1, out hasCarry);
			} while (--y > 0);

			a = ASMHelper.ROL(a, 1, ref hasCarry);
			a &= _0647[x];
			return (a != 0);
		}


		public void Menu_Initialization()
		{// $9E55
			ParseDimensions();

			// $9E63 Menu_ParseWidthAndPosition
			zeroPages[0x75] = (byte)(_6AE9 >> 1); // half of byte 0
			zeroPages[0x74] = _6AEA;
			zeroPages[0x77] = (byte)(menuPositionA & 0x0F); // menu_PositionLowNibble
			zeroPages[0x76] = (byte)(menuPositionA >> 4); // menu_PositionHighNibble

			// F39E7F
			if (zeroPages[0x77] >= 0x08)
			{
				PositionLowNibble_08OrGreater(zeroPages[0x77]);
			}
			else
			{
				F39EAA(zeroPages[0x77]);

				// $9EC5 F39EC5
				byte x = (byte)(zeroPages[0x76] << 1); // menu_PositionHighNibble * 2
				if (x == 0) // L39ECB
					L39ECB();
				else
					L39ED0(x);

				byte decCount = (byte)(0x08 - zeroPages[0x77]); // low nibble of positionA

				while (decCount-- != 0)
				{
					--zeroPages[0x75]; // half of byte 0
				}

				if (zeroPages[0x75] != 0)
				{
					//F39E9D()
					zeroPages[0x77] = 0x08;
					PositionLowNibble_08OrGreater(zeroPages[0x77]);
				}
			}
		}



		private void ParseDimensions()
		{
			_6AC7 = selectedCharacterFormationIndex;
			if (romData[menuPointer.offset] >= 0x80)
			{ // GetNewMenuAddress
				menuPositionA = romData[menuPointer.offset + 1];
				menuPositionB = menuPositionA;

				var newLowByte = romData[menuPointer.offset + 2];
				var newHighByte = romData[menuPointer.offset + 3];

				menuPointer = new Address("Redirected menu pointer",
					(newHighByte << 8) + newLowByte);
			}
			else
			{
				menuPositionA = romData[menuPointer.offset + 2];
				menuPositionB = menuPositionA;
			}

			_6AE9 = romData[menuPointer.offset + 0];
			writeDimensions = (_6AE9 >> 1) | 0x10;

			// Menu_GetHeight();
			// 8D3C
			var height = romData[menuPointer.offset + 1];
			if (height >= 0x10)
			{
				GetAdjustableHeight(height);
			}

			zeroPages[0x7D] = (byte)(height - 1);

			// 8D01
			zeroPages[0x8E] = 0xFF;
			zeroPages[0x7F] = 0xFF;

			bool carry = false;
			byte a = ASMHelper.ROL(romData[menuPointer.offset + 4], 3, ref carry); // take top two bits and ROL to lowest two bits
			a &= 0x03;
			_6AC6 = a;
			_6AEB = a;

			a = (byte)(_6AE9 << 1); // byte 0 * 2
			a += _6AEB; // plus top two bits of byte 4 in lowest two bits
			_6AC2 = (byte)(a + 1);
			++_6AC6;

			_6AEB = 0;
			_6AC1 = 0;
			_6AC9 = 0;
			_6ACA = 0;

			_6AC3 = 0x0B;
		}

		private void L39ECB()
		{
			byte x = (byte)(zeroPages[0x76] << 1); // menu_PositionHighNibble
			L39ED0(++x);
		}

		/// <summary>
		/// is this setting up for writting palette addresses to PPU?
		/// </summary>
		/// <param name="x"></param>
		private void L39ED0(byte x)
		{
			var y = zeroPages[0x74];

			do
			{
				_0647[x] = (byte)(zeroPages[0x78] | _0647[x]);
				x += 2;
			}
			while (--y > 0);
		}

		private void PositionLowNibble_08OrGreater(byte a)
		{ // F39EA7
			F39EAA((byte)(a - 8));
			L39ECB();
		}

		private void F39EAA(byte x)
		{// $9EAA 
			zeroPages[0x78] = 0x80;
			for (int i = x; i > 0; --i)
			{
				zeroPages[0x78] >>= 1;
			}

			byte a = 0;
			for (int i = zeroPages[0x75]; i > 0; --i)
			{
				a |= zeroPages[0x78]; // turn on every bit starting from highest?
				zeroPages[0x78] >>= 1;
			}

			zeroPages[0x78] = a;
		}

		private void GetAdjustableHeight(byte heightCode)
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
			var adjHeightInstruction = romData[ROM.AdjustableHeightInstructions.offset + heightCode];
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

			_6AEA = zeroPages[0x04];
			zeroPages[0x7D] = zeroPages[0x04];
			++_6AEA;
		}


		private void CopyLinesToStagingArea()
		{ // C74C
		  //Menu_WaitForNMI_GetPPUPos_CopyLines()
		  //Menu_CheckIfWaitForNMINeeded()
		  //Menu_WaitFor_NMI()
		}
	}
}
