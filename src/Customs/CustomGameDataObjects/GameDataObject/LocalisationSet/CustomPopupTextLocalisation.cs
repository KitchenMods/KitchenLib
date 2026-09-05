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

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not PopupTextLocalisation popupTextLocalisation) return;

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref popupTextLocalisation.Info);
			}
			else if (Info != null)
			{
				OverrideVariable(popupTextLocalisation, "Info", Info);
			}
		}
	}
}