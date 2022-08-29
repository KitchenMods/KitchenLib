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

        public override int BaseApplianceId
        {
            get { return -1029710921; }
        }

		public override int BasePrefabId
        {
            get { return -1029710921; }
        }

        public override void OnRegister(Appliance appliance) {
			Mod.Log("Listing Processes");
			Mod.Log(appliance.Processes.Count.ToString());
			foreach (Appliance.ApplianceProcesses proc in appliance.Processes)
			{
				Mod.Log("Process: " + proc.Process.name);
				Mod.Log("ID: " + proc.Process.ID);
				Mod.Log("Info: " + proc.Process.Info);
				Mod.Log("IsPseudoprocessFor: " + proc.Process.IsPseudoprocessFor);
				Mod.Log("BasicEnablingAppliance: " + proc.Process.BasicEnablingAppliance);
				Mod.Log("CanObfuscateProgress: " + proc.Process.CanObfuscateProgress);
				Mod.Log("EnablingApplianceCount: " + proc.Process.EnablingApplianceCount);
				Mod.Log("Icon: " + proc.Process.Icon);
				Mod.Log("IsAutomatic: " + proc.IsAutomatic);
				Mod.Log("Speed: " + proc.Speed);
				Mod.Log("Validity: " + proc.Validity);
			}
            //MaterialUtils.ApplyMaterial(appliance.Prefab, "Counter2/Counter Surface", new Material[] { MaterialUtils.GetExistingMaterial("Blueprint Light") });
		}
        

        public override void OnInteract(InteractionData data) {
			Mod.Log("Hey, don't you touch me!");
		}

		public override bool OnCheckInteractPossible(InteractionData data) {
			return true;
		}
	}
}
