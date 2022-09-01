using System;
using System.Collections.Generic;
using AtomosZ.DragonAid.Libraries;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static AtomosZ.DragonAid.ReverseEngineering.ROMPlaceholders;


namespace AtomosZ.DragonAid.ReverseEngineering
{
	public class Bank78000
	{
		public static void ResetAPU_78000()
		{
			// set interrupt flag
			registers[Registers.APUStatus] = 0x00; // disable all channels
			registers[Registers.Ctrl2_FrameCounter] = 0x80;
			registers[Registers.APUStatus] = 0x0F;
			registers[Registers.SQ0Sweep] = 0x08;
			registers[Registers.SQ1Sweep] = 0x08;
			nesRam[NESRAM.updateTracks] = 0xFF;

			zeroPages[0xFA] = 0;
			APU_StartNewSequence(0);
			APU_StartNewSequence(0x80); // this is an SFX
		}

		/// <summary>
		/// 0x7B904
		/// </summary>
		/// <param name="a">sequenceId</param>
		private static void APU_StartNewSequence(byte a)
		{
			APU_GetPreviousSequenceID();
			a = ASMHelper.ASL(a, 1, out bool hasCarry);
			if (hasCarry)
			{ //
				APU_SFX_Init(a);
				return;
			}
			// normal track
			if (a == nesRam[NESRAM.APU_SequenceID])
			{
				APU_DisableUpdate();
				return; // this track is already playing
			}
		}

		/// <summary>
		///  0x7B970
		/// </summary>
		/// <param name="a">index of SFX</param>
		private static void APU_SFX_Init(byte a)
		{
			byte x = a;
			registers[Registers.DMCFreq] = 0; // disable DMC
			byte y = 0x0F;
			if (saveRam[SRAM.APU_enableDMC] != 0)
				y = 0x1F;
			a = y;
			registers[Registers.APUStatus] = a;
			zeroPages[0xFA] = (byte)(zeroPages[0xFA] & 0xDF);
			a = romData[ROM.SFX_Pointers.offset + x + 1];
			if (a >= 0x80)
				zeroPages[0xFA] = (byte)(zeroPages[0xFA] | 0x20);
			// _L7B99B_
			a |= 0x80;
			zeroPages[ZeroPages.SFXPointer + 1] = a;
			zeroPages[ZeroPages.SFXPointer + 0] = romData[ROM.SFX_Pointers.offset + x + 0];
			zeroPages[ZeroPages.APU_TrackInstructions + 10] = 0x01;
			zeroPages[0xF5] = 0x00;
			nesRam[NESRAM.APU_Track_Vector + 8] = 0;
			APU_DisableUpdate();
		}

		/// <summary>
		/// 0x7B9BC
		/// </summary>
		private static void APU_DisableUpdate()
		{
			nesRam[NESRAM.APU_SequenceID] |= 0x01; // turn off updates
		}

		private static void APU_GetPreviousSequenceID()
		{
			byte a = nesRam[NESRAM.APU_SequenceID];
			a &= 0xFE; // clear the skip update bit
					   // APU_UpdateCurrentSequence();
			nesRam[NESRAM.APU_SequenceID] = a;
		}

	}
}
