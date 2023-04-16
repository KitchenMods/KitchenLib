﻿using KitchenData;
using KitchenLib.Colorblind;
using KitchenLib.Customs;
using KitchenLib.Patches;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomRestaurantSetting : CustomGenericLocalisation<RestaurantSetting>
	{
		public virtual WeatherMode WeatherMode { get; protected set; }
		public virtual List<IDecorationConfiguration> Decorators { get; protected set; }
		public virtual UnlockPack UnlockPack { get; protected set; }
		public virtual Unlock StartingUnlock { get; protected set; }
		public virtual GameObject Prefab { get; protected set; }
		public virtual bool AlwaysLight { get; protected set; }
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			RestaurantSetting result = ScriptableObject.CreateInstance<RestaurantSetting>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<RestaurantSetting>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.WeatherMode != WeatherMode) result.WeatherMode = WeatherMode;
			if (result.Prefab != Prefab) result.Prefab = Prefab;
			if (result.AlwaysLight != AlwaysLight) result.AlwaysLight = AlwaysLight;
			if (result.Info != Info) result.Info = Info;

			if (InfoList.Count > 0)
			{
				result.Info = new LocalisationObject<BasicInfo>();
				foreach ((Locale, BasicInfo) info in InfoList)
					result.Info.Add(info.Item1, info.Item2);
			}

			gameDataObject = result;
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			RestaurantSetting result = (RestaurantSetting)GameDataObject;

			if (result.Decorators != Decorators) result.Decorators = Decorators;
			if (result.UnlockPack != UnlockPack) result.UnlockPack = UnlockPack;
			if (result.StartingUnlock != StartingUnlock) result.StartingUnlock = StartingUnlock;
		}
	}
}
