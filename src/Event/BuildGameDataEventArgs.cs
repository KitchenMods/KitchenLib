using KitchenData;
using System;

namespace KitchenLib.Event
{
	public class BuildGameDataEventArgs : EventArgs
	{
		public readonly GameData gamedata;
		public readonly bool firstBuild;
		internal BuildGameDataEventArgs(GameData gamedata, bool firstBuild)
		{
			this.gamedata = gamedata;
			this.firstBuild = firstBuild;
		}
	}
}