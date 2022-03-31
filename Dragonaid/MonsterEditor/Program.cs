using System;
using System.Windows.Forms;

namespace AtomosZ.Dragonaid.MonsterEditor
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MonsterEditorMainForm());
		}
	}
}
