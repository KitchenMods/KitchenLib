using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
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

			Main.LogDebug($"[CustomRestaurantSetting.Convert] [1.1] Converting Base");

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<GlobalLocalisation>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.Info != Info) result.Info = Info;
			if (result.Text != Text) result.Text = Text;
			
			if (result.PatienceFactorIcons != PatienceFactorIcons) result.PatienceFactorIcons = PatienceFactorIcons;
			if (result.PatienceReasonIcons != PatienceReasonIcons) result.PatienceReasonIcons = PatienceReasonIcons;
			if (result.DecorationIcons != DecorationIcons) result.DecorationIcons = DecorationIcons;
			if (result.NewsItemFallbackLocalisation != NewsItemFallbackLocalisation) result.NewsItemFallbackLocalisation = NewsItemFallbackLocalisation;
			if (result.StartDayWarningLocalisation != StartDayWarningLocalisation) result.StartDayWarningLocalisation = StartDayWarningLocalisation;
			if (result.PopupTextLocalisation != PopupTextLocalisation) result.PopupTextLocalisation = PopupTextLocalisation;
			if (result.Recipes != Recipes) result.Recipes = Recipes;
			if (result.Fonts != Fonts) result.Fonts = Fonts;
			if (result.ControllerIcons != ControllerIcons) result.ControllerIcons = ControllerIcons;
			
			gameDataObject = result;
		}
	}
}
