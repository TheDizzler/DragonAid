using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AtomosZ.DragonAid.Libraries;
using AtomosZ.MiNesEmulator.CPU2A03;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static AtomosZ.DragonAid.Libraries.ASM.Opcode;
using static AtomosZ.DragonAid.Libraries.PointerList.Pointers;

namespace AtomosZ.MiNesEmulator.PPU2C02
{
	public class PPU
	{
		private class ObjectAttribute
		{
			public byte yPos;
			public byte spriteTileNum;
			public byte attribute;
			public byte xPos;
		}

		//private delegate void RegisterWrite(byte value);
		//private RegisterWrite[] registerReads = new RegisterWrite[];

		//private IRegister[] registers = new IRegister[8]
		//{
		//	new PPUCTRL(),
		//	new PPUMASK(),
		//	new PPUSTATUS(),
		//	new OAMADDR(),
		//	new OAMDATA(),
		//	new PPUSCROLL(),
		//	new PPUADDR(),
		//	new PPUDATA(),
		//};
		//internal PPUCTRL ppuCtrl = new PPUCTRL();
		//internal PPUMASK ppuMask = new PPUMASK();
		//internal PPUSTATUS ppuStatus = new PPUSTATUS();
		//internal OAMADDR oamAddr = new OAMADDR();
		//internal OAMDATA oamData = new OAMDATA();
		//internal PPUSCROLL ppuScroll = new PPUSCROLL();
		//internal PPUADDR ppuAddr = new PPUADDR();
		//internal PPUDATA ppuData = new PPUDATA();




		/// <summary>
		/// Runs at 1 / 4 the master CLK, 3 times the CPU clock rate. <br/>
		/// In seconds per cycle (1 / 5369318 = .00018624339 sec/cycle). <br/>
		/// 5369318 Hz = 1.8624339255^-7 sec/cycle
		/// </summary>
		public const double CLK = MiNes.CLK * 4;
		/// <summary>
		/// In scanlines (2273 cycles).
		/// </summary>
		private const int VBlank = 20;
		/// <summary>
		/// Scanlines 0 to 239 are visible.
		/// </summary>
		private const int scanlinesPerFrame = 262;
		/// <summary>
		/// In PPU clock cycles.
		/// </summary>
		private const int cyclesPerScanline = 341;


		/// <summary>
		/// 0x0000-0x1FFF Pattern tables (CHR ROM) <br/>
		/// 0x2000-0x3EFF Name tables (VRAM) <br/>
		/// 0x3F00-0x3FFF Palettes <br/>
		/// 0x4000-0xFFFF Mirrors
		/// </summary>
		private byte[] memory = new byte[0x4000];
		/// <summary>
		/// Internal memory to keep state of sprites.
		/// </summary>
		private byte[] oam = new byte[256];
		//private ObjectAttribute[] oam = new ObjectAttribute[64];
		//private byte[] patternTables = new byte[0x2000];
		//private byte[] nameTables = new byte[0x2000];
		/// <summary>
		/// 0x0000 to 0x0FFF
		/// </summary>
		//private byte[] patternTable0 = new byte[0x1000];

		/// <summary>
		/// 0x1000 to 0x1FFF
		/// </summary>
		//private byte[] patternTable1 = new byte[0x1000];
		/// <summary>
		/// A nametable is a 1024 byte area of memory used by the PPU to lay out backgrounds.
		/// Each byte in the nametable controls one 8x8 pixel character cell, and each
		/// nametable has 30 rows of 32 tiles each, for 960 ($3C0) bytes; the rest is
		/// used by each nametable's attribute table. With each tile being 8x8 pixels,
		/// this makes a total of 256x240 pixels in one map, the same size as one full screen. 
		/// </summary>
		//private byte[] nameTable0 = new byte[0x400];
		//private byte[] nameTable1 = new byte[0x400];
		//private byte[] nameTable2 = new byte[0x400];
		//private byte[] nameTable3 = new byte[0x400];

		/// <summary>
		/// 0x3000 to 0x301F
		/// </summary>
		//private byte[] paletteRamIndices = new byte[0x20];



		//private PPUCTRL ppuctrl;
		//private PPUMASK ppumask;
		//private PPUSTATUS ppustatus;


		/// <summary>
		/// 1 CPU cycle == 3 PPU cycles.
		/// 1 clock cycle can produce 1 pixel.
		/// </summary>
		public int cycle = 0;
		/// <summary>
		/// 0 to 340
		/// </summary>
		public int opIndex = 0;
		/// <summary>
		/// 0 to 261
		/// </summary>
		public int scanLine = 0;
		/// <summary>
		/// 
		/// </summary>
		public int horzV;
		public int vert;
		private int frameCount = 0;
		private bool evenFrame = true;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cyclesToRender">Should always be a multiple of 3 to sync up with CPU more easily?</param>
		private void RenderCycles(int cyclesToRender)
		{
			for (; cycle < cycle + cyclesToRender; ++cycle)
			{
				++opIndex;
				if (opIndex > 340)
				{
					++scanLine;
					if (scanLine == 262)
					{
						scanLine = 0;
						if (!evenFrame)
							opIndex = 1;
						else
							opIndex = 0;
					}
					else
						opIndex = 0;
				}

				if (scanLine < 240)
				{
					PerformRenderLineOperation();
				}
				else if (scanLine == 241)
				{
					if (opIndex == 1)
					{
						isVBlank = true;
						// cancel out of RenderCycles and raise interrupt?
					}
				}
				else if (scanLine == 261)
				{
					if (opIndex == 1)
						isVBlank = false;
					PerformRenderLineOperation();
				}
			}
		}

		private void PerformRenderLineOperation()
		{
			if (opIndex < 257 || (opIndex >= 321 && opIndex < 337))
			{
				var pixelOpType = (opIndex - 1) % 8;
				switch (pixelOpType)
				{
					case 0:
						// NT byte
						break;
					case 2:
						// AT byte
						break;
					case 4:
						// Low BG tile byte
						break;
					case 6:
						// High BG tile byte
						break;

					case 7:
						++horzV;
						break;
					default:
						// no need for anything?
						break;
				}
				++cycle;
			}
			else if (opIndex == 257)
			{
				++vert;
				horzV = 0; // ??
			}
		}

		private void RenderBackground()
		{
			/// visible scanlines (0 to 239)
			for (int scanLine = 0; scanLine < 240; ++scanLine)
			{
				/// cycle 0: idle on EVEN frames // what does this NOT do? the NOP is skipped so....NOP?
				// cycles 1 to 256
				int scanLineCycle;
				for (scanLineCycle = 1; scanLineCycle < 257; ++scanLineCycle)
				{// for each 8 pixels of bg
				 // get NT byte (2 cycles)

					// get AT byte (2 cycles)

					// get low BG tile byte (2 cycles)

					// get high BG tile byte (2 cycles)

					// (inc hor(v)) (0 cycles)
				}

				// (inc vert(v)) (0 cycles)
				// (hor(v) = hor(t) (1 cycle)

				/// HBlank

				/// cycles 257-320
				for (; scanLineCycle < 321; ++scanLineCycle)
				{   // data for sprites on NEXT scanline fetched here
					// garbage NT byte (2 cycles)
					// garbage NT byte (2 cycles)

					// pattern table tile low

					// pattern table tile high
				}

				/// cycles 321-336
				for (; scanLineCycle < 337; ++scanLineCycle)
				{ // first 2 tiles for NEXT scanline fetched here and loaded into shift registers
				  // NT byte

					// AT byte

					// pattern table tile low

					// pattern table tile high
				}

				/// cycles 337-340
				// unknown usage? MMC5 uses to clock scanline counter

				// get NT byte (2 cycles)

				// get NT byte (2 cycles)
			}

			/// scanline 240 - post-render scanline
			// do nothing for 340 cycles - not yet VBlank


			/// scanlines 241-260 - VBlank

			// set VBlank on 2nd cycle


			/// scanline 261

			// clear VBlank, clear sprite 0
			// fill shift registers with data for first two tiles of first scan line

			// skip last cycle if frame is odd
		}

		//int updateCount = 0;
		//long frame60 = Stopwatch.GetTimestamp();

		/// <summary>
		/// 16 bit register. Contains pattern table data for two tiles.
		/// Every 8 cycles, the data for the next tile is loaded int the upper 8 bits.
		/// Meanwhile, the pixel to render is fetched from one of the lower 8 bits.
		/// </summary>
		private class ShiftRegister16 : IRegister
		{
			private byte highValue;
			private byte lowValue;
			private byte shiftCount = 0;

			/// <summary>
			/// Writes a 1 bit value into the high byte.
			/// </summary>
			/// <param name="value">1 bit value</param>
			public void Write(byte value)
			{
				highValue <<= 1;
				highValue |= value;
			}

			/// <summary>
			/// Reads 1 bit from the low byte.
			/// </summary>
			/// <returns></returns>
			public byte Read()
			{
				ASMHelper.ASL(lowValue, 1, out bool hasCarry);
				if (++shiftCount == 8)
				{
					shiftCount = 0;
					lowValue = highValue;
				}
				return hasCarry ? (byte)0x01 : (byte)0x00;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}

		}

		/// <summary>
		/// Contains palette attributes for the lower 8 pixels of the 16-bit shift register.
		/// Fed by a latch which contains palette attribute for the next tile.
		/// Every 8 cycles, the latch is loaded with the palette attribute for the next tile.
		/// </summary>
		private class ShiftRegister8 : IRegister
		{
			private byte value;

			public void Write(byte value)
			{
				throw new NotImplementedException();
			}
			public byte Read()
			{
				throw new NotImplementedException();
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}

		private ShiftRegister16 bgReg16a, bgReg16b;
		private ShiftRegister8 bgReg8a, bgReg8b;


		public PPU()
		{
			ppuLatch = new PPULatch(this);
		}


		/// <summary>
		/// Scanlines 0-261. 340 cycles.
		/// <para>
		/// Every cycle, a bit is fetched from the 4 BG shift registers in order
		/// to creata a pixel on screen. Exactly which bit is fetched depends on the fine X scroll,
		/// set by $2005. Afterwards, the shift registers are shifted once, to the data
		/// for the next pixel.<br/>
		/// Every 8 cycles/shifts, new data is loaded into these registers.<br/>
		/// </para>
		/// </summary>
		private void VisibleScanLineProc()
		{
			// cycle 0: idle

			// one cycle:
			// fetch data
			// render pixel


			// cycles 1 to 256
			// 8 cycles per tile:
			// 0 - nametable
			// 1 - attribute table data
			// 2 - bit 0 of BG pattern
			// 3 - bit 1 of BG pattern
			// 4
		}

		public void Update()
		{


			// where in tile fetch (NT byte, AT byte, Low BG tile byte, High BG tile byte)
			byte tileCycleMask = (byte)((cycle - 1) & 0x07);


			++cycle;
			//++updateCount;
			//if (updateCount == 88740)
			//{
			//	updateCount = 0;
			//	++frameCount;
			//	if (frameCount == 60)
			//	{
			//		frameCount = 0;
			//		var ticks = Stopwatch.GetTimestamp() - frame60;
			//		//Debug.WriteLine("60 Frames in " + ticks + " ticks");
			//		//Debug.WriteLine("60 Frames in " + (float)(ticks) / Stopwatch.Frequency);
			//		//Debug.WriteLine("Timespan: " + new TimeSpan(ticks));
			//		frame60 = Stopwatch.GetTimestamp();
			//	}
			//	return true;
			//}
		}


		private byte this[int address]
		{
			get
			{
				if (address < 0x2000 || address >= 0x3F00)
					return memory[address];
				else if (address < 0x3F00)
				{
					address &= 0xEFFF;
					switch (mirroringMode)
					{
						case MirroringMode.Horizontal:
							return memory[address & 0xEBFF]; // clear bit 10 and 12
						case MirroringMode.Vertical:
							return memory[address & 0xE7FF]; // clear bit 11 and 12
						case MirroringMode.OneScreen:
							return memory[address & 0xE3FF]; // clear bits 10, 11 and 12
						case MirroringMode.FourScreen:
							return memory[address];
					}
				}

				throw new Exception($"Invalid address {address}");
			}
		}

		private enum MirroringMode
		{
			Horizontal = 0x00,
			Vertical = 0x01,
			OneScreen,
			FourScreen
		}


		public void Initialize(byte mirroring, bool use4ScreenRam)
		{
			if (use4ScreenRam)
				mirroringMode = MirroringMode.FourScreen;
			else
				mirroringMode = (MirroringMode)mirroring;
		}


		public void Reset()
		{
			memory = new byte[0x4000];
		}

		public PPULatch ppuLatch;
		private byte baseNameTable;
		private byte spritePatternTable;
		private byte bgPatternTable;
		private byte spriteSize;
		private byte ppuMasterSlaveSelect;
		private bool nmiEnabled;
		private bool greyscaleEnabled;
		private bool showBGLeft8Pixels;
		private bool showSpritesLeft8Pixels;
		private bool bgEnabled;
		private bool spritesEnabled;
		private bool redEmphasis;
		private bool greenEmphasis;
		private bool blueEmphasis;
		private bool spriteOverflow;
		private bool spriteZero;
		private bool isVBlank = false;
		private byte ppuAddrLatch = 1;
		/// <summary>
		/// VRAM Address.
		/// </summary>
		private int ppuAddr;
		private byte vramAddrIncrement = 1;
		private byte internalDataBuffer = 0;
		private MirroringMode mirroringMode;
		/// <summary>
		/// Address in OAM you want to access. 
		/// During rendering acts as pointer to Sprite 0.
		/// </summary>
		private byte oamAddr;
		private bool isRendering;



		/// <summary>
		/// $2000
		/// Writes are ignored for the first ~30,000 cycles.
		/// </summary>
		/// <param name="value"></param>
		private void PPUCTRL_Write(byte value)
		{
			baseNameTable = (byte)(
				(value & 0x01) + (value & 0x02) << 1);

			if ((value & 0x04) == 0x04)
				vramAddrIncrement = 32;
			else
				vramAddrIncrement = 1;

			spritePatternTable = (byte)(value & 0x08);
			bgPatternTable = (byte)(value & 0x10);
			spriteSize = (byte)(value & 0x20);
			ppuMasterSlaveSelect = (byte)(value & 0x40);

			var newNMI = (value & 0x80) == 0x80;
			if (!nmiEnabled && newNMI && isVBlank)
			{    // generare NMI

			}

			nmiEnabled = newNMI;
		}

		private void PPUMASK_Write(byte value)
		{
			greyscaleEnabled = (value & 0x01) == 0x01;
			showBGLeft8Pixels = (value & 0x02) == 0x02;
			showSpritesLeft8Pixels = (value & 0x04) == 0x04;
			bgEnabled = (value & 0x08) == 0x08;
			spritesEnabled = (value & 0x10) == 0x10;
			redEmphasis = (value & 0x20) == 0x20;
			greenEmphasis = (value & 0x40) == 0x40;
			blueEmphasis = (value & 0x80) == 0x80;
		}

		private void PPUSTATUS_Write(byte value)
		{
			throw new Exception("What should happen in this case?");
		}

		/// <summary>
		/// $2002
		/// <para>
		/// @TODO: Race Condition Warning: Reading PPUSTATUS within two cycles of
		/// the start of vertical blank will return 0 in bit 7 but clear the latch 
		/// anyway, causing NMI to not occur that frame. See NMI and PPU_frame_timing
		/// for details.
		/// </para>
		/// </summary>
		/// <returns></returns>
		private byte PPUSTATUS_Read()
		{
			int sprOverflow = spriteOverflow ? 0x20 : 0;
			int sprZero = spriteZero ? 0x40 : 0;
			int vBlankStarted = isVBlank ? 0x80 : 0;

			ppuAddrLatch = 1;
			isVBlank = false;

			return (byte)(vBlankStarted | sprZero | sprOverflow);
		}

		/// <summary>
		/// $2003
		/// <para>
		/// Write the address of OAM you want to access here. Most games
		/// just write $00 here and then use OAMDMA. (DMA is implemented
		/// in the 2A03/7 chip and works by repeatedly writing to OAMDATA)
		/// OAMADDR is set to 0 during each of ticks 257-320 (the sprite
		/// tile loading interval) of the pre-render and visible scanlines. <br/>
		/// The value of OAMADDR when sprite evaluation starts at tick 65
		/// of the visible scanlines will determine where in OAM sprite evaluation
		/// starts, and hence which sprite gets treated as sprite 0. The first OAM
		/// entry to be checked during sprite evaluation is the one starting at
		/// OAM[OAMADDR]. If OAMADDR is unaligned and does not point to the y
		/// position (first byte) of an OAM entry, then whatever it points to
		/// (tile index, attribute, or x coordinate) will be reinterpreted as a y
		/// position, and the following bytes will be similarly reinterpreted. No
		/// more sprites will be found once the end of OAM is reached, effectively
		/// hiding any sprites before OAM[OAMADDR]. <br/>
		/// </para>
		/// </summary>
		/// <param name="value"></param>
		private void OAMADDR_Write(byte value)
		{
			oamAddr = value;
		}
		/// <summary>
		/// $2003
		/// </summary>
		/// <exception cref="Exception"></exception>
		private byte OAMADDR_Read()
		{
			throw new Exception("OAMADDR is not meant to be read");
		}

		/// <summary>
		/// $2004
		/// @TODO: everything
		/// <para>
		/// Write OAM data here. Writes will increment OAMADDR after the write.
		/// Do not write directly to this register in most cases. Because
		/// changes to OAM should normally be made only during vblank, writing
		/// through OAMDATA is only effective for partial updates (it is too slow),
		/// and partial writes cause corruption. Most games will use the DMA
		/// feature through OAMDMA instead.<br/>
		/// Writes to OAMDATA during rendering (on the pre-render line and
		/// the visible lines 0-239, provided either sprite or background rendering
		/// is enabled) do not modify values in OAM, but do perform a glitchy
		/// increment of OAMADDR, bumping only the high 6 bits (i.e., it bumps the
		/// [n] value in PPU sprite evaluation - it's plausible that it could bump
		/// the low bits instead depending on the current status of sprite evaluation).
		/// This extends to DMA transfers via OAMDMA, since that uses writes to $2004.
		/// For emulation purposes, it is probably best to completely ignore writes
		/// during rendering.<br/>
		/// </para>
		/// </summary>
		private void OAMDATA_Write(byte value)
		{
			if (isRendering)
			{
				byte low2bits = (byte)(oamAddr & 0x03);
				oamAddr &= 0xFC;
				++oamAddr;
				oamAddr |= low2bits;
			}
			else
			{
				oam[oamAddr] = value;
				++oamAddr;
			}
		}
		/// <summary>
		/// $2004
		/// <para>Reads during vertical or forced blanking return the value
		/// from OAM at that address but do not increment.
		/// Reading OAMDATA while the PPU is rendering will expose
		/// internal OAM accesses during sprite evaluation and loading;
		/// Micro Machines does this.<br/>
		/// </para>
		/// </summary>
		/// <returns></returns>
		private byte OAMDATA_Read()
		{
			if (isVBlank) /* @TODO: What is forced blanking? */
				return oam[oamAddr];
			else
				return oam[oamAddr++];
		}


		/// <summary>
		/// $2005
		/// <para>
		/// This register is used to change the scroll position, that is, to tell
		/// the PPU which pixel of the nametable selected through PPUCTRL should be
		/// at the top left corner of the rendered screen. Typically, this register is
		/// written to during vertical blanking, so that the next frame starts rendering
		/// from the desired location, but it can also be modified during rendering in
		/// order to split the screen. Changes made to the vertical scroll during rendering
		/// will only take effect on the next frame.<br/>
		/// After reading PPUSTATUS to reset the address latch, write the horizontal
		/// and vertical scroll offsets here just before turning on the screen: <br/>
		///		bit PPUSTATUS<br/>
		///		lda cam_position_x<br/>
		///		sta PPUSCROLL<br/>
		///		lda cam_position_y<br/>
		///		sta PPUSCROLL<br/>
		/// </para>
		/// </summary>
		private void SCROLL_Write(byte value)
		{
			/* @TODO: delay vertical scroll update by one frame during rendering */
			ppuAddr <<= 8 * ppuAddrLatch;
			ppuAddr |= value;
			if (--ppuAddrLatch >= 0x80)
				ppuAddrLatch = 1;
		}


		/// <summary>
		/// $2006
		/// <para>
		/// Because the CPU and the PPU are on separate buses, neither has direct
		/// access to the other's memory. The CPU writes to VRAM through a pair of
		/// registers on the PPU. First it loads an address into PPUADDR, and then
		/// it writes repeatedly to PPUDATA to fill VRAM.<br/>
		/// After reading PPUSTATUS to reset the address latch, write the 16-bit
		/// address of VRAM you want to access here, upper byte first.<br/>
		/// </para>
		/// <para>
		/// Bus conflict: @TODO<br/>
		/// During raster effects, if the second write to PPUADDR happens at specific
		/// times, at most one axis of scrolling will be set to the bitwise AND of
		/// the written value and the current value. The only safe time to finish the
		/// second write is during blanking; see PPU scrolling for more specific timing.<br/>
		/// </para>
		/// </summary>
		/// <param name="value"></param>
		private void PPUADDR_Write(byte value)
		{
			ppuAddr <<= 8 * ppuAddrLatch; // address is big-endian
			ppuAddr |= value;
			if (--ppuAddrLatch >= 0x80)
			{
				ppuAddr &= 0x3FFF;
				ppuAddrLatch = 1;
				if (ppuAddr < 0x3F00)
					// fetch data from address and store in buffer which can't be accessed right away
					internalDataBuffer = this[ppuAddr];  // @TODO: needs one frame delay before data is stored in buffer
			}
		}
		/// <summary>
		/// $2006
		/// </summary>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		private byte PPUADDR_Read()
		{
			throw new Exception("@TODO: PPUADDR_Read");
		}

		/// <summary>
		/// $2007
		/// <para>
		/// When the screen is turned off by disabling the background/sprite
		/// rendering flag with the PPUMASK or during vertical blank, you can
		/// read or write data from VRAM through this port. Since accessing
		/// this register increments the VRAM address, it should not be accessed
		/// outside vertical or forced blanking because it will cause graphical
		/// glitches, and if writing, write to an unpredictable address in VRAM.
		/// However, two games are known to read from PPUDATA during rendering:
		/// see Tricky-to-emulate games. 
		/// VRAM reading and writing shares the same internal address register
		/// that rendering uses. So after loading data into video memory, the program
		/// should reload the scroll position afterwards with PPUSCROLL and PPUCTRL
		/// (bits 1..0) writes in order to avoid wrong scrolling. <br/>
		/// </para>
		/// @TODO: Read conflict with DPCM samples
		/// </summary>
		private void PPUDATA_Write(byte value)
		{
			// increment Address Register.
			ppuAddr += vramAddrIncrement;
			ppuAddr &= 0x3FFF;
		}

		/// <summary>
		/// $2007
		/// <para>
		/// When the screen is turned off by disabling the background/sprite
		/// rendering flag with the PPUMASK or during vertical blank, you can
		/// read or write data from VRAM through this port. Since accessing
		/// this register increments the VRAM address, it should not be accessed
		/// outside vertical or forced blanking because it will cause graphical
		/// glitches. However, two games are known to read from PPUDATA during
		/// rendering: see Tricky-to-emulate games.<br/>
		/// VRAM reading and writing shares the same internal address register
		/// that rendering uses. So after loading data into video memory, the program
		/// should reload the scroll position afterwards with PPUSCROLL and PPUCTRL
		/// (bits 1..0) writes in order to avoid wrong scrolling.<br/>
		/// </para>
		/// <para>The PPUDATA read buffer (post-fetch):<br/>
		/// When reading while the VRAM address is in the range 0-$3EFF
		/// (i.e., before the palettes), the read will return the contents of an
		/// internal read buffer. This internal buffer is updated only when reading
		/// PPUDATA, and so is preserved across frames. After the CPU reads and gets
		/// the contents of the internal buffer, the PPU will immediately update the
		/// internal buffer with the byte at the current VRAM address. Thus, after
		/// setting the VRAM address, one should first read this register to prime
		/// the pipeline and discard the result.<br/>
		/// </para>
		/// <para>
		/// Reading palette data from $3F00-$3FFF works differently. The palette
		/// data is placed immediately on the data bus, and hence no priming read is
		/// required. Reading the palettes still updates the internal buffer though,
		/// but the data placed in it is the mirrored nametable data that would appear
		/// "underneath" the palette. (Checking the PPU memory map should make this clearer.)<br/>
		/// </para>
		/// @TODO: Read conflict with DPCM samples
		/// </summary>
		private byte PPUDATA_Read()
		{
			// increment Address Register.
			ppuAddr += vramAddrIncrement;
			ppuAddr &= 0x3FFF;
			if (ppuAddr >= 0x3F00)
			{
				internalDataBuffer = this[ppuAddr];
				return this[ppuAddr];
			}
			else
			{
				var data = internalDataBuffer;
				internalDataBuffer = this[ppuAddr];
				return data;
			}
		}

		/// <summary>
		/// $4014
		/// </summary>
		/// <param name="value"></param>
		private void OAMDMA_Write(byte value)
		{

		}


		public class PPULatch
		{
			private delegate void RegisterWrite(byte value);
			private RegisterWrite[] registerWrites = new RegisterWrite[8];

			private byte latchTargetRegister;
			private int latchValue;
			private PPU ppu;
			private byte writeCount = 0;

			public PPULatch(PPU ppu)
			{
				this.ppu = ppu;
				registerWrites[0] = ppu.PPUCTRL_Write;
				registerWrites[1] = ppu.PPUMASK_Write;
				registerWrites[2] = ppu.PPUSTATUS_Write;
			}

			public byte Read(byte registerMask)
			{
				throw new NotImplementedException();
			}

			public void Write(byte registerMask, byte value)
			{
				//ppu.registers[registerMask].Write(value);

				//latchTargetRegister = registerMask;
				//latchValue <<= 8 * writeCount;
				//latchValue |= value;

				//if (++writeCount == 2)
				//{
				//	writeCount = 0;
				//}
			}
		}
	}
}
