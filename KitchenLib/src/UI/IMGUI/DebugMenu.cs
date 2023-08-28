using Kitchen;
using Kitchen.NetworkSupport;
using Kitchen.Transports;
using KitchenData;
using KitchenLib.DataDumper;
using KitchenLib.DataDumper.Dumpers;
using KitchenLib.DevUI;
using KitchenLib.Patches;
using KitchenLib.Utils;
using KitchenLib.Systems;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Entities;
using UnityEngine;
using KitchenLib.Customs;
using System.Reflection;

namespace KitchenLib.UI
{
	public class DebugMenu : BaseUI
	{
		public GameObject MaterialDisplay = null;
		public DebugMenu()
		{
			ButtonName = "Debug";
		}

		public override void OnInit()
		{
		}
		public override void Setup()
		{
			if (GUILayout.Button("References"))
			{
				GenerateReferences(GameData.Main);
			}
			if (GUILayout.Button("DataDump"))
			{

				Dump<CrateSetDumper>();
				Dump<DecorDumper>();
				Dump<EffectDumper>();
				Dump<GameDifficultySettingsDumper>();
				Dump<GardenProfileDumper>();
				Dump<ItemDumper>();
				Dump<ItemGroupDumper>();
				Dump<LayoutProfileDumper>();
				Dump<LevelUpgradeSetDumper>();
				Dump<ProcessDumper>();
				Dump<RandomUpgradeSetDumper>();
				Dump<ShopDumper>();
				Dump<CompositeUnlockPackDumper>();
				Dump<ModularUnlockPackDumper>();
				Dump<WorkshopRecipeDumper>();
				Dump<ApplianceDumper>();
				Dump<EffectRepresentationDumper>();
				Dump<PlayerCosmeticDumper>();
				Dump<ResearchDumper>();
				Dump<DishDumper>();
				Dump<ThemeUnlockDumper>();
				Dump<UnlockCardDumper>();
				Dump<RestaurantSettingDumper>();
				Dump<FranchiseUpgradeDumper>();
				Dump<ContractDumper>();
				Dump<CustomerTypeDumper>();
				Dump<CustomerGroupDumper>();
				List<string> GDOs = new List<string>();
				if (File.Exists(Application.dataPath + "/Managed/Kitchen.GameData.dll"))
				{
					Assembly assem = Assembly.LoadFile(Application.dataPath + "/Managed/Kitchen.GameData.dll");
					foreach (Type type in assem.GetTypes())
					{
						if (type.IsSubclassOf(typeof(GameDataObject)))
						{
							GDOs.Add(type.Name);
							foreach (FieldInfo info in type.GetFields())
							{
								GDOs.Add("	" + info.Name);
							}
						}
					}
				}
				File.WriteAllLines(Path.Combine(Application.persistentDataPath, "DataDump", "GDOs.txt"), GDOs.ToArray());
			}
			if (GUILayout.Button("Refresh Dish Options"))
			{
				RefreshDishUpgrades.Refresh = true;
			}
			if (GUILayout.Button("Create Feature Flag Preferences File"))
			{
				FeatureFlags.SaveFeatureFlagFile();
			}

			GUILayout.Label("Log Levels");
			GUILayout.BeginHorizontal();
			foreach (LogType logType in Enum.GetValues(typeof(LogType)))
			{
				DebugLogPatch.EnabledLevels[logType] = GUILayout.Toggle(DebugLogPatch.EnabledLevels[logType], logType.ToString());
			}
			GUILayout.EndHorizontal();
		}

		public override void Disable()
		{
		}

		public void Dump<T>() where T : BaseDataDumper, new()
		{
			(new T()).Dump();
		}

		private void GenerateReferences(GameData gameData)
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

			File.WriteAllLines(Path.Combine(Application.persistentDataPath, "Debug", "References.cs"), classGenerator.ToArray());
		}

		private void GenerateClass<T>(ref List<string> list, GameData gamedata) where T : GameDataObject
		{
			GenerateReferenceClass<T>(ref list, gamedata);
			GenerateEnumClass<T>(ref list, gamedata);
		}

		private void GenerateReferenceClass<T>(ref List<string> list, GameData gamedata) where T : GameDataObject
		{
			list.Add($"    public static class {typeof(T).Name}References");
			list.Add("    {");
			foreach (T x in gamedata.Get<T>())
			{
				list.Add($"        public static int {(x.name).Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "")} => (int)_{typeof(T).Name}References.{(x.name).Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "")};\n");
			}
			list.Add("    }");
		}

		private void GenerateEnumClass<T>(ref List<string> list, GameData gamedata) where T : GameDataObject
		{
			list.Add($"    public enum _{typeof(T).Name}References");
			list.Add("    {");
			foreach (T x in gamedata.Get<T>())
			{
				list.Add($"        {(x.name).Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "")} = {x.ID},\n");
			}
			list.Add("    }");
		}

	}
}
