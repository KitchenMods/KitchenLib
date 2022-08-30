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

        public override bool OnCheckRotatePossible(InteractionData data)
        {
            return true;
        }

        public override void OnRotate(InteractionData data)
        {
            Mod.Log("You rotated me!");
        }

        public override bool OnCheckInteractPossible(InteractionData data)
        {
            return true;
        }

        public override void OnInteract(InteractionData data)
        {
            Mod.Log("You interacted me!");
        }
    }
}
