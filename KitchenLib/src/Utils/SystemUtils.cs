using Unity.Entities;
using Kitchen;

namespace KitchenLib.Utils
{
	public class SystemUtils
	{
		public static T GetSystem<T>() where T : GenericSystemBase {
			return World.DefaultGameObjectInjectionWorld.GetExistingSystem<T>();
		}

		public static T AddSystem<T>() where T : GenericSystemBase, new() {
			return World.DefaultGameObjectInjectionWorld.AddSystem<T>(new T());
		}
	}
}
