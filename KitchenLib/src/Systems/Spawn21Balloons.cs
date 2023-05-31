using Kitchen;
using KitchenData;
using KitchenLib.Customs.GDOs;
using KitchenLib.Utils;
using KitchenMods;
using System;
using UnityEngine;

namespace KitchenLib.Systems
{
	public class Spawn21Balloons : FranchiseFirstFrameSystem, IModSystem
	{
		protected override void OnUpdate()
		{
			if ((DateTime.Now.Month == 4) && (DateTime.Now.Day == 17) && (DateTime.Now.Year == 2023))
			{
				base.Create((Appliance)GDOUtils.GetCustomGameDataObject<_21Balloon>().GameDataObject, new Vector3(-3f, 0f, -5f), Vector3.down);
				base.Create((Appliance)GDOUtils.GetCustomGameDataObject<_21Balloon>().GameDataObject, new Vector3(1f, 0f, -5f), Vector3.down);
			}
		}
	}
}
