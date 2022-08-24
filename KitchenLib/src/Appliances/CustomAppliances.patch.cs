using System;
using System.Linq;
using HarmonyLib;
using KitchenData;
using KitchenLib.Utils;

namespace KitchenLib.Appliances
{
	[HarmonyPatch(typeof(GameDataConstructor), "BuildGameData", new Type[] { })]
	class GameDataConstructor_Patch
	{
		static void Postfix(KitchenData.GameDataConstructor __instance, KitchenData.GameData __result) {
			MaterialUtils.SetupMaterialIndex(__result);
			foreach(var appliance in CustomAppliances.Appliances.Values) {
				var newApp = UnityEngine.Object.Instantiate(__result.Get<Appliance>().FirstOrDefault(a => a.ID == appliance.BaseApplianceId));
				newApp.ID = appliance.ID;
				newApp.Name = appliance.Name;
				newApp.Description = appliance.Description;
				newApp.Info = new LocalisationObject<ApplianceInfo>();
				newApp.ShoppingTags = appliance.ShoppingTags;
				newApp.name = $"{newApp.Name}(Clone)";
				newApp.Processes.Clear();

				UnityEngine.GameObject prefab = newApp.Prefab;
				if(appliance.Prefab != null) {
					prefab = appliance.Prefab;
				} else if(appliance.BasePrefabId != appliance.BaseApplianceId) {
					var prefabAppliance = __result.Get<Appliance>().FirstOrDefault(a => a.ID == appliance.BasePrefabId);
					if(prefabAppliance)
						prefab = prefabAppliance.Prefab;
				}

				var newAppHasPrefab = newApp as IHasPrefab;
				if(newAppHasPrefab != null) {
					newApp.Prefab = UnityEngine.Object.Instantiate(prefab);
					newApp.Prefab.transform.position = new UnityEngine.Vector3(1000000.0f, 1000000.0f, 1000000.0f);
				}

				appliance.Appliance = newApp;
				appliance.OnRegister(newApp);

				newApp.SetupForGame();
				newApp.Localise(Localisation.CurrentLocale, __result.Substitutions);

				__result.Objects.Add(newApp.ID, newApp);
				if(newAppHasPrefab != null)
					__result.Prefabs.Add(newApp.ID, newAppHasPrefab.Prefab);
			}

			foreach(var info in CustomAppliances.Appliances.Values)
				info.Appliance.SetupFinal();

			__result.Dispose();
			__result.InitialiseViews();
		}
	}
}
