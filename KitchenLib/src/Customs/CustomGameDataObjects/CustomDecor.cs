using KitchenData;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomDecor : CustomGameDataObject
    {
        public virtual Material Material { get; protected set; }
        public virtual Appliance ApplicatorAppliance { get; protected set; }
        public virtual LayoutMaterialType Type { get; protected set; }
        public virtual bool IsAvailable { get; protected set; } = true;

        //private static readonly Decor empty = ScriptableObject.CreateInstance<Decor>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Decor result = ScriptableObject.CreateInstance<Decor>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Decor>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.Material != Material) result.Material = Material;
            if (result.Type != Type) result.Type = Type;
            if (result.IsAvailable != IsAvailable) result.IsAvailable = IsAvailable;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Decor result = (Decor)gameDataObject;

            if (result.ApplicatorAppliance != ApplicatorAppliance) result.ApplicatorAppliance = ApplicatorAppliance;
        }
    }
}