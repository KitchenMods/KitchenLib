using KitchenLib.Event;
using Kitchen;
using System.Reflection;
using System.IO;
using System;
using UnityEngine;
using KitchenLib.Customs;

using KitchenData;
using System.Collections.Generic;
using KitchenLib.Utils;


#if BEPINEX
using BepInEx;
#endif
#if MELONLOADER
using MelonLoader;
#endif
#if MELONLOADER
[assembly: MelonInfo(typeof(KitchenLib.Main), "KitchenLib", "0.2.4", "KitchenMods")]
[assembly: MelonGame("It's Happening", "PlateUp")]
[assembly: MelonPriority(-1000000)]
[assembly: MelonColor(System.ConsoleColor.Green)]
#endif
namespace KitchenLib
{
#if BEPINEX
	[BepInProcess("PlateUp.exe")]
	[BepInPlugin(GUID, "KitchenLib", "0.2.4")]
#endif
	public class Main : BaseMod
	{
		public const string GUID = "kitchenmods.kitchenlib";
#if MELONLOADER
		public Main() : base("kitchenlib", "1.1.1") { }
#endif
#if BEPINEX
		public Main() : base("1.1.1", Assembly.GetExecutingAssembly()) { }
#endif


#if MELONLOADER
		public override void OnInitializeMelon()
		{
			SetupMenus();
		}
#endif

#if BEPINEX
		public void Start()
		{
			SetupMenus();
		}
#endif

#if WORKSHOP
		public Main() : base("KitchenLib", "0.2.4", "1.1.2", Assembly.GetExecutingAssembly()) { }
		protected override void OnUpdate(){}

		protected override void Initialise()
		{
			//GenerateReferences();
			SetupMenus();
		}
#endif

		private void GenerateReferences()
		{
			Events.BuildGameDataEvent += (s, args) =>
			{
				List<string> classGenerator = new List<string>();
				classGenerator.Add("namespace KitchenLib.Reference");
				classGenerator.Add("{");

				GenerateClass<Appliance>(ref classGenerator, args.gamedata);
				GenerateClass<CompositeUnlockPack>(ref classGenerator, args.gamedata);
				GenerateClass<CrateSet>(ref classGenerator, args.gamedata);
				GenerateClass<Decor>(ref classGenerator, args.gamedata);
				GenerateClass<Dish>(ref classGenerator, args.gamedata);
				GenerateClass<Effect>(ref classGenerator, args.gamedata);
				GenerateClass<EffectRepresentation>(ref classGenerator, args.gamedata);
				GenerateClass<GardenProfile>(ref classGenerator, args.gamedata);
				GenerateClass<Item>(ref classGenerator, args.gamedata);
				GenerateClass<ItemGroup>(ref classGenerator, args.gamedata);
				GenerateClass<LayoutProfile>(ref classGenerator, args.gamedata);
				GenerateClass<LevelUpgradeSet>(ref classGenerator, args.gamedata);
				GenerateClass<ModularUnlockPack>(ref classGenerator, args.gamedata);
				GenerateClass<PlayerCosmetic>(ref classGenerator, args.gamedata);
				GenerateClass<Process>(ref classGenerator, args.gamedata);
				GenerateClass<RandomUpgradeSet>(ref classGenerator, args.gamedata);
				GenerateClass<Research>(ref classGenerator, args.gamedata);
				GenerateClass<Shop>(ref classGenerator, args.gamedata);
				GenerateClass<ThemeUnlock>(ref classGenerator, args.gamedata);
				GenerateClass<Unlock>(ref classGenerator, args.gamedata);
				GenerateClass<UnlockCard>(ref classGenerator, args.gamedata);
				GenerateClass<UnlockPack>(ref classGenerator, args.gamedata);
				GenerateClass<WorkshopRecipe>(ref classGenerator, args.gamedata);

				classGenerator.Add("}");

				File.WriteAllLines(Path.Combine(Application.dataPath, "References.cs"), classGenerator.ToArray());
				UnityEngine.Debug.Log("Data saved to: " + Path.Combine(Application.dataPath, "References.cs"));
			};
		}

		private void GenerateClass<T>(ref List<string> list, GameData gamedata) where T : GameDataObject
		{
			list.Add($"	public class {typeof(T).Name}Reference");
			list.Add("	{");
			foreach (T x in gamedata.Get<T>())
			{
				list.Add($"		public const int {(x.name).Replace(" ", "").Replace("-", "")} = {x.ID};\n");
			}
			list.Add("	}");
		}

		private void SetupMenus()
		{
			//Setting Up For Main Menu
			Events.StartMainMenu_SetupEvent += (s, args) =>
			{
				args.addSubmenuButton.Invoke(args.instance, new object[] { "Mods", typeof(ModsMenu<MainMenuAction>), false });
				args.addSubmenuButton.Invoke(args.instance, new object[] { "Mod Preferences", typeof(ModsPreferencesMenu<MainMenuAction>), false });
			};
			Events.MainMenuView_SetupMenusEvent += (s, args) =>
			{
				args.addMenu.Invoke(args.instance, new object[] { typeof(RevisedMainMenu), new RevisedMainMenu(args.instance.ButtonContainer, args.module_list) });
				args.addMenu.Invoke(args.instance, new object[] { typeof(ModsMenu<MainMenuAction>), new ModsMenu<MainMenuAction>(args.instance.ButtonContainer, args.module_list) });
				args.addMenu.Invoke(args.instance, new object[] { typeof(ModsPreferencesMenu<MainMenuAction>), new ModsPreferencesMenu<MainMenuAction>(args.instance.ButtonContainer, args.module_list) });
			};

			//Setting Up For Pause Menu
			Events.MainMenu_SetupEvent += (s, args) =>
			{
				args.addSubmenuButton.Invoke(args.instance, new object[] { "Mods", typeof(ModsMenu<PauseMenuAction>), false });
				args.addSubmenuButton.Invoke(args.instance, new object[] { "Mod Preferences", typeof(ModsPreferencesMenu<PauseMenuAction>), false });
			};
			Events.PlayerPauseView_SetupMenusEvent += (s, args) =>
			{
				args.addMenu.Invoke(args.instance, new object[] { typeof(ModsMenu<PauseMenuAction>), new ModsMenu<PauseMenuAction>(args.instance.ButtonContainer, args.module_list) });
				args.addMenu.Invoke(args.instance, new object[] { typeof(ModsPreferencesMenu<PauseMenuAction>), new ModsPreferencesMenu<PauseMenuAction>(args.instance.ButtonContainer, args.module_list) });
			};
		}
	}
}
