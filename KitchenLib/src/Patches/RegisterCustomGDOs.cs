using System;
using HarmonyLib;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using KitchenLib.Event;
using KitchenLib.src.ContentPack;

namespace KitchenLib.Customs
{
	[HarmonyPatch(typeof(GameDataConstructor), "BuildGameData", new Type[] { })]
	public class GameDataConstructor_Patch
	{
		static void Postfix(KitchenData.GameDataConstructor __instance, KitchenData.GameData __result) {
			MaterialUtils.SetupMaterialIndex();
			GDOUtils.SetupGDOIndex(__result);

			ContentPackManager.Preload();

			List<GameDataObject> gameDataObjects = new List<GameDataObject>();

			foreach (CustomGameDataObject gdo in CustomGDO.GDOs.Values)
			{
				GameDataObject gameDataObject;
				gdo.Convert(__result, out gameDataObject);
				gdo.OnRegister(gameDataObject);
				gdo.GameDataObject = gameDataObject;
				gameDataObjects.Add(gameDataObject);
			}

			foreach (GameDataObject gameDataObject in gameDataObjects)
			{
				try
				{
					gameDataObject.SetupForGame();
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
			__result.Dispose();
			__result.InitialiseViews();

			EventUtils.InvokeEvent(nameof(Events.BuildGameDataEvent), Events.BuildGameDataEvent?.GetInvocationList(), null, new BuildGameDataEventArgs(__result));
		}
	}
}
