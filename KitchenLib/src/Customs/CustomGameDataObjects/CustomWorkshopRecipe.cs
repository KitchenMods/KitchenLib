using KitchenData;
using KitchenData.Workshop;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomWorkshopRecipe : CustomGameDataObject<WorkshopRecipe>
    {
	    // Base-Game Variables
        public virtual List<IWorkshopIndividualCondition> Conditions { get; protected set; } = new List<IWorkshopIndividualCondition>();
        public virtual List<IWorkshopGroupCondition> GroupConditions { get; protected set; } = new List<IWorkshopGroupCondition>();
        public virtual IWorkshopProduct Output { get; protected set; }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            WorkshopRecipe result = ScriptableObject.CreateInstance<WorkshopRecipe>();
            
            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "Conditions", Conditions);
            OverrideVariable(result, "GroupConditions", GroupConditions);
            OverrideVariable(result, "Output", Output);

            gameDataObject = result;
        }
    }
}