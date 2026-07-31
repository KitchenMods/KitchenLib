using System;
using KitchenData;
using System.Collections.Generic;
using Kitchen;
using KitchenLib.References;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomSeasonalDecorationLayout : CustomGameDataObject<SeasonalDecorationLayout>
    {
	    // Base-Game Variables
        public virtual Season SeasonActive { get; protected set; }
        public virtual List<SeasonalDecorationLayout.Decoration> Decorations { get; protected set; } = new List<SeasonalDecorationLayout.Decoration>();
        public virtual List<SeasonalDecorationLayout.DecorOverride> DecorOverrides { get; protected set; } = new List<SeasonalDecorationLayout.DecorOverride>();
        
        // KitchenLib Variables
        public virtual List<ValueTuple<Season, ValueTuple<ValueTuple<int, int>, ValueTuple<int, int>>>> DateRange { get; protected set; } = new List<ValueTuple<Season, ValueTuple<ValueTuple<int, int>, ValueTuple<int, int>>>>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
	        SeasonalDecorationLayout result = ScriptableObject.CreateInstance<SeasonalDecorationLayout>();

			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "SeasonActive", SeasonActive);

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

			gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
	        SeasonalDecorationLayout result = (SeasonalDecorationLayout)gameDataObject;

			OverrideVariable(result, "Decorations", Decorations);
			OverrideVariable(result, "DecorOverrides", DecorOverrides);
        }
    }
}