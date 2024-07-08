using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Components
{
	public struct SAchievementTicketView : IBufferElementData, IModComponent
	{
		public FixedString32 modId;
		public FixedString32 achivementKey;
		
		public struct Marker : IComponentData, IModComponent { }
	}
}