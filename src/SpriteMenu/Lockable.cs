using System;

namespace SpriteMenu {
	/// <summary>
	/// Used so that a window may lock its children, such that they are still visible but do not respond to input.
	/// This is important when, for example, a main menu brings up submenus. When a submenu is up, you might not want
	/// the user to be able to click buttons on the main menu, but for aesthetic purposes it may be better to leave
	/// the main menu visible.
	/// </summary>
	public interface Lockable {
		/// <summary>
		/// Set the object to ignore user input
		/// </summary>
		void Lock();
		/// <summary>
		/// Set the object to respond to user input
		/// </summary>
		void Unlock();
		/// <summary>
		/// Whether the object is currently locked
		/// </summary>
		bool Locked();
	}
}
