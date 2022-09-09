using System;
using UnityEngine;
using Kitchen;
using KitchenData;
using KitchenLib.Appliances;
using KitchenLib.Utils;

namespace KitchenLib.TestMod
{
	public class TestingTerminalAppliance : CustomAppliance
	{
        public override string Name
        {
            get { return "Custom Appliance Lol"; }
        }

        public override bool PreInteract(InteractionData data, bool isSecondary)
        {
			return true;
        }

        public override bool PreRotate(InteractionData data, bool isSecondary)
        {
			return false;
        }
    }
}
