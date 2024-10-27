using System;
using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomRestaurantSetting : CustomGenericLocalisation<RestaurantSetting>
	{
		// Base-Game Variables
		public virtual WeatherMode WeatherMode { get; protected set; }
		public virtual List<IDecorationConfiguration> Decorators { get; protected set; }
		public virtual UnlockPack UnlockPack { get; protected set; }
		public virtual Unlock StartingUnlock { get; protected set; }
		public virtual Dish FixedDish { get; protected set; }
		public virtual GameObject Prefab { get; protected set; }
		public virtual LayoutProfile ForceLayout { get; protected set; }
		public virtual bool AlwaysLight { get; protected set; }
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			RestaurantSetting result = ScriptableObject.CreateInstance<RestaurantSetting>();

			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "WeatherMode", WeatherMode);
			OverrideVariable(result, "Prefab", Prefab);
			OverrideVariable(result, "AlwaysLight", AlwaysLight);
			OverrideVariable(result, "Info", Info);

			if (InfoList.Count > 0)
			{
				SetupLocalisation<BasicInfo>(InfoList, ref result.Info);
			}

			gameDataObject = result;
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			RestaurantSetting result = (RestaurantSetting)GameDataObject;

			OverrideVariable(result, "Decorators", Decorators);
			OverrideVariable(result, "UnlockPack", UnlockPack);
			OverrideVariable(result, "StartingUnlock", StartingUnlock);
			OverrideVariable(result, "FixedDish", FixedDish);
			OverrideVariable(result, "ForceLayout", ForceLayout);
		}
	}
}
