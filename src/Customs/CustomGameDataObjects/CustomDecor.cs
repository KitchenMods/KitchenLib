using KitchenData;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomDecor : CustomGameDataObject<Decor>
    {
	    // Base-Game Variables
        public virtual Material Material { get; protected set; }
        public virtual Appliance ApplicatorAppliance { get; protected set; }
        public virtual LayoutMaterialType Type { get; protected set; }
        public virtual bool IsAvailable { get; protected set; } = true;
		
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Decor result = ScriptableObject.CreateInstance<Decor>();
            
            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "Material", Material);
            OverrideVariable(result, "Type", Type);
            OverrideVariable(result, "IsAvailable", IsAvailable);

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Decor result = (Decor)gameDataObject;

			OverrideVariable(result, "ApplicatorAppliance", ApplicatorAppliance);
        }
    }
}