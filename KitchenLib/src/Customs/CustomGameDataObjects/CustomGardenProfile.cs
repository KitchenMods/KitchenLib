using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomGardenProfile : CustomGameDataObject<GardenProfile>
    {
        public virtual Appliance SpawnHolder { get; protected set; }
        public virtual List<GardenProfile.SpawnProbability> Spawns { get; protected set; } = new List<GardenProfile.SpawnProbability>();

        //private static readonly GardenProfile empty = ScriptableObject.CreateInstance<GardenProfile>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            GardenProfile result = ScriptableObject.CreateInstance<GardenProfile>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<GardenProfile>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            GardenProfile result = (GardenProfile)gameDataObject;

            if (result.SpawnHolder != SpawnHolder) result.SpawnHolder = SpawnHolder;
            if (result.Spawns != Spawns) result.Spawns = Spawns;
        }
    }
}