using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomHeatLevels : CustomGameDataObject<HeatLevels>
	{

		#region Base Game Variables

		public virtual bool IsDefaultLevels { get; protected set; }
		public virtual List<ICard> Cards { get; protected set; } = new List<ICard>();
		public virtual List<ICard> ChillCards { get; protected set; } = new List<ICard>();

		#endregion

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not HeatLevels heatLevels) return;

			#region Apply Properties

			OverrideVariable(heatLevels, "IsDefaultLevels", IsDefaultLevels);
			OverrideVariable(heatLevels, "Cards", Cards);
			OverrideVariable(heatLevels, "ChillCards", ChillCards);

			#endregion
		}
	}
}