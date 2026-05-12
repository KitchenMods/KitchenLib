using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomSeasonalDecorationLayout : CustomGameDataObject<SeasonalDecorationLayout>
	{
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is not SeasonalDecorationLayout seasonalDecorationLayout)
			{
				return;
			}

			#region Apply Properties

			OverrideVariable(seasonalDecorationLayout, "SeasonActive", SeasonActive);

			#endregion
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not SeasonalDecorationLayout seasonalDecorationLayout)
			{
				return;
			}

			#region Apply Properties

			OverrideVariable(seasonalDecorationLayout, "Decorations", Decorations);
			OverrideVariable(seasonalDecorationLayout, "DecorOverrides", DecorOverrides);

			#endregion
		}

		#region Base Game Variables

		public virtual Season SeasonActive { get; protected set; }
		public virtual List<SeasonalDecorationLayout.Decoration> Decorations { get; protected set; } = new();
		public virtual List<SeasonalDecorationLayout.DecorOverride> DecorOverrides { get; protected set; } = new();

		#endregion
	}
}