using System;
using System.Reflection;

namespace KitchenLib.Event
{
	public class PreferenceMenu_SetupArgs : EventArgs
	{
		public readonly object instance;
		public readonly MethodInfo mInfo;
		internal PreferenceMenu_SetupArgs(object instance, MethodInfo mInfo)
		{
			this.instance = instance;
			this.mInfo = mInfo;
		}
	}
}