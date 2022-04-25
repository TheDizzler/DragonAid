using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			return (byte) total;
		}
	}
}
