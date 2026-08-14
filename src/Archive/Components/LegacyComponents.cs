using KitchenData;
using Unity.Entities;

namespace KitchenLib.Systems
{
	public struct CViewHolder : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CCommandView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CInfoView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CSendToClientView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CTileHightlighterView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CClientEquipCapeView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CSyncModsView : IApplianceProperty, IAttachableProperty, IComponentData { }
}