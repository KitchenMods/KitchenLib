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

        public override void OnInteract(InteractionData data)
        {
            Mod.Log("You interacted me!");
        }

        public override void OnRotate(InteractionData data)
        {
            Mod.Log("You Rotated Me!");
        }
    }
}
