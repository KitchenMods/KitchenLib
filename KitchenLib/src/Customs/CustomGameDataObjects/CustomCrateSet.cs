using KitchenData;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomCrateSet : CustomGameDataObject<CrateSet>
    {
	    // Base-Game Variables
        public virtual List<Appliance> Options { get; protected set; } = new List<Appliance>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            CrateSet result = ScriptableObject.CreateInstance<CrateSet>();

			OverrideVariable(result, "ID", ID);

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            CrateSet result = (CrateSet)gameDataObject;

			OverrideVariable(result, "Options", Options);
        }
    }
}