using System;
using System.Collections.Generic;

namespace KitchenLib.Registry
{
	public class ModRegistery
	{
		public static Dictionary<Type, BaseMod> Registered = new Dictionary<Type, BaseMod>();


		public static BaseMod Get<T>() {
			return Registered[typeof(T)];
		}
		public static bool Register(BaseMod mod)
        {
			if (!Registered.ContainsKey(mod.GetType()))
			{
				Registered.Add(mod.GetType(), mod);
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
