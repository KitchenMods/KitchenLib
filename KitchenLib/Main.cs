using System.Reflection;
using KitchenLib.Event;
using Kitchen;
using KitchenData;
using KitchenMods;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using KitchenLib.DevUI;
using KitchenLib.UI;
using System.Linq;
using System.CodeDom;
using Newtonsoft.Json;
using KitchenLib.Customs;

#if MELONLOADER
using MelonLoader;
#endif
#if BEPINEX
using BepInEx;
#endif
#if MELONLOADER
[assembly: MelonInfo(typeof(KitchenLib.Main), KitchenLib.Main.MOD_NAME, KitchenLib.Main.MOD_VERSION, KitchenLib.Main.MOD_AUTHOR)]
[assembly: MelonGame("It's Happening", "PlateUp")]
#endif
namespace KitchenLib
{
#if BEPINEX
	[BepInProcess("PlateUp.exe")]
	[BepInPlugin(MOD_ID, MOD_NAME, MOD_VERSION)]
#endif
	public class Main : BaseMod
	{
		public const string MOD_ID = "kitchenlib";
		public const string MOD_NAME = "KitchenLib";
		public const string MOD_AUTHOR = "KitchenMods";
		public const string MOD_VERSION = "0.3.3";
		public const string MOD_COMPATIBLE_VERSIONS = "1.1.2";

		public static AssetBundle bundle;
		public Main() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_COMPATIBLE_VERSIONS, Assembly.GetExecutingAssembly()) { }

#if !WORKSHOP
		protected override void OnInitialise()
		{
			SetupMenus();
		}
#else
		protected override void OnPostActivate(Mod mod)
		{
			//GenerateReferences();
			SetupMenus();
			RegisterMenu<MaterialsUI>();
		}
		protected override void OnInitialise()
		{
			GameObject go = new GameObject();
			go.AddComponent<DevUIController>();
		}
#endif

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

		private void GenerateReferences()
		{
			Events.BuildGameDataEvent += (s, args) =>
			{
				List<string> classGenerator = new List<string>();
				classGenerator.Add("namespace KitchenLib.References");
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