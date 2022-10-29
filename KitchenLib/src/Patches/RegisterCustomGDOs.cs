using System;
using HarmonyLib;
using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using KitchenLib.Event;

namespace KitchenLib.Customs
{
	[HarmonyPatch(typeof(GameDataConstructor), "BuildGameData", new Type[] { })]
	public class GameDataConstructor_Patch
	{
		static void Postfix(KitchenData.GameDataConstructor __instance, KitchenData.GameData __result) {
			MaterialUtils.SetupMaterialIndex(__result);

			GDOUtils.SetupGDOIndex(__result);

			BuildGameDataEventArgs buildGameDataEventArgs = new BuildGameDataEventArgs(__result);
			EventUtils.InvokeEvent(nameof(Events.BuildGameDataEvent), Events.BuildGameDataEvent?.GetInvocationList(), null, buildGameDataEventArgs);
			__result = buildGameDataEventArgs.gamedata;

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
					Mod.Log(e.Message);
				}
			}

			foreach (GameDataObject gameDataObject in gameDataObjects)
			{
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
		}
	}
}
