using Kitchen;
using KitchenLib.Appliances;

namespace KitchenLib.Systems
{
    public class CustomRotateSystem : RotateAppliances
    {
		protected override bool IsPossible(ref InteractionData data)
		{
			return true;
		}

		protected override void Perform(ref InteractionData data)
		{
			Mod.Log("Running Rotation");
			CustomAppliance customAppliance = GetApplianceFromRotation(ref data);
			customAppliance?.OnRotate(data);
		}

		private CustomAppliance GetApplianceFromRotation(ref InteractionData data)
		{
			if (!base.Require<CAppliance>(data.Target, out var appliance))
				return null;

			CustomAppliance customAppliance = CustomAppliances.Get(appliance.ID);
			if (customAppliance == null || customAppliance.Appliance == null)
				return null;

			return customAppliance;
		}
	}
}