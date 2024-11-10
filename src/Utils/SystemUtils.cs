using Kitchen;
using Unity.Entities;

namespace KitchenLib.Utils
{
	public class SystemUtils
	{
		public static T GetSystem<T>() where T : GenericSystemBase
		{
			return World.DefaultGameObjectInjectionWorld.GetExistingSystem<T>();
		}
	}

}
