using KitchenLib.Appliances;
using KitchenLib.Utils;
using UnityEngine;
using HarmonyLib;
using KitchenData;
using Kitchen;

namespace KitchenLib.TestMod
{
	public class Mod : BaseMod
	{
        public static int d;
		public Mod() : base() { }
		
		public override void OnApplicationStart() {
			base.OnApplicationStart();
            CustomAppliance app = RegisterCustomAppliance<TestingTerminalAppliance>();
            d = app.ID;
        }
    }

    [HarmonyPatch(typeof(ProvideStartingEnvelopes), "OnUpdate")]
    class ProvideStartingEnvelopes_Patch
    {
        [HarmonyPostfix]
        static void Postfix(ProvideStartingEnvelopes __instance)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                SpawnUtils.SpawnApplianceBlueprint(Mod.d, 0f, -1);
            }
        }

    }
}