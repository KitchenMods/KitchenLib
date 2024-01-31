using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Font = KitchenData.Font;

namespace KitchenLib.Customs
{
	public abstract class CustomGlobalLocalisation : CustomDictionaryLocalisation<GlobalLocalisation>
	{
		// Base-Game Variables
		public virtual Dictionary<DisplayedPatienceFactor, string> PatienceFactorIcons { get; protected set; } = new Dictionary<DisplayedPatienceFactor, string>();
		public virtual Dictionary<PatienceReason, string> PatienceReasonIcons { get; protected set; } = new Dictionary<PatienceReason, string>();
		public virtual Dictionary<DecorationType, string> DecorationIcons { get; protected set; } = new Dictionary<DecorationType, string>();
		public virtual NewsItemFallbackLocalisation NewsItemFallbackLocalisation { get; protected set; }
		public virtual StartDayWarningLocalisation StartDayWarningLocalisation { get; protected set; }
		public virtual PopupTextLocalisation PopupTextLocalisation { get; protected set; }
		public virtual RecipeLocalisation Recipes { get; protected set; }

		public virtual Dictionary<KitchenData.Font, TMP_FontAsset> Fonts { get; protected set; } = new Dictionary<Font, TMP_FontAsset>();
		public virtual ControllerIcons ControllerIcons { get; protected set; }

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			GlobalLocalisation result = ScriptableObject.CreateInstance<GlobalLocalisation>();
			
			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "Info", Info);
			OverrideVariable(result, "Text", Text);
			OverrideVariable(result, "PatienceFactorIcons", PatienceFactorIcons);
			OverrideVariable(result, "PatienceReasonIcons", PatienceReasonIcons);
			OverrideVariable(result, "DecorationIcons", DecorationIcons);
			OverrideVariable(result, "NewsItemFallbackLocalisation", NewsItemFallbackLocalisation);
			OverrideVariable(result, "StartDayWarningLocalisation", StartDayWarningLocalisation);
			OverrideVariable(result, "PopupTextLocalisation", PopupTextLocalisation);
			OverrideVariable(result, "Recipes", Recipes);
			OverrideVariable(result, "Fonts", Fonts);
			OverrideVariable(result, "ControllerIcons", ControllerIcons);
			
			gameDataObject = result;
		}
	}
}
