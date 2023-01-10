using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomCrateSet : CustomGameDataObject
    {
        public virtual List<Appliance> Options { get { return new List<Appliance>(); } }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            CrateSet result = ScriptableObject.CreateInstance<CrateSet>();
			CrateSet empty = ScriptableObject.CreateInstance<CrateSet>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<CrateSet>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (empty.ID != ID) result.ID = ID;

            gameDataObject = result ;
        }

        public override void AttachDependentProperties(GameDataObject gameDataObject)
        {
            CrateSet result = (CrateSet)gameDataObject;
            CrateSet empty = ScriptableObject.CreateInstance<CrateSet>();

            if (empty.Options != Options) result.Options = Options;
        }
    }
}