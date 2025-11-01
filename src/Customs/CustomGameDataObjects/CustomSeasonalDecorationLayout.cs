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
        public virtual ValueTuple<ValueTuple<int, int>, ValueTuple<int, int>> DateRange { get; protected set; } = new ValueTuple<ValueTuple<int, int>, ValueTuple<int, int>>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
	        SeasonalDecorationLayout result = ScriptableObject.CreateInstance<SeasonalDecorationLayout>();

			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "SeasonActive", SeasonActive);

			if (!Seasons.Dates.ContainsKey(SeasonActive))
			{
				Seasons.Dates.Add(SeasonActive, DateRange);
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