using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomUnlockCard : CustomUnlock<UnlockCard>
	{
		#region Base Game Variables

		public virtual List<UnlockEffect> Effects { get; protected set; } = new();

		#endregion

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is UnlockCard unlockCard)
			{
				#region Apply Properties

				OverrideVariable(unlockCard, "Effects", Effects);

				#endregion
			}
		}
	}
}