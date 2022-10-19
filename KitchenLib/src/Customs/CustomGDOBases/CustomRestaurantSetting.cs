using KitchenData;
using KitchenLib.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomRestaurantSetting
	{
        public virtual string Name { get; internal set; }
        public virtual WeatherMode WeatherMode { get; internal set; }
        public virtual List<IDecorationConfiguration> Decorators { get; internal set; }

        public virtual int ID { get; internal set; }
        public string ModName { get; internal set; }
        public virtual int BaseRestaurantSettingId { get { return -1; } }
        public RestaurantSetting RestaurantSetting{ get; internal set; }
        public virtual void OnRegister(RestaurantSetting restaurantSetting) { }
        public int GetHash() { return StringUtils.GetInt32HashCode($"{ModName}:{Name}"); }
    }
}