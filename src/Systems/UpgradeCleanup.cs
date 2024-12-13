using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using KitchenMods;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Systems
{
	public class UpgradeCleanup : GenericSystemBase, IModSystem
	{
		private EntityQuery query;
		private Dictionary<int, int> dishes = new Dictionary<int, int>();
		/*
		 *  0 = Base Game
		 *  1 = Not In Lobby
		 *  2 = In Lobby
		 */
		protected override void Initialise()
		{
			query = GetEntityQuery(typeof(CUpgrade));
			foreach (Dish dish in GameData.Main.Get<Dish>())
			{
				CustomDish customDish = (CustomDish)GDOUtils.GetCustomGameDataObject(dish.ID);
				if (customDish != null)
				{
					if (customDish.IsAvailableAsLobbyOption)
					{
						dishes.Add(dish.ID, 2);
					}
					else
					{
						dishes.Add(dish.ID, 1);
					}
				}
				else
				{
					dishes.Add(dish.ID, 0);
				}
			}
		}

		protected override void OnUpdate()
		{
			NativeArray<Entity> entities = query.ToEntityArray(Allocator.Temp);

			for (int i = 0; i < entities.Length; i++)
			{
				if (Require<CUpgrade>(entities[i], out CUpgrade upgrade))
				{
					if (dishes.ContainsKey(upgrade.ID))
					{
						if (dishes[upgrade.ID] == 1)
						{
							EntityManager.DestroyEntity(entities[i]);
						}
					}
				}
			}
		}
	}
}