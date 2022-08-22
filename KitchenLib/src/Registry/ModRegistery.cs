using System;
using System.Collections;
using System.Collections.Generic;

namespace KitchenLib.Registry
{
	public class ModRegistery
	{
		public static Dictionary<Type, BaseMod> Registered = new Dictionary<Type, BaseMod>();

		public static BaseMod Get<T>() {
			return Registered[typeof(T)];
		}

		public static void Register(BaseMod mod) {
			Registered.Add(mod.GetType(), mod);
		}
	}
}
