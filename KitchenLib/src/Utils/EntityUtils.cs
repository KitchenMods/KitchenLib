using Unity.Entities;

namespace KitchenLib.Utils
{
	public class EntityUtils
	{
		public static EntityManager GetEntityManager()
		{
			return Unity.Entities.World.DefaultGameObjectInjectionWorld.GetExistingSystem<Kitchen.PlayerManager>().EntityManager;
		}
	}
}
