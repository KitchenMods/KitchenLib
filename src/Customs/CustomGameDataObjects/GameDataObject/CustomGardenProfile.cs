using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomGardenProfile : CustomGameDataObject<GardenProfile>
	{
		#region Base Game Variables
		
		public virtual Appliance SpawnHolder { get; protected set; }
		public virtual List<GardenProfile.SpawnProbability> Spawns { get; protected set; } = new();
		
		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is GardenProfile gardenProfile)
			{
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);
			
			if (gameDataObject is GardenProfile gardenProfile)
			{
				#region Apply Properties

				OverrideVariable(gardenProfile, "SpawnHolder", SpawnHolder);
				OverrideVariable(gardenProfile, "Spawns", Spawns);
				
				#endregion
			}
		}
	}
}