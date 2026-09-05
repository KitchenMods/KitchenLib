using System;
using System.Collections.Generic;
using Kitchen;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomSeasonalDecorationLayout : CustomGameDataObject<SeasonalDecorationLayout>
	{
		
		#region Base Game Variables

		public virtual Season SeasonActive { get; protected set; }
		public virtual List<SeasonalDecorationLayout.Decoration> Decorations { get; protected set; } = new();
		public virtual List<SeasonalDecorationLayout.DecorOverride> DecorOverrides { get; protected set; } = new();

		#endregion

		#region KitchenLib Variables

		public virtual List<ValueTuple<Season, ValueTuple<ValueTuple<int, int>, ValueTuple<int, int>>>> DateRange { get; protected set; } = new List<ValueTuple<Season, ValueTuple<ValueTuple<int, int>, ValueTuple<int, int>>>>();

		#endregion

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not SeasonalDecorationLayout seasonalDecorationLayout) return;

			#region Apply Properties

			OverrideVariable(seasonalDecorationLayout, "Decorations", Decorations);
			OverrideVariable(seasonalDecorationLayout, "DecorOverrides", DecorOverrides);
			OverrideVariable(seasonalDecorationLayout, "SeasonActive", SeasonActive);
			
			#endregion
			
			bool AddToDates = true;
			
			foreach (ValueTuple<Season, ValueTuple<ValueTuple<int, int>, ValueTuple<int, int>>> tuple in Seasons.Dates)
			{
				if (tuple.Item1 == SeasonActive)
				{
					AddToDates = false;
					break;
				}
			}

			if (AddToDates)
			{
				Seasons.Dates.AddRange(DateRange);
			}
		}
	}
}