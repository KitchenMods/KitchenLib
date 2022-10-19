using KitchenData;
using KitchenLib.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomShop
	{
        public virtual string Name { get; internal set; }
        public virtual List<Appliance> Stock { get; internal set; }
        public virtual List<Decor> Decors { get; internal set; }
        public virtual ShopType Type { get; internal set; }
        public virtual int ItemsForSaleCount { get {return 3;} }
        public virtual int WallpapersForSaleCount { get {return 6;} }

        public virtual int ID { get; internal set; }
        public string ModName { get; internal set; }
        public virtual int BaseShopId { get { return -1; } }
        public Shop Shop{ get; internal set; }
        public virtual void OnRegister(Shop shop) { }
        public int GetHash() { return StringUtils.GetInt32HashCode($"{ModName}:{Name}"); }
    }
}