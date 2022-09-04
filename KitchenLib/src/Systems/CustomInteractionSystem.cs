using Kitchen;
using KitchenLib.Appliances;

namespace KitchenLib.Systems
{
    public class CustomInteractionSystem : TriggerActivation
    {
        protected override bool IsPossible(ref InteractionData data)
		{
			return base.IsPossible(ref data);
		}
		
		protected override void Perform(ref InteractionData data)
        {
			Mod.Log("Performing Interaction");
			CustomAppliance customAppliance = GetApplianceFromInteraction(ref data);
			if (customAppliance != null)
			{
				if (!customAppliance.PreInteract(data))
				{
					base.Perform(ref data);
				}
				customAppliance.PostInteract(data);
			}
			else
			{
				base.Perform(ref data);
			}
		}

		private CustomAppliance GetApplianceFromInteraction(ref InteractionData data)
		{
			if (!base.Require<CAppliance>(data.Target, out var appliance))
			{
				return null;
			}

			CustomAppliance customAppliance = CustomAppliances.Get(appliance.ID);
			if (customAppliance == null || customAppliance.Appliance == null)
			{
				return null;
			}

			return customAppliance;
		}
	}
}
