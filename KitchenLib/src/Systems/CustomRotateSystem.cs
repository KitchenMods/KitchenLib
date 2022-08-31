using Kitchen;
using KitchenLib.Appliances;
using Unity.Mathematics;
using Unity.Entities;
using System;

namespace KitchenLib.Systems
{
	[UpdateBefore(typeof(Kitchen.RotateAppliances))]
	public class CustomRotateSystem : RotateAppliances
	{
		private CPosition Position;
		protected override bool IsPossible(ref InteractionData data)
		{
			return base.IsPossible(ref data);
		}

		protected override void Perform(ref InteractionData data)
		{
			CustomAppliance customAppliance = GetApplianceFromRotation(ref data);
			if (customAppliance != null)
			{
				if (!customAppliance.PreRotate(data))
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

			CustomAppliance customAppliance = CustomAppliances.Get(appliance.ID);
			if (customAppliance == null || customAppliance.Appliance == null)
			{
				return null;
			}

			return customAppliance;
		}
	}
}