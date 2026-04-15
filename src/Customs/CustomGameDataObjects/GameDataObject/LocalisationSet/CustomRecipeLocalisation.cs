using System.Collections.Generic;
using KitchenData;
using KitchenData.Localisations;

namespace KitchenLib.Customs
{
	public abstract class CustomRecipeLocalisation : CustomLocalisationSet<RecipeLocalisation, RecipeInfo>
	{
		#region Base Game Variables
		
		public virtual LocalisationObject<RecipeInfo> Info { get; protected set; }
		
		#endregion
		
		#region KitchenLib Variables

		public virtual List<(Locale, RecipeInfo)> InfoList { get; protected set; } = new();

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is RecipeLocalisation recipeLocalisation))
				return;

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref recipeLocalisation.Info);
			}
			else
			{
				OverrideVariable(recipeLocalisation, "Info", Info);
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (!(gameDataObject is RecipeLocalisation recipeLocalisation))
				return;
		}
	}
}