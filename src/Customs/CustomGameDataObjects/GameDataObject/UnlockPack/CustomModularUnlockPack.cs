using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomModularUnlockPack : CustomUnlockPack<ModularUnlockPack>
	{

		#region Base Game Variables

		public virtual List<IUnlockSet> Sets { get; protected set; } = new();
		public virtual List<IUnlockFilter> Filter { get; protected set; } = new();
		public virtual List<IUnlockSorter> Sorters { get; protected set; } = new();
		public virtual List<ConditionalOptions> ConditionalOptions { get; protected set; } = new();

		#endregion
		
		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not ModularUnlockPack modularUnlockPack) return;

			#region Apply Properties

			OverrideVariable(modularUnlockPack, "Sets", Sets);
			OverrideVariable(modularUnlockPack, "Filter", Filter);
			OverrideVariable(modularUnlockPack, "Sorters", Sorters);
			OverrideVariable(modularUnlockPack, "ConditionalOptions", ConditionalOptions);

			#endregion
		}
	}
}