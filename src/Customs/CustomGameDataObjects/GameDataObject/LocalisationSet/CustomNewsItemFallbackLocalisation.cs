using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomNewsItemFallbackLocalisation : CustomLocalisationSet<NewsItemFallbackLocalisation, NewsItemFallbackInfo>
	{
		#region Base Game Variables

		public virtual LocalisationObject<NewsItemFallbackInfo> Info { get; protected set; } = new();

		#endregion

		#region KitchenLib Variables

		public virtual List<(Locale, NewsItemFallbackInfo)> InfoList { get; protected set; } = new();

		#endregion

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is NewsItemFallbackLocalisation newsItemFallbackLocalisation))
			{
				return;
			}

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref newsItemFallbackLocalisation.Info);
			}
			else
			{
				OverrideVariable(newsItemFallbackLocalisation, "Info", Info);
			}
		}
	}
}