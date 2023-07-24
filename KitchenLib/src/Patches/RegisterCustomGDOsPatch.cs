using System;
using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.Colorblind;
using KitchenLib.Customs;
using KitchenLib.Event;
using KitchenLib.Systems;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

namespace KitchenLib.Patches
{
    [HarmonyPatch(typeof(GameDataConstructor), "BuildGameData", new Type[] { })]
	internal class RegisterCustomGDOsPatch
	{
		private static readonly List<GameDataObject> GameDataObjects = new List<GameDataObject>();
		private static bool FirstRun = true;

		static void Postfix(KitchenData.GameDataConstructor __instance, KitchenData.GameData __result)
		{
			Main.LogDebug("[RegisterCustomGDOsPatch.Postfix] [1.1] Begin Custom GDO Registration");
			MaterialUtils.SetupMaterialIndex();
			GDOUtils.SetupGDOIndex(__result);
			ColorblindUtils.Init(__result);

			if (FirstRun) // only build custom GDOs once
			{
				foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
				{
					Main.LogDebug($"[RegisterCustomGDOsPatch.Postfix] [1.2] Converting {gdo.ModID} - {gdo.UniqueNameID}");
					GameDataObject gameDataObject;
					gdo.Convert(__result, out gameDataObject);
					gameDataObject.name = $"{gdo.ModID} - {gdo.UniqueNameID}";
					gdo.GameDataObject = gameDataObject;
					GameDataObjects.Add(gameDataObject);
				}

				foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
				{
					Main.LogDebug($"[RegisterCustomGDOsPatch.Postfix] [1.3] Attaching Dependent Properties of {gdo.ModID} - {gdo.UniqueNameID}");
					gdo.AttachDependentProperties(__result, gdo.GameDataObject);
				}

				foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
				{
					Main.LogDebug($"[RegisterCustomGDOsPatch.Postfix] [1.4] Registering {gdo.ModID} - {gdo.UniqueNameID}");
					gdo.OnRegister(gdo.GameDataObject);
				}
			}

			EventUtils.InvokeEvent(nameof(Events.BuildGameDataPreSetupEvent), Events.BuildGameDataPreSetupEvent?.GetInvocationList(), null, new BuildGameDataEventArgs(__result, FirstRun));

			Main.LogDebug($"[RegisterCustomGDOsPatch.Postfix] [2.1] Begin Base-Game Registration (For CustomGDOs)");

			foreach (GameDataObject gameDataObject in GameDataObjects)
			{
				try
				{
					Main.LogDebug($"[RegisterCustomGDOsPatch.Postfix] [2.2] Setting Up For Game {gameDataObject.name}");
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
				Main.LogDebug($"[RegisterCustomGDOsPatch.Postfix] [2.3] Setting Up Prefab {gameDataObject.name}");
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
				Main.LogDebug($"[RegisterCustomGDOsPatch.Postfix] [2.4] Setting Up Final {gameDataObject.name}");
				gameDataObject.SetupFinal();
			}

			/*
			 * Custom setup for specific cases of certain GDOs:
			 *  - setting localisation for Recipes on a per-dish-basis (sets to whatever language is set in Preferences, defaults to English if one is not set)
			 *  - attaches all side item prefabs to "Side Prefab"
			 */
			if (FirstRun) // only register recipes once
			{
				Main.LogDebug($"[RegisterCustomGDOsPatch.Postfix] [3.1] Executing GDO Specifics");
				foreach (GameDataObject gameDataObject in GameDataObjects)
				{
					// Dishes
					if (gameDataObject.GetType() == typeof(Dish))
					{
						Dish dish = (Dish)gameDataObject;
						CustomDish customDish = (CustomDish)GDOUtils.GetCustomGameDataObject(dish.ID);
						foreach (Locale locle in customDish.Recipe.Keys)
						{
							Main.LogDebug($"[RegisterCustomGDOsPatch.Postfix] [3.2] Setting Up Dish Recipe For {dish.name}");
							__result.GlobalLocalisation.Recipes.Info.Get(locle).Text.Add(dish, customDish.Recipe[locle]);
							foreach (RecipeLocalisation loc in __result.Get<RecipeLocalisation>())
							{
								if (!loc.Text.ContainsKey(dish))
								{
									if (Enum.TryParse<Locale>(PlayerPrefs.GetString(Pref.Localisation), out Locale locale))
									{
										if (customDish.Recipe.ContainsKey(locale))
										{
											loc.Text.Add(dish, customDish.Recipe[locale]);
										}
										else
										{
											loc.Text.Add(dish, customDish.Recipe[Locale.English]);
										}
									}
									else
									{
										Main.LogError($"Invalid Locale: {PlayerPrefs.GetString(Pref.Localisation)} encountered when registering {customDish.UniqueNameID}");
									}
								}
							}
						}
						if (customDish.IsAvailableAsLobbyOption)
						{
							Main.LogDebug($"[RegisterCustomGDOsPatch.Postfix] [3.3] Setting Up Dish Lobby Option For {dish.name}");
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
						Main.LogDebug($"[RegisterCustomGDOsPatch.Postfix] [3.4] Setting Up Side {item.name}");
						if (!item.IsMergeableSide)
						{
							continue;
						}

						ItemGroupViewUtils.AddPossibleSide(__result, item);
					}
				}
			}

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
