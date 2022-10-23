using KitchenData;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
    public abstract class CustomCrateSet : CustomGameDataObject
    {
        public virtual List<Appliance> Options { get { return new List<Appliance>(); } }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            CrateSet result = new CrateSet();
            CrateSet empty = new CrateSet();

            if (empty.ID != ID) result.ID = ID;
            if (empty.Options != Options) result.Options = Options;

            gameDataObject = result ;
        }
    }
}