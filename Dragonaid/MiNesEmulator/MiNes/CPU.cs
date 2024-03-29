﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.ASM;
using AtomosZ.MiNesEmulator.PPU2C02;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static AtomosZ.MiNesEmulator.CPU2A03.ControlUnit6502;

namespace AtomosZ.MiNesEmulator.CPU2A03
{
	/// <summary>
	/// @TODO: (after syncing to git) move to CPU2a03 directory.
	/// <para>
	/// Representational class to encapsulate all relevant components of a computing unit
	/// (Ex: NES => 2A03 (6502CPU + APU), PPU, and cartridge).<br/>
	/// In this way, this class acts as a sort of memory buss (but I don't know if I want to keep it this way)<br/>
	/// Includes methods for running, reseting, and running virtually.<br/>
	/// </para>
	/// </summary>
	public class CPU
	{
		/// <summary>
		/// Runs at  1 / 12 of master CLK. <br/>
		/// CPU = CLK / 12 = 1.789773 MHz
		/// </summary>
		public const double CLK = MiNes.CLK * 12;

		internal ControlUnit6502 controlUnit;
		internal Memory6502 memory;

		protected byte[] romData;

		private PPU.PPULatch ppuLatch;
		private PPU ppu;
		public int cycle;

		public CPU(PPU ppu)
		{
			controlUnit = new ControlUnit6502(this);
			this.ppuLatch = ppu.ppuLatch;
			this.ppu = ppu;
		}

		public void LoadRom(byte[] romData)
		{
			this.romData = romData;
			Initialize();
		}

		public void Reset()
		{
			controlUnit.Reset();
			Initialize();
		}

		/// <summary>
		/// Sets program counter to Reset pointer
		/// </summary>
		public virtual void Initialize()
		{
			memory = Memory6502.Initialize(controlUnit, ppu/*Latch*/, romData);
			controlUnit.pc = GetPointerAt(UniversalConsts.RESET_Pointer);
		}

		/// <summary>
		/// This isn't really needed. Maybe just hold on to it for readability?
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		internal byte Read(int address)
		{
			return memory[address];
		}

		/// <summary>
		/// This isn't really needed. Maybe just hold on to it for readability?
		/// </summary>
		/// <param name="address"></param>
		/// <param name="value"></param>
		internal void Write(int address, byte value)
		{
			memory[address] = value;
		}


		public int GetPointerAt(int lowByteAddress)
		{
			return memory[lowByteAddress] + (memory[lowByteAddress + 1] << 8);
		}


		public void Update()
		{
			throw new Exception("Update not implemented");
		}

		public void ParseAndRunNextInstruction()
		{
			var instr = GetInstruction(controlUnit.pc);
			controlUnit.RunInstruction(instr);
		}

		protected Instruction GetInstruction(int address)
		{
			var instrByte = memory[address];
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
				instr.operands[i - 1] = memory[address + i];
			}

			return instr;
		}

		internal int GetBRKPointer()
		{
			var BRKPointer = UniversalConsts.IRQBRK_Pointer;
			return memory[BRKPointer] + (memory[BRKPointer + 1] << 8);
		}


		public class OAMPort
		{

		}
	}
}
