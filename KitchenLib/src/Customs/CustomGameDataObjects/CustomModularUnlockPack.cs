using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomModularUnlockPack : CustomUnlockPack<ModularUnlockPack>
    {
	    // Base-Game Variables
        public virtual List<IUnlockSet> Sets { get; protected set; } = new List<IUnlockSet>();
        public virtual List<IUnlockFilter> Filter { get; protected set; } = new List<IUnlockFilter>();
        public virtual List<IUnlockSorter> Sorters { get; protected set; } = new List<IUnlockSorter>();
        public virtual List<ConditionalOptions> ConditionalOptions { get; protected set; } = new List<ConditionalOptions>();

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            ModularUnlockPack result = ScriptableObject.CreateInstance<ModularUnlockPack>();
            
            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "Filter", Filter);
            OverrideVariable(result, "Sorters", Sorters);
            OverrideVariable(result, "ConditionalOptions", ConditionalOptions);

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            ModularUnlockPack result = (ModularUnlockPack)gameDataObject;

			OverrideVariable(result, "Sets", Sets);
        }
    }
}
