using Unity.Entities;
using Kitchen;
using System;

namespace KitchenLib.Utils
{
	public class SystemUtils
	{
		public static T GetSystem<T>() where T : GenericSystemBase {
			return World.DefaultGameObjectInjectionWorld.GetExistingSystem<T>();
		}

		[Obsolete("Check wiki for system registration")]
		public static T AddSystem<T>() where T : GenericSystemBase, new() {
			Mod.Log($"Registered system '{typeof(T).ToString()}'");
			return World.DefaultGameObjectInjectionWorld.AddSystem<T>(new T());
		}

		[Obsolete("Check wiki for system registration")]
		public static void NewSystem<T1, T2>() where T1 : GenericSystemBase where T2 : ComponentSystemGroup, new()
		{
			World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<T2>().AddSystemToUpdateList(World.DefaultGameObjectInjectionWorld.GetOrCreateSystem(typeof(T1)));
		}
	}
	
}
