using System.Reflection;
using KitchenLib.Event;
using Kitchen;

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
		public const string MOD_VERSION = "0.2.7";
		public const string MOD_COMPATIBLE_VERSIONS = "1.1.2";
		public Main() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_COMPATIBLE_VERSIONS, Assembly.GetExecutingAssembly()) { }

		protected override void OnFrameUpdate()
		{
		}

		protected override void OnInitialise()
		{
			SetupMenus();
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