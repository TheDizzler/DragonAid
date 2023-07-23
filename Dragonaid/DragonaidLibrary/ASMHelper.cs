using System;
using System.Collections.Generic;

using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;

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

		public static byte ADC(byte a, byte mem, ref bool hasCarry, out bool z, out bool n)
		{
			int result = a + mem + (hasCarry ? 1 : 0);
			hasCarry = result > byte.MaxValue;
			z = result == 0;
			n = result >= 0x80;
			return (byte)result;
		}

		public static byte SBC(byte a, byte mem, ref bool hasCarry)
		{
			//byte memInv = (byte)(-mem);
			//if (hasCarry)
			//	--memInv;
			//byte result = ADC(a, memInv, ref hasCarry); // this way causes issues because we are using int math, not byte

			//byte result = (byte)(a - mem - (hasCarry ? 0 : 1));
			int intResult = a - mem - (hasCarry ? 0 : 1);
			byte result = (byte)(intResult);
			hasCarry = intResult >= 0;
			return result;
		}

		public static byte SBC(byte a, byte mem, ref bool hasCarry, out bool z, out bool n)
		{
			//byte memInv = (byte)(-mem);
			//if (hasCarry)
			//	--memInv;
			//byte result = ADC(a, memInv, ref hasCarry); // this way causes issues because we are using int math, not byte

			int intResult = a - mem - (hasCarry ? 0 : 1);
			byte result = (byte)(intResult);
			hasCarry = intResult >= 0;
			z = result == 0;
			n = result >= 0x80;
			return result;
		}

		public static byte INC(byte operand, out bool z, out bool n)
		{
			var result = ++operand;
			z = result == 0;
			n = result >= 0x80;
			return result;
		}

		public static byte DEC(byte operand, out bool z, out bool n)
		{
			var result = --operand;
			z = result == 0;
			n = result >= 0x80;
			return result;
		}

		public static byte AND(byte a, byte operand, out bool z, out bool n)
		{
			var result = (byte)(a & operand);
			z = result == 0;
			n = result >= 0x80;
			return result;
		}

		public static byte ORA(byte a, byte operand, out bool z, out bool n)
		{
			var result = (byte)(a | operand);
			z = result == 0;
			n = result >= 0x80;
			return result;
		}

		public static byte EOR(byte a, byte operand, out bool z, out bool n)
		{
			var result = (byte)(a ^ operand);
			z = result == 0;
			n = result >= 0x80;
			return result;
		}

		public static void CMP(byte a, byte operand, out bool hasCarry, out bool z, out bool n)
		{
			hasCarry = true;
			var result = SBC(a, operand, ref hasCarry);
			z = result == 0;
			n = result >= 0x80;
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
			n = (operand & 0x80) == 0x80;
			v = (operand & 0x40) == 0x40;
		}


		/// <summary>
		/// Not ASM but a common function in DQIII ROM.
		/// </summary>
		/// <param name="zeroPages"></param>
		/// <param name="a"></param>
		/// <param name="x">zeroPages address</param>
		public static void MultiplyValueAtXByA(byte[] zeroPages, byte a, byte x)
		{ // C055
			zeroPages[ZeroPage.dynamicSubroutine_21 + 0] = a;
			zeroPages[ZeroPage.dynamicSubroutine_21 + 1] = 0;
			zeroPages[ZeroPage.dynamicSubroutine_21 + 2] = 0;

			do
			{
				zeroPages[ZeroPage.dynamicSubroutine_21 + 0]
				= ASMHelper.LSR(zeroPages[ZeroPage.dynamicSubroutine_21 + 0], 1, out bool hasCarry);
				if (hasCarry)
				{
					hasCarry = false;
					zeroPages[ZeroPage.dynamicSubroutine_21 + 1]
						= ASMHelper.ADC(zeroPages[x + 0], zeroPages[ZeroPage.dynamicSubroutine_21 + 1], ref hasCarry);
					zeroPages[ZeroPage.dynamicSubroutine_21 + 2]
						= ASMHelper.ADC(zeroPages[x + 1], zeroPages[ZeroPage.dynamicSubroutine_21 + 2], ref hasCarry);
				}

				zeroPages[x + 0] = ASMHelper.ASL(zeroPages[x + 0], 1, out hasCarry);
				zeroPages[x + 1] = ASMHelper.ROL(zeroPages[x + 1], 1, ref hasCarry);
			}
			while (zeroPages[ZeroPage.dynamicSubroutine_21 + 0] != 0);

			zeroPages[x + 0] = zeroPages[ZeroPage.dynamicSubroutine_21 + 1];
			zeroPages[x + 1] = zeroPages[ZeroPage.dynamicSubroutine_21 + 2];
		}

		/// <summary>
		/// Not ASM op but a common function in DQIII ROM
		/// </summary>
		/// <param name="zeroPages"></param>
		/// <param name="a"></param>
		/// <param name="zeroPagePointer"></param>
		/// <param name="y"></param>
		public static void IncrementValueAtXBy_AandY(byte[] zeroPages, byte a, byte zeroPagePointer, byte y)
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

	}
}
