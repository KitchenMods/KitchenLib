using System;
using System.Linq;
using HarmonyLib;
using KitchenData;

namespace KitchenLib.Appliances
{
	[HarmonyPatch(typeof(GameDataConstructor), "BuildGameData", new Type[] { })]
	class GameDataConstructor_Patch
	{
		static void Postfix(KitchenData.GameDataConstructor __instance, KitchenData.GameData __result) {
			foreach(var appliance in CustomAppliances.Appliances.Values) {
				var newApp = UnityEngine.Object.Instantiate(__result.Get<Appliance>().FirstOrDefault(a => a.ID == appliance.BaseApplianceId));
				newApp.ID = appliance.ID;
				newApp.Name = appliance.Name;
				newApp.Description = appliance.Description;
				newApp.Info = new LocalisationObject<ApplianceInfo>();
				newApp.ShoppingTags = appliance.ShoppingTags;
				newApp.name = $"{newApp.Name}(Clone)";
				newApp.Processes.Clear();

				if (appliance.Prefab == null){
					if (appliance.BasePrefabId != appliance.BaseApplianceId){
						var prefabApp = UnityEngine.Object.Instantiate(__result.Get<Appliance>().FirstOrDefault(a => a.ID == appliance.BasePrefabId));
						newApp.Prefab = prefabApp.Prefab;
					}
				} else {
					newApp.Prefab = appliance.Prefab;
				}

				appliance.Appliance = newApp;
				appliance.OnRegister(newApp);

				newApp.SetupForGame();
				newApp.Localise(Localisation.CurrentLocale, __result.Substitutions);

				__result.Objects.Add(newApp.ID, newApp);
				if(newApp as IHasPrefab != null)
					__result.Prefabs.Add(newApp.ID, (newApp as IHasPrefab).Prefab);
			}

			foreach(var info in CustomAppliances.Appliances.Values)
				info.Appliance.SetupFinal();

			__result.Dispose();
			__result.InitialiseViews();
		}
	}
}
