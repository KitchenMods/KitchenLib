using KitchenData;
using KitchenData.Workshop;
using System.Collections.Generic;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class WorkshopRecipeDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder workshopRecipeDump = new StringBuilder();
			StringBuilder workshopRecipeConditionsDump = new StringBuilder();
			StringBuilder workshopRecipeGroupConditionsDump = new StringBuilder();

			workshopRecipeDump.AppendLine("ID,Type,Output");
			workshopRecipeConditionsDump.AppendLine("ID,Type,IWorkshopIndividualCondition");
			workshopRecipeGroupConditionsDump.AppendLine("ID,Type,Group,IWorkshopGroupCondition");

			foreach (WorkshopRecipe workshopRecipe in GameData.Main.Get<WorkshopRecipe>())
			{
				workshopRecipeDump.AppendLine($"{workshopRecipe.ID},{workshopRecipe.name},{workshopRecipe.Output}");

				foreach (IWorkshopIndividualCondition condition in workshopRecipe.Conditions)
				{
					workshopRecipeConditionsDump.AppendLine($"{workshopRecipe.ID},{workshopRecipe.name},{condition}");
				}

				foreach (IWorkshopGroupCondition condition in workshopRecipe.GroupConditions)
				{
					workshopRecipeGroupConditionsDump.AppendLine($"{workshopRecipe.ID},{workshopRecipe.name},{condition}");
				}
			}

			SaveCSV("WorkshopRecipe", "WorkshopRecipes", workshopRecipeDump);
			SaveCSV("WorkshopRecipe", "WorkshopRecipeConditions", workshopRecipeConditionsDump);
			SaveCSV("WorkshopRecipe", "WorkshopRecipeGroupConditions", workshopRecipeGroupConditionsDump);
		}
	}
}
