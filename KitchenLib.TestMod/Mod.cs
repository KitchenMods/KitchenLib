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
}