using Kitchen;
using KitchenLib.Appliances;
using KitchenLib.Utils;
using UnityEngine;
using HarmonyLib;

namespace KitchenLib.TestMod
{
	public class Mod : BaseMod
	{
		public Mod() : base() { }
        public static CustomAppliance d;
		public override void OnApplicationStart() {
			base.OnApplicationStart();
			d = AddAppliance<TestingTerminalAppliance>();
        }
        [HarmonyPatch(typeof(ProvideStartingEnvelopes), "OnUpdate")]
        class ProvideStartingEnvelopes_Patch
        {
            [HarmonyPostfix]
            static void Postfix(ProvideStartingEnvelopes __instance)
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    SpawnUtils.SpawnApplianceBlueprint(Mod.d.ID, 0f, -1);
                }
            }

        }
    }
}