using System;
using System.Collections.Generic;
using System.Reflection;

namespace KitchenLib.Registry
{
	public class ModRegistery
	{
		public static Dictionary<Type, BaseMod> Registered = new Dictionary<Type, BaseMod>();
		public static Dictionary<Type, Assembly> keyValuePairs = new Dictionary<Type, Assembly>();


		public static BaseMod Get<T>() {
			return Registered[typeof(T)];
		}
		public static bool Register(BaseMod mod)
        {
			if (!Registered.ContainsKey(mod.GetType()))
			{
				Main.instance.Log("Registered: " + mod.ModName + ":" + mod.ModID + " v" + mod.ModVersion);
				Registered.Add(mod.GetType(), mod);
				keyValuePairs.Add(mod.GetType(), Assembly.GetAssembly(mod.GetType()));
				return true;
			}
			return false;
        }

		public static bool isModSafeForVersion(BaseMod mod)
		{
			if (Main.semVersion.SatisfiesNpm(mod.CompatibleVersions))
				return true;
			return false;
		}
	}
}
