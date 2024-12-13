using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Components
{
	public struct CRequestAchievementUnlock : IComponentData, IModComponent
	{
		public FixedString128 modId;
		public FixedString128 achivementKey;
	}
}