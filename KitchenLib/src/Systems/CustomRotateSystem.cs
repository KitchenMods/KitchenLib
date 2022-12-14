using Kitchen;
using KitchenLib.Customs;
using Unity.Entities;
using Controllers;
using KitchenLib.Utils;

namespace KitchenLib.Systems
{
	[UpdateBefore(typeof(Kitchen.RotateAppliances))]
	public class CustomRotateSystem : RotateAppliances
	{
		//private CPosition Position;
		protected override bool IsPossible(ref InteractionData data)
		{
			CustomAppliance customAppliance = GetApplianceFromRotation(ref data);
			if (customAppliance != null)
			{
				if (customAppliance.ForceIsRotationPossible())
					return customAppliance.IsRotationPossible(data);
				else
					return base.IsPossible(ref data);
			}
			return base.IsPossible(ref data);
		}

		protected override void Perform(ref InteractionData data)
		{
			CustomAppliance customAppliance = GetApplianceFromRotation(ref data);
			Require(data.Interactor, out CInputData input);
			if (customAppliance != null)
			{
				if (!customAppliance.PreRotate(data, (input.State.StopMoving == ButtonState.Held)))
				{
					base.Perform(ref data);
				}
				customAppliance.PostRotate(data);
			}
			else
			{
				base.Perform(ref data);
			}
		}


		private CustomAppliance GetApplianceFromRotation(ref InteractionData data)
		{
			if (!base.Require<CAppliance>(data.Target, out var appliance))
			{
				return null;
			}

			CustomAppliance customAppliance = (CustomAppliance)GDOUtils.GetCustomGameDataObject(appliance.ID);
			if (customAppliance == null || customAppliance.GameDataObject == null)
			{
				return null;
			}

			return customAppliance;
		}
	}
}