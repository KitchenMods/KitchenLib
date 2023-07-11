using KitchenData;
using KitchenData.Workshop;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomWorkshopRecipe : CustomGameDataObject<WorkshopRecipe>
    {
        public virtual List<IWorkshopIndividualCondition> Conditions { get; protected set; } = new List<IWorkshopIndividualCondition>();
        public virtual List<IWorkshopGroupCondition> GroupConditions { get; protected set; } = new List<IWorkshopGroupCondition>();
        public virtual IWorkshopProduct Output { get; protected set; }

        //private static readonly WorkshopRecipe empty = ScriptableObject.CreateInstance<WorkshopRecipe>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            WorkshopRecipe result = ScriptableObject.CreateInstance<WorkshopRecipe>();

			Main.LogDebug($"[CustomWorkshopRecipe.Convert] [1.1] Convering Base");

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<WorkshopRecipe>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.Conditions != Conditions) result.Conditions = Conditions;
            if (result.GroupConditions != GroupConditions) result.GroupConditions = GroupConditions;
            if (result.Output != Output) result.Output = Output;

            gameDataObject = result;
        }
    }
}