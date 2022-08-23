namespace AtomosZ.DragonAid.Libraries
{
	public enum DynamicLoader { Load07, Load17, NoLoader }

	public class DynamicSubroutine
	{
		public readonly static Address[] DynamicSubroutineBankPointers = new Address[]
		{
			new Address("DynamicSubroutine_Addresses_00000", 0x00000, 0x4000),
			new Address("DynamicSubroutine_Addresses_04000", 0x04000, 0x4000),
			new Address("DynamicSubroutine_Addresses_08000", 0x08000, 0x4000),
			new Address("DynamicSubroutine_Addresses_0C000", 0x0C000, 0x4000), // only 17
			new Address("DynamicSubroutine_Addresses_10000", 0x10000, 0x4000),
			new Address("DynamicSubroutine_Addresses_14000", 0x14000, 0x4000),
			new Address("DynamicSubroutine_Addresses_18000", 0x18000, 0x4000),
			new Address("DynamicSubroutine_Addresses_1C000", 0x1C000, 0x4000),
			new Address("DynamicSubroutine_Addresses_20000", 0x20000, 0x4000),
			new Address("DynamicSubroutine_Addresses_24000", 0x24000, 0x4000), // only 17
			new Address("DynamicSubroutine_Addresses_28000", 0x28000, 0x4000),
			new Address("DynamicSubroutine_Addresses_2C000", 0x2C000, 0x4000), // only 17
			new Address("DynamicSubroutine_Addresses_30000", 0x30000, 0x4000),
			new Address("DynamicSubroutine_Addresses_34000", 0x34000, 0x4000),
			new Address("DynamicSubroutine_Addresses_38000", 0x38000, 0x4000),
			new Address("DynamicSubroutine_Addresses_3C000", 0x3C000, 0x4000),
		};

		public int code { get; set; }
		public DynamicLoader loader;
		public int bankId;
		public int dsIndex;
		public string name;
		public string notes;
		public Address prgAddress;
		/// <summary>
		/// CPU address
		/// </summary>
		public int address;

		public override string ToString()
		{
			var hasNotes = !string.IsNullOrEmpty(prgAddress.notes) && prgAddress.notes.Length > 0;
			return $"{code.ToString("X2")} - {name} " + (hasNotes ? "*" : "") + $" (${prgAddress.pointer.ToString("X5")})";
		}
	}
}
