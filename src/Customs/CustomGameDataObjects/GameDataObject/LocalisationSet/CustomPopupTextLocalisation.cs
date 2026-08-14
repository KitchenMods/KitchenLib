using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomPopupTextLocalisation : CustomLocalisationSet<PopupTextLocalisation, PopupText>
	{
		#region Base Game Variables

		public virtual LocalisationObject<PopupText> Info { get; protected set; } = new();

		#endregion

		#region KitchenLib Variables

		public virtual List<(Locale, PopupText)> InfoList { get; protected set; } = new();

		#endregion

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is PopupTextLocalisation popupTextLocalisation))
			{
				return;
			}

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref popupTextLocalisation.Info);
			}
			else
			{
				OverrideVariable(popupTextLocalisation, "Info", Info);
			}
		}
	}
}