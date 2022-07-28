using System;
using System.Collections.Generic;

namespace AtomosZ.DragonAid.Libraries
{
	public static class ASMHelper
	{
		public static byte ASL(byte operand, int iterations, out bool hasCarry)
		{
			hasCarry = false;
			for (int i = 0; i < iterations; ++i)
			{
				hasCarry = (operand & 0x80) == 0x80;
				operand <<= 1;
			}

			return operand;
		}

		public static byte LSR(byte operand, int iterations, out bool hasCarry)
		{
			hasCarry = false;
			for (int i = 0; i < iterations; ++i)
			{
				hasCarry = (operand & 0x01) == 0x01;
				operand >>= 1;
			}

			return operand;
		}

		public static byte ROL(byte operand, int iterations, ref bool carrySet)
		{
			for (int i = 0; i < iterations; ++i)
			{
				bool hasCarry = (operand & 0x80) == 0x80;
				operand <<= 1;
				if (carrySet)
					operand |= 0x01;
				carrySet = hasCarry;
			}

			return operand;
		}

		public static byte ROR(byte operand, int iterations, ref bool carrySet)
		{
			for (int i = 0; i < iterations; ++i)
			{
				bool hasCarry = (operand & 0x01) == 0x01;
				operand >>= 1;
				if (carrySet)
					operand |= 0x80;
				carrySet = hasCarry;
			}

			return operand;
		}

		public static void Add16Bit(byte operand, ref byte lowByte, ref byte highByte)
		{
			int i = operand + lowByte;
			if (i > Byte.MaxValue)
			{
				lowByte = (byte)(i - Byte.MaxValue);
				++highByte;
			}
			else
			{
				lowByte = (byte)i;
			}
		}

		public static void Add16Bit(byte operand, ref Address address)
		{
			address.Add(operand);
		}

		public static byte ADC(byte a, byte mem, ref bool hasCarry)
		{
			int total = a + mem + (hasCarry ? 1 : 0);
			hasCarry = total > byte.MaxValue;
			return (byte)total;
		}

		/// <summary>
		/// Is this correct?
		/// </summary>
		/// <param name="a"></param>
		/// <param name="mem"></param>
		/// <param name="hasCarry"></param>
		/// <returns></returns>
		public static byte SBC(byte a, byte mem, out bool hasCarry)
		{
			int aInt = a;
			if (aInt - mem < 0)
				hasCarry = false;
			else
				hasCarry = true;
			return (byte)(a - mem);
		}

		/// <summary>
		/// Not ASM but a common function in DQ ROM.
		/// </summary>
		/// <param name="zeroPages"></param>
		/// <param name="a"></param>
		/// <param name="x">zeroPages address</param>
		public static void MultiplyValueAtXByA(byte[] zeroPages, byte a, byte x)
		{ // C055
			zeroPages[PointerList.dynamicSubroutineAddr + 0] = a;
			zeroPages[PointerList.dynamicSubroutineAddr + 1] = 0;
			zeroPages[PointerList.dynamicSubroutineAddr + 2] = 0;

			do
			{
				zeroPages[PointerList.dynamicSubroutineAddr + 0]
				= ASMHelper.LSR(zeroPages[PointerList.dynamicSubroutineAddr + 0], 1, out bool hasCarry);
				if (hasCarry)
				{
					hasCarry = false;
					zeroPages[PointerList.dynamicSubroutineAddr + 1]
						= ASMHelper.ADC(zeroPages[x + 0], zeroPages[PointerList.dynamicSubroutineAddr + 1], ref hasCarry);
					zeroPages[PointerList.dynamicSubroutineAddr + 2]
						= ASMHelper.ADC(zeroPages[x + 1], zeroPages[PointerList.dynamicSubroutineAddr + 2], ref hasCarry);
				}

				zeroPages[x + 0] = ASMHelper.ASL(zeroPages[x + 0], 1, out hasCarry);
				zeroPages[x + 1] = ASMHelper.ROL(zeroPages[x + 1], 1, ref hasCarry);
			}
			while (zeroPages[PointerList.dynamicSubroutineAddr + 0] != 0);

			zeroPages[x + 0] = zeroPages[PointerList.dynamicSubroutineAddr + 1];
			zeroPages[x + 1] = zeroPages[PointerList.dynamicSubroutineAddr + 2];
		}

		/// <summary>
		/// Not ASM op but a common function in DQ ROM
		/// </summary>
		/// <param name="zeroPages"></param>
		/// <param name="a"></param>
		/// <param name="zeroPagePointer"></param>
		/// <param name="y"></param>
		public static void IncrementPointerXBy_AandY(byte[] zeroPages, byte a, byte zeroPagePointer, byte y)
		{
			ASMHelper.Add16Bit(
				a, ref zeroPages[zeroPagePointer], ref zeroPages[zeroPagePointer + 1]);
			zeroPages[zeroPagePointer + 1] += y;
		}

		/// <summary>
		/// Dummy RNG. Always returns 0;
		/// </summary>
		/// <returns></returns>
		public static byte RNG()
		{
			return 0;
		}

		/// <summary>
		/// BIT sets the Z flag as though the value in the address tested were ANDed with the accumulator.
		/// The N and V flags are set to match bits 7 and 6 respectively in the value stored at the tested address. 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="operand"></param>
		/// <param name="n"></param>
		/// <param name="v"></param>
		/// <param name="z"></param>
		public static void BIT(byte a, byte operand, out bool n, out bool v, out bool z)
		{
			z = (a & operand) == 0;
			n = (operand & 0x07) == 0x07;
			v = (operand & 0x06) == 0x06;
		}
	}
}
