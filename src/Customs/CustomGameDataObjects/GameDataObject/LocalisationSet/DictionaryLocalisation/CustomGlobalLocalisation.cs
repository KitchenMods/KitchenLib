using System.Collections.Generic;
using KitchenData;
using TMPro;

namespace KitchenLib.Customs
{
	public abstract class CustomGlobalLocalisation : CustomDictionaryLocalisation
	{

		#region Base Game Variables

		public virtual Dictionary<DisplayedPatienceFactor, string> PatienceFactorIcons { get; protected set; } = new();
		public virtual Dictionary<PatienceReason, string> PatienceReasonIcons { get; protected set; } = new();
		public virtual Dictionary<DecorationType, string> DecorationIcons { get; protected set; } = new();
		public virtual NewsItemFallbackLocalisation NewsItemFallbackLocalisation { get; protected set; }
		public virtual StartDayWarningLocalisation StartDayWarningLocalisation { get; protected set; }
		public virtual PopupTextLocalisation PopupTextLocalisation { get; protected set; }
		public virtual RecipeLocalisation Recipes { get; protected set; }
		public virtual Dictionary<Font, TMP_FontAsset> Fonts { get; protected set; } = new();
		public virtual ControllerIcons ControllerIcons { get; protected set; }

		#endregion
		
		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not GlobalLocalisation globalLocalisation) return;
				
			#region Apply Properties

			OverrideVariable(globalLocalisation, "PatienceFactorIcons", PatienceFactorIcons);
			OverrideVariable(globalLocalisation, "PatienceReasonIcons", PatienceReasonIcons);
			OverrideVariable(globalLocalisation, "DecorationIcons", DecorationIcons);
			OverrideVariable(globalLocalisation, "Fonts", Fonts);
			OverrideVariable(globalLocalisation, "ControllerIcons", ControllerIcons);
			OverrideVariable(globalLocalisation, "NewsItemFallbackLocalisation", NewsItemFallbackLocalisation);
			OverrideVariable(globalLocalisation, "StartDayWarningLocalisation", StartDayWarningLocalisation);
			OverrideVariable(globalLocalisation, "PopupTextLocalisation", PopupTextLocalisation);
			OverrideVariable(globalLocalisation, "Recipes", Recipes);
			
			#endregion
		}
	}
}