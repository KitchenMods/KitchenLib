using KitchenLib.Event;
using Kitchen;
using System.Reflection;
#if BEPINEX
using BepInEx;
#endif
#if MELONLOADER
using MelonLoader;
#endif


#if MELONLOADER
[assembly: MelonInfo(typeof(KitchenLib.Mod), "KitchenLib", "0.2.1", "KitchenMods")]
[assembly: MelonGame("It's Happening", "PlateUp")]
[assembly: MelonPriority(-1000000)]
[assembly: MelonColor(System.ConsoleColor.Green)]
#endif
namespace KitchenLib
{
#if BEPINEX
	[BepInProcess("PlateUp.exe")]
	[BepInPlugin("kitchenmods.kitchenlib", "KitchenLib", "0.2.1")]
#endif
	public class Mod : BaseMod
	{
#if MELONLOADER
		public Mod() : base("kitchenlib", "1.1.0") { }
#endif
#if BEPINEX
		public Mod() : base("1.1.0", Assembly.GetExecutingAssembly()) { }
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

		private void UniversalSetup()
		{
			Mod.Log("");

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
