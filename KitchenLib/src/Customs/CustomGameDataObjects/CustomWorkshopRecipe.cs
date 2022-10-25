using KitchenData;
using KitchenData.Workshop;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
    public abstract class CustomWorkshopRecipe : CustomGameDataObject
    {
		public virtual List<IWorkshopIndividualCondition> Conditions { get { return new List<IWorkshopIndividualCondition>(); } }
		public virtual List<IWorkshopGroupCondition> GroupConditions { get { return new List<IWorkshopGroupCondition>(); } }
		public virtual IWorkshopProduct Output { get; internal set; }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            WorkshopRecipe result = new WorkshopRecipe();
            WorkshopRecipe empty = new WorkshopRecipe();

            if (empty.ID != ID) result.ID = ID;
            if (empty.Conditions != Conditions) result.Conditions = Conditions;
            if (empty.GroupConditions != GroupConditions) result.GroupConditions = GroupConditions;
            if (empty.Output != Output) result.Output = Output;

            gameDataObject = result;
        }
    }
}