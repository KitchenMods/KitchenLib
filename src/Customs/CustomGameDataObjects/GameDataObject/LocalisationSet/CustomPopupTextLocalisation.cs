using System.Collections.Generic;
using KitchenData;
using KitchenData.Localisations;

namespace KitchenLib.Customs
{
	public abstract class CustomPopupTextLocalisation : CustomLocalisationSet<PopupTextLocalisation, PopupText>
	{
		#region Base Game Variables

		public virtual LocalisationObject<PopupText> Info { get; protected set; } = new LocalisationObject<PopupText>();
		
		#endregion
		
		#region KitchenLib Variables

		public virtual List<(Locale, PopupText)> InfoList { get; protected set; } = new List<(Locale, PopupText)>();

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is PopupTextLocalisation popupTextLocalisation))
				return;

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref popupTextLocalisation.Info);
			}
			else
			{
				OverrideVariable(popupTextLocalisation, "Info", Info);
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (!(gameDataObject is PopupTextLocalisation popupTextLocalisation))
				return;
		}
	}
}