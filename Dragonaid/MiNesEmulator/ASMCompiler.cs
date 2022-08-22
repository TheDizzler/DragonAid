using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AtomosZ.DragonAid.Libraries;
using static AtomosZ.DragonAid.Libraries.Opcode;

namespace AtomosZ.MiNesEmulator
{
	public static class ASMCompiler
	{
		class PartiallyCompiledCode
		{
			public byte opcByte;
			public Opcode opcode;
			public string operandStr;
			public int address;
			public byte bankId;
			/// <summary>
			/// For debugging/compiler errors.
			/// </summary>
			public int sourceCodeLine;
			/// <summary>
			/// For debugging/compiler errors.
			/// </summary>
			public string sourceCode;


			public PartiallyCompiledCode(byte opcByte, string operandStr)
			{
				this.opcByte = opcByte;
				opcode = Opcodes.opcodes[opcByte];
				this.operandStr = operandStr;
			}
		}

		private class Label
		{
			public int address;
			public int bankId;
			public string label;

			public Label(string label, int pc, byte bankId)
			{
				this.label = label;
				this.address = pc;
				this.bankId = bankId;
			}
		}

		class PartiallyCompiledDataBlock
		{
			public enum PCDType { dword, dsequence, dbyte };
			public PCDType type;
			public int address;
			public string[] operandStrings;
			public int bankId;
			/// <summary>
			/// For debugging/compiler errors.
			/// </summary>
			public int sourceCodeLine;
			/// <summary>
			/// For debugging/compiler errors.
			/// </summary>
			public string sourceCode;
		}

		private const char LABEL_CHAR = ':';

		private static readonly List<char> labelForbiddenCharacters = new List<char>()
		{
			'+', '\\', '/', '%', '$', '#', '(', ')', ',', '.',
		};


		private static byte nesprg;

		private static Dictionary<string, Label> labels;
		private static List<PartiallyCompiledCode> pcCode;
		private static List<PartiallyCompiledDataBlock> pcData;




		public static byte[] Compile(string asmFilepath)
		{
			var file = File.ReadAllLines(asmFilepath);

			GetLabelsAndPartialCompiledCode(file);

			if (nesprg == 0)
				throw new Exception($"Invalid or no nesprg : {nesprg}");


			var machineCode = Enumerable.Repeat((byte)0xFF, nesprg * 0x4000 + Address.iNESHeaderLength).ToArray();

			// Create iNES header
			machineCode[0] = (byte)'N';
			machineCode[1] = (byte)'E';
			machineCode[2] = (byte)'S';
			machineCode[3] = 0x1A;

			machineCode[4] = nesprg; /* @TODO: error checking on prg vs mapper chip */
			machineCode[5] = 0; // neschr;
			machineCode[6] = 0; // MAPPER;
			machineCode[7] = 0; // MAPPER;

			for (int i = 8; i < 16; ++i)
				machineCode[i] = 0; // pad rest with zeros

			foreach (var pcc in pcCode)
			{
				/* @TODO: make sure to check if labels are pointing to zeropages and change opc mode accordingly */
				var instr = ParseInstruction(pcc);
				int romAddress = GetRomJustifiedAddress(pcc.address, pcc.bankId);

				/* 6502 bug: AN INDIRECT JUMP MUST NEVER USE A VECTOR BEGINNING ON THE LAST BYTE OF A PAGE */
				if (pcc.opcByte == Opcodes.JMP_ind)
				{
					if (instr.operands[0] == 0xFF)
						throw new Exception($"{pcc.sourceCodeLine} : {pcc.sourceCode}"
							+ " - AN INDIRECT JUMP MUST NEVER USE A VECTOR BEGINNING ON THE LAST BYTE OF A PAGE");
				}

				/* @TODO: make sure only writing to unwritten addresses? */
				CheckAndWriteByte(machineCode, romAddress, instr.opcode.opc);
				for (int i = 0; i < instr.operands.Length; ++i)
				{
					CheckAndWriteByte(machineCode, romAddress + i + 1, instr.operands[i]);
				}
			}

			foreach (var pcd in pcData)
			{
				int romAddress = GetRomJustifiedAddress(pcd.address, pcd.bankId);

				switch (pcd.type)
				{
					case PartiallyCompiledDataBlock.PCDType.dsequence:
						if (!int.TryParse(pcd.operandStrings[0], out int sequenceCount)
							|| sequenceCount > 0xFF || sequenceCount <= 0)
						{
							throw new Exception($"Invalid sequence count on line {pcd.sourceCodeLine} : {pcd.sourceCode}"
								+ " - sequence must be an unsigned int between 1 and 256 (inclusive)");
						}

						byte value;
						if (pcd.operandStrings.Length == 1)
							value = 0;
						else if (!TryGetHexByte(pcd.operandStrings[1], out value))
						{
							throw new Exception($"Invalid sequence parameter on line {pcd.sourceCodeLine} : {pcd.sourceCode}"
								+ "");
						}

						for (int i = 0; i < sequenceCount; ++i)
							CheckAndWriteByte(machineCode, romAddress + i, value);
						break;


					case PartiallyCompiledDataBlock.PCDType.dbyte:
						for (int i = 0; i < pcd.operandStrings.Length; ++i)
						{
							if (!byte.TryParse(pcd.operandStrings[i], out value))
							{
								throw new Exception($"Invalid dbyte parameter on line {pcd.sourceCodeLine} : {pcd.sourceCode}"
									+ "");
							}
							CheckAndWriteByte(machineCode, romAddress + i, value);
						}

						break;

					case PartiallyCompiledDataBlock.PCDType.dword:
						for (int i = 0; i < pcd.operandStrings.Length; ++i)
						{ // little endian!
							byte highByte;
							byte lowByte;

							if (labels.ContainsKey(pcd.operandStrings[i])) // label
							{
								Label lbl = labels[pcd.operandStrings[i]];
								lowByte = (byte)(lbl.address);
								highByte = (byte)(lbl.address >> 8);
							}
							else // raw hex
							{
								if (!TryGetHexByte(pcd.operandStrings[i].Substring(0, 2), out highByte))
								{
									throw new Exception($"Invalid dword parameter on line {pcd.sourceCodeLine} : {pcd.sourceCode}"
										+ "");
								}

								if (!TryGetHexByte(pcd.operandStrings[i].Substring(2, 2),
									out lowByte))
								{
									throw new Exception($"Invalid dword parameter on line {pcd.sourceCodeLine} : {pcd.sourceCode}"
										+ "");
								}
							}

							CheckAndWriteByte(machineCode, romAddress + (i * 2) + 0, lowByte);
							CheckAndWriteByte(machineCode, romAddress + (i * 2) + 1, highByte);
						}
						break;
				}
			}


			return machineCode;
		}

		private static int GetRomJustifiedAddress(int address, int bankId)
		{
			int addr = address;
			while (addr >= 0x4000)
				addr -= 0x4000;
			/* @TODO: bank address validation? */
			addr += bankId * 0x4000;
			return addr;
		}

		/// <summary>
		/// Adds iNESHeaderLength to addr
		/// </summary>
		/// <param name="writeTo"></param>
		/// <param name="address">Non-iNES header offset address</param>
		/// <param name="writeByte"></param>
		/// <exception cref="Exception"></exception>
		private static void CheckAndWriteByte(byte[] writeTo, int address, byte writeByte)
		{
			address += Address.iNESHeaderLength;
			if (writeTo[address] != 0xFF)
				throw new Exception($"Attempting to write to {address} but it has already been written to.");
			writeTo[address] = writeByte;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="file"></param>
		/// <exception cref="Exception"></exception>
		private static void GetLabelsAndPartialCompiledCode(string[] file)
		{
			pcCode = new List<PartiallyCompiledCode>();
			pcData = new List<PartiallyCompiledDataBlock>();
			labels = new Dictionary<string, Label>();


			int pc = 0;
			byte bankId = 0xFF;
			for (int i = 0; i < file.Length; ++i)
			{
				// remove comments and whitespace
				var line = file[i].Trim();
				var commentStart = line.IndexOf(";");
				if (commentStart != -1)
					line = line.Substring(0, commentStart).Trim();

				if (string.IsNullOrEmpty(line))
					continue;

				if (i == 21)
					Debug.WriteLine("");

				if (line.StartsWith(".nesprg"))
				{
					/* @TODO: check bank count ranges of selected mapper chip. */
					if (!byte.TryParse(line.Substring(".nesprg".Length + 1), out nesprg))
					{
						throw new Exception($"Cannot parse nesprg on line {i + 1} : {line}");
					}

					continue;
				}

				/* @TODO: */
				// .incbin
				// .neschr
				// .nesmap
				// .nesmir


				/* Banks are 0x4000 bytes long.
				 * @TODO: We will need some way to ensure that only main banks (ex: bank #0F and #1F in an MMC1 chip)
				 * are between $C000 and $FFFF and other banks are between $8000 and $BFFF.
				 * @TODO: Ensure not too many banks are assigned for the selected mapper chip.
				 * */
				if (line.StartsWith(".bank")) // @TODO: learn how to manage banks
				{
					if (!byte.TryParse(line.Substring(5), out bankId))
					{
						throw new Exception($"Invalid bank # on line {i + 1} - must be unsigned, non-hex byte between 0 and [bankMaximum]: {file[i]}");
					}

					/* TODO: Check against mapper chip for proper starting location. 
					 * Although, if it's required to always have a .org then it shouldn't be necessary? */
					continue;
				}

				if (bankId == 0xFF)
					throw new Exception($"Invalid format: a bank must be declared - ex: .bank 0");

				/* .org #### is the area of CPU memory that the block will be, NOT the location in the ROM. 
				 *	@TODO: We will need some way to ensure that only main banks (ex: bank #0F and #1F in an MMC1 chip)
				 *	are between $C000 and $FFFF and other banks are between $8000 and $BFFF.
				 */
				if (line.StartsWith(".org"))
				{
					var addrStart = line.IndexOf('$');
					if (addrStart == -1)
						throw new Exception($"Syntax error: No '$' found on line {i + 1} : {file[i]}");

					pc = int.Parse(line.Substring(addrStart + 1), NumberStyles.HexNumber);

					continue;
				}

				/* A label. */
				if (line.Contains(":"))
				{
					var label = line.Substring(0, line.IndexOf(":"));
					if (labelForbiddenCharacters.Any(labelForbiddenCharacters => label.Contains(labelForbiddenCharacters)))
						throw new Exception($"Invalid label line {i + 1} : {file[i]} - Labels may not contain forbidden characters "
							/*+ " and must not start with a number"*/);
					//if (!label.Any(c => char.IsLetter(c)))
					//	throw new Exception($"Invalid label line {i + 1} : {file[i]} - Labels must contain at least one letter");
					if (label.Length == 1 && label[0] == 'A')
						throw new Exception($"Invalid label name 'A', line {i + 1} : {file[i]}");
					labels.Add(label, new Label(label, pc, bankId));
					continue;
				}


				/* should these be treated like regular code? */

				if (line.StartsWith(".db")) // this could be operated on exactly like .dw probably?
				{ // one or more bytes
				  // .db $01,$02,$06,$0A
					var index = line.IndexOf(".db ");
					if (index == -1)
						throw new Exception($"Invalid db syntax at line {i + 1} : {line}");
					var operands = line.Substring(index + 4);
					var operandSplit = operands.Split(',');
					var operandsStr = new List<string>();
					foreach (var operand in operandSplit)
					{
						var opr = operand.Trim();
						if (opr[0] == '$')
							operandsStr.Add(opr.Substring(1)); // skip the '$'
						else if (char.IsLetter(opr[0]))
							operandsStr.Add(opr);
						else
							throw new Exception($"Invalid db syntax at line {i + 1} : {line}");
					}

					var pcd = new PartiallyCompiledDataBlock()
					{
						type = PartiallyCompiledDataBlock.PCDType.dbyte,
						address = pc,
						operandStrings = operandsStr.ToArray(),
						bankId = bankId,
						sourceCodeLine = i,
						sourceCode = line,
					};
					pcData.Add(pcd);
					pc += operandSplit.Length;
					continue;
				}

				if (line.StartsWith(".ds"))
				{ // sequence of defined byte length, if only one parameter, then all bytes 0
				  // .ds 3,$CC		; define 3 bytes of $CC
				  // .ds 1			; define 1 byte of $00
					var index = line.IndexOf(".ds ");
					if (index == -1)
						throw new Exception($"Invalid ds syntax at line {i + 1} : {line}");
					var operands = line.Substring(index + 4);

					var operandSplit = operands.Split(',');
					if (operandSplit.Length > 2 || operandSplit.Length == 0)
						throw new Exception($"Invalid ds syntax at line {i + 1} - *must have one or two parameters* : {line}");

					if (!char.IsDigit(operandSplit[0][0]))
						throw new Exception($"Invalid ds syntax at line {i + 1} - *must use unsigned integer* : {line}");

					var operandsStr = new List<string>();
					operandsStr.Add(operandSplit[0].Trim());

					if (!byte.TryParse(operandSplit[0].Trim(), out byte sequenceLength))
					{
						throw new Exception($"Invalid ds syntax at line {i + 1} - *Could note parse sequence length* : {line}");
					}

					if (operandSplit.Length == 2)
					{
						var opr = operandSplit[1].Trim();
						if (opr[0] != '$')
							throw new Exception($"Invalid ds syntax at line {i + 1} : {line}");
						operandsStr.Add(opr.Substring(1)); // skip the '$'
					}

					var pcd = new PartiallyCompiledDataBlock()
					{
						type = PartiallyCompiledDataBlock.PCDType.dsequence,
						address = pc,
						operandStrings = operandsStr.ToArray(),
						bankId = bankId,
						sourceCodeLine = i + 1,
						sourceCode = line,
					};
					pcData.Add(pcd);
					pc += sequenceLength;
					continue;
				}

				if (line.StartsWith(".dw"))
				{ // define one or more 2 byte words
				  // .dw $F1F0,$F1F2
					var index = line.IndexOf(".dw ");
					if (index == -1)
						throw new Exception($"Invalid dw syntax at line {i + 1} : {line}");

					var operands = line.Substring(index + 4);
					var operandSplit = operands.Split(',');
					var operandsStr = new List<string>();
					foreach (var operand in operandSplit)
					{
						var opr = operand.Trim();
						if (opr[0] == '$')
							operandsStr.Add(opr.Substring(1)); // must be written in little endian
						else if (char.IsLetter(opr[0]))
							operandsStr.Add(opr);
						else
							throw new Exception($"Invalid dw syntax at line {i + 1} : {line}");
					}

					var pcd = new PartiallyCompiledDataBlock()
					{
						type = PartiallyCompiledDataBlock.PCDType.dword,
						address = pc,
						operandStrings = operandsStr.ToArray(),
						bankId = bankId,
						sourceCodeLine = i + 1,
						sourceCode = line,
					};
					pcData.Add(pcd);
					pc += operandSplit.Length * 2;
					continue;
				}


				// we can assume this is code for our purposes
				try
				{
					var pcc = PartiallyCompile(line);
					pcc.address = pc;
					pcc.bankId = bankId;
					pcc.sourceCodeLine = i + 1;
					pcc.sourceCode = line;

					pcCode.Add(pcc);

					pc += pcc.opcode.bytes;
				}
				catch (Exception e)
				{
					throw new Exception($"Error on line {i + 1} : {line} - {e.Message}");
				}
			}
		}

		private static PartiallyCompiledCode PartiallyCompile(string line)
		{
			var opcStr = line.Substring(0, 3);
			var operands = line.Substring(3).Trim();
			var operandStr = "";
			Opcode.Mode mode = Opcode.Mode.Implied;

			if (operands.Length == 0)
			{   // might need to switch modes, but I think that's done automatically for these later

			}
			else if (operands[0] == '#') // Immediate
			{
				// @TODO?: allow for immediate with labels. Probably would require special label declaration.
				mode = Opcode.Mode.Immediate;           // LDA #$44
				operandStr = operands.Substring(2);
				// if single digit, pad with leading 0 to prevent byte parse error later
				if (operandStr.Length == 1)
					operandStr = operandStr.Insert(0, "0");
				else if (operandStr.Length != 2)
					throw new Exception($"Syntax error: illegal Immediate operand.");
			}
			else if (char.IsLetter(operands[0])) // Accumulator or Absolute
			{
				if (operands.Length == 1 && operands[0] == 'A') // Accumulator
				{
					mode = Opcode.Mode.Accumulator;     // LSR A
					operandStr = "A";
				}
				else // this should be a label
				{
					/* this could be:
					 * Absolute ,X ,Y;	
					 * Relative;		- this is taken care of automatically when OPC is decoded?
					 * ZeroPage ,X ,Y;	- these will need to be determined when resolving the label address
					 * (Immediate? This could be done) */
					var x = operands.LastIndexOf(",X");
					var y = operands.LastIndexOf(",Y");

					if (y >= 0 && x == -1)
					{
						mode = Mode.Absolute_Y;     // STA label,Y
						operandStr = LABEL_CHAR + operands.Substring(0, y);
					}
					else if (x >= 0 && y == -1)
					{
						mode = Mode.Absolute_X;     // STA label,X
						operandStr = LABEL_CHAR + operands.Substring(0, x);
					}
					else if (x == -1 && y == -1)
					{
						mode = Mode.Absolute;       // STA label
						operandStr = LABEL_CHAR + operands;
					}
					else
						throw new Exception("Syntax error: invalid Absolute label syntax");
				}
			}
			else if (operands[0] == '(') // Indirect
			{
				if (operands[1] != '$') // this should be a label
				{
					var x = operands.LastIndexOf(",X");
					var y = operands.LastIndexOf(",Y");
					if (y >= 0 && x == -1)
					{
						mode = Mode.Indirect_Y;         // LDA (label,Y)
						operandStr = LABEL_CHAR + operands.Substring(1, operands.IndexOf(')') - 1);
					}
					else if (x >= 0 && y == -1)
					{
						mode = Mode.Indirect_X;         // LDA (label),X
						operandStr = LABEL_CHAR + operands.Substring(1, x - 1);
					}
					else if (y == -1 && x == -1)
					{
						mode = Opcode.Mode.Indirect;    // JMP (label)
						operandStr = LABEL_CHAR + operands.Substring(1, operands.IndexOf(')') - 1);
					}
					else
						throw new Exception("Syntax error: invalid Indirect label syntax");
				}
				else // not a label
				{
					var x = operands.LastIndexOf(",X");
					var y = operands.LastIndexOf(",Y");

					if (y >= 0 && x == -1)
					{
						mode = Opcode.Mode.Indirect_Y;  // LDA ($44),Y
						operandStr = line.Substring(2, 2);
					}
					else if (x >= 0 && y == -1)
					{
						mode = Opcode.Mode.Indirect_X;  // LDA ($44,X)
						operandStr = line.Substring(2, 2);
					}
					else if (y == -1 && x == -1)
					{
						mode = Opcode.Mode.Indirect;    // JMP ($5597)
						operandStr = line.Substring(2, 4);
					}
					else
						throw new Exception($"Syntax error: invalid Indirect syntax");
				}
			}
			else if (operands[0] == '$')
			{
				/* This could be:
				 * Absolute ,X,Y
				 * Relative			- resolved when opc decoded
				 * ZeroPage ,X,Y */

				var x = operands.LastIndexOf(",X");
				var y = operands.LastIndexOf(",Y");
				if (x >= 0 && y == -1)
				{
					operandStr = operands.Substring(1, x - 1);
					if (operandStr.Length == 2)
						mode = Mode.ZeroPage_X;         // AND $44,X
					else if (operandStr.Length == 4 || operandStr.Length == 3)
						mode = Mode.Absolute_X;         // AND $4444,X
					else
						throw new Exception($"Syntax error: invalid operand length");
				}
				else if (y >= 0 && x == -1)
				{
					operandStr = operands.Substring(1, y - 1);
					if (operandStr.Length == 2)
						mode = Mode.ZeroPage_Y;         // AND $44,Y
					else if (operandStr.Length == 4 || operandStr.Length == 3)
						mode = Mode.Absolute_Y;         // AND $4444,Y
					else
						throw new Exception($"Syntax error: invalid operand length");
				}
				else if (x == -1 && y == -1)
				{
					operandStr = operands.Substring(1);
					if (operandStr.Length == 2)
						mode = Mode.ZeroPage;         // AND $44
					else if (operandStr.Length == 4 || operandStr.Length == 3)
						mode = Mode.Absolute;         // AND $4444
					else
						throw new Exception($"Syntax error: invalid operand length");
				}
				else
					throw new Exception($"Syntax error");
			}
			else
				throw new Exception("Invalid syntax? What the hell is this?");

			try
			{
				var opc = new PartiallyCompiledCode(Opcodes.GetOpcodeByte(opcStr, mode, operandStr), operandStr);
				return opc;
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}


		/// <summary>
		/// Labels must have already been parsed!
		/// </summary>
		/// <param name="pcc"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		private static Instruction ParseInstruction(PartiallyCompiledCode pcc)
		{
			var instr = new Instruction();
			instr.address = pcc.address;
			var opc = Opcodes.opcodes[pcc.opcByte];
			instr.opcode = opc;

			instr.operands = new byte[opc.bytes - 1];
			if (opc.mode == Opcode.Mode.Implied         // Implied requires no more extra steps
				|| opc.mode == Opcode.Mode.Accumulator) // Accumulator requires no extra steps
			{
				return instr;
			}


			if (pcc.operandStr[0] == LABEL_CHAR) // this is a label
			{
				var operandStr = pcc.operandStr.Substring(1);
				byte offset = 0;
				int plus = operandStr.IndexOf('+');
				if (plus != -1)
				{
					if (!byte.TryParse(operandStr.Substring(plus + 1), out offset))
						throw new Exception($"Label parse error on line {pcc.sourceCodeLine} : {pcc.sourceCode}"
							+ $" - Unable to determine offset from {operandStr}");
					operandStr = operandStr.Substring(0, plus);
				}

				if (!labels.TryGetValue(operandStr, out Label label))
				{
					throw new Exception($"Label error on line {pcc.sourceCodeLine} : {pcc.sourceCode}"
						+ $" - could not find definition for label {operandStr}");
				}

				var labelAddr = label.address;
				// if address below 0x100 then it's in the zeropages and we need to change the opcode
				// UNLESS it's Indirect, JMP, JSR
				var opcTag = opc.asm.Substring(0, 3);
				if (labelAddr <= 0xFF && opcTag != "JMP" && opcTag != "JSR" && opc.asm.Substring(3) != "_ind")
				{
					// this is a mode.Zeropage
					opc = Opcodes.opcodes[(byte)(opc.opc - 0x08)];
					instr.opcode = opc;
				}

				labelAddr += offset;

				switch (opc.mode)
				{
					case Mode.Absolute:
					case Mode.Absolute_X:
					case Mode.Absolute_Y:
						instr.operands[0] = (byte)(labelAddr);
						instr.operands[1] = (byte)(labelAddr >> 8);
						break;

					case Mode.Indirect:
					case Mode.Indirect_X:
					case Mode.Indirect_Y:
						instr.operands[0] = (byte)labelAddr;
						break;

					case Mode.Relative:
					{
						int diff = labelAddr - pcc.address;
						if (diff > 127 || diff < -127)
						{
							throw new Exception($"Invalid relative address on line {pcc.sourceCodeLine} : {pcc.sourceCode}"
								+ " - {operandStr} (${labelAddr}) to far from ${pcc.address}");
						}
						else if (diff == 0)
						{
							throw new Exception($"Warning: You have created an infinite loop on line {pcc.sourceCodeLine} : {pcc.sourceCode} -"
								+ $": {pcc.opcode.asm.Substring(0, 3)} {pcc.operandStr}");
						}

						byte relativeDistance = (byte)(labelAddr - pcc.address);
						instr.operands[0] = relativeDistance;
					}
					break;

					default:
						throw new Exception("No, bro, this shouldn't show");
				}
			}
			else
			{
				var operandStr = pcc.operandStr;

				if (!TryGetHexInt(operandStr, out int address))
					throw new Exception($"Cannot parse address on line {pcc.sourceCodeLine} : {pcc.sourceCode}.");

				if (opc.bytes == 3 && address <= 0xFF)
					Debug.WriteLine($"Warning on line {pcc.sourceCodeLine} : {pcc.sourceCode}"
						+ " - an Absolute mode opcode has a Zeropage operand.");
				else if (opc.bytes == 2 && address > 0xFF)
					throw new Exception("Should this even be possible? I seems I mucked up the parser somewhere.");

				instr.operands[0] = (byte)(address);
				if (opc.bytes == 3)
					instr.operands[1] = (byte)(address >> 8);
			}


			return instr;
		}


		private static void GetWordBytes(int address, out byte lowByte, out byte highByte)
		{
			lowByte = (byte)(address);
			highByte = (byte)(address >> 8);
		}

		private static bool TryGetHexByte(string text, out byte value)
		{
			return byte.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
		}

		private static bool TryGetHexInt(string text, out int value)
		{
			return int.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
		}
	}
}
