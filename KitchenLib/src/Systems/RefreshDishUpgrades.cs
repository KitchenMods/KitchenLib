using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using Random = UnityEngine.Random;

namespace KitchenLib.Systems
{

	[UpdateAfter(typeof(CreateOffice))]
	public class RefreshDishUpgrades : GenericSystemBase, IModSystem
	{
		private EntityQuery DishUpgrades;
		private EntityQuery DishOptions;
		public static bool Refresh;
		protected override void Initialise()
		{
			DishUpgrades = GetEntityQuery(typeof(CDishUpgrade));
			DishOptions = GetEntityQuery(typeof(CDishChoice));
		}
		protected override void OnUpdate()
		{
			if (Refresh)
			{
				Refresh = false;
				
				NativeArray<Entity> dishUpgrades = DishUpgrades.ToEntityArray(Allocator.Temp);
				NativeArray<Entity> dishOptions = DishOptions.ToEntityArray(Allocator.Temp);

				for (int i = 0; i < dishOptions.Length; i++)
				{
					int x = EntityManager.GetComponentData<CDishUpgrade>(dishUpgrades[Random.Range(0, dishUpgrades.Length - 1)]).DishID;
					EntityManager.SetComponentData<CDishChoice>(dishOptions[i], new CDishChoice { Dish = x });
				}
				
				dishUpgrades.Dispose();
				dishOptions.Dispose();
			}
		}
	}
}
