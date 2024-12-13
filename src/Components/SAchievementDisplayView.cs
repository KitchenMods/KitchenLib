using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Components
{
	public struct SAchievementDisplayView : IBufferElementData, IModComponent
	{
		public FixedString128 modId;
		public FixedString128 achivementKey;
		
		public struct Marker : IComponentData, IModComponent { }
	}
}