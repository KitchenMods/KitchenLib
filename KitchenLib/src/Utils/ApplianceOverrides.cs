using System.Collections.Generic;

namespace KitchenLib.Utils
{
	public class ApplianceOverrides
	{
		internal static Dictionary<int, int> PurchaseCostOverrides = new Dictionary<int, int>();

		/// <summary>
		/// Override the cost of an appliance.
		/// </summary>
		/// <param name="applianceId">The ID of the appliance.</param>
		/// <param name="cost">The new cost of the appliance.</param>
		public static void AddPurchaseCostOverride(int applianceId, int cost)
		{
			PurchaseCostOverrides[applianceId] = cost;
		}

		/// <summary>
		/// Remove an appliance cost override.
		/// </summary>
		/// <param name="applianceId">The ID of the appliance.</param>
		public static void RemovePurchaseCostOverride(int applianceId)
		{
			PurchaseCostOverrides.Remove(applianceId);
		}
	}
}
