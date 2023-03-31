using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.src.Customs;
using KitchenLib.Utils;
using KitchenMods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenLib.src.Systems
{
	public class Spawn21Balloons : FranchiseFirstFrameSystem, IModSystem
	{
		protected override void OnUpdate()
		{
			if ((DateTime.Now.Month == 4) && (DateTime.Now.Day == 17) && (DateTime.Now.Year == 2023))
			{
				base.Create((Appliance)GDOUtils.GetCustomGameDataObject<_21_Balloon>().GameDataObject, new Vector3(-3f, 0f, -5f), Vector3.down);
				base.Create((Appliance)GDOUtils.GetCustomGameDataObject<_21_Balloon>().GameDataObject, new Vector3(1f, 0f, -5f), Vector3.down);
			}
		}
	}
}
