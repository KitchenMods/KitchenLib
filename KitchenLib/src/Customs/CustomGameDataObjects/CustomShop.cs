using KitchenData;
using System.Collections.Generic;

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
            Shop result = new Shop();
            Shop empty = new Shop();

            if (empty.ID != ID) result.ID = ID;
            if (empty.Stock != Stock) result.Stock = Stock;
            if (empty.Decors != Decors) result.Decors = Decors;
            if (empty.Type != Type) result.Type = Type;
            if (empty.ItemsForSaleCount != ItemsForSaleCount) result.ItemsForSaleCount = ItemsForSaleCount;
            if (empty.WallpapersForSaleCount != WallpapersForSaleCount) result.WallpapersForSaleCount = WallpapersForSaleCount;

            gameDataObject = result;
        }
    }
}