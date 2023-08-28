using KitchenData;
using System.Collections.Generic;
using System.Linq;
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

			Main.LogDebug($"[CustomCrateSet.Convert] [1.1] Converting Base");

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<CrateSet>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            CrateSet result = (CrateSet)gameDataObject;

			Main.LogDebug($"[CustomCrateSet.AttachDependentProperties] [1.1] Converting Base");

			if (result.Options != Options) result.Options = Options;
        }
    }
}