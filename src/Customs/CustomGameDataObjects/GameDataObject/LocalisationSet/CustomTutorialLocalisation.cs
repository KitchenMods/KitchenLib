using System.Collections.Generic;
using KitchenData;
using KitchenData.Localisations;

namespace KitchenLib.Customs
{
	public abstract class CustomTutorialLocalisation : CustomLocalisationSet<TutorialLocalisation, TutorialText>
	{
		#region Base Game Variables

		public virtual LocalisationObject<TutorialText> Info { get; protected set; } = new LocalisationObject<TutorialText>();
		
		#endregion
		
		#region KitchenLib Variables

		public virtual List<(Locale, TutorialText)> InfoList { get; protected set; } = new List<(Locale, TutorialText)>();

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is TutorialLocalisation tutorialLocalisation))
				return;

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref tutorialLocalisation.Info);
			}
			else
			{
				OverrideVariable(tutorialLocalisation, "Info", Info);
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (!(gameDataObject is TutorialLocalisation tutorialLocalisation))
				return;
		}
	}
}