using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomTutorialLocalisation : CustomLocalisationSet<TutorialLocalisation, TutorialText>
	{
		#region Base Game Variables

		public virtual LocalisationObject<TutorialText> Info { get; protected set; } = new();

		#endregion

		#region KitchenLib Variables

		public virtual List<(Locale, TutorialText)> InfoList { get; protected set; } = new();

		#endregion
		
		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not TutorialLocalisation tutorialLocalisation) return;

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref tutorialLocalisation.Info);
			}
			else if (Info != null)
			{
				OverrideVariable(tutorialLocalisation, "Info", Info);
			}
		}
	}
}