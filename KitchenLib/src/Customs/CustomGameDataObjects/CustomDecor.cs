using KitchenData;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomDecor : CustomGameDataObject
    {
        public virtual Material Material { get; internal set; }
        public virtual Appliance ApplicatorAppliance { get; internal set; }
        public virtual LayoutMaterialType Type { get; internal set; }
        public virtual bool IsAvailable { get { return true; } }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Decor result = new Decor();
            Decor empty = new Decor();
            
            if (empty.ID != ID) result.ID = ID;
            if (empty.Material != Material) result.Material = Material;
            if (empty.ApplicatorAppliance != ApplicatorAppliance) result.ApplicatorAppliance = ApplicatorAppliance;
            if (empty.Type != Type) result.Type = Type;
            if (empty.IsAvailable != IsAvailable) result.IsAvailable = IsAvailable;

            gameDataObject = result;
        }
    }
}