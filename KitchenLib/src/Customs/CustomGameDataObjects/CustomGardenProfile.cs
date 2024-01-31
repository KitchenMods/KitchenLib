using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomGardenProfile : CustomGameDataObject<GardenProfile>
    {
	    // Base-Game Variables
        public virtual Appliance SpawnHolder { get; protected set; }
        public virtual List<GardenProfile.SpawnProbability> Spawns { get; protected set; } = new List<GardenProfile.SpawnProbability>();
		
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            GardenProfile result = ScriptableObject.CreateInstance<GardenProfile>();
            
            OverrideVariable(result, "ID", ID);

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            GardenProfile result = (GardenProfile)gameDataObject;
            
            OverrideVariable(result, "SpawnHolder", SpawnHolder);
            OverrideVariable(result, "Spawns", Spawns);
        }
    }
}