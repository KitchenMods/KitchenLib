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

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not GardenProfile gardenProfile) return;

			#region Apply Properties

			OverrideVariable(gardenProfile, "SpawnHolder", SpawnHolder);
			OverrideVariable(gardenProfile, "Spawns", Spawns);

			#endregion
		}
	}
}