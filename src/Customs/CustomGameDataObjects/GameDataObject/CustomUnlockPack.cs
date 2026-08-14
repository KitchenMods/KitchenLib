using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomUnlockPack<T> : CustomGameDataObject<T> where T : UnlockPack
	{

		#region Base Game Variables

		public virtual bool OverrideIconToBeHeat { get; protected set; }

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is not CustomUnlockPack<T> customUnlockPack)
			{
				return;
			}

			#region Apply Properties

			OverrideVariable(customUnlockPack, "OverrideIconToBeHeat", OverrideIconToBeHeat);

			#endregion
		}
	}
}