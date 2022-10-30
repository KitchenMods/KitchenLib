using KitchenData;
using System;
using System.Reflection;

namespace KitchenLib.Event
{
    public class PreferenceMenu_SetupArgs : EventArgs
    {
        internal Type type;
        internal object instance;
        internal MethodInfo mInfo;
        internal PreferenceMenu_SetupArgs(object instance, MethodInfo mInfo)
        {
            this.type = type;
            this.instance = instance;
            this.mInfo = mInfo;
        }
    }
}