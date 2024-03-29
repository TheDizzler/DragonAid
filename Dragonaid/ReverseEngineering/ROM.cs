﻿using System;
using System.Collections.Generic;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;

namespace AtomosZ.DragonAid.ReverseEngineering
{
	public static class ROMPlaceholders
	{
		public static byte[] romData;
		public static CPUMemory cpuMemory = new CPUMemory();

		public static byte[] zeroPage = new byte[0x100];
		/// <summary>
		/// 0x100 to 0x1FF of CPU Memory.
		/// </summary>
		public static TheStack theStack = new TheStack();
		/// <summary>
		/// This is first 2000bytes of CPU Memory, including zeroPages [0x000-0x099] and TheStack [0x100-0x200].
		/// </summary>
		public static byte[] nesRam = new byte[0x2000];
		/// <summary>
		/// CPU Memory between 0x6000 and 0x7FFF. For simplicity, using cpu index.
		/// </summary>
		public static byte[] saveRam = new byte[0x2000 + (0x6000)];
		/// <summary>
		/// <para>
		/// 0x2000 - 0x2007 PPU registers (mirrors from 0x2008 to 0x3FFF)<br/>
		/// 0x4000 - 0x4017 APU and I/O registers<br/>
		/// 0x4018 - 0x401F APU and I/O functionality that is normally disabled.<br/>
		/// </para>
		/// </summary>
		public static Dictionary<int, byte> registers = new Dictionary<int, byte>()
		{
			// 0x2000
			[Registers.PPU_Control] = 0x0,
			[Registers.PPU_Status] = 0x0,
			[Registers.PPU_Addr] = 0x0,

			// 0x4000
			[Registers.SpriteDMA] = 0x0,
		};

		public class CPUMemory
		{
			public static byte[] memory = new byte[0x10000];

			public byte this[int index]
			{
				get { return memory[index]; }
				set
				{
					memory[index] = value;
				}
			}

		}

		public class TheStack
		{
			public byte stackPointer = 0xFF;
			private byte[] stack = new byte[0x100];


			public byte this[byte index]
			{
				get { return stack[index]; }
				set
				{
					stack[index] = value;
				}
			}

			public byte this[int index]
			{
				get { return stack[(byte)index]; }
				set
				{
					stack[(byte)index] = value;
				}
			}

			public void Push(byte value)
			{
				stack[stackPointer] = value;
				--stackPointer;
			}

			public void Push(int value)
			{
				stack[stackPointer] = (byte)value;
				--stackPointer;
			}

			public byte Pop(out bool z, out bool n)
			{
				var a = stack[++stackPointer];
				z = a == 0;
				n = a >= 0x80;
				return a;
			}
		}
	}
}
