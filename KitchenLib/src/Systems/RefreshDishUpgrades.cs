using Kitchen;
using KitchenMods;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Systems
{
	[UpdateAfter(typeof(CreateOffice))]
	public class RefreshDishUpgrades : GenericSystemBase, IModSystem
	{
		protected override void Initialise()
		{
			base.Initialise();
			this.DishUpgrades = base.GetEntityQuery(new ComponentType[]
			{
				typeof(CDishUpgrade)
			});
			this.DishOptions = base.GetEntityQuery(new ComponentType[]
			{
				typeof(CDishChoice)
			});
		}

		public static bool Refresh;

		protected override void OnUpdate()
		{
			if (Refresh)
			{
				Refresh = false;
				NativeArray<CDishUpgrade> nativeArray = this.DishUpgrades.ToComponentDataArray<CDishUpgrade>(Allocator.Temp);
				List<CDishUpgrade> list3 = nativeArray.ToList<CDishUpgrade>().Shuffle<CDishUpgrade>();

				using var ents = DishOptions.ToEntityArray(Allocator.Temp);
				int x = 0;
				foreach (var ent in ents)
				{
					EntityManager.SetComponentData<CDishChoice>(ent, new CDishChoice { Dish = list3[x].DishID });
					x++;
				}
			}
		}

		private EntityQuery DishUpgrades;
		private EntityQuery DishOptions;
	}
}
