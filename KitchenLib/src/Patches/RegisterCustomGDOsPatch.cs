using System;
using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Event;
using KitchenLib.Systems;
using KitchenLib.Utils;
using System.Collections.Generic;
using KitchenLib.Colorblind;
using UnityEngine;
using KitchenLib.src.Patches;
using KitchenMods;

namespace KitchenLib.Patches
{
    [HarmonyPatch(typeof(GameDataConstructor), "BuildGameData", new Type[] { })]
	internal class RegisterCustomGDOsPatch
	{
		private static readonly List<GameDataObject> GameDataObjects = new List<GameDataObject>();
		private static bool FirstRun = true;

		static void Postfix(KitchenData.GameDataConstructor __instance, KitchenData.GameData __result)
		{
			MaterialUtils.SetupMaterialIndex();
			FontUtils.SetupFontIndex();
			VFXUtils.SetupVFXIndex();
			GDOUtils.SetupGDOIndex(__result);
			ColorblindUtils.Init(__result);
			
			foreach ((string, Type) x in ModsPreferencesMenu<MainMenuAction>.MenusToRegister.Keys)
				ModsPreferencesMenu<MainMenuAction>.Register(x.Item1, x.Item2, ModsPreferencesMenu<MainMenuAction>.MenusToRegister[x]);
			foreach ((string, Type) x in ModsPreferencesMenu<PauseMenuAction>.MenusToRegister.Keys)
				ModsPreferencesMenu<PauseMenuAction>.Register(x.Item1, x.Item2, ModsPreferencesMenu<PauseMenuAction>.MenusToRegister[x]);

			if (FirstRun) // only build custom GDOs once
			{
				foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
				{
					if (gdo.mod == null)
					{
						Main.LogWarning($"{gdo.GetType().FullName} Failed to register correctly. Attempting to fix...");
						bool found = false;
						foreach (Mod mod in ModPreload.Mods)
						{
							foreach (AssemblyModPack pack in mod.GetPacks<AssemblyModPack>())
							{
								foreach (Type type in pack.Asm.GetTypes())
								{
									if (type == gdo.GetType())
									{
										gdo.mod = mod;
										Main.LogWarning($"Successfully fixed {gdo.GetType().FullName}");
										found = true;
										break;
									}
								}
								if (found)
									break;
							}
							if (found)
								break;
						}
					}
				}
				
				foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
				{
					Main.LogDebug($"-----===== Convert GDO : ({gdo.GetType().BaseType}) {gdo.GetType().FullName} =====-----");
					GameDataObject gameDataObject;
					gdo.Convert(__result, out gameDataObject);
					gameDataObject.name = $"{gdo.ModID} - {gdo.UniqueNameID}";
					gdo.GameDataObject = gameDataObject;
					GameDataObjects.Add(gameDataObject);
				}

				foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
				{
					
					Main.LogDebug($"-----===== AttachDependentProperties GDO : ({gdo.GetType().BaseType}) {gdo.GetType().FullName} =====-----");
					gdo.AttachDependentProperties(__result, gdo.GameDataObject);
				}

				foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
				{
					Main.LogDebug($"-----===== OnRegister GDO : ({gdo.GetType().BaseType}) {gdo.GetType().FullName} =====-----");
					gdo.OnRegister(gdo.GameDataObject);
				}
			}

			EventUtils.InvokeEvent(nameof(Events.BuildGameDataPreSetupEvent), Events.BuildGameDataPreSetupEvent?.GetInvocationList(), null, new BuildGameDataEventArgs(__result, FirstRun));


			foreach (GameDataObject gameDataObject in GameDataObjects)
			{
				Main.LogDebug($"-----===== SetupForGame GDO : ({gameDataObject.GetType().BaseType}) {gameDataObject.name} =====-----");
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
				Main.LogDebug($"-----===== Prefabs.Add GDO : ({gameDataObject.GetType().BaseType}) {gameDataObject.name} =====-----");
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
				Main.LogDebug($"-----===== SetupFinal GDO : ({gameDataObject.GetType().BaseType}) {gameDataObject.name} =====-----");
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
					Main.LogDebug($"-----===== Specifics GDO : ({gameDataObject.GetType().BaseType}) {gameDataObject.name} =====-----");
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
							if (customDish.DestroyAfterModUninstall)
								MainMenuDishDebugSystem.MenuOptions.Add(dish.ID);
							else
								MainMenuDishSystem.MenuOptions.Add(dish.ID);
							
							if (customDish.mod != null && customDish.mod.Name == "")
								BuildLocalDishOptions.MenuOptions.Add(dish.ID);
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

					if (gameDataObject.GetType() == typeof(PlayerCosmetic))
					{
						PlayerCosmetic cosmetic = (PlayerCosmetic)gameDataObject;
						if (!cosmetic.DisableInGame)
						{
							if (cosmetic.CosmeticType == CosmeticType.Hat) { CosmeticMenuPatch.Hats.Add(cosmetic); }
							if (cosmetic.CosmeticType == CosmeticType.Outfit) { CosmeticMenuPatch.Outfits.Add(cosmetic); }
						}
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
