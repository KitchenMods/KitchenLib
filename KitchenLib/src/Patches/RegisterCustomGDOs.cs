using System;
using HarmonyLib;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using KitchenLib.Event;
using KitchenLib.Systems;
using UnityEngine;
using Kitchen;
using KitchenLib.Colorblind;

namespace KitchenLib.Customs
{
	[HarmonyPatch(typeof(GameDataConstructor), "BuildGameData", new Type[] { })]
	public class GameDataConstructor_Patch
	{
		static void Postfix(KitchenData.GameDataConstructor __instance, KitchenData.GameData __result) {
			MaterialUtils.SetupMaterialIndex();
			GDOUtils.SetupGDOIndex(__result);
			ColorblindUtils.Init(__result);

			List<GameDataObject> gameDataObjects = new List<GameDataObject>();

			foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
			{
				GameDataObject gameDataObject;
				gdo.Convert(__result, out gameDataObject);
				gdo.GameDataObject = gameDataObject;
				gameDataObjects.Add(gameDataObject);
			}

			foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
			{
				gdo.AttachDependentProperties(__result, gdo.GameDataObject);
			}

			foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
            {
				gdo.OnRegister(gdo.GameDataObject);
			}

			foreach (GameDataObject gameDataObject in gameDataObjects)
			{
				try
				{
					gameDataObject.SetupForGame();
					gameDataObject.Localise(Localisation.CurrentLocale, __instance.StringSubstitutions);
					GlobalLocalisation globalLocalisation = gameDataObject as GlobalLocalisation;
					if (globalLocalisation != null)
					{
						__result.GlobalLocalisation = globalLocalisation;
					}
				}
				catch (Exception e)
				{
					Main.instance.Log(e.Message);
				}
			}

			foreach (GameDataObject gameDataObject in gameDataObjects)
			{
				if (__result.Objects.ContainsKey(gameDataObject.ID))
					break;
				__result.Objects.Add(gameDataObject.ID, gameDataObject);
				IHasPrefab hasPrefab = gameDataObject as IHasPrefab;
				if (hasPrefab != null)
				{
					__result.Prefabs.Add(gameDataObject.ID, hasPrefab.Prefab);
				}
			}

			foreach (GameDataObject gameDataObject in gameDataObjects)
			{
				gameDataObject.SetupFinal();
			}

			/*
			 * Setting localisation for Recipes on a per-dish-basis
			 * Sets to whatever language is set in Preferences, defaults to English if one is not set.
			 */
			
			foreach (GameDataObject gameDataObject in gameDataObjects)
			{
				if (gameDataObject.GetType() == typeof(Dish))
				{
					Dish dish = (Dish)gameDataObject;
					CustomDish customDish = (CustomDish)GDOUtils.GetCustomGameDataObject(dish.ID);
					foreach (Locale locle in customDish.Recipe.Keys)
					{
						__result.GlobalLocalisation.Recipes.Info.Get(locle).Text.Add(dish, customDish.Recipe[locle]);
						foreach (RecipeLocalisation loc in __result.Get<RecipeLocalisation>())
						{
							if (!loc.Text.ContainsKey(dish))
								if (customDish.Recipe.ContainsKey((Locale)Enum.Parse(typeof(Locale), PlayerPrefs.GetString(Pref.Localisation))))
								loc.Text.Add(dish, customDish.Recipe[(Locale)Enum.Parse(typeof(Locale), PlayerPrefs.GetString(Pref.Localisation))]);
							else
								loc.Text.Add(dish, customDish.Recipe[Locale.English]);
						}
					}
					if (customDish.IsAvailableAsLobbyOption)
					{
						if (customDish.DestroyAfterModUninstall)
							MainMenuDishDebugSystem.MenuOptions.Add(dish.ID);
						else
							MainMenuDishSystem.MenuOptions.Add(dish.ID);
					}
				}
			}

			/*
			 * Used to extract assets
			foreach (GameDataObject gameDataObject in gameDataObjects)
			{
				Texture2D texture = new Texture2D(1, 1);
				if (gameDataObject.GetType() == typeof(Appliance))
					texture = PrefabSnapshot.GetApplianceSnapshot(((Appliance)gameDataObject).Prefab);
				if (gameDataObject.GetType() == typeof(Dish))
					texture = PrefabSnapshot.GetApplianceSnapshot(((Dish)gameDataObject).DisplayPrefab);
				if (gameDataObject.GetType() == typeof(Item))
					texture = PrefabSnapshot.GetApplianceSnapshot(((Item)gameDataObject).Prefab);


				byte[] bytes = texture.EncodeToPNG();
				var dirPath = Application.dataPath + "/../SaveImages/";
				if (!Directory.Exists(dirPath))
				{
					Directory.CreateDirectory(dirPath);
				}
				File.WriteAllBytes(dirPath + gameDataObject.ID + ".png", bytes);
			}
			 */

			__result.Dispose();
			__result.InitialiseViews();

			EventUtils.InvokeEvent(nameof(Events.BuildGameDataEvent), Events.BuildGameDataEvent?.GetInvocationList(), null, new BuildGameDataEventArgs(__result));
		}
	}
}
