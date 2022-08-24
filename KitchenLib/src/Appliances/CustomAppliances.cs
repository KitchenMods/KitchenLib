using System.Collections.Generic;

namespace KitchenLib.Appliances
{
	public class CustomAppliances
	{
		public static Dictionary<int, CustomAppliance> Appliances = new Dictionary<int, CustomAppliance>();

		public static bool Register(CustomAppliance appliance) {
			if (appliance.ID == 0)
				appliance.ID = appliance.GetHash();

			if (Appliances.ContainsKey(appliance.ID)){
                Mod.Error("Appliance: " + appliance.Name + " failed to register - key:" + appliance.ID + " already in use. Generating custom key. " + appliance.GetHash());
				return false;
                // appliance.ID = appliance.GetHash();
				// if (Appliances.ContainsKey(appliance.GetHash())){
                //     Mod.Error("Appliance: " + appliance.Name + " failed to register - key:" + appliance.GetHash() + " Unable to generate custom key.");
				// 	return -1;
                // }
			}
			
			Appliances.Add(appliance.ID, appliance);
			Mod.Log($"Registered appliance '{appliance.ModName}:{appliance.Name}' as {appliance.ID}");
			return true;
		}

		public static CustomAppliance Get(int id) {
			Appliances.TryGetValue(id, out var result);
			return result;
		}
    }
}
