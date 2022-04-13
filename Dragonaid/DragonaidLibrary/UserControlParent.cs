using System;

namespace AtomosZ.DragonAid.Libraries
{
	public interface UserControlParent
	{
		void Save(object sender, EventArgs e);
		void Defocus(object sender, EventArgs e);
		void UpdateView();
	}
}
