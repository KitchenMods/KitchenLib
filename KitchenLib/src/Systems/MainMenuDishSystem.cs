using Kitchen;
using KitchenMods;
using System.Collections.Generic;
using Unity.Entities;

namespace KitchenLib.Systems
{
	public class MainMenuDishSystem : GenericSystemBase, IModSystem
	{
		public static List<int> MenuOptions = new List<int>();
		protected override void Initialise()
		{
			foreach (int dishID in MenuOptions)
			{
				Entity e = EntityManager.CreateEntity(typeof(CUpgrade), typeof(CPersistThroughSceneChanges));
				Set<CUpgrade>(e, new CUpgrade { ID = dishID });
			}
		}
		protected override void OnUpdate()
		{
		}
	}
}
