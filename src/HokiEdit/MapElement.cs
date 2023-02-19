using System;
using Microsoft.DirectX;
using SpriteUtilities;

namespace HokiEdit {
	interface MapElement {
		/// <summary>
		/// Fired when the object is being removed from the workspace
		/// </summary>
		event EventHandler Delete;
		/// <summary>
		/// Causes the object to be deleted - returns false if this procedure is redundant
		/// </summary>
		bool TriggerDelete();
		/// <summary>
		/// Notifies the object that it is selected
		/// </summary>
		void Select();
		/// <summary>
		/// Notifies the object that it is not selected
		/// </summary>
		void Deselect();
		/// <summary>
		/// Whether the object is currently selected
		/// </summary>
		bool Selected();
		/// <summary>
		/// Moves the object
		/// </summary>
		/// <param name="v">The vector to move the object by</param>
		void Move(Vector2 v);
		/// <summary>
		/// Determines whether a point in screen coordinates is inside the object.
		/// </summary>
		/// <param name="pt">A point in screen coordinates</param>
		bool PtInShape(Vector2 pt);
		/// <summary>
		/// Determines whether the element is inside a box
		/// </summary>
		/// <param name="box"></param>
		bool InBox(SpriteBox box);
		/// <summary>
		/// Returns a line of map code to represent itself
		/// </summary>
		string Compile();
	}
}