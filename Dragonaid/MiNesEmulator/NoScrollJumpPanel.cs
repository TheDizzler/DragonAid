using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtomosZ.MiNesEmulator
{
	/// <summary>
	/// A Panel with overridden ScrollToControl to prevent a scrollbar from jumping
	/// all over the place when focus changes.
	/// </summary>
	public partial class NoScrollJumpPanel : Panel
	{
		public NoScrollJumpPanel()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Prevent the scroll bar from jumping all over the place when focus is changed.
		/// Seriously, microsoft. How could this be allowed to happened?
		/// </summary>
		/// <param name="activeControl"></param>
		/// <returns></returns>
		protected override Point ScrollToControl(System.Windows.Forms.Control activeControl)
		{
			// Returning the current location prevents the panel from
			// scrolling to the active control when the panel loses and regains focus
			return this.DisplayRectangle.Location;
		}
	}
}
