using Kitchen;
using System;

namespace KitchenLib.Event
{
	public static class Events
	{
		public static EventHandler<BuildGameDataEventArgs> BuildGameDataPreSetupEvent;
		public static EventHandler<BuildGameDataEventArgs> BuildGameDataEvent;
		public static EventHandler<BuildGameDataEventArgs> BuildGameDataPostViewInitEvent;
		public static EventHandler<PlayerViewEventArgs> PlayerViewEvent;

		public static EventHandler<PerformInitialSetupEventArgs> PerformInitialSetupEvent;

		public static EventHandler<PreferencesSaveArgs> PreferencesSaveEvent;

		public static EventHandler<MainMenuView_SetupMenusArgs> MainMenuView_SetupMenusEvent;
		public static EventHandler<StartMainMenu_SetupArgs> StartMainMenu_SetupEvent;
		public static EventHandler<MainMenu_SetupArgs> MainMenu_SetupEvent;
		public static EventHandler<PlayerPauseView_SetupMenusArgs> PlayerPauseView_SetupMenusEvent;

		//Client
		[Obsolete("Please use ModsPreferencesMenu<T>.RegisterMenu")]
		public static EventHandler<PreferenceMenu_SetupArgs> PreferenceMenu_MainMenu_SetupEvent;
		[Obsolete("Please use ModsPreferencesMenu<T>.RegisterMenu")]
		public static EventHandler<PreferenceMenu_SetupArgs> PreferenceMenu_PauseMenu_SetupEvent;
		public static EventHandler<PreferenceMenu_CreateSubmenusArgs<MainMenuAction>> PreferenceMenu_MainMenu_CreateSubmenusEvent;
		public static EventHandler<PreferenceMenu_CreateSubmenusArgs<PauseMenuAction>> PreferenceMenu_PauseMenu_CreateSubmenusEvent;
	}
}