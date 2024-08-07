using KitchenData;
using KitchenLib.DevUI;
using KitchenLib.Patches;
using KitchenLib.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using KitchenLib.DataDumper;
using UnityEngine;

namespace KitchenLib.UI
{
	public class DebugMenu : BaseUI
	{
		public DebugMenu()
		{
			ButtonName = "Debug";
		}

		public override void OnInit()
		{
		}

		public override void Setup()
		{
			GUILayout.BeginArea(new Rect(0, 10, 265, 30));
			if (GUILayout.Button("References"))
			{
				GenerateReferences(GameData.Main);
			}
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(265, 10, 265, 30));
			if (GUILayout.Button("DataDump"))
			{
				DumpData(GameData.Main);
			}
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(530, 10, 265, 30));
			if (GUILayout.Button("Refresh Dish Options"))
			{
				RefreshDishUpgrades.Refresh = true;
			}
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 50, 795, 60));
			GUILayout.Label("Log Levels");
			GUILayout.BeginHorizontal();
			foreach (LogType logType in Enum.GetValues(typeof(LogType)))
			{
				DebugLogPatch.EnabledLevels[logType] = GUILayout.Toggle(DebugLogPatch.EnabledLevels[logType], logType.ToString());
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		public override void Disable()
		{
		}

		public void DumpData(GameData gameData)
		{
			foreach (GameDataObject gdo in GameData.Main.Get<GameDataObject>())
			{
				string dirpath = Path.Combine(Application.persistentDataPath, "DataDump");
				string gdopath = Path.Combine(dirpath, gdo.GetType().ToString());
				string filepath = Path.Combine(gdopath, gdo.name + ".txt");

				if (!Directory.Exists(dirpath))
					Directory.CreateDirectory(dirpath);

				if (!Directory.Exists(gdopath))
					Directory.CreateDirectory(gdopath);
				
				
				File.WriteAllLines(dirpath + "/" + gdo.GetType() + ".txt", DataDump.GetFieldNames(gdo));
				File.WriteAllLines(filepath, DataDump.GetFieldValues(gdo).ToArray());
			}
		}

		public void GenerateReferences(GameData gameData)
		{
			List<string> classGenerator = new List<string>();
			classGenerator.Add("namespace KitchenLib.References");
			classGenerator.Add("{");

			GenerateClass<Appliance>(ref classGenerator, gameData);
			GenerateClass<CompositeUnlockPack>(ref classGenerator, gameData);
			GenerateClass<CrateSet>(ref classGenerator, gameData);
			GenerateClass<Decor>(ref classGenerator, gameData);
			GenerateClass<Dish>(ref classGenerator, gameData);
			GenerateClass<Effect>(ref classGenerator, gameData);
			GenerateClass<EffectRepresentation>(ref classGenerator, gameData);
			GenerateClass<GardenProfile>(ref classGenerator, gameData);
			GenerateClass<Item>(ref classGenerator, gameData);
			GenerateClass<ItemGroup>(ref classGenerator, gameData);
			GenerateClass<LayoutProfile>(ref classGenerator, gameData);
			GenerateClass<LevelUpgradeSet>(ref classGenerator, gameData);
			GenerateClass<ModularUnlockPack>(ref classGenerator, gameData);
			GenerateClass<PlayerCosmetic>(ref classGenerator, gameData);
			GenerateClass<Process>(ref classGenerator, gameData);
			GenerateClass<RandomUpgradeSet>(ref classGenerator, gameData);
			GenerateClass<Research>(ref classGenerator, gameData);
			GenerateClass<Shop>(ref classGenerator, gameData);
			GenerateClass<ThemeUnlock>(ref classGenerator, gameData);
			GenerateClass<Unlock>(ref classGenerator, gameData);
			GenerateClass<UnlockCard>(ref classGenerator, gameData);
			GenerateClass<UnlockPack>(ref classGenerator, gameData);
			GenerateClass<WorkshopRecipe>(ref classGenerator, gameData);
			GenerateClass<CustomerType>(ref classGenerator, gameData);
			GenerateClass<RestaurantSetting>(ref classGenerator, gameData);

			classGenerator.Add("}");

			if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Debug")))
				Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Debug"));
			File.WriteAllLines(Path.Combine(Application.persistentDataPath, "Debug", "References.cs"), classGenerator.ToArray());
		}

		private void GenerateClass<T>(ref List<string> list, GameData gamedata) where T : GameDataObject
		{
			GenerateReferenceClass<T>(ref list, gamedata);
			GenerateEnumClass<T>(ref list, gamedata);
		}

		private void GenerateReferenceClass<T>(ref List<string> list, GameData gamedata) where T : GameDataObject
		{
			list.Add($"	public static class {typeof(T).Name}References");
			list.Add("	{");
			foreach (T x in gamedata.Get<T>())
			{
				list.Add($"		public static int {(x.name).Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "")} => (int)_{typeof(T).Name}References.{(x.name).Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "")};\n");
			}
			list.Add("	}");
		}

		private void GenerateEnumClass<T>(ref List<string> list, GameData gamedata) where T : GameDataObject
		{
			list.Add($"	public enum _{typeof(T).Name}References");
			list.Add("	{");
			foreach (T x in gamedata.Get<T>())
			{
				list.Add($"		{(x.name).Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "")} = {x.ID},\n");
			}
			list.Add("	}");
		}
	}
}
