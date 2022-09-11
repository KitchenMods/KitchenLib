using System;
using UnityEngine;
using Kitchen;
using KitchenData;
using KitchenLib.Appliances;
using KitchenLib.Utils;
using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;

namespace KitchenLib.TestMod
{
	public class TestingTerminalAppliance : CustomAppliance
	{
		public override string Name
		{
			get { return "Sushi Provider"; }
		}

		public override int BaseApplianceId
		{
			get { return -13481890; }
		}
		
		public override List<IApplianceProperty> Properties
		{
			get { return new List<IApplianceProperty> { KitchenPropertiesUtils.GetUnlimitedCItemProvider(Mod.sushiRoll.ID) }; }
		}
    }
}
