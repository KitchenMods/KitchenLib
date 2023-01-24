using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomModularUnlockPack : CustomUnlockPack
    {

        public virtual List<IUnlockSet> Sets { get; protected set; } = new List<IUnlockSet>();
        public virtual List<IUnlockFilter> Filter { get; protected set; } = new List<IUnlockFilter>();
        public virtual List<IUnlockSorter> Sorters { get; protected set; } = new List<IUnlockSorter>();
        public virtual List<ConditionalOptions> ConditionalOptions { get; protected set; } = new List<ConditionalOptions>();

        private static readonly ModularUnlockPack empty = ScriptableObject.CreateInstance<ModularUnlockPack>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            ModularUnlockPack result = ScriptableObject.CreateInstance<ModularUnlockPack>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<ModularUnlockPack>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (empty.ID != ID) result.ID = ID;
            if (empty.Filter != Filter) result.Filter = Filter;
            if (empty.Sorters != Sorters) result.Sorters = Sorters;
            if (empty.ConditionalOptions != ConditionalOptions) result.ConditionalOptions = ConditionalOptions;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            ModularUnlockPack result = (ModularUnlockPack)gameDataObject;

            if (empty.Sets != Sets) result.Sets = Sets;
        }
    }
}
