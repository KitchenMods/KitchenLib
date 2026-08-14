using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomContract : CustomContractLocalisation
	{

		#region Base Game Variables

		public virtual RestaurantStatus Status { get; protected set; }
		public virtual float ExperienceMultiplier { get; protected set; } = 1;

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is Contract contract))
			{
				return;
			}

			#region Apply Properties

			OverrideVariable(contract, "Status", Status);
			OverrideVariable(contract, "ExperienceMultiplier", ExperienceMultiplier);

			#endregion
		}
	}
}