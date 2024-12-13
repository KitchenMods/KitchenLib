using KitchenData;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomCompositeUnlockPack : CustomUnlockPack<CompositeUnlockPack>
    {
	    // Base-Game Variables
        public virtual List<UnlockPack> Packs { get; protected set; } = new List<UnlockPack>();

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            CompositeUnlockPack result = ScriptableObject.CreateInstance<CompositeUnlockPack>();
			
            OverrideVariable(result, "ID", ID);

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            CompositeUnlockPack result = (CompositeUnlockPack)gameDataObject;
			
			OverrideVariable(result, "Packs", Packs);
        }
    }
}
