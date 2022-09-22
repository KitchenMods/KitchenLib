using KitchenData;
using System;

namespace Kitchenlib.Event
{
    public class BuildGameDataEventArgs : EventArgs
    {
        public readonly GameData gamedata;
        internal BuildGameDataEventArgs(GameData gamedata)
        {
            this.gamedata = gamedata;
        }
    }
}