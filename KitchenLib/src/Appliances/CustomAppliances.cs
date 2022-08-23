using System.Collections.Generic;

namespace KitchenLib.Appliances
{
	public partial class CustomAppliances
	{
		public static Dictionary<int, CustomApplianceInfo> Appliances = new Dictionary<int, CustomApplianceInfo>();

		public static int Register(CustomApplianceInfo info) {
			if (info.ID == 0)
				info.ID = info.GetHash();
			if (Appliances.ContainsKey(info.ID)){
                Mod.Error("Appliance: " + info.Name + " failed to register - key:" + info.ID + " already in use. Generating custom key. " + info.GetHash());
                info.ID = info.GetHash();
				if (Appliances.ContainsKey(info.GetHash())){
                    Mod.Error("Appliance: " + info.Name + " failed to register - key:" + info.GetHash() + " Unable to generate custom key.");
					return -1;
                }
			}
			Appliances.Add(info.ID, info);
			Mod.Log($"Registered appliance '{info.ModName}:{info.Name}' as {info.ID}");
			return info.ID;
		}

		/*
		 * Temporary fix for the Github issue #1
		 */
		public static CustomApplianceInfo Get(int id) {
			if (Appliances.ContainsKey(id))
				return Appliances[id];
			else
				return null;
		}
		/*
        public static CustomApplianceInfo Get(int id)
        {
			return Appliances[id];
        }
		*/
    }
}
