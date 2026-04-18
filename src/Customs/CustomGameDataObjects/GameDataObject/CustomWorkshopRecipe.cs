using System.Collections.Generic;
using KitchenData;
using KitchenData.Workshop;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomWorkshopRecipe : CustomGameDataObject<WorkshopRecipe>
	{
		#region Base Game Variables

		public virtual List<IWorkshopIndividualCondition> Conditions { get; protected set; } = new List<IWorkshopIndividualCondition>();
		public virtual List<IWorkshopGroupCondition> GroupConditions { get; protected set; } = new List<IWorkshopGroupCondition>();
		public virtual IWorkshopProduct Output { get; protected set; }
		
		#endregion
		
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is not WorkshopRecipe workshopRecipe)
				return;

			#region Apply Properties

			OverrideVariable(workshopRecipe, "Conditions", Conditions);
			OverrideVariable(workshopRecipe, "GroupConditions", GroupConditions);
			OverrideVariable(workshopRecipe, "Output", Output);
				
			#endregion
			
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not WorkshopRecipe workshopRecipe)
				return;
		}
	}
}