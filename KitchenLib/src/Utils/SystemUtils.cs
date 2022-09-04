using Unity.Entities;
using Kitchen;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace KitchenLib.Utils
{
	public class SystemUtils
	{
		public static List<Type> systems = new List<Type>();
		public static T GetSystem<T>() where T : GenericSystemBase {
			return World.DefaultGameObjectInjectionWorld.GetExistingSystem<T>();
		}

		[Obsolete("Use the NewSystem method instead")]
		public static T AddSystem<T>() where T : GenericSystemBase, new() {
			Mod.Log($"Registered system '{typeof(T).ToString()}'");
			return World.DefaultGameObjectInjectionWorld.AddSystem<T>(new T());
		}

		public static void NewSystem<T>() where T : GenericSystemBase, new()
		{
			systems.Add(typeof(T));
		}
	}

	[HarmonyPatch(typeof(WorldBootstrapper))]
	class WorldBootstrapper_Patch
	{
		[HarmonyPatch(typeof(WorldBootstrapper))]
		[HarmonyPatch(MethodType.Constructor, new Type[] { typeof(string), typeof(GameConnectionMode) })]
		static void Postfix(WorldBootstrapper __instance)
		{
			foreach (Type system in SystemUtils.systems)
			{
				__instance.CreatedWorld.GetOrCreateSystem<SimulationSystemGroup>().AddSystemToUpdateList(__instance.CreatedWorld.GetOrCreateSystem(system));
			}
		}
	}
	
}
