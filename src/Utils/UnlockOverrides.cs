using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Utils
{
	public class UnlockOverrides
	{
		internal static Dictionary<int, Color> ColourOverrides = new Dictionary<int, Color>();
		internal static Dictionary<int, string> IconOverrides = new Dictionary<int, string>();

		/// <summary>
		/// Override the color of an unlock.
		/// </summary>
		/// <param name="unlockId">The ID of the unlock.</param>
		/// <param name="colour">The new color.</param>
		public static void AddColourOverride(int unlockId, Color colour)
		{
			ColourOverrides[unlockId] = colour;
		}

		/// <summary>
		/// Remove the color override of an unlock.
		/// </summary>
		/// <param name="unlockId">The ID of the unlock.</param>
		public static void RemoveColourOverride(int unlockId)
		{
			ColourOverrides.Remove(unlockId);
		}

		/// <summary>
		/// Override the icon of an unlock.
		/// </summary>
		/// <param name="unlockId">The ID of the unlock.</param>
		/// <param name="icon">The new icon.</param>
		public static void AddIconOverride(int unlockId, string icon)
		{
			IconOverrides[unlockId] = icon;
		}

		/// <summary>
		/// Remove the icon override of an unlock.
		/// </summary>
		/// <param name="unlockId">The ID of the unlock.</param>
		public static void RemoveIconOverride(int unlockId)
		{
			IconOverrides.Remove(unlockId);
		}
	}
}
