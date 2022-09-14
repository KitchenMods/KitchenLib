using Kitchen;
using KitchenLib.Customs;
using Controllers;

namespace KitchenLib.Systems
{
    public class CustomInteractionSystem : TriggerActivation
    {
		protected override bool IsPossible(ref InteractionData data)
		{
			CustomAppliance customAppliance = GetApplianceFromInteraction(ref data);
			if (customAppliance != null)
			{
				if (customAppliance.ForceIsInteractionPossible())
					return customAppliance.IsInteractionPossible(data);
				else
					return base.IsPossible(ref data);
			}
			return base.IsPossible(ref data);
		}
		
		protected override void Perform(ref InteractionData data)
        {
			CustomAppliance customAppliance = GetApplianceFromInteraction(ref data);
			Require(data.Interactor, out CInputData input);
			if (customAppliance != null)
			{
				if (!customAppliance.PreInteract(data, (input.State.StopMoving == ButtonState.Held)))
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

			CustomAppliance customAppliance = CustomGDO.GetCustomAppliance(appliance.ID);
			if (customAppliance == null || customAppliance.Appliance == null)
			{
				return null;
			}

			return customAppliance;
		}
	}
}
