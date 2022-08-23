using Kitchen;
using KitchenData;

namespace KitchenLib.Appliances
{
	public class CustomApplianceInteractionSystem : InteractionSystem
	{
		protected override bool AllowActOrGrab
		{
			get { return true; }
		}

		protected override bool AllowAnyMode
		{
			get { return true; }
		}
        
        protected override bool IsPossible(ref InteractionData data) {	
			CustomApplianceInfo applianceInfo = GetApplianceFromInteraction(ref data);
			if(applianceInfo == null || applianceInfo.OnCheckInteractPossible == null)
				return false;

			return applianceInfo.OnCheckInteractPossible(data);
		}
		
		protected override void Perform(ref InteractionData data) {
			CustomApplianceInfo applianceInfo = GetApplianceFromInteraction(ref data);
			if(applianceInfo == null || applianceInfo.OnInteract == null)
				return;

			applianceInfo.OnInteract(data);
		}
		
        private CustomApplianceInfo GetApplianceFromInteraction(ref InteractionData data) {
			if(!base.Require<CAppliance>(data.Target, out var appliance))
				return null;
			
			CustomApplianceInfo applianceInfo = CustomAppliances.Get(appliance.ID);
			if(applianceInfo == null || applianceInfo.Appliance == null)
				return null;

			return applianceInfo;
		}
	}
}
