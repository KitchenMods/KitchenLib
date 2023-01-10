using KitchenData;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomDecor : CustomGameDataObject
    {
        public virtual Material Material { get; internal set; }
        public virtual Appliance ApplicatorAppliance { get; internal set; }
        public virtual LayoutMaterialType Type { get; internal set; }
        public virtual bool IsAvailable { get { return true; } }

        private static readonly Decor empty = ScriptableObject.CreateInstance<Decor>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Decor result = ScriptableObject.CreateInstance<Decor>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Decor>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (empty.ID != ID) result.ID = ID;
            if (empty.Material != Material) result.Material = Material;
            if (empty.Type != Type) result.Type = Type;
            if (empty.IsAvailable != IsAvailable) result.IsAvailable = IsAvailable;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameDataObject gameDataObject)
        {
            Decor result = (Decor)gameDataObject;

            if (empty.ApplicatorAppliance != ApplicatorAppliance) result.ApplicatorAppliance = ApplicatorAppliance;
        }
    }
}