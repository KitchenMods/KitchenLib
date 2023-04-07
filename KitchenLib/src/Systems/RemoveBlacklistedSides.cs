using Kitchen;
using KitchenLib.Utils;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib
{
	[UpdateAfter(typeof(AssignMenuRequests))]
	public class RemoveBlacklistedSides : GameSystemBase, IModSystem
	{
		private EntityQuery AllOrderedItems;
		protected override void Initialise()
		{
			AllOrderedItems = GetEntityQuery(typeof(CWaitingForItem.Marker));
		}
		protected override void OnUpdate()
		{
			using var ents = AllOrderedItems.ToEntityArray(Allocator.Temp);
			foreach (Entity ent in ents)
			{
				DynamicBuffer<CWaitingForItem> buffer;
				if (EntityManager.RequireBuffer<CWaitingForItem>(ent, out buffer))
				{
					int DishHasBlacklist = -1;
					for (int i = 0; i < buffer.Length; i++)
					{
						CWaitingForItem orderedItem = buffer[i];
						if (GDOUtils.BlacklistedDishSides.ContainsKey(orderedItem.ItemID))
						{
							DishHasBlacklist = orderedItem.ItemID;
							break;
						}
					}
					for (int i = 0; i < buffer.Length; i++)
					{
						CWaitingForItem orderedItem = buffer[i];
						if (DishHasBlacklist != -1)
						{
							if (GDOUtils.BlacklistedDishSides[DishHasBlacklist].Contains(orderedItem.ItemID))
							{
								buffer.RemoveAt(i);
							}
						}
					}
				}
			}
		}
	}
}
