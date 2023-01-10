using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomShop : CustomGameDataObject
    {
		public virtual List<Appliance> Stock { get { return new List<Appliance>(); } }
		public virtual List<Decor> Decors { get { return new List<Decor>(); } }
		public virtual ShopType Type { get; internal set; }
		public virtual int ItemsForSaleCount { get { return 3; } }
		public virtual int WallpapersForSaleCount { get { return 6; } }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Shop result = ScriptableObject.CreateInstance<Shop>();
			Shop empty = ScriptableObject.CreateInstance<Shop>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<Shop>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (empty.ID != ID) result.ID = ID;
            if (empty.Type != Type) result.Type = Type;
            if (empty.ItemsForSaleCount != ItemsForSaleCount) result.ItemsForSaleCount = ItemsForSaleCount;
            if (empty.WallpapersForSaleCount != WallpapersForSaleCount) result.WallpapersForSaleCount = WallpapersForSaleCount;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameDataObject gameDataObject)
        {
            Shop result = (Shop)gameDataObject;
            Shop empty = ScriptableObject.CreateInstance<Shop>();

            if (empty.Stock != Stock) result.Stock = Stock;
            if (empty.Decors != Decors) result.Decors = Decors;
        }
    }
}