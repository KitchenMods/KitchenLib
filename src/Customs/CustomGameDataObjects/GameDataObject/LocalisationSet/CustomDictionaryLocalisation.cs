using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomDictionaryLocalisation : CustomLocalisationSet<DictionaryLocalisation, DictionaryInfo>
	{
		#region Base Game Variables

		public virtual LocalisationObject<DictionaryInfo> Info { get; protected set; } = new();

		#endregion

		#region KitchenLib Variables

		public virtual List<(Locale, DictionaryInfo)> InfoList { get; protected set; } = new();

		#endregion

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is DictionaryLocalisation dictionaryLocalisation))
			{
				return;
			}

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref dictionaryLocalisation.Info);
			}
			else
			{
				OverrideVariable(dictionaryLocalisation, "Info", Info);
			}
		}
	}
}