using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomStartDayWarningLocalisation : CustomLocalisationSet<StartDayWarningLocalisation, StartDayWarningInfo>
	{
		#region Base Game Variables

		public virtual LocalisationObject<StartDayWarningInfo> Info { get; protected set; } = new();

		#endregion

		#region KitchenLib Variables

		public virtual List<(Locale, StartDayWarningInfo)> InfoList { get; protected set; } = new();

		#endregion

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is StartDayWarningLocalisation startDayWarningLocalisation))
			{
				return;
			}

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref startDayWarningLocalisation.Info);
			}
			else
			{
				OverrideVariable(startDayWarningLocalisation, "Info", Info);
			}
		}
	}
}