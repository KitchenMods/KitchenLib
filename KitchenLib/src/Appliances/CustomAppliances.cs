using System.Collections.Generic;

namespace KitchenLib.Appliances
{
	public partial class CustomAppliances
	{
		public static Dictionary<int, CustomApplianceInfo> Appliances = new Dictionary<int, CustomApplianceInfo>();

		public static int Register(CustomApplianceInfo info) {
			info.ID = info.GetHash();
			Appliances.Add(info.ID, info);
			Mod.Log($"Registered appliance '{info.ModName}:{info.Name}' as {info.ID}");
			return info.ID;
		}
	}
}
