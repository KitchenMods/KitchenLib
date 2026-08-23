using Kitchen;
using System;

namespace KitchenLib.Event
{
	public static class Events
	{
		// BuildGameData Events
		public static EventHandler<BuildGameDataEventArgs> BuildGameDataPreSetupEvent;
		public static EventHandler<BuildGameDataEventArgs> BuildGameDataEvent;
		public static EventHandler<BuildGameDataEventArgs> BuildGameDataPostViewInitEvent;

		[Obsolete("")]
		public static EventHandler<MainMenuView_SetupMenusArgs> MainMenuView_SetupMenusEvent;
		public static EventHandler<MainMenu_SetupArgs> MainMenu_SetupEvent;
		public static EventHandler<PlayerPauseView_SetupMenusArgs> PlayerPauseView_SetupMenusEvent;
		

		#region Obsolete

		[Obsolete("Please use PreferenceManager Instead")]
		public static EventHandler<PreferencesSaveArgs> PreferencesSaveEvent;
		[Obsolete("Functionality removed, Really shouldn't be messing with this")]
		public static EventHandler<PerformInitialSetupEventArgs> Perfor1mInitialSetupEvent;
		[Obsolete]
		public static EventHandler<PlayerViewEventArgs> PlayerViewEvent;
		[Obsolete]
		public static EventHandler<StartMainMenu_SetupArgs> StartMainMenu_SetupEvent;
		[Obsolete("Please use ModsPreferencesMenu<T>.RegisterMenu")]
		public static EventHandler<PreferenceMenu_SetupArgs> PreferenceMenu_MainMenu_SetupEvent;
		[Obsolete("Please use ModsPreferencesMenu<T>.RegisterMenu")]
		public static EventHandler<PreferenceMenu_SetupArgs> PreferenceMenu_PauseMenu_SetupEvent;
		[Obsolete]
		public static EventHandler<PreferenceMenu_CreateSubmenusArgs<MainMenuAction>> PreferenceMenu_MainMenu_CreateSubmenusEvent;
		[Obsolete]
		public static EventHandler<PreferenceMenu_CreateSubmenusArgs<PauseMenuAction>> PreferenceMenu_PauseMenu_CreateSubmenusEvent;

		#endregion
	}
}