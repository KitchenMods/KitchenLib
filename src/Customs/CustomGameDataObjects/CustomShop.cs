using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomShop : CustomGameDataObject<Shop>
    {
	    // Base-Game Variables
        public virtual List<Appliance> Stock { get; protected set; } = new List<Appliance>();
        public virtual List<Decor> Decors { get; protected set; } = new List<Decor>();
        public virtual ShopType Type { get; protected set; }
        public virtual int ItemsForSaleCount { get; protected set; } = 3;
        public virtual int WallpapersForSaleCount { get; protected set; } = 6;
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Shop result = ScriptableObject.CreateInstance<Shop>();

            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "Type", Type);
            OverrideVariable(result, "ItemsForSaleCount", ItemsForSaleCount);
            OverrideVariable(result, "WallpapersForSaleCount", WallpapersForSaleCount);
            
            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Shop result = (Shop)gameDataObject;

            OverrideVariable(result, "Stock", Stock);
            OverrideVariable(result, "Decors", Decors);
        }
    }
}