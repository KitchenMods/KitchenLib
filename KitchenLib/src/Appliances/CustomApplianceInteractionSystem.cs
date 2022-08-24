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
			CustomAppliance customAppliance = GetApplianceFromInteraction(ref data);
			if(customAppliance == null)
				return false;

			return customAppliance.OnCheckInteractPossible(data);
		}
		
		protected override void Perform(ref InteractionData data) {
			CustomAppliance customAppliance = GetApplianceFromInteraction(ref data);
			customAppliance?.OnInteract(data);
		}
		
        private CustomAppliance GetApplianceFromInteraction(ref InteractionData data) {
			if(!base.Require<CAppliance>(data.Target, out var appliance))
				return null;
			
			CustomAppliance customAppliance = CustomAppliances.Get(appliance.ID);
			if(customAppliance == null || customAppliance.Appliance == null)
				return null;

			return customAppliance;
		}
	}
}
