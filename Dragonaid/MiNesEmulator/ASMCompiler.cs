using System;
using System.Collections.Generic;
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
			public string operandsStr;
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


			public PartiallyCompiledCode(byte opcByte, string operandsStr)
			{
				this.opcByte = opcByte;
				opcode = Opcodes.opcodes[opcByte];
				this.operandsStr = operandsStr;
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
				var instr = ParseInstruction(pcc);
				int addr = pcc.address - 0x8000; // justify to bank address
				/* @TODO: bank address validation? */
				addr += pcc.bankId * 0x4000;

				/* @TODO: make sure only writing to unwritten addresses? */
				CheckAndWriteByte(machineCode, romAddress, instr.opcode.opc);
				for (int i = 0; i < instr.operands.Length; ++i)
				{
					CheckAndWriteByte(machineCode, romAddress + i + 1, instr.operands[i]);
				}
			}

			foreach (var pcd in pcData)
			{
				int addr = pcd.address - 0x8000;
				addr += pcd.bankId * 0x4000;

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
						else if (!byte.TryParse(pcd.operandStrings[1], out value))
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

							if (!byte.TryParse(pcd.operandStrings[i].Substring(0, 2),
								NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte highByte))
							{
								throw new Exception($"Invalid dword parameter on line {pcd.sourceCodeLine} : {pcd.sourceCode}"
									+ "");
							}

							if (!byte.TryParse(pcd.operandStrings[i].Substring(2, 2),
								NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte lowByte))
							{
								throw new Exception($"Invalid dword parameter on line {pcd.sourceCodeLine} : {pcd.sourceCode}"
									+ "");
							}

							CheckAndWriteByte(machineCode, romAddress + (i * 2) + 0, lowByte);
							CheckAndWriteByte(machineCode, romAddress + (i * 2) + 1, highByte);
						}
						break;
				}

			}

			return machineCode;
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


				/* @TODO: */
				// .incbin
				if (line.StartsWith(".nesprg"))
				{
					/* @TODO: check bank count ranges of selected mapper chip. */
					if (!byte.TryParse(line.Substring(".nesprg".Length + 1), out nesprg))
					{
						throw new Exception($"Cannot parse nesprg on line {i + 1} : {line}");
					}

					continue;
				}
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
						throw new Exception($"Invalid label line {i + 1} : {file[i]} - Labels may not contain forbidden characters.");
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
						if (operand[0] == '$')
							operandsStr.Add(operand.Substring(1)); // skip the '$'
						else if (char.IsLetter(operand[0]))
							operandsStr.Add(operand);
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

					if (!byte.TryParse(operandSplit[0], out byte sequenceLength))
					{
						throw new Exception($"Invalid ds syntax at line {i + 1} - *Could note parse sequence length* : {line}");
					}

					if (operandSplit.Length == 2)
					{
						if (operandSplit[1][0] != '$')
							throw new Exception($"Invalid ds syntax at line {i + 1} : {line}");
						operandsStr.Add(operandSplit[1].Substring(1)); // skip the '$'
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
						if (operand[0] == '$')
							operandsStr.Add(operand.Substring(1)); // must be written in little endian
						else if (char.IsLetter(operand[0]))
							operandsStr.Add(operand);
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
					throw new Exception($"Error on line {i + 1} : {e.Message}");
				}
			}
		}

		private static PartiallyCompiledCode PartiallyCompile(string line)
		{
			var opcStr = line.Substring(0, 3);
			string operandStr = "";
			Opcode.Mode mode = Opcode.Mode.Implied;

			for (int i = 4; i < line.Length; ++i)
			{
				//if (line[i] == ';') // this should already have been covered above
				//	break;
				if (char.IsWhiteSpace(line[i]))
					continue;


				if (line[i] == '#')
				{
					mode = Opcode.Mode.Immediate;       // LDA #$44
					operandStr = line.Substring(i + 2, 2);
					break;
				}

				if (line[i] == '(')
				{
					char nextChar = line[i + 4];
					if (nextChar == ')')
					{
						mode = Opcode.Mode.Indirect_Y;  // LDA ($44),Y
						operandStr = line.Substring(i + 2, 2);
					}
					else if (nextChar == ',')
					{
						mode = Opcode.Mode.Indirect_X;  // LDA ($44,X)
						operandStr = line.Substring(i + 2, 2);
					}
					else if (Uri.IsHexDigit(nextChar))
					{
						mode = Opcode.Mode.Indirect;    // JMP ($5597)
						operandStr = line.Substring(i + 2, 4);
					}
					else
						throw new Exception($"Invalid syntax line: {line}");

					break;
				}

				if (line[i] == '$')
				{
					var c = line[++i];
					while (Uri.IsHexDigit(c))
					{
						operandStr += c;
						if (i >= line.Length - 1)
							break;
						c = line[++i];
					}

					if (c == ',')
					{
						if (line[i + 1] == 'X')
						{
							if (operandStr.Length == 2)     // ORA $44,X
								mode = Opcode.Mode.ZeroPage_X;
							else if (operandStr.Length == 4)// ORA $4400,X
								mode = Opcode.Mode.Absolute_X;
							else
								throw new Exception($"Invalid syntax line: {line}");
						}
						else if (line[i + 1] == 'Y')        // ORA $4400,Y
						{
							mode = Opcode.Mode.Absolute_Y;
						}
						else
						{
							throw new Exception($"Invalid syntax line: {line}");
						}
					}
					else
					{
						if (operandStr.Length == 2)         // INC $44
							mode = Opcode.Mode.ZeroPage;
						else if (operandStr.Length == 4)    // JMP $5597
							mode = Opcode.Mode.Absolute;
						else
							throw new Exception($"Invalid syntax line: {line}");
					}
					break;
				}

				if (char.IsLetter(line[i]))
				{
					if (line[i] == 'A' && (line.Length <= i || !char.IsLetter(line[i + 1])))
					{
						mode = Opcode.Mode.Accumulator;     // LSR A
						operandStr = "A";
						break;
					}

					// this should be a label
					operandStr += ":" + line.Substring(i);
					break;
				}
			}

			return new PartiallyCompiledCode(Opcodes.GetOpcodeByte(opcStr, mode), operandStr);
		}



		private static Instruction ParseInstruction(PartiallyCompiledCode halfcode)
		{
			var instr = new Instruction();
			instr.address = halfcode.address;
			var opc = Opcodes.opcodes[halfcode.opcByte];
			instr.opcode = opc;

			instr.operands = new byte[opc.bytes - 1];
			if (opc.mode == Opcode.Mode.Implied         // Implied requires no more extra steps
				|| opc.mode == Opcode.Mode.Accumulator) // Accumulator requires no extra steps
			{
				return instr;
			}


			if (halfcode.operandsStr[0] == ':') // this is a label
			{
				var operandStr = halfcode.operandsStr.Substring(1);
				if (!labels.TryGetValue(operandStr, out Label label))
				{
					// @TODO: can we get line # here?
					throw new Exception($"Label error on line {halfcode.sourceCodeLine} : {halfcode.sourceCode}"
						+ " - could not find definition for label {operandStr}");
				}

				var labelAddr = label.address;
				if (opc.mode == Opcode.Mode.Absolute)
				{
					instr.operands[0] = (byte)(labelAddr);
					instr.operands[1] = (byte)(labelAddr >> 8);
				}
				else if (opc.mode == Opcode.Mode.Relative)
				{
					int diff = labelAddr - halfcode.address;
					if (diff > 127 || diff < -127)
					{
						// @TODO: can we get line # here?
						throw new Exception($"Invalid relative address: {operandStr} (${labelAddr}) to far from ${halfcode.address}");
					}
					else if (diff == 0)
					{
						// @TODO: can we get line # here?
						throw new Exception($"Warning: You have created an infinite loop at {halfcode.address} :"
							+ $": {halfcode.opcode.asm.Substring(0, 3)} {halfcode.operandsStr}");
					}

					byte relativeDistance = (byte)(labelAddr - halfcode.address);
					instr.operands[0] = relativeDistance; // I think I'm off by one (when branching backwards atleast)
				}
			}
			else
			{
				var operandStr = halfcode.operandsStr;
				for (int i = 0; i / 2 < opc.bytes - 1; i += 2)
				{
					if (!byte.TryParse(operandStr.Substring(i, 2),
							NumberStyles.HexNumber,
							CultureInfo.CurrentCulture,
							out byte operandByte))
					{
						throw new Exception($"Unable parse hex from {operandStr.Substring(i, 2)}");
					}

					instr.operands[i / 2] = operandByte;
				}
			}

			return instr;
		}
	}
}
