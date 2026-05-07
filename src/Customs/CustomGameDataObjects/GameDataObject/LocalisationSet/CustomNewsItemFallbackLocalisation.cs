using System.Collections.Generic;
using KitchenData;
using KitchenData.Localisations;

namespace KitchenLib.Customs
{
	public abstract class CustomNewsItemFallbackLocalisation : CustomLocalisationSet<NewsItemFallbackLocalisation, NewsItemFallbackInfo>
	{
		#region Base Game Variables

		public virtual LocalisationObject<NewsItemFallbackInfo> Info { get; protected set; } = new LocalisationObject<NewsItemFallbackInfo>();
		
		#endregion
		
		#region KitchenLib Variables

		public virtual List<(Locale, NewsItemFallbackInfo)> InfoList { get; protected set; } = new List<(Locale, NewsItemFallbackInfo)>();

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is NewsItemFallbackLocalisation newsItemFallbackLocalisation))
				return;

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref newsItemFallbackLocalisation.Info);
			}
			else
			{
				OverrideVariable(newsItemFallbackLocalisation, "Info", Info);
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (!(gameDataObject is NewsItemFallbackLocalisation newsItemFallbackLocalisation))
				return;
		}
	}
}