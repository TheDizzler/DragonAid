using System;
using System.Collections.Generic;
using System.Text;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.DragonAid.Libraries.PointerList;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;
using static AtomosZ.DragonAid.ReverseEngineering.ROMPlaceholders;

namespace AtomosZ.DragonAid.ReverseEngineering
{
	/// <summary>
	/// This bank or Bank3C000 is always set to $C000 -$FFFF
	/// </summary>
	internal class Bank7C000
	{
		/// <summary>
		/// <para>7FF94 - identical to 3C000.BankSwitch
		/// <br>Switch out 0x8000 to 0xBFFF with bank from ROM.</br>
		/// <br>If BankId &lt; 0x10, 0xC000 to 0xFFFF is set to ($0F) $3C000 bank from ROM.</br>
		/// <br>If BankId &gt;= 0x10, 0xC000 to 0xFFFF is set to ($1F) $7C000 bank from ROM.</br>
		/// </para>
		/// </summary>
		/// <param name="bankId"></param>
		public static void BankSwitch(byte bankId)
		{

		}

		/// <summary>
		/// Subroutines that should only run during NMI
		/// </summary>
		internal class NMI
		{
			internal static void NMI_RunAPUEngine()
			{
				// Switch to High Banks // this is totally pointless...and it does it twice!
				APU_RunEngine();
				BankSwitch(nesRam[Pointers.NESRAM.bankSwitch_CurrentBankId]); // return to 3C000 NMI
			}


			internal static void APU_RunEngine()
			{
				if ((registers[Registers.APUStatus] & 0x10) == 0) // if DMC interupt is not set
				{
					// APU_SetDMCChanel
					// this is probably used to control the volume of the triangle & noise channels
					registers[Registers.DmcCounter] = 0x7E;
					//APU_PrepChanneslForUpdate
					registers[Registers.APUStatus] = 0x0F; // turn on all channels except DMC
					saveRam[SRAM.APU_enableDMC] = 0;
				}
				// APU_Commence_Update
				APU_UpdateSequence();
			}

			private static void APU_UpdateSequence()
			{
				byte a = (byte)(nesRam[NESRAM.APU_SequenceID] & 0x01);
				if (a == 0)
					return;
				APU_Sub_Update_A(a);

				byte y = 0x00;
				byte x;
				if (APU_TrackNeedsUpdating(0x08)) // DMC?
					APU_PreUpdateTrack(0x08, 0x00);
				bool hasCarry = false;
				a = ASMHelper.ADC(nesRam[NESRAM.updateTracks], nesRam[0x047D], ref hasCarry); // APU_SequenceHeader
				nesRam[NESRAM.updateTracks] = a;

				if (hasCarry)
				{
					// APU_UpdateTracks_Loop
					do
					{
						nesRam[NESRAM.updateTracks] -= 0x96;
						++y;
						if (APU_TrackNeedsUpdating(0x00))
							APU_PreUpdateTrack(0x00, y);
						if (APU_TrackNeedsUpdating(0x04))
							APU_PreUpdateTrack(0x04, y);
						--y;
						if (APU_TrackNeedsUpdating(0x06))
							APU_PreUpdateTrack(0x06, y);
					}
					while (nesRam[NESRAM.updateTracks] < 0x6A);

					x = 0x06;

					while (x < 0x80)// APU_CheckNextTrackByte_Loop
					{
						int address = zeroPages[ZeroPages.APU_TrackPosition + x + 0]
							+ zeroPages[ZeroPages.APU_TrackPosition + x + 1] << 8;
						a = romData[address];
						a ^= 0xFC;
						if (a == 0)
							break;
						x -= 2;
					}

					x = 0x06;
					while (x < 0x80) // __APU_FindNonZeroTrackInstruction_Loop_
					{
						a = zeroPages[ZeroPages.APU_TrackInstructions + x + 2];
						if (a != 0)
							break;
						x -= 2;
					}

					if (a != 0)
					{
						a = 0xDF;
						a &= nesRam[0x047F];
					}
					else
					{ // found a non-zero in track instructions
						a = 0x20;
						a |= nesRam[0x047F];
					}
					// _L7B3EF_
					nesRam[0x047F] = a;
				}
				// APU_Sq0_Update();
				x = 0;
				y = 0;
				APU_CheckIfNextNote(x, y);
				APU_DutySetup(x);
				// APU_Sq1_PreSetup();
				x = 0x02;
				y = 0x04;
				if (zeroPages[ZeroPages.APU_TrackInstructions + 10] != 0
					&& (zeroPages[0xFA] & 0x20) != 0)
				{
					APU_Track_Vector_ClearBit7();
					... // hidden ...
				}
				// APU_Sq1_Update
				APU_CheckIfNextNote(x, y);
				APU_DutySetup(x);

				// APU_Tri_Update
				x = 0x04;
				y = 0x08;
				APU_CheckIfNextNote(x, y);
				a = zeroPages[0xFB]; // APU_Tri_Linear_Store
				a ^= 0xFF;
				a &= 0xFE;
				registers[Registers.TriangleLinear] = a;

				// APU_Noise_Update
				x = 0x06;
				y = 0x0C;
				if (zeroPages[ZeroPages.APU_TrackInstructions + 10] != 0)
				{
					a = zeroPages[0xFA]; // APU_NewSeq_20or00
					a &= 0x20;
					if (a == 0)
					{
						APU_Track_Vector_ClearBit7();
						... // hidden ..
					}
				}
				// _APU_Noise_update_cont_
				APU_CheckIfNextNote(x, y);
				APU_DutySetup(x);
			}

			private static void APU_CheckIfNextNote(byte x, byte y)
			{
				byte a = nesRam[0x0472 + x + 1]; // APU_Track_Vector

				if (!APU_SetOrClearCarry(x)
					&& a >= 0x80)
					return;
				// _L7B729_
				a |= 0x80;
				nesRam[0x0472 + x + 1] = a; // APU_Track_Vector
				if (y == 0x0C)
					APU_SetNoise_PeriodAndLength();
				else
					APU_GetNote();
			}

			/// <summary>
			/// 7B449
			/// </summary>
			/// <param name="x">track index</param>
			private static void APU_DutySetup(byte x)
			{
				nesRam[0x06F1] = x; // APU_StoreTrackIndex
				byte a = zeroPages[ZeroPages.APU_TempDutySettings + x + 1]; // APU_DutySetup_Vector
				if (a > 0x80)
				{
					APU_GetDutySetting(ref x);
				}
				else
				{
					x = 0x30;
					if (a != 0xFF)
						x = 0x32;
				}
				// _APU_ReturnFromGetDutySetting_
				a = x;
				if (nesRam[0x06F1] < 0x08) // APU_StoreTrackIndex
				{ // track 0, 2, 4, 6
					zeroPages[ZeroPages.APU_TrackInstructions + 1] = (byte)(a & 0x0F);
					zeroPages[ZeroPages.APU_TrackInstructions + 0] = nesRam[0x047F];
					zeroPages[ZeroPages.APU_TrackInstructions + 0] = APU_GetFinalDuty();
					a &= 0xF0;
					a &= zeroPages[ZeroPages.APU_TrackInstructions + 0];
				}
				//APU_SetDuty()

			}

			/// <summary>
			/// 7B478
			/// </summary>
			private static void APU_GetDutySetting(ref byte x)
			{
				byte y = zeroPages[ZeroPages.APU_TempDutySettings + x + 0];
				zeroPages[ZeroPages.APU_TrackInstructions + 0]
					= romData[Pointers.ROM.APU_DutySetup_Addresses.offset + y + 0];
				zeroPages[ZeroPages.APU_TrackInstructions + 1]
					= romData[Pointers.ROM.APU_DutySetup_Addresses.offset + y + 1];

				int dutySettingsAddress = zeroPages[ZeroPages.APU_TrackInstructions + 0]
					+ zeroPages[ZeroPages.APU_TrackInstructions + 1 << 8];
				byte a;
				while (true)
				{
					a = romData[dutySettingsAddress + y];
					if (a >= 0x30)
						break;
					zeroPages[ZeroPages.APU_TempDutySettings + x + 1] = a;
					y = a;
				}

				++zeroPages[ZeroPages.APU_TempDutySettings + x + 1];
				x = a;
			}

			private static void APU_Sub_Update_A(byte a)
			{
				ASMHelper.BIT(a, zeroPages[0xFA], out bool n, out bool v, out bool z);
				if (v)
					return;
				a = zeroPages[0xFA];
				a = (byte)(a - 0x01);
				zeroPages[0xFA] = a;
				if ((a & 0x1F) != 0)
					return;
				a = (byte)(a | 0x04);
				zeroPages[0xFA] = a;
				if ((a & 0x80) != 0)
					L7B8B3();
				else
				{
					APU_TrackByte_is_F4();
					... // and more that we have not seen
				}
			}

			private static bool APU_TrackNeedsUpdating(byte x)
			{
				return (zeroPages[ZeroPages.APU_TrackInstructions + x + 2] != 0);
			}

			/// <summary>
			/// 7B4E3
			/// </summary>
			/// <param name="x"></param>
			private static void APU_PreUpdateTrack(byte x, byte y)
			{
				if (zeroPages[ZeroPages.APU_TrackInstructions + x + 12] != 0
					&& --zeroPages[ZeroPages.APU_TrackInstructions + x + 12] == 0)
				{
					byte a = zeroPages[ZeroPages.APU_TempDutySettings + x + 1]; // APU_DutySetup_Vector
					if (a != 0xFF)
					{
						zeroPages[ZeroPages.APU_TempDutySettings + x + 1] = 0xFE; // APU_DutySetup_Vector
						if (APU_SetOrClearCarry(x))
							nesRam[0x06F4] = 0xFF;
					}
				}
				// APU_UpdateTrack_cont_
				if (--zeroPages[ZeroPages.APU_TrackInstructions + x + 2] != 0)// when this reaches 0, update track. Note length tracker?
					return;
				//B503
				if (x == 0x00)
					APU_DisableSweep(Registers.SQ0Sweep);
				else if (x == 0x08)
					APU_DisableSweep(Registers.SQ1Sweep);
				else if (x == 0x02 && zeroPages[ZeroPages.APU_TrackInstructions + 10] == 0)
					APU_DisableSweep(Registers.SQ1Sweep); // this probably never happens?
				APU_ParseNextTrackByte(zeroPages[ZeroPages.APU_TrackInstructions + 10], x, y);
			}

			/// <summary>
			/// 7B51F
			/// </summary>
			/// <param name="x"></param>
			private static void APU_ParseNextTrackByte(byte a, byte x, byte y)
			{
				theStack.Push(a);
				byte nextTrackByte = APU_ReadNextTrackByte(x);
				bool hasCarry = false;
				switch (nextTrackByte)
				{
					case 0xFF:
						nextTrackByte = APU_ReadNextTrackByte(x);
						... // hidden....
							break;

					case 0xFE:
						APU_LoopTrack();
						break;

					case 0xFD: // move track position to stored address at 0x06F7
						zeroPages[ZeroPages.APU_TrackPosition + x + 0] = nesRam[0x06F7 + x + 0];
						zeroPages[ZeroPages.APU_TrackPosition + x + 1] = nesRam[0x06F7 + x + 1];
						APU_ParseNextTrackByte(a, x, y);
						break;

					case 0xFC:  // 7B535
						APU_ParseNextTrackByte(a, x, y); // nothing code?
						break;

					/// 7B54E
					case 0xFB:
						nextTrackByte = APU_ReadNextTrackByte(x);
						... // hidden ...
						break;

					// 7B4BD
					case 0xFA: /// Rewind track by ($FF - $YY bytes) [$FA $XX $YY]. $XX is stored sometimes.
						byte storeTrackByte = nesRam[0x0472 + x];
						nextTrackByte = APU_ReadNextTrackByte(x);
						if (storeTrackByte == 0)
							nesRam[0x0472 + x] = nextTrackByte;
						nextTrackByte = APU_ReadNextTrackByte(x);
						if (--nesRam[0x0472 + x] != 0)
						{
							hasCarry = false;
							zeroPages[ZeroPages.APU_TrackPosition + x + 0]
								= ASMHelper.ADC(nextTrackByte, zeroPages[ZeroPages.APU_TrackPosition + x + 0], ref hasCarry);
							if (!hasCarry)
								--zeroPages[ZeroPages.APU_TrackPosition + x + 1];
						}

						APU_ParseNextTrackByte(a, x, y);
						break;

					case 0xF9:
						... // hidden ...
						break;

					case 0xF8:
						// ??
						break;

					case 0xF7:
						// ??
						break;

					// B583
					case 0xF6:
						nextTrackByte = APU_ReadNextTrackByte(x);
						... // hidden ...
						break;

					case 0xF5:
						APU_TrackByte_is_F5();
						... // hidden ...
						break;

					case 0xF4:
						APU_TrackByte_is_F4();
						... // hidden ...
						break;

					case 0xF3:
						APU_ParseNextTrackByte(x);
						break;

					case 0xF2:
						nextTrackByte = APU_ReadNextTrackByte(x);
						... // hidden ...
						break;

					case 0xF1:
						nextTrackByte = APU_ReadNextTrackByte(x);
						... // hidden ...
						break;

					case 0xF0:
						APU_TrackByte_Is_F0();
						APU_ParseNextTrackByte(a, x, y);
						break;

					case 0xEF:
						zeroPages[0xFA] = (byte)((zeroPages[0xFA] & 0x20) | 0x44); // == 44 or 64
						APU_ParseNextTrackByte(a, x, y);
						break;

					case 0xEE:
					case 0xED:
					case 0xEC:
					case 0xEB:
					case 0xEA:
					case 0xE9:
					case 0xE8:
					case 0xE7:
					case 0xE6:
					case 0xE5:
					case 0xE4:
					case 0xE3:
					case 0xE2:
					case 0xE1:
						nextTrackByte = APU_ReadNextTrackByte(x);
						... // hidden ...
						break;

					/// hypothesis: this is the next note info
					default: // below 0xE1
						hasCarry = true;
						a = ASMHelper.SBC(nextTrackByte, 0x4B, ref hasCarry);
						while (hasCarry)
							a = ASMHelper.SBC(nextTrackByte, 0x4B, ref hasCarry);
						a = ASMHelper.ADC(a, 0x4B, ref hasCarry);
						if (a == 0x49)
						{
							a = 0xFF;
							zeroPages[ZeroPages.APU_TempDutySettings + x + 1] = a; // APU_DutySetup_Vector
						}
						else if (a < 0x49)
						{
							if (y != 0)
								a += nesRam[0x47E];
							nesRam[0x0472 + x + 1] = a; // APU_Track_Vector
							if (APU_SetOrClearCarry(x))
								nesRam[0x06F4] = 0x00;
							a = zeroPages[ZeroPages.APU_TrackInstructions + x + 13];
							if (a < 0x80)
							{
								a = 0x0;
								zeroPages[ZeroPages.APU_TempDutySettings + x + 1] = a; // APU_DutySetup_Vector
							}
						}
						// L72624();
						if (x == 0x02 || x == 0x04) // probably 2 is SQ1 and 4 is triangle?
						{
							a = (byte)(romData[0x7B579 + x] & nesRam[0x47F]); //APU_NoteLoByte_Effector_Vector & APU_NoteLoByte_Effector
							if (a != 0)
							{
								zeroPages[ZeroPages.APU_TrackInstructions + x + 3] = zeroPages[ZeroPages.APU_TrackInstructions + x + 1];
								zeroPages[ZeroPages.APU_TrackInstructions + x + 13] = zeroPages[ZeroPages.APU_TrackInstructions + x + 11];
							}
						}
						// APU_All_Tracks
						nextTrackByte = theStack.Pop();
						if (nextTrackByte <= 0x96)
						{
							nextTrackByte = APU_ReadNextTrackByte(x);
							... // hidden ...
						} // this MIGHT fall into...
						  //_L7B64D_

						if (nextTrackByte >= 0x4B)
						{
							nextTrackByte = APU_ReadNextTrackByte(x);
							zeroPages[ZeroPages.APU_TrackInstructions + x + 3] = (byte)(nextTrackByte & 0x7F);
							if (nextTrackByte >= 0x80)
							{
								nextTrackByte = APU_ReadNextTrackByte(x);
								zeroPages[ZeroPages.APU_TrackInstructions + x + 13] = nextTrackByte;
							}
						}
						// _APU_ParseTrackByte_End_

						zeroPages[ZeroPages.APU_TrackInstructions + x + 2] // this effects whether the track gets updated next frame?
							= zeroPages[ZeroPages.APU_TrackInstructions + x + 3];
						a = zeroPages[ZeroPages.APU_TrackInstructions + x + 13];
						a &= 0x7F;
						a = APU_Track04_SpecialOps(a, x);
						zeroPages[ZeroPages.APU_TrackInstructions + x + 12] = a;
						break;
				}
			}


			private static byte APU_Track04_SpecialOps(byte a, byte x)
			{
				if (x != 0x04)
					return a;
				zeroPages[ZeroPages.APU_TrackInstructions + 1] = a;
				a = nesRam[0x047F];
				a &= 0x1F;
				if (a >= 0x06)
				{ // _L7B6F7_
					zeroPages[ZeroPages.APU_TrackInstructions + 0] = a;
					a = APU_GetFinalDuty();
					if (a != 0)
						return a;
				}
				else // B6F3
					a = 0x00; // what's the point of this?? I wonder if this is a bug?
							  // _L7B700_
				zeroPages[ZeroPages.APU_TempDutySettings + 1 + x] = 0xFF;
				a = 0x01;
				return a;
			}

			/// <summary>
			/// 7B854
			/// </summary>
			/// <returns>result in a</returns>
			private static byte APU_GetFinalDuty()
			{
				byte a = zeroPages[ZeroPages.APU_TrackInstructions + 0];
				a ^= 0xFF; // invert byte
				a <<= 3;
				zeroPages[ZeroPages.APU_TrackInstructions + 0] = a;
				a = 0;
				zeroPages[ZeroPages.APU_TrackInstructions + 0]
					= ASMHelper.ASL(zeroPages[ZeroPages.APU_TrackInstructions + 0], 1, out bool hasCarry);
				if (!hasCarry)
					a = ASMHelper.ADC(a, zeroPages[ZeroPages.APU_TrackInstructions + 1], ref hasCarry);

				for (int i = 0; i < 4; ++i)
				{
					zeroPages[ZeroPages.APU_TrackInstructions + 1] >>= 1;
					zeroPages[ZeroPages.APU_TrackInstructions + 0]
						= ASMHelper.ASL(zeroPages[ZeroPages.APU_TrackInstructions + 0], 1, out hasCarry);
					if (!hasCarry)
						a = ASMHelper.ADC(a, zeroPages[ZeroPages.APU_TrackInstructions + 1], ref hasCarry);
				}

				return a;
			}

			private static byte APU_ReadNextTrackByte(byte x)
			{
				int address = zeroPages[ZeroPages.APU_TrackPosition + x + 0]
							+ zeroPages[ZeroPages.APU_TrackPosition + x + 1] << 8;
				if (++zeroPages[ZeroPages.APU_TrackPosition + x] == 0)
					++zeroPages[ZeroPages.APU_TrackPosition + x + 1];
				return romData[0x78000 + address - 0x8000];
			}

			private static void APU_DisableSweep(int sweepControlRegister)
			{
				registers[sweepControlRegister] = 0x08;
			}

			/// <summary>
			/// 0x7B707
			/// <para>Always false if x >= 0x06, or 0x6F3 == 0xFF</para>
			/// </summary>
			/// <param name="x">track index.</param>
			/// <returns>true is set, false if clear</returns>
			private static bool APU_SetOrClearCarry(byte x)
			{
				if (x >= 0x06)
					return false;
				if (nesRam[0x6F3] == 0xFF) // APU_Const_FF
					return false;

				x >>= 1;
				x ^= nesRam[0x6F2]; // APU_Const_03
				x &= 0x03;
				if (x != 0)
					return false;

				return true;
			}
		}

		/// <summary>
		/// 0x7C9F9
		/// </summary>
		public static void ResetNMI_7C000_sub()
		{
			BankSwitchRegister2_7C000(0);
		}
		/// <summary>
		/// 0x7E819
		/// </summary>
		public static void ResetNMI_7C000_sub_cont()
		{
			BankSwitch_SaveNextBank_7C000(0x01);
		}

		private static void BankSwitch_SaveNextBank_7C000(byte a)
		{
			nesRam[NESRAM.bankSwitch_CurrentBankId] = a;
			BankSwitch_7C000(a);
			Bank3C000.ResetNMI_3C000_sub_cont();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// /// <summary>
		/// <para>0x7FF94 - identical to 3C000.BankSwitch
		/// <br>Switch out 0x8000 to 0xBFFF with bank from ROM.</br>
		/// <br>If BankId &lt; 0x10, 0xC000 to 0xFFFF is set to ($0F) $3C000 bank from ROM.</br>
		/// <br>If BankId &gt;= 0x10, 0xC000 to 0xFFFF is set to ($1F) $7C000 bank from ROM.</br>
		/// </para>
		/// </summary>
		/// <param name="bankId"></param>
		private static void BankSwitch_7C000(byte a)
		{

		}

		/// <summary>
		/// 0x7E8DA
		/// <para>comes from a non-standard BRK instruction. First instruction byte is in dynamicSubroutine+1.</para>
		/// </summary>
		public static void DynamicSubRoutine_Setup()
		{
			byte a = zeroPages[ZeroPages.dynamicSubroutineAddr + 1];
			a <<= 1;
			byte x = a;
			int addr = romData[ROM.LocalPointers_78000.offset + x] + (romData[ROM.LocalPointers_78000.offset + x + 1] << 8);
			// possible addr include APU_RunEngine and
			if (x == 0x04)
				Bank78000.ResetAPU_78000();
		}
	}
}
