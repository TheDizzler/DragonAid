namespace AtomosZ.DragonAid
{
	public static class Opcodes
	{
		/// <summary>
		///   Hex	|	  ASM		|	Notes
		/// **************************************************************
		/// 69 XX	;	adc #X		;	add value XX to accumulator with carry	(zeropage)
		/// 65 XX	;	adc $XX		;	add value at mem addr XX to accumulator with carry	(zeropage)
		///	
		///	A9 XX	;	lda #XX		;	load value XX into accumulator			(immediate)
		///	A5 XX	;	lda $XX		;	load data at addr XX into accumulator	(zeropage)
		///	AD XXXX	;	lda $XXXX	;	load data at addr XXXX into accumulator	(absolute)
		///	
		/// 85 XX	;	sta $XX		;	store value in accumulator to mem addr	(zeropage)
		/// 8D XXXX	;	sta $XXXX	;	store value in accumulator to mem addr	(absolute)
		///	
		/// A2 XX	;	ldx #X		;	loads value X into x register	(immediate)
		/// A6 XX	;	ldx $XX		;	loads value at memory addr $XX into x register		(zeropage)
		/// AE XXXX	;	ldx $XXXX	;	loads value at memory addr $XXXX into x register	(absolute)
		/// 
		/// A0 XX	;	ldy #X		;	loads value X into y register	(immediate)
		/// A4 XX	;	ldy $XX		;	loads value at memory addr $XX into y register		(zeropage) 
		/// AC XXXX	;	ldy $XXXX	;	loads value at memory addr $XXXX into y register	(absolute)
		/// 
		/// E6 XX	;	inc $XX		;	increment value at memory address $XX	(zeropage)
		/// C6 XX	;	dec $XX		;	decrement value at memory address $XX	(zeropage)
		/// 
		///	86 XX	;	stx	$XX		;	store value of x register in memory addr $XX	(zeropage)
		///	8E XXXX	;	stx	$XXXX	;	store index X in memory location $XXXX			(absolute)
		///	
		///	84 XX	;	sty	$XX		;	store value of y register in memory addr $XX	(zeropage)
		///	8C XXXX	;	sty	$XXXX	;	store index Y in memory location $XXXX			(absolute)
		/// 
		/// C9 XX	;	cmp #X		;	compare mem with accumulator	(immediate)
		/// C5 XX	;	cmp $XX		;	compare mem with accumulator	(zeropage)
		/// CD XXXX	;	cmp $XXXX	;	compare mem with accumulator	(absolute)
		/// 
		/// 09 XX	;	ora #X		;	Bitwise OR (|) on accumulator and value X				(immediate)
		/// 05 XX	;	ora $XX		;	Bitwise OR (|) on accumulator and value at addr $XX		(zeropage)
		/// 0D XXXX	;	ora $XXXX	;	Bitwise OR (|) on accumulator and value at addr $XXXX	(absolute)
		/// 
		/// 49 XX	;	eor #X		;	Bitwise Exclusive-OR on accumulator and value X					(immediate)
		/// 45 XX	;	eor $XX		;	Bitwise Exclusive-OR on accumulator and value at addr $XXXX		(immediate)
		/// 4D XXXX	;	eor $XXXX	;	Bitwise Exclusive-OR on accumulator and value at addr $XXXX		(immediate)
		/// 
		/// 2A		;	rol			;	rotate one bit left on accumulator		(immediate)
		/// 26 XX	;	rol $XX		;	rotate one bit left on value at addr	(zeropage)
		/// 2E XXXX	;	rol $XXXX	;	rotate one bit left on value at addr	(absolute)
		/// 
		/// 20 XXXX	;	jsr $XXXX	;
		/// 
		/// E8		;	inx			;	increments value in x register
		/// C8		;	iny			;	increments value in y register
		/// CA		;	dex			;	decrement value in x register
		/// 88		;	dey			;	decrement value in y register
		/// 
		/// 18		;	clc			;	Clear Carry Flag
		/// 
		/// 90 XX	;	bcc			;	Branch on Carry Clear
		/// F0 XX	;	beq			;	Branch on Result Zero
		/// 
		/// 60		;	rts			;	Return from Subroutine
		/// 
		/// EA		;	nop			;	No operation
		/// 
		/// 
		/// Addressing Modes:
		///		Immediate Addressing		ex: ldx #$E8
		///			Can never load more than a single byte
		///		
		///		ZeroPage Addressing			ex: ldx $2F
		///			The first 256 bytes of SystemRam is ZeroPage ($0000 - $00FF)
		///			
		///		Absolute Addressing			ex: stx $0301
		///			Can be used to access all 64KB of mem
		///			Slower than ZeroPage
		///			
		///		Implicit Addressing			ex: inx
		///			Does one thing
		///			
		/// </summary>

		public class Opcode
		{
			/// <summary>
			/// As written in ASM.
			/// </summary>
			public string opcode;
			/// <summary>
			/// As compiled in 6502 machine code.
			/// </summary>
			public string hexcode;
			public string notes;
		}

		public static Opcode loadAccumulator2 = new Opcode()
		{
			opcode = "lda",
			hexcode = "AD",
			notes = "loads 2 bytes in to accumulator",
		};

		public static Opcode loadAccumulator1 = new Opcode()
		{
			opcode = "lda",
			hexcode = "A5",
			notes = "loads 1 byte in to accumulator",
		};

		public static Opcode loadAccumulatorX = new Opcode()
		{
			opcode = "ldx",
			hexcode = "A2",
			notes = "loads value into x register",
		};

		public static Opcode storeAccumulator = new Opcode()
		{
			opcode = "sta",
			hexcode = "85",
			notes = "stores 1 byte from accumulator",
		};

		// taken from https://tcrf.net/Dragon_Warrior_IV_(NES) DW4 encounter code
		// 
		// and		29 F0         and #$F0
		// lsr		4A            lsr
		// tay		A8            tay
		// lda		B1 00         lda ($00),y
		// ldx		AE 15 05      ldx $0515
		// cpx		E0 01         cpx #$01
		// beq		F0 05         beq ship		// go to(?) label ship
		// jmp		4C 88 9C      jmp storeit	// go to label storeit
		// asl		0A            asl
	}
}
