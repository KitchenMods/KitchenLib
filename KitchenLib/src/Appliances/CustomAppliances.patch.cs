using System;
using System.Linq;
using HarmonyLib;
using KitchenData;

namespace KitchenLib.Appliances
{
	[HarmonyPatch(typeof(GameDataConstructor), "BuildGameData", new Type[] { })]
	public partial class CustomAppliances
	{
		static void Postfix(KitchenData.GameDataConstructor __instance, KitchenData.GameData __result) {
			foreach(var info in CustomAppliances.Appliances.Values) {
				var newApp = UnityEngine.Object.Instantiate(__result.Get<Appliance>().FirstOrDefault(a => a.ID == info.BaseApplianceId));
				newApp.ID = info.ID;
				newApp.Name = info.Name;
				newApp.Description = info.Description;
				newApp.Info = new LocalisationObject<ApplianceInfo>();
				newApp.name = $"{newApp.Name}(Clone)";
				info.Appliance = newApp;

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
