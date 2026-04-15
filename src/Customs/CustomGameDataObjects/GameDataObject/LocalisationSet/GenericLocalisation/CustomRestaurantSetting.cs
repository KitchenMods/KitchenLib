using System.Collections.Generic;
using KitchenData;
using TMPro;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomRestaurantSetting : CustomGenericLocalisation<RestaurantSetting>
	{
		#region Base Game Variables
		
		public virtual WeatherMode WeatherMode { get; protected set; }
		public virtual List<IDecorationConfiguration> Decorators { get; protected set; }
		public virtual UnlockPack UnlockPack { get; protected set; }
		public virtual Unlock StartingUnlock { get; protected set; }
		public virtual Dish FixedDish { get; protected set; }
		public virtual GameObject Prefab { get; protected set; }
		public virtual LayoutProfile ForceLayout { get; protected set; }
		public virtual bool AlwaysLight { get; protected set; }
		public virtual Season FixedRunSeason { get; protected set; }
		
		#endregion
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is RestaurantSetting restaurantSetting))
				return;
			
			#region Apply Properties

			OverrideVariable(restaurantSetting, "WeatherMode", WeatherMode);
			OverrideVariable(restaurantSetting, "Prefab", Prefab);
			OverrideVariable(restaurantSetting, "AlwaysLight", AlwaysLight);
			OverrideVariable(restaurantSetting, "FixedRunSeason", FixedRunSeason);
				
			#endregion
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);
			
			if (!(gameDataObject is RestaurantSetting restaurantSetting))
				return;
			
			#region Apply Properties

			OverrideVariable(restaurantSetting, "Decorators", Decorators);
			OverrideVariable(restaurantSetting, "UnlockPack", UnlockPack);
			OverrideVariable(restaurantSetting, "StartingUnlock", StartingUnlock);
			OverrideVariable(restaurantSetting, "FixedDish", FixedDish);
			OverrideVariable(restaurantSetting, "ForceLayout", ForceLayout);
				
			#endregion
		}
	}
}