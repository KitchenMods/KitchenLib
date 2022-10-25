using KitchenData;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
    public abstract class CustomGardenProfile : CustomGameDataObject
    {
		public virtual Appliance SpawnHolder { get; internal set; }
		public virtual List<Item> Spawns { get { return new List<Item>(); } }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            GardenProfile result = new GardenProfile();
            GardenProfile empty = new GardenProfile();
            
            if (empty.ID != ID) result.ID = ID;
            if (empty.SpawnHolder != SpawnHolder) result.SpawnHolder = SpawnHolder;
            if (empty.Spawns != Spawns) result.Spawns = Spawns;

            gameDataObject = result;
        }
    }
}