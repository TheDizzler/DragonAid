using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using AtomosZ.MiNesEmulator.CPU2A03;
using AtomosZ.MiNesEmulator.PPU2C02;
using System.ComponentModel;

namespace AtomosZ.MiNesEmulator
{
	public class MiNes
	{
		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(
			out long lpFrequency);

		/// <summary>
		/// In cycles per second. <br/>
		/// 21.47727 MHz
		/// </summary>
		public const double CLK = 1.0 / 21477272;
		public const double PPU_CLK = 4 * CLK;
		public const double CPU_CLK = 12 * CLK;
		public bool isEmulatorRunning;
		public ControlUnit6502 controlUnit { get { return cpu.controlUnit; } }
		public VirtualCPU cpu;

		private int cycleCount = 0;
		private Thread updateThread;

		private PPU ppu;
		private byte[] romData;



		private long perfCountFrequency;

		public MiNes()
		{
			ppu = new PPU();
			cpu = new VirtualCPU(ppu/*.ppuLatch*/);


		}

		public void LoadRom(string filepath)
		{
			romData = File.ReadAllBytes(filepath);

			ppu.Initialize((byte)(romData[6] & 0x01), (romData[6] & 0x04) == 0x04);
			cpu.LoadRom(romData);
		}

		public void LoadRom(byte[] romData)
		{
			this.romData = romData;

			ppu.Initialize((byte)(romData[6] & 0x01), (romData[6] & 0x04) == 0x04);
			cpu.LoadRom(romData);
		}

		public async void Start()
		{
			updateThread = new Thread(RunEngineLoop);
			updateThread.Start();
		}

		public void Reset()
		{
			cpu.Reset();
			ppu.Reset();
		}


		public void Stop()
		{
			isEmulatorRunning = false;
		}

		public byte Memory(int address)
		{
			return cpu.memory[address];
		}

		public byte[] Memory(int address, int length)
		{
			return cpu.memory[address, length];
		}


		private void RunEngineLoop()
		{
			isEmulatorRunning = true;
			var stopwatch = new Stopwatch();

			//if (QueryPerformanceFrequency(out perfCountFrequency) == false)
			//{
			//	// high-performance counter not supported
			//	throw new Win32Exception();
			//}

			//var lastCounter = Win32GetCurrentTime();
			var lastStopwatch = stopwatch.Elapsed.TotalSeconds;

			stopwatch.Start();
			double timeSinceLastCycle = 0;
			while (isEmulatorRunning)
			{
				//Thread.Sleep(5);
				var currentStopwatch = stopwatch.Elapsed.TotalSeconds;
				var elapsedTime = currentStopwatch - lastStopwatch;
				var masterCyclesElapsed = elapsedTime / CLK;
				Debug.WriteLine($"Cyles: {masterCyclesElapsed}");
				Debug.WriteLine($"PPU Cyles: {masterCyclesElapsed / 4}");
				Debug.WriteLine($"CPU Cyles: {masterCyclesElapsed / 12}\n");
				//timeSinceLastCycle += elapsedTime;



				//if (timeSinceLastCycle < PPU_CLK)
				//	Debug.WriteLine("WOW!");
				//if (timeSinceLastCycle >= CLK)
				//{
				//	timeSinceLastCycle = timeSinceLastCycle - CLK;
				//	++cycleCount;
				//	if (cycleCount % 12 == 0)
				//	{
				//		// update CPU
				//		++cpu.cycle;
				//	}
				//	if (cycleCount % 3 == 0)
				//	{
				//		// update PPU
				//		++ppu.cycle;
				//		var diff = currentStopwatch - ppu.timeOfLastCycle;
				//		//if (diff > 0)
				//		//{
				//		//	Debug.WriteLine($"PPU DIFF: PPU missed timing by {diff}s");
				//		//}

				//		ppu.timeOfLastCycle = currentStopwatch;
				//		if (diff > PPU_CLK)
				//		{
				//			//Debug.WriteLine($"PPU missed timing by {timeSinceLastCycle - (PPU_CLK)}s");
				//		}

				//		timeSinceLastCycle = timeSinceLastCycle - (PPU_CLK);
				//	}
				//}


				//var currentCounter = Win32GetCurrentTime();
				//var elapsed = Win32GetSecondsElapsed(lastCounter, currentCounter);

				//Debug.WriteLine($"Stopwatch: {currentStopwatch - lastStopwatch}s");


				//Debug.WriteLine($"QPC: {elapsed}s\n");

				//lastCounter = currentCounter;

				lastStopwatch = currentStopwatch;
			}
		}


		private long Win32GetCurrentTime()
		{
			QueryPerformanceCounter(out var result);
			return result;
		}

		private double Win32GetSecondsElapsed(long start, long end)
		{
			double tickCount = (double)(end - start) / perfCountFrequency;
			return tickCount;
		}

		/// <summary>
		/// TODO: keep cpu and ppu insync in realtime.
		/// </summary>
		private void CycleCounter()
		{
			int ppuCount = 0;
			long lastFrameTick = 0;
			double secondsPerFrame = PPU.CYCLE_TIME * 88740;
			double ticksPerFrame = secondsPerFrame * Stopwatch.Frequency;

			isEmulatorRunning = true;

			while (isEmulatorRunning)
			{
				//if (frameDone)
				{ // sync here
					/* Make sure frame doesn't get to far ahead */
					var currentTick = Stopwatch.GetTimestamp();
					var diff = currentTick - lastFrameTick;
					if (diff < ticksPerFrame)
					{
						Debug.WriteLine($"Ahead by {currentTick - lastFrameTick} ticks or {(double)(currentTick - lastFrameTick) / Stopwatch.Frequency} s.");
					}

					lastFrameTick = Stopwatch.GetTimestamp();
				}

				ppu.Update();
				if (++ppuCount == 3)
				{
					/* @RESEARCH: If data in the PPU or CPU is updated in the same cycle, the other should not
					 * be able to see the updates until the next cycle?
					 * Is this where emulation of a bus is needed? */
					cpu.Update();
				}

				// update busses?


			}
		}

	}
}
