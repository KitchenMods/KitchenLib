using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Components
{
	public struct SAchievementDisplayView : IBufferElementData, IModComponent
	{
		public FixedString64 modId;
		public FixedString64 achivementKey;
		
		public struct Marker : IComponentData, IModComponent { }
	}
}