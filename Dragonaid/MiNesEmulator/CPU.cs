using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.ASM;


namespace AtomosZ.MiNesEmulator.CPU2A03
{
	/// <summary>
	/// @TODO: (after syncing to git) move to CPU2a03 directory.
	/// </summary>
	public class CPU
	{
		private enum MirroringMode
		{
			Horizontal = 0x0,
			Vertical = 0x01,
		}
		/// <summary>
		/// In bytes, of course.
		/// </summary>
		private const int BankLength = 0x4000;

		/// <summary>
		/// A class to encapsulate memory so as to simulate memory mirroring without
		/// having to worry about someone doing something silly.
		/// </summary>
		private class CPUMemory
		{
			private byte[] mem;

			public CPUMemory()
			{
				mem = Enumerable.Repeat((byte)0x00, 0x10000).ToArray();
			}

			public int Length { get { return mem.Length; } }

			public byte this[byte zeroPagePointer]
			{
				get
				{
					return mem[zeroPagePointer];
				}
				set
				{
					mem[zeroPagePointer] = value;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="pointer"></param>
			/// <returns>nothing</returns>
			public byte this[int pointer]
			{
				get
				{
					Clamp(ref pointer);
					return mem[pointer];
				}

				set
				{
					Clamp(ref pointer);
					mem[pointer] = value;
				}
			}

			/// <summary>
			/// <para>
			/// <br>Copies memory of length bytes to byte array or </br>
			/// <br>Copies bytes from byte array to pointer address. </br>
			/// <br>NOTE: Since this clamps the pointer value from a mirrored address
			///		to a non-mirrored address, this COULD have unexpected behaviour.</br>
			///	</para>
			/// </summary>
			/// <param name="pointer"></param>
			/// <param name="length"></param>
			/// <returns></returns>
			public byte[] this[int pointer, int length]
			{
				get
				{
					Clamp(ref pointer);
					byte[] b = new byte[length];
					for (int i = 0; i < length; ++i)
						b[i] = mem[pointer + i];

					return b;
				}

				set
				{
					Clamp(ref pointer);
					for (int i = 0; i < length; ++i)
						mem[pointer + i] = value[i];
				}
			}

			/// <summary>
			/// Clamp pointer to simulate memory mirroring.
			/// TODO: Come up with better solution?
			/// </summary>
			/// <param name="pointer"></param>
			private void Clamp(ref int pointer)
			{
				if (pointer <= 0x1FFF)
				{
					pointer &= 0x07FF;
				}
				else if (pointer > 0x2007 && pointer <= 0x3FFF)
				{
					pointer &= 0x2007;
				}
			}
		}

		/// <summary>
		/// This class stores no data or status. It is just a go-between to the memory
		/// for simplicity.
		/// All indexing is relative to the start of the STACK (not cpu memory).
		/// </summary>
		public class Stack
		{
			internal const int stackStart = 0x100;

			private CPU cpu;


			public Stack(CPU cpu)
			{
				this.cpu = cpu;
			}

			public byte this[byte index]
			{
				get { return cpu.mem[stackStart + index]; }
				set { cpu.mem[stackStart + index] = value; }
			}
		}


		public Stack theStack;
		public ControlUnit6502 controlUnit;

		private CPUMemory mem;
		/// <summary>
		/// In 16KB units.
		/// </summary>
		private byte prgRomSize;
		/// <summary>
		/// In 8KB units. 0 means board uses CHR RAM
		/// </summary>
		private byte chrRomSize;
		/// <summary>
		/// <para>
		/// <br>0: Horizontal</br>
		/// <br>1: Vertical</br>
		/// </para>
		/// </summary>
		private MirroringMode mirroring;
		/// <summary>
		/// PRG RAM $6000-7FFF
		/// </summary>
		private bool hasBattery;
		/// <summary>
		/// Ignore mirroring mode and instead use four-screen VRAM.
		/// </summary>
		private bool use4ScreenRam;
		private byte mapperNum;
		private byte lastBankLowId;
		private byte lastBankHighId;

		public CPU()
		{
			mem = new CPUMemory();
			theStack = new Stack(this);
			controlUnit = new ControlUnit6502(this);
		}

		public void LoadRom(byte[] romData)
		{
			// read iNES header
			if (romData[0] != 'N' && romData[1] != 'E' && romData[2] != 'S' && romData[3] != 0x1A)
			{
				throw new Exception("Invalid rom header");
			}

			prgRomSize = romData[4];
			chrRomSize = romData[5];

			// flag 6
			mirroring = (MirroringMode)(romData[6] & 0x01);
			hasBattery = (romData[6] & 0x02) == 0x02;
			// 0x02 is trainer. Don't think we need to worry about this
			use4ScreenRam = (romData[6] & 0x04) == 0x04;

			// flag 7
			// 0x00 VS Unisystem
			// 0x01 PlayChoice-10
			// 0x02 & 0x04 == NES 2.0 format (flags 8-15)

			mapperNum = (byte)((romData[6] & 0xF0) + ((romData[7] & 0xF0) << 4));

			/* TODO: Determine memory banks by mapperNum. */
			// this should work for mappers with high and low banks
			lastBankLowId = (byte)((prgRomSize / 2) - 1);
			lastBankHighId = (byte)(prgRomSize - 1);

			byte[] bank = new byte[BankLength];
			Array.Copy(romData, Address.iNESHeaderLength, bank, 0, BankLength);
			SetPRGROMBankLow(bank);

			Array.Copy(romData, (BankLength * lastBankLowId) + Address.iNESHeaderLength, bank, 0, BankLength);
			SetPRGROMBankHigh(bank);

			Initialize();
		}

		public void Reset()
		{
			mem = new CPUMemory();
			controlUnit.Reset();
			Initialize();
		}



		/// <summary>
		/// Sets program counter to Reset pointer
		/// </summary>
		public void Initialize()
		{
			controlUnit.pc = GetPointerAt(UniversalConsts.RESET_Pointer);
		}

		public byte[] GetMemory()
		{
			return mem[0, 0x10000];
		}

		/// <summary>
		/// Writes data to 0x8000-0xBFFF
		/// </summary>
		/// <param name="bankData"></param>
		private void SetPRGROMBankLow(byte[] bankData)
		{
			mem[0x8000, bankData.Length] = bankData;
		}

		internal byte Read(byte zeropageAddress)
		{
			return mem[zeropageAddress];
		}

		/// <summary>
		///  @TODO: Check for reading register/mirrored memory
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		internal byte Read(int address)
		{
			return mem[address];
		}

		internal void Write(byte zeropageAddress, byte value)
		{
			mem[zeropageAddress] = value;
		}

		/// <summary>
		/// @TODO: Check for writing to register/mirrored memory
		/// </summary>
		/// <param name="address"></param>
		/// <param name="value"></param>
		internal void Write(int address, byte value)
		{
			mem[address] = value;
		}

		/// <summary>
		/// Writes data to 0xC000-0xFFFF
		/// </summary>
		/// <param name="bankData"></param>
		private void SetPRGROMBankHigh(byte[] bankData)
		{
			mem[0xC000, bankData.Length] = bankData;
		}


		public byte this[int pointer]
		{
			get { return mem[pointer]; }
		}

		public byte[] this[int pointer, int length]
		{
			get { return mem[pointer, length]; }
		}
		public byte[] zeroPages
		{
			get { return mem[0, 0x100]; }
		}

		public byte[] stack
		{
			get { return mem[Stack.stackStart, 0x100]; }
		}

		public byte[] nesRAM
		{
			get { return mem[0, 0x2000]; }
		}

		public byte[] saveRAM
		{
			get { return mem[0x6000, 0x2000]; }
		}

		/// <summary>
		/// Get all instructions and branch addresses starting from address until a JMP, RTI, or RTS.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public Tuple<List<Instruction>, List<int>> GetInstructionBlock(int address)
		{
			var readAddrHash = new HashSet<int>();
			var instrItems = new List<Instruction>();
			var addrToTraverse = new List<int>();

			int pseudoPC = address;
			// traverse code from address
			readAddrHash.Add(pseudoPC);
			var instr = GetInstruction(pseudoPC);
			instrItems.Add(instr);
			while (!instr.opcode.IsControlFlowEnd())
			{
				if (instr.opcode.IsControlFlow())
				{
					if (GetPointerFromInstruction(instr, out int targetAddr))
					{
						// check if target already visited
						if (readAddrHash.Add(targetAddr))
						{
							addrToTraverse.Add(targetAddr);
						}
					}
					else
						MessageBox.Show($"What's the deal with {pseudoPC} : {instr.ToString()}?");
				}

				pseudoPC += instr.opcode.bytes;
				instr = GetInstruction(pseudoPC);
				instrItems.Add(instr);
				readAddrHash.Add(pseudoPC);

			}



			return new Tuple<List<Instruction>, List<int>>(instrItems, addrToTraverse);
		}

		private bool GetPointerFromInstruction(Instruction instr, out int targetAddr)
		{
			switch (instr.opcode.mode)
			{
				case Opcode.Mode.Relative:  // any branch
					targetAddr = instr.GetRelativeAddress();
					return true;

				case Opcode.Mode.Absolute: // JMP, JSR $4444
					targetAddr = instr.GetPointer();
					return true;

				case Opcode.Mode.Indirect: // JMP ($4444)
					targetAddr = GetPointerAt(instr.GetPointer());
					return true;

				default: // BRK, RTI, RTS
					targetAddr = -1;
					return false;
			}
		}

		/// <summary>
		/// @Obsolete
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public string GetASMAt(int address)
		{
			var line = mem[address];
			if (!Opcodes.opcodes.TryGetValue(line, out Opcode opc))
			{
				return $"??? ({line.ToString("X2")})";
			}

			Instruction instr = new Instruction();
			instr.address = address;
			instr.opcode = opc;

			instr.operands = new byte[opc.bytes - 1];
			int addr = 0;
			for (int i = 1; i < opc.bytes; ++i)
			{
				instr.operands[i - 1] = mem[address + i];
				addr += mem[address + i] << i;
			}

			byte pointsTo = mem[addr];

			return instr.ToString() + " => " + pointsTo.ToString("X4");
		}


		public int GetPointerAt(int lowByteAddress)
		{
			return mem[lowByteAddress] + (mem[lowByteAddress + 1] << 8);
		}

		/// <summary>
		/// Use ViewNextInstruction() for human readable string of instruction.
		/// </summary>
		/// <returns></returns>
		public Instruction PeekNextInstruction()
		{
			return GetInstruction(controlUnit.pc);
		}

		/// <summary>
		/// Use PeekNextInstruction() to get next Instruction.
		/// </summary>
		/// <returns>human readable string of instruction</returns>
		public string ViewNextInstruction()
		{
			var instr = GetInstruction(controlUnit.pc);
			return controlUnit.Peek(instr);
		}

		public string ViewNextInstruction(Instruction instr)
		{
			return controlUnit.Peek(instr);
		}

		public void ParseNextInstruction()
		{
			var instr = GetInstruction(controlUnit.pc);
			controlUnit.Parse(instr);
		}

		private Instruction GetInstruction(int address)
		{
			var instrByte = mem[address];
			if (!Opcodes.opcodes.TryGetValue(instrByte, out Opcode opc))
			{
				throw new Exception($"Invalid opcode $({instrByte:X2)}) at ${address:X4}");
			}

			var instr = new Instruction();
			instr.opcode = opc;
			instr.address = address;
			instr.operands = new byte[opc.bytes - 1];
			for (int i = 1; i < opc.bytes; ++i)
			{
				instr.operands[i - 1] = mem[address + i];
			}

			return instr;
		}


		internal int GetBRKPointer()
		{
			var BRKPointer = UniversalConsts.IRQBRK_Pointer;
			return mem[BRKPointer] + (mem[BRKPointer + 1] << 8);
		}
	}
}
