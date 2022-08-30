using Kitchen;
using KitchenLib.Appliances;
using KitchenLib.Utils;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System;
using Unity.Entities;

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
    }

    [HarmonyPatch(typeof(ProvideStartingEnvelopes), "OnUpdate")]
    class ProvideStartingEnvelopes_Patch
    {
        public static bool complete = false;

        [HarmonyPrefix]
        static bool Prefix(ProvideStartingEnvelopes __instance)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Entity entity = PostHelpers.CreateBlueprintLetter(__instance.EntityManager, GameObject.Find("Player(Clone)").transform.position, Mod.d.ID, 0f, -1);

            }

            return false;
        }
    }
}