using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Components
{
	public struct CRequestAchievementUnlock : IComponentData, IModComponent
	{
		public FixedString64 modId;
		public FixedString64 achivementKey;
	}
}