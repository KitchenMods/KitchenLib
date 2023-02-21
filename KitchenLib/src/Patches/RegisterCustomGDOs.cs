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
		private static readonly List<GameDataObject> GameDataObjects = new List<GameDataObject>();
		private static bool FirstRun = true;

		static void Postfix(KitchenData.GameDataConstructor __instance, KitchenData.GameData __result) {
			MaterialUtils.SetupMaterialIndex();
			GDOUtils.SetupGDOIndex(__result);
			ColorblindUtils.Init(__result);

			if (FirstRun) // only build custom GDOs once
			{
				foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
				{
					GameDataObject gameDataObject;
					gdo.Convert(__result, out gameDataObject);
					gameDataObject.name = $"{gdo.ModName} - {gdo.UniqueNameID}";
					gdo.GameDataObject = gameDataObject;
					GameDataObjects.Add(gameDataObject);
				}

				foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
				{
					gdo.AttachDependentProperties(__result, gdo.GameDataObject);
				}

				foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
				{
					gdo.OnRegister(gdo.GameDataObject);
				}
			}

			EventUtils.InvokeEvent(nameof(Events.BuildGameDataPreSetupEvent), Events.BuildGameDataPreSetupEvent?.GetInvocationList(), null, new BuildGameDataEventArgs(__result, FirstRun));

			foreach (GameDataObject gameDataObject in GameDataObjects)
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
					Main.LogInfo(e.Message);
				}
			}

			foreach (GameDataObject gameDataObject in GameDataObjects)
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

			foreach (GameDataObject gameDataObject in GameDataObjects)
			{
				gameDataObject.SetupFinal();
			}

			/*
			 * Custom setup for specific cases of certain GDOs:
			 *  - setting localisation for Recipes on a per-dish-basis (sets to whatever language is set in Preferences, defaults to English if one is not set)
			 *  - attaches all side item prefabs to "Side Prefab"
			 */
			if (FirstRun) // only register recipes once
			{
				foreach (GameDataObject gameDataObject in GameDataObjects)
				{
					// Dishes
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

					// Items
					if (gameDataObject.GetType() == typeof(Item) || gameDataObject.GetType() == typeof(ItemGroup))
                    {
						Item item = (Item)gameDataObject;
						if (!item.IsMergeableSide)
                        {
							continue;
						}

						ItemGroupViewUtils.AddPossibleSide(__result, item);
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

			EventUtils.InvokeEvent(nameof(Events.BuildGameDataEvent), Events.BuildGameDataEvent?.GetInvocationList(), null, new BuildGameDataEventArgs(__result, FirstRun));

			__result.Dispose();
			__result.InitialiseViews();

			EventUtils.InvokeEvent(nameof(Events.BuildGameDataPostViewInitEvent), Events.BuildGameDataPostViewInitEvent?.GetInvocationList(), null, new BuildGameDataEventArgs(__result, FirstRun));

			if (FirstRun)
            {
                FirstRun = false;
            }
		}
	}
}
