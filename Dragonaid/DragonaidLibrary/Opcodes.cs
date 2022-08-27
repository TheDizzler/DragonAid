using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomosZ.DragonAid.Libraries
{
	public class Opcodes
	{
		public const byte BRK = 0x00;

		public const byte BCC = 0x90;
		public const byte BCS = 0xB0;
		public const byte BEQ = 0xF0;
		public const byte BMI = 0x30;
		public const byte BNE = 0xD0;
		public const byte BPL = 0x10;
		public const byte BVC = 0x50;
		public const byte BVS = 0x70;

		public const byte JMP_abs = 0x4C;
		public const byte JMP_ind = 0x6C;
		public const byte JSR = 0x20;

		public const byte RTI = 0x40;
		public const byte RTS = 0x60;


		public const byte ADC_imm = 0x69;
		public const byte ADC_zpg = 0x65;
		public const byte ADC_zpx = 0x75;
		public const byte ADC_abs = 0x6D;
		public const byte ADC_abx = 0x7D;
		public const byte ADC_aby = 0x79;
		public const byte ADC_inx = 0x61;
		public const byte ADC_iny = 0x71;

		public const byte AND_imm = 0x29;
		public const byte AND_zpg = 0x25;
		public const byte AND_zpx = 0x35;
		public const byte AND_abs = 0x2D;
		public const byte AND_abx = 0x3D;
		public const byte AND_aby = 0x39;
		public const byte AND_inx = 0x21;
		public const byte AND_iny = 0x31;


		public const byte ASL_acc = 0x0A;
		public const byte ASL_zpg = 0x06;
		public const byte ASL_zpx = 0x16;
		public const byte ASL_abs = 0x0E;
		public const byte ASL_abx = 0x1A;

		public const byte BIT_zpg = 0x24;
		public const byte BIT_abs = 0x2C;

		public const byte CLC = 0x18;
		public const byte CLI = 0x58;
		public const byte CLV = 0xB8;

		public const byte CLD = 0xD8;
		public const byte SED = 0xF8;

		public const byte CMP_zpg = 0xC5;
		public const byte CMP_zpx = 0xD5;
		public const byte CMP_imm = 0xC9;
		public const byte CMP_abs = 0xCD;
		public const byte CMP_abx = 0xDD;
		public const byte CMP_aby = 0xD9;
		public const byte CMP_inx = 0xC1;
		public const byte CMP_iny = 0xD1;

		public const byte CPX_zpg = 0xE4;
		public const byte CPX_imm = 0xE0;
		public const byte CPX_abs = 0xEC;

		public const byte CPY_zpg = 0xC0;
		public const byte CPY_imm = 0xC4;
		public const byte CPY_abs = 0xCC;

		public const byte DEC_zpg = 0xC6;
		public const byte DEC_zpx = 0xD6;
		public const byte DEC_abs = 0xCE;
		public const byte DEC_abx = 0xDE;

		public const byte DEX = 0xCA;
		public const byte DEY = 0x88;

		public const byte EOR_zpg = 0x45;
		public const byte EOR_zpx = 0x55;
		public const byte EOR_imm = 0x49;
		public const byte EOR_abs = 0x4D;
		public const byte EOR_abx = 0x5D;
		public const byte EOR_aby = 0x59;
		public const byte EOR_inx = 0x41;
		public const byte EOR_iny = 0x51;

		public const byte INC_zpg = 0xE6;
		public const byte INC_zpx = 0xF6;
		public const byte INC_abs = 0xEE;
		public const byte INC_abx = 0xFE;

		public const byte INX = 0xE8;
		public const byte INY = 0xC8;

		public const byte LDA_zpg = 0xA5;
		public const byte LDA_zpx = 0xB5;
		public const byte LDA_imm = 0xA9;
		public const byte LDA_abs = 0xAD;
		public const byte LDA_abx = 0xBD;
		public const byte LDA_aby = 0xB9;
		public const byte LDA_inx = 0xA1;
		public const byte LDA_iny = 0xB1;

		public const byte LDX_zpg = 0xA6;
		public const byte LDX_zpy = 0xB6;
		public const byte LDX_imm = 0xA2;
		public const byte LDX_abs = 0xAE;
		public const byte LDX_aby = 0xBE;

		public const byte LDY_zpg = 0xA4;
		public const byte LDY_zpx = 0xB4;
		public const byte LDY_imm = 0xA0;
		public const byte LDY_abs = 0xAC;
		public const byte LDY_abx = 0xBC;

		public const byte LSR_zpg = 0x46;
		public const byte LSR_zpx = 0x56;
		public const byte LSR_acc = 0x4A;
		public const byte LSR_abs = 0x4E;
		public const byte LSR_abx = 0x5E;

		public const byte NOP = 0xEA;

		public const byte ORA_zpg = 0x05;
		public const byte ORA_zpx = 0x15;
		public const byte ORA_imm = 0x09;
		public const byte ORA_abs = 0x0D;
		public const byte ORA_abx = 0x1D;
		public const byte ORA_aby = 0x19;
		public const byte ORA_inx = 0x01;
		public const byte ORA_iny = 0x11;

		public const byte PHA = 0x48;
		public const byte PHP = 0x08;
		public const byte PLA = 0x68;
		public const byte PLP = 0x28;

		public const byte ROL_zpg = 0x26;
		public const byte ROL_zpx = 0x36;
		public const byte ROL_acc = 0x2A;
		public const byte ROL_abs = 0x2E;
		public const byte ROL_abx = 0x3E;

		public const byte ROR_zpg = 0x66;
		public const byte ROR_zpx = 0x76;
		public const byte ROR_acc = 0x6A;
		public const byte ROR_abs = 0x6E;
		public const byte ROR_abx = 0x7E;

		public const byte SBC_zpg = 0xE5;
		public const byte SBC_zpx = 0xF5;
		public const byte SBC_imm = 0xE9;
		public const byte SBC_abs = 0xED;
		public const byte SBC_abx = 0xFD;
		public const byte SBC_aby = 0xF9;
		public const byte SBC_inx = 0xE1;
		public const byte SBC_iny = 0xF1;

		public const byte SEC = 0x38;
		public const byte SEI = 0x78;

		public const byte STA_zpg = 0x85;
		public const byte STA_zpx = 0x95;
		public const byte STA_abs = 0x8D;
		public const byte STA_abx = 0x9D;
		public const byte STA_aby = 0x99;
		public const byte STA_inx = 0x81;
		public const byte STA_iny = 0x91;

		public const byte STX_zpg = 0x86;
		public const byte STX_zpy = 0x96;
		public const byte STX_abs = 0x8E;

		public const byte STY_zpg = 0x84;
		public const byte STY_zpx = 0x94;
		public const byte STY_abs = 0x8C;

		public const byte TAX = 0xAA;
		public const byte TAY = 0xA8;
		public const byte TSX = 0xBA;
		public const byte TXA = 0x8A;
		public const byte TXS = 0x9A;
		public const byte TYA = 0x98;

		private static Dictionary<string, byte> singleModeOpcodes = new Dictionary<string, byte>()
		{
			["BRK"] = BRK,

			["BCC"] = BCC,
			["BCS"] = BCS,
			["BEQ"] = BEQ,
			["BMI"] = BMI,
			["BNE"] = BNE,
			["BPL"] = BPL,
			["BVC"] = BVC,
			["BVS"] = BVS,

			["CLC"] = CLC,
			["CLD"] = CLD,
			["CLI"] = CLI,
			["CLV"] = CLV,

			["DEX"] = DEX,
			["DEY"] = DEY,

			["INX"] = INX,
			["INY"] = INY,

			["JSR"] = JSR,

			["NOP"] = NOP,

			["PHA"] = PHA,
			["PLA"] = PLA,
			["PLP"] = PLP,
			["PHP"] = PHP,

			["RTI"] = RTI,
			["RTS"] = RTS,

			["SEC"] = SEC,
			["SED"] = SED,
			["SEI"] = SEI,

			["TAX"] = TAX,
			["TAY"] = TAY,
			["TSX"] = TSX,
			["TXA"] = TXA,
			["TXS"] = TXS,
			["TYA"] = TYA,
		};

		public static Opcode GetOpcode(string opcStr, Opcode.Mode mode)
		{
			if (singleModeOpcodes.TryGetValue(opcStr, out byte opc))
				return opcodes[opc];

			string modeStr = GetModeTag(mode);
			string oCode = opcStr + modeStr;
			return opcodes.FirstOrDefault(o => o.Value.asm == oCode).Value;
		}

		public static byte GetOpcodeByte(string opcStr, Opcode.Mode mode, string operandStr)
		{
			if (singleModeOpcodes.TryGetValue(opcStr, out byte opc))
				return opc;

			if (operandStr.Length == 0)
				throw new Exception("Invalid operator or operands");

			string modeStr = GetModeTag(mode);
			string oCode = opcStr + modeStr;
			byte opcByte = opcodes.FirstOrDefault(o => o.Value.asm == oCode).Key;
			if (opcByte == 0)
				throw new Exception("Illegal Instruction");

			return opcByte;
		}

		private static string GetModeTag(Opcode.Mode mode)
		{
			switch (mode)
			{
				case Opcode.Mode.Absolute:
					return "_abs";
				case Opcode.Mode.Absolute_X:
					return "_abx";
				case Opcode.Mode.Absolute_Y:
					return "_aby";

				case Opcode.Mode.Accumulator:
					return "_acc";

				case Opcode.Mode.Immediate:
					return "_imm";

				case Opcode.Mode.Implied:
					return "";

				case Opcode.Mode.Indirect:
					return "_ind";
				case Opcode.Mode.Indirect_X:
					return "_inx";
				case Opcode.Mode.Indirect_Y:
					return "_iny";

				case Opcode.Mode.Relative:
					return "";

				case Opcode.Mode.ZeroPage:
					return "_zpg";
				case Opcode.Mode.ZeroPage_X:
					return "_zpx";
				case Opcode.Mode.ZeroPage_Y:
					return "_zpy";

				default:
					throw new Exception($"what the hell is a {mode}?");
			}
		}


		public static Dictionary<byte, Opcode> opcodes = new Dictionary<byte, Opcode>()
		{
			[ADC_imm] = new Opcode("ADC_imm", ADC_imm, 2, 2),
			[ADC_zpg] = new Opcode("ADC_zpg", ADC_zpg, 2, 3),
			[ADC_zpx] = new Opcode("ADC_zpx", ADC_zpx, 2, 4),
			[ADC_abs] = new Opcode("ADC_abs", ADC_abs, 3, 4),
			[ADC_abx] = new Opcode("ADC_abx", ADC_abx, 3, 4),
			[ADC_aby] = new Opcode("ADC_aby", ADC_aby, 3, 4),
			[ADC_inx] = new Opcode("ADC_inx", ADC_inx, 2, 6),
			[ADC_iny] = new Opcode("ADC_iny", ADC_iny, 2, 5),

			[AND_imm] = new Opcode("AND_imm", AND_imm, 2, 2),
			[AND_zpg] = new Opcode("AND_zpg", AND_zpg, 2, 3),
			[AND_zpx] = new Opcode("AND_zpx", AND_zpx, 2, 4),
			[AND_abs] = new Opcode("AND_abs", AND_abs, 3, 4),
			[AND_abx] = new Opcode("AND_abx", AND_abx, 3, 4),
			[AND_aby] = new Opcode("AND_aby", AND_aby, 3, 4),
			[AND_inx] = new Opcode("AND_inx", AND_inx, 2, 6),
			[AND_iny] = new Opcode("AND_iny", AND_iny, 2, 5),

			[ASL_zpg] = new Opcode("ASL_zpg", ASL_zpg, 2, 5),
			[ASL_acc] = new Opcode("ASL_acc", ASL_acc, 1, 2),
			[ASL_zpx] = new Opcode("ASL_zpx", ASL_zpx, 2, 6),
			[ASL_abs] = new Opcode("ASL_abs", ASL_abs, 3, 6),
			[ASL_abx] = new Opcode("ASL_abx", ASL_abx, 3, 7),

			[BCC] = new Opcode("BCC", BCC, 2, 2),
			[BCS] = new Opcode("BCS", BCS, 2, 2),
			[BEQ] = new Opcode("BEQ", BEQ, 2, 2),
			[BMI] = new Opcode("BMI", BMI, 2, 2),
			[BNE] = new Opcode("BNE", BNE, 2, 2),
			[BPL] = new Opcode("BPL", BPL, 2, 2),
			[BVC] = new Opcode("BVC", BVC, 2, 2),
			[BVS] = new Opcode("BVS", BVS, 2, 2),

			[BRK] = new Opcode("BRK", BRK, 1, 7),

			[BIT_zpg] = new Opcode("BIT_zpg", BIT_zpg, 2, 3),
			[BIT_abs] = new Opcode("BIT_abs", BIT_abs, 3, 4),

			[CLC] = new Opcode("CLC", CLC, 1, 2),
			[CLI] = new Opcode("CLI", CLI, 1, 2),
			[CLV] = new Opcode("CLV", CLV, 1, 2),

			[CLD] = new Opcode("CLD", CLD, 1, 2),
			[SED] = new Opcode("SED", SED, 1, 2),

			[CMP_zpg] = new Opcode("CMP_zpg", CMP_zpg, 2, 3),
			[CMP_zpx] = new Opcode("CMP_zpx", CMP_zpx, 2, 4),
			[CMP_imm] = new Opcode("CMP_imm", CMP_imm, 2, 2),
			[CMP_abs] = new Opcode("CMP_abs", CMP_abs, 3, 4),
			[CMP_abx] = new Opcode("CMP_abx", CMP_abx, 3, 4),
			[CMP_aby] = new Opcode("CMP_aby", CMP_aby, 3, 4),
			[CMP_inx] = new Opcode("CMP_inx", CMP_inx, 2, 6),
			[CMP_iny] = new Opcode("CMP_iny", CMP_iny, 2, 5),

			[CPX_zpg] = new Opcode("CPX_zpg", CPX_zpg, 2, 3),
			[CPX_imm] = new Opcode("CPX_imm", CPX_imm, 2, 2),
			[CPX_abs] = new Opcode("CPX_abs", CPX_abs, 3, 4),

			[CPX_zpg] = new Opcode("CPX_zpg", CPX_zpg, 2, 3),
			[CPX_imm] = new Opcode("CPX_imm", CPX_imm, 2, 2),
			[CPX_abs] = new Opcode("CPX_abs", CPX_abs, 3, 4),

			[DEC_zpg] = new Opcode("DEC_zpg", DEC_zpg, 2, 5),
			[DEC_zpx] = new Opcode("DEC_zpx", DEC_zpx, 2, 6),
			[DEC_abs] = new Opcode("DEC_abs", DEC_abs, 3, 6),
			[DEC_abx] = new Opcode("DEC_abx", DEC_abx, 3, 7),

			[DEX] = new Opcode("DEX", DEX, 1, 2),
			[DEY] = new Opcode("DEY", DEY, 1, 2),

			[EOR_zpg] = new Opcode("EOR_zpg", EOR_zpg, 2, 3),
			[EOR_zpx] = new Opcode("EOR_zpx", EOR_zpx, 2, 4),
			[EOR_imm] = new Opcode("EOR_imm", EOR_imm, 2, 2),
			[EOR_abs] = new Opcode("EOR_abs", EOR_abs, 3, 4),
			[EOR_abx] = new Opcode("EOR_abx", EOR_abx, 3, 4),
			[EOR_aby] = new Opcode("EOR_aby", EOR_aby, 3, 4),
			[EOR_inx] = new Opcode("EOR_inx", EOR_inx, 2, 6),
			[EOR_iny] = new Opcode("EOR_iny", EOR_iny, 2, 5),

			[INC_zpg] = new Opcode("INC_zpg", INC_zpg, 2, 5),
			[INC_zpx] = new Opcode("INC_zpx", INC_zpx, 2, 6),
			[INC_abs] = new Opcode("INC_abs", INC_abs, 3, 6),
			[INC_abx] = new Opcode("INC_abx", INC_abx, 3, 7),

			[INX] = new Opcode("INX", INX, 1, 2),
			[INY] = new Opcode("INY", INY, 1, 2),

			[JMP_abs] = new Opcode("JMP_abs", JMP_abs, 3, 3),
			[JMP_ind] = new Opcode("JMP_ind", JMP_ind, 3, 5),
			[JSR] = new Opcode("JSR", JSR, 3, 6),

			[LSR_zpg] = new Opcode("LSR_zpg", LSR_zpg, 2, 5),
			[LSR_zpx] = new Opcode("LSR_zpx", LSR_zpx, 2, 6),
			[LSR_acc] = new Opcode("LSR_acc", LSR_acc, 1, 2),
			[LSR_abs] = new Opcode("LSR_abs", LSR_abs, 3, 6),
			[LSR_abx] = new Opcode("LSR_abx", LSR_abx, 3, 7),

			[LDY_zpg] = new Opcode("LDY_zpg", LDY_zpg, 2, 3),
			[LDY_zpx] = new Opcode("LDY_zpx", LDY_zpx, 2, 4),
			[LDY_imm] = new Opcode("LDY_imm", LDY_imm, 2, 2),
			[LDY_abs] = new Opcode("LDY_abs", LDY_abs, 3, 4),
			[LDY_abx] = new Opcode("LDY_abx", LDY_abx, 3, 4),

			[LDX_zpg] = new Opcode("LDX_zpg", LDX_zpg, 2, 3),
			[LDX_zpy] = new Opcode("LDX_zpy", LDX_zpy, 2, 4),
			[LDX_imm] = new Opcode("LDX_imm", LDX_imm, 2, 2),
			[LDX_abs] = new Opcode("LDX_abs", LDX_abs, 3, 4),
			[LDX_aby] = new Opcode("LDX_aby", LDX_aby, 3, 4),

			[LDA_zpg] = new Opcode("LDA_zpg", LDA_zpg, 2, 3),
			[LDA_zpx] = new Opcode("LDA_zpx", LDA_zpx, 2, 4),
			[LDA_imm] = new Opcode("LDA_imm", LDA_imm, 2, 2),
			[LDA_abs] = new Opcode("LDA_abs", LDA_abs, 3, 4),
			[LDA_abx] = new Opcode("LDA_abx", LDA_abx, 3, 4),
			[LDA_aby] = new Opcode("LDA_aby", LDA_aby, 3, 4),
			[LDA_inx] = new Opcode("LDA_inx", LDA_inx, 2, 6),
			[LDA_iny] = new Opcode("LDA_iny", LDA_iny, 2, 5),

			[NOP] = new Opcode("NOP", NOP, 1, 2),

			[ORA_zpg] = new Opcode("ORA_zpg", ORA_zpg, 2, 3),
			[ORA_zpx] = new Opcode("ORA_zpx", ORA_zpx, 2, 3),
			[ORA_imm] = new Opcode("ORA_imm", ORA_imm, 2, 2),
			[ORA_abs] = new Opcode("ORA_abs", ORA_abs, 3, 4),
			[ORA_abx] = new Opcode("ORA_abx", ORA_abx, 3, 4),
			[ORA_aby] = new Opcode("ORA_aby", ORA_aby, 3, 4),
			[ORA_inx] = new Opcode("ORA_inx", ORA_inx, 2, 6),
			[ORA_iny] = new Opcode("ORA_iny", ORA_iny, 2, 5),

			[PHA] = new Opcode("PHA", PHA, 1, 3),
			[PHP] = new Opcode("PHP", PHP, 1, 3),
			[PLA] = new Opcode("PLA", PLA, 1, 4),
			[PLP] = new Opcode("PLP", PLP, 1, 4),

			[ROL_zpg] = new Opcode("ROL_zpg", ROL_zpg, 2, 5),
			[ROL_zpx] = new Opcode("ROL_zpx", ROL_zpx, 2, 6),
			[ROL_acc] = new Opcode("ROL_acc", ROL_acc, 1, 2),
			[ROL_abs] = new Opcode("ROL_abs", ROL_abs, 3, 6),
			[ROL_abx] = new Opcode("ROL_abx", ROL_abx, 3, 7),

			[ROR_zpg] = new Opcode("ROR_zpg", ROR_zpg, 2, 5),
			[ROR_zpx] = new Opcode("ROR_zpx", ROR_zpx, 2, 6),
			[ROR_acc] = new Opcode("ROR_acc", ROR_acc, 1, 2),
			[ROR_abs] = new Opcode("ROR_abs", ROR_abs, 3, 6),
			[ROR_abx] = new Opcode("ROR_abx", ROR_abx, 3, 7),

			[SBC_zpg] = new Opcode("SBC_zpg", SBC_zpg, 2, 3),
			[SBC_zpx] = new Opcode("SBC_zpx", SBC_zpx, 2, 4),
			[SBC_imm] = new Opcode("SBC_imm", SBC_imm, 2, 2),
			[SBC_abs] = new Opcode("SBC_abs", SBC_abs, 3, 4),
			[SBC_abx] = new Opcode("SBC_abx", SBC_abx, 3, 4),
			[SBC_aby] = new Opcode("SBC_aby", SBC_aby, 3, 4),
			[SBC_inx] = new Opcode("SBC_inx", SBC_inx, 2, 6),
			[SBC_iny] = new Opcode("SBC_iny", SBC_iny, 2, 5),

			[SEC] = new Opcode("SEC", SEC, 1, 2),
			[SEI] = new Opcode("SEI", SEI, 1, 2),

			[STA_zpg] = new Opcode("STA_zpg", STA_zpg, 2, 3),
			[STA_zpx] = new Opcode("STA_zpx", STA_zpx, 2, 4),
			[STA_abs] = new Opcode("STA_abs", STA_abs, 3, 4),
			[STA_abx] = new Opcode("STA_abx", STA_abx, 3, 5),
			[STA_aby] = new Opcode("STA_aby", STA_aby, 3, 5),
			[STA_inx] = new Opcode("STA_inx", STA_inx, 2, 6),
			[STA_iny] = new Opcode("STA_iny", STA_iny, 2, 6),

			[STY_zpg] = new Opcode("STY_zpg", STY_zpg, 2, 3),
			[STY_zpx] = new Opcode("STY_zpx", STY_zpx, 2, 4),
			[STY_abs] = new Opcode("STY_abs", STY_abs, 3, 4),

			[STX_zpg] = new Opcode("STX_zpg", STX_zpg, 2, 3),
			[STX_zpy] = new Opcode("STX_zpy", STX_zpy, 2, 4),
			[STX_abs] = new Opcode("STX_abs", STX_abs, 3, 4),

			[RTI] = new Opcode("RTI", RTI, 1, 6),
			[RTS] = new Opcode("RTS", RTS, 1, 6),

			[TAX] = new Opcode("TAX", TAX, 1, 2),
			[TAY] = new Opcode("TAY", TAY, 1, 2),
			[TSX] = new Opcode("TSX", TSX, 1, 2),
			[TXA] = new Opcode("TXA", TXA, 1, 2),
			[TXS] = new Opcode("TXS", TXS, 1, 2),
			[TYA] = new Opcode("TYA", TYA, 1, 2),
		};
	}

	public class Opcode
	{
		public enum Mode
		{
			Absolute, Absolute_X, Absolute_Y,
			Accumulator,
			Immediate,
			Implied,
			Indirect, Indirect_X, Indirect_Y,
			Relative,
			ZeroPage, ZeroPage_X, ZeroPage_Y
		};
		public string asm;
		public byte opc;
		/// <summary>
		/// Total # of bytes for this opcode.
		/// </summary>
		public byte bytes;
		/// <summary>
		/// For funsies only. Cycle count can change from +0 to +2 depending on page boundary crossing!
		/// </summary>
		public byte cycles;
		public Mode mode;


		public Opcode(string asm, byte opc, byte bytes, byte cycles)
		{
			this.asm = asm;
			this.opc = opc;
			this.bytes = bytes;
			this.cycles = cycles;

			string t;
			if (asm.Length == 3)
			{
				switch (asm)
				{
					case "JSR":
						t = "_abs";
						break;
					case "BCC":
					case "BCS":
					case "BEQ":
					case "BMI":
					case "BNE":
					case "BPL":
					case "BVC":
					case "BVS":
						t = "_rel";
						break;
					default:
						t = "_imp";
						break;
				}
			}
			else
				t = asm.Substring(3, asm.Length - 3);
			switch (t)
			{
				case "_abs":
					mode = Mode.Absolute;
					break;
				case "_abx":
					mode = Mode.Absolute_X;
					break;
				case "_aby":
					mode = Mode.Absolute_Y;
					break;

				case "_acc":
					mode = Mode.Accumulator;
					break;

				case "_imm":
					mode = Mode.Immediate;
					break;
				case "_imp":
					mode = Mode.Implied;
					break;

				case "_ind":
					mode = Mode.Indirect;
					break;
				case "_inx":
					mode = Mode.Indirect_X;
					break;
				case "_iny":
					mode = Mode.Indirect_Y;
					break;

				case "_rel":
					mode = Mode.Relative;
					break;

				case "_zpg":
					mode = Mode.ZeroPage;
					break;
				case "_zpx":
					mode = Mode.ZeroPage_X;
					break;
				case "_zpy":
					mode = Mode.ZeroPage_Y;
					break;
			}
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		{



		/// <summary>
		/// </summary>
		/// <returns></returns>
		{
			{
			}

		}
	}

}
