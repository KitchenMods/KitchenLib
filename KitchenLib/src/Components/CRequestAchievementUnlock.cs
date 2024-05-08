using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Components
{
	public struct CRequestAchievementUnlock : IComponentData, IModComponent
	{
		public FixedString32 modId;
		public FixedString32 achivementKey;
	}
}