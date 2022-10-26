using System;
using Kitchen;

namespace KitchenLib.Event
{
    public static class Events
    {
        public static EventHandler<BuildGameDataEventArgs> BuildGameDataEvent;
        public static EventHandler<PlayerViewEventArgs> PlayerViewEvent;

        
        public static EventHandler<MainMenuView_SetupMenusArgs> MainMenuView_SetupMenusEvent;
        public static EventHandler<StartMainMenu_SetupArgs> StartMainMenu_SetupEvent;
        public static EventHandler<MainMenu_SetupArgs> MainMenu_SetupEvent;
        public static EventHandler<PlayerPauseView_SetupMenusArgs> PlayerPauseView_SetupMenusEvent;

        //Client
        public static EventHandler<PreferenceMenu_SetupArgs> PreferenceMenu_SetupEvent;
        public static EventHandler<PreferenceMenu_CreateSubmenusArgs<>> PreferenceMenu_CreateSubmenusEvent;
    }
}