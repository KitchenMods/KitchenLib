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

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<ModularUnlockPack>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.Filter != Filter) result.Filter = Filter;
            if (result.Sorters != Sorters) result.Sorters = Sorters;
            if (result.ConditionalOptions != ConditionalOptions) result.ConditionalOptions = ConditionalOptions;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            ModularUnlockPack result = (ModularUnlockPack)gameDataObject;

			if (result.Sets != Sets) result.Sets = Sets;
        }
    }
}
