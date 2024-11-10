using System.Collections.Generic;

namespace KitchenLib.Utils
{
	public class ItemOverrides
	{
		internal static Dictionary<int, int> RewardOverrides = new Dictionary<int, int>();

		/// <summary>
		/// Override the reward of an item.
		/// </summary>
		/// <param name="itemId">The ID of the item.</param>
		/// <param name="reward">The new reward of the item.</param>
		public static void AddRewardOverride(int itemId, int reward)
		{
			RewardOverrides[itemId] = reward;
		}

		/// <summary>
		/// Remove an item reward override.
		/// </summary>
		/// <param name="itemId">The ID of the item.</param>
		public static void RemoveRewardOverride(int itemId)
		{
			RewardOverrides.Remove(itemId);
		}
	}
}
