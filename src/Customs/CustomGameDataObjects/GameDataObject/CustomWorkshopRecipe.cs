using System.Collections.Generic;
using KitchenData;
using KitchenData.Workshop;

namespace KitchenLib.Customs
{
	public abstract class CustomWorkshopRecipe : CustomGameDataObject<WorkshopRecipe>
	{

		#region Base Game Variables

		public virtual List<IWorkshopIndividualCondition> Conditions { get; protected set; } = new();
		public virtual List<IWorkshopGroupCondition> GroupConditions { get; protected set; } = new();
		public virtual IWorkshopProduct Output { get; protected set; }

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is not WorkshopRecipe workshopRecipe)
			{
				return;
			}

			#region Apply Properties

			OverrideVariable(workshopRecipe, "Conditions", Conditions);
			OverrideVariable(workshopRecipe, "GroupConditions", GroupConditions);
			OverrideVariable(workshopRecipe, "Output", Output);

			#endregion
		}
	}
}