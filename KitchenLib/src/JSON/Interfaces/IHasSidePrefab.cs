using UnityEngine;

namespace KitchenLib.JSON.Interfaces
{
	/// <summary>
	/// Represents an class that has a side prefab.
	/// </summary>
	internal interface IHasSidePrefab
	{
		/// <summary>
		/// Gets the side prefab associated with the class.
		/// </summary>
		GameObject SidePrefab { get; }
	}
}
