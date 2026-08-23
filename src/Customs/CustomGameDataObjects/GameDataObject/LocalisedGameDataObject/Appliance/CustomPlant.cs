using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomPlant : CustomLocalisedGameDataObject<Plant,ApplianceInfo>
	{
		#region Base Game Variables

		public virtual bool IsSeedling { get; protected set; }
		public virtual List<IPlantProperty> PlantProperties { get; protected set; } = new List<IPlantProperty>();

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is Plant plant)
			{
				#region Apply Properties

				OverrideVariable(plant, "IsSeedling", IsSeedling);
				OverrideVariable(plant, "PlantProperties", PlantProperties);

				#endregion
				
			}
		}
	}
}