using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Components
{
	public struct CAchievementTicketView : IComponentData, IModComponent
	{
		public FixedString32 modId;
		public FixedString32 achivementKey;
	}
}