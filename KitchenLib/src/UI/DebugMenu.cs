using KitchenLib.DevUI;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using KitchenLib.DataDumper;
using KitchenLib.DataDumper.Dumpers;
using KitchenData;
using KitchenLib.Systems;

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
			}
			if (GUILayout.Button("Refresh Dish Options"))
			{
				RefreshDishUpgrades.Refresh = true;
			}
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

			classGenerator.Add("}");

			File.WriteAllLines(Path.Combine(Application.dataPath, "References.cs"), classGenerator.ToArray());
			UnityEngine.Debug.Log("Data saved to: " + Path.Combine(Application.dataPath, "References.cs"));
		}

		private void GenerateClass<T>(ref List<string> list, GameData gamedata) where T : GameDataObject
		{
			list.Add($"    public class {typeof(T).Name}References");
			list.Add("    {");
			foreach (T x in gamedata.Get<T>())
			{
				list.Add($"        public const int {(x.name).Replace(" ", "").Replace("-", "")} = {x.ID};\n");
			}
			list.Add("    }");
		}

	}
}
