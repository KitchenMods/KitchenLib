using Kitchen;
using KitchenLib.Customs;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib
{
	public class RepairLegacyFranchiseSystem : FranchiseFirstFrameSystem
	{
		protected override void Initialise()
		{
			base.Initialise();
			availableFranchises = GetEntityQuery(typeof(CFranchiseItem));
		}
		protected override void OnUpdate()
		{
			using var ents = availableFranchises.ToEntityArray(Allocator.Temp);
			foreach (var ent in ents)
			{
				CFranchiseItem item = EntityManager.GetComponentData<CFranchiseItem>(ent);
				for (int i = 0; i < item.Cards.Count; i++)
				{
					if (CustomGDO.LegacyGDOIDs.ContainsKey(item.Cards[i]))
					{
						Main.LogInfo($"Updated {item.Cards[i]} => {CustomGDO.LegacyGDOIDs[item.Cards[i]]}");
						item.Cards[i] = CustomGDO.LegacyGDOIDs[item.Cards[i]];
					}
				}
				EntityManager.SetComponentData(ent, item);
			}
		}
		private EntityQuery availableFranchises;
	}
}
