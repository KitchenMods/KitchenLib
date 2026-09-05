using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomResearch : CustomLocalisedGameDataObject<Research, ResearchLocalisation>
	{

		#region Base Game Variables

		public virtual List<IUpgrade> Rewards { get; protected set; } = new();
		public virtual List<Research> EnablesResearchOf { get; protected set; } = new();
		public virtual List<Research> RequiresForResearch { get; protected set; } = new();

		#endregion

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not Research research) return;

			#region Apply Properties

			OverrideVariable(research, "Rewards", Rewards);
			OverrideVariable(research, "EnablesResearchOf", EnablesResearchOf);
			OverrideVariable(research, "RequiresForResearch", RequiresForResearch);

			#endregion
		}
	}
}