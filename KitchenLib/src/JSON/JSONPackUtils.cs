using KitchenData;
using KitchenLib.JSON.Enums;
using KitchenLib.JSON.Interfaces;
using KitchenLib.JSON.Models.Containers;
using KitchenLib.References;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.JSON
{
	internal class JSONPackUtils
	{
		internal static Dictionary<string, Type> SoftDependencies = new();
		internal static Dictionary<string, MethodInfo> SoftMethodCache = new();

		internal static MethodInfo GetSoftDependency(string typeFullName, string methodName, bool isGeneric, object[] parameters)
		{
			string key = $"{typeFullName}.{methodName}" + (isGeneric ? "[T}" : "");

			if (SoftMethodCache.TryGetValue(key, out MethodInfo info))
			{
				return info;
			}

			Type type = null;
			if (SoftDependencies.TryGetValue(typeFullName, out Type cacheType))
			{
				type = cacheType;
			}
			else
			{
				type = AppDomain.CurrentDomain
					.GetAssemblies()
					.SelectMany(asm => asm.GetTypes())
					.Where(_ => _.FullName == typeFullName)
					.FirstOrDefault();
			}

			MethodInfo CacheMethod = type
				.GetMethods()
				.Where(_ => _.Name == methodName && _.ContainsGenericParameters == isGeneric)
				.FirstOrDefault();

			SoftMethodCache[key] = CacheMethod;
			return CacheMethod;
		}

		internal static Material GetMaterialByName(string name)
		{
			return MaterialUtils.GetExistingMaterial(name) ??
				MaterialUtils.GetCustomMaterial(name) ??
				null;
		}

		internal static bool TryParseIdentifier(string identifier, out int ID)
		{
			if (int.TryParse(identifier, out int intID))
			{
				ID = intID;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _ApplianceReferences applianceReferences))
			{
				ID = (int)applianceReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _CompositeUnlockPackReferences compositeUnlockPackReferences))
			{
				ID = (int)compositeUnlockPackReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _CrateSetReferences crateSetReferences))
			{
				ID = (int)crateSetReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _DecorReferences decorReferences))
			{
				ID = (int)decorReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _DishReferences dishReferences))
			{
				ID = (int)dishReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _EffectReferences effectReferences))
			{
				ID = (int)effectReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _GardenProfileReferences gardenProfileReferences))
			{
				ID = (int)gardenProfileReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _ItemReferences itemReferences))
			{
				ID = (int)itemReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _ItemGroupReferences itemGroupReferences))
			{
				ID = (int)itemGroupReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _LayoutProfileReferences layoutProfileReferences))
			{
				ID = (int)layoutProfileReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _LevelUpgradeSetReferences levelUpgradeSetReferences))
			{
				ID = (int)levelUpgradeSetReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _ModularUnlockPackReferences modularUnlockPackReferences))
			{
				ID = (int)modularUnlockPackReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _PlayerCosmeticReferences playerCosmeticReferences))
			{
				ID = (int)playerCosmeticReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _ProcessReferences processReferences))
			{
				ID = (int)processReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _RandomUpgradeSetReferences randomUpgradeSetReferences))
			{
				ID = (int)randomUpgradeSetReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _ResearchReferences researchReferences))
			{
				ID = (int)researchReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _ShopReferences shopReferences))
			{
				ID = (int)shopReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _ThemeUnlockReferences themeUnlockReferences))
			{
				ID = (int)themeUnlockReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _UnlockReferences unlockReferences))
			{
				ID = (int)unlockReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _UnlockCardReferences unlockCardReferences))
			{
				ID = (int)unlockCardReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _UnlockPackReferences unlockPackReferences))
			{
				ID = (int)unlockPackReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _WorkshopRecipeReferences workshopRecipeReferences))
			{
				ID = (int)workshopRecipeReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _CustomerTypeReferences customerTypeReferences))
			{
				ID = (int)customerTypeReferences;
				return true;
			}

			if (Enum.TryParse(identifier, true, out _RestaurantSettingReferences restaurantSettingReferences))
			{
				ID = (int)restaurantSettingReferences;
				return true;
			}
			ID = -1;
			return false;
		}

		internal static GameObject PrefabConverter<T>(string modname, string guid, PrefabContext context, object temp) where T : GameDataObject
		{
			if (temp == null)
				return null;

			if (temp is string identifier)
			{
				if (TryParseIdentifier(identifier, out int ID))
				{
					switch (context)
					{
						case PrefabContext.Prefab:
							IHasPrefab hasPrefab = ((T)GDOUtils.GetExistingGDO(ID) ?? (T)GDOUtils.GetCustomGameDataObject(ID)?.GameDataObject) as IHasPrefab;
							return hasPrefab.Prefab;
						case PrefabContext.SidePrefab:
							IHasSidePrefab hasSidePrefab = ((T)GDOUtils.GetExistingGDO(ID) ?? (T)GDOUtils.GetCustomGameDataObject(ID)?.GameDataObject) as IHasSidePrefab;
							return hasSidePrefab.SidePrefab;
						case PrefabContext.IconPrefab:
							IHasIconPrefab hasIconPrefab = ((T)GDOUtils.GetExistingGDO(ID) ?? (T)GDOUtils.GetCustomGameDataObject(ID)?.GameDataObject) as IHasIconPrefab;
							return hasIconPrefab.IconPrefab;
						case PrefabContext.DisplayPrefab:
							IHasDisplayPrefab hasDisplayPrefab = ((T)GDOUtils.GetExistingGDO(ID) ?? (T)GDOUtils.GetCustomGameDataObject(ID)?.GameDataObject) as IHasDisplayPrefab;
							return hasDisplayPrefab.DisplayPrefab;
					}
				}
				else
				{
					if (JSONPackManager.AssetBundleCache.TryGetValue(modname, out List<AssetBundle> bundles))
					{
						return bundles.FirstOrDefault(_ => _.LoadAsset<GameObject>(identifier) != null)?.LoadAsset<GameObject>(identifier);
					}
				}

			}
			else if (temp is PrefabContainer container)
			{
				return container.Convert(guid);
			}
			return null;
		}

		internal static T GDOConverter<T>(string identifier) where T : GameDataObject
		{
			if (identifier == null)
				return null;

			if (TryParseIdentifier(identifier, out int ID))
			{
				return (T)GDOUtils.GetExistingGDO(ID) ?? (T)GDOUtils.GetCustomGameDataObject(ID)?.GameDataObject;
			}
			else
			{
				string[] split = identifier.Split(':');
				if (split.Length == 1)
				{
					Main.Logger.LogInfo($"{identifier} isn't recognized");
					return null;
				}
				string mod_id = split[0];
				string name = split[1];
				return (T)GDOUtils.GetCustomGameDataObject(mod_id, name).GameDataObject;
			}
		}
	}
}
