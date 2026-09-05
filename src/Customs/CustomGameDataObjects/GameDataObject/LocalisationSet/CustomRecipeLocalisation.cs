using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomRecipeLocalisation : CustomLocalisationSet<RecipeLocalisation, RecipeInfo>
	{
		#region Base Game Variables

		public virtual LocalisationObject<RecipeInfo> Info { get; protected set; } = new();

		#endregion

		#region KitchenLib Variables

		public virtual List<(Locale, RecipeInfo)> InfoList { get; protected set; } = new();

		#endregion

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not RecipeLocalisation recipeLocalisation) return;

			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref recipeLocalisation.Info);
			}
			else if (Info != null)
			{
				OverrideVariable(recipeLocalisation, "Info", Info);
			}
		}
	}
}