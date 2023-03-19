using HarmonyLib;
using Kitchen;
using KitchenData;
using Unity.Entities;
using KitchenLib.Systems;

namespace KitchenLib.Patches
{
	[HarmonyPatch(typeof(ItemExtensions), nameof(ItemExtensions.ChangeItemType))]
	static class ItemExtensions_Patch
	{
		static void Postfix(Entity item, int new_type)
		{
			if (GameData.Main.TryGet<Item>(new_type, out var output, warn_if_fail: true) && output.IsSplittable)
			{
				EntityManager entityManager = SplittableDepletedDelaySystem.instance.EntityManager;

				SplittableDepletedDelaySystem.CSplittableDepletedDelay delay =
					new SplittableDepletedDelaySystem.CSplittableDepletedDelay
					{
						IsFirstFrame = true,
						TimeRemaining = 0.1f,
						SplitCount = output.SplitCount
					};

				entityManager.AddComponent<SplittableDepletedDelaySystem.CSplittableDepletedDelay>(item);
				entityManager.SetComponentData(item, delay);
			}
		}
	}
}
