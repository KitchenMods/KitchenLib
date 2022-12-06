using Unity.Entities;
using Kitchen;

namespace KitchenLib.Utils
{
	public class SystemUtils
	{
		public static T GetSystem<T>() where T : GenericSystemBase {
			return World.DefaultGameObjectInjectionWorld.GetExistingSystem<T>();
		}
	}
	
}
