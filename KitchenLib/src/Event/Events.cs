using System;

namespace KitchenLib.Event
{
    public static class Events
    {
        public static EventHandler<BuildGameDataEventArgs> BuildGameDataEvent;
        public static EventHandler<PlayerViewEventArgs> PlayerViewEvent;
    }
}