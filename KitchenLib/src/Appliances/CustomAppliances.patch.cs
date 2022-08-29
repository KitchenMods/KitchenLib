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

			var prefabHostObject = new UnityEngine.GameObject();
			prefabHostObject.name = "Custom Appliance Prefab Host";
			prefabHostObject.SetActive(false);

			foreach(var appliance in CustomAppliances.Appliances.Values) {
				Appliance newApp;
				if (appliance.BaseApplianceId != -1)
				{
					newApp = UnityEngine.Object.Instantiate(__result.Get<Appliance>().FirstOrDefault(a => a.ID == appliance.BaseApplianceId));
                    newApp.ID = appliance.ID;
                }
				else
				{
                    newApp = UnityEngine.Object.Instantiate(__result.Get<Appliance>().FirstOrDefault(a => a.ID == -1248669347));
					newApp.ID = appliance.ID;
					if (appliance.CrateItem != null) newApp.CrateItem = appliance.CrateItem;
					if (appliance.EffectCondition != null) newApp.EffectCondition = appliance.EffectCondition;
					if (appliance.EffectRange != null) newApp.EffectRange = appliance.EffectRange;
					if (appliance.EffectRepresentation != null) newApp.EffectRepresentation = appliance.EffectRepresentation;
					if (appliance.EffectType != null) newApp.EffectType = appliance.EffectType;
					if (appliance.HeldAppliancePrefab != null) newApp.HeldAppliancePrefab = appliance.HeldAppliancePrefab;
					if (appliance.Prefab != null) newApp.Prefab = appliance.Prefab;
					if (appliance.Processes != null) newApp.Processes = appliance.Processes;
					if (appliance.Properties != null) newApp.Properties = appliance.Properties;
					if (appliance.RequiresForShop != null) newApp.RequiresForShop = appliance.RequiresForShop;
					if (appliance.RequiresProcessForShop != null) newApp.RequiresProcessForShop = appliance.RequiresProcessForShop;
					if (appliance.Sections != null) newApp.Sections = appliance.Sections;
					if (appliance.Tags != null) newApp.Tags = appliance.Tags;
					if (appliance.Upgrades != null) newApp.Upgrades = appliance.Upgrades;
					newApp.Description = appliance.Description;
					newApp.EntryAnimation = appliance.EntryAnimation;
					newApp.ExitAnimation = appliance.ExitAnimation;
					newApp.ForceHighInteractionPriority = appliance.ForceHighInteractionPriority;
					newApp.IsAnUpgrade = appliance.IsAnUpgrade;
					newApp.IsNonCrated = appliance.IsNonCrated;
					newApp.IsNonInteractive = appliance.IsNonInteractive;
					newApp.IsPurchasable = appliance.IsPurchasable;
					newApp.IsPurchasableAsUpgrade = appliance.IsPurchasableAsUpgrade;
					newApp.Layer = appliance.Layer;
					newApp.Name = appliance.Name;
					newApp.PreventSale = appliance.PreventSale;
					newApp.RarityTier = appliance.RarityTier;
					newApp.SellOnlyAsDuplicate = appliance.SellOnlyAsDuplicate;
					newApp.ShoppingTags = appliance.ShoppingTags;
					newApp.ShopRequirementFilter = appliance.ShopRequirementFilter;
					newApp.SkipRotationAnimation = appliance.SkipRotationAnimation;
					newApp.ThemeRequired = appliance.ThemeRequired;
                }
				newApp.Info = new LocalisationObject<ApplianceInfo>();
				newApp.name = $"{newApp.Name}(Clone)";

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
					newApp.Prefab.transform.SetParent(prefabHostObject.transform);
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
