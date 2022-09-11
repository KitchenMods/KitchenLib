using UnityEngine;
using KitchenData;
using KitchenLib.Appliances;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace KitchenLib.TestMod
{
	public class SushiRoll : CustomItem
	{

		public override int BaseItemId
		{
			get { return 681117884; }
		}

		public override string Name
		{
			get { return "Sushi Roll"; }
		}
		
		public override List<Item.ItemProcess> DerivedProcesses
		{
			get { return new List<Item.ItemProcess> { GDOUtils.GetCustomItemProcess("ChopSushi") }; }
		}
		

		public override GameObject Prefab
		{
			get { return (GameObject)Mod.bundle.LoadAsset("Sushi Roll"); }
		}

		public override void OnRegister(Item item)
		{
			MaterialUtils.ApplyMaterial(item.Prefab, "Roll1", new Material[] { MaterialUtils.GetExistingMaterial("Plastic - Black"), MaterialUtils.GetExistingMaterial("Rice"), MaterialUtils.GetExistingMaterial("Knob"), MaterialUtils.GetExistingMaterial("Knob") });
			MaterialUtils.ApplyMaterial(item.Prefab, "Roll2", new Material[] { MaterialUtils.GetExistingMaterial("Plastic - Black"), MaterialUtils.GetExistingMaterial("Rice"), MaterialUtils.GetExistingMaterial("Knob"), MaterialUtils.GetExistingMaterial("Knob") });
		}

	}
}
