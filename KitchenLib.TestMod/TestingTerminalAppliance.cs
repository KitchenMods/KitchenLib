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

		public override string Name {
			get { return "Test Appliance"; }
		}

		public override string Description {
			get { return "It's an appliance for testing things"; }
		}
        public override GameObject Prefab {
			get { return null; }
		}

        public override void OnRegister(Appliance appliance) {
            MaterialUtils.ApplyMaterial(appliance.Prefab, "Counter2/Counter Surface", new Material[] { MaterialUtils.GetExistingMaterial("Blueprint Light") });
		}
        

        public override void OnInteract(InteractionData data) {
			Mod.Log("Hey, don't you touch me!");
		}

		public override bool OnCheckInteractPossible(InteractionData data) {
			return true;
		}
	}
}
