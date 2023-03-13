using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomCrateSet : CustomGameDataObject<CrateSet>
    {
        public virtual List<Appliance> Options { get; protected set; } = new List<Appliance>();

        //private static readonly CrateSet empty = ScriptableObject.CreateInstance<CrateSet>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            CrateSet result = ScriptableObject.CreateInstance<CrateSet>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<CrateSet>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            CrateSet result = (CrateSet)gameDataObject;

            if (result.Options != Options) result.Options = Options;
        }
    }
}