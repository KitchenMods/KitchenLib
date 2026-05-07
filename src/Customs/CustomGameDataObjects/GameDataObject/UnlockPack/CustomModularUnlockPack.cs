using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomModularUnlockPack : CustomUnlockPack<ModularUnlockPack>
	{
		#region Base Game Variables

		public virtual List<IUnlockSet> Sets { get; protected set; } = new List<IUnlockSet>();
		public virtual List<IUnlockFilter> Filter { get; protected set; } = new List<IUnlockFilter>();
		public virtual List<IUnlockSorter> Sorters { get; protected set; } = new List<IUnlockSorter>();
		public virtual List<ConditionalOptions> ConditionalOptions { get; protected set; } = new List<ConditionalOptions>();
		
		#endregion
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);
			
			if (!(gameDataObject is ModularUnlockPack modularUnlockPack))
				return;
			
			#region Apply Properties

			OverrideVariable(modularUnlockPack, "Sets", Sets);
			OverrideVariable(modularUnlockPack, "Filter", Filter);
			OverrideVariable(modularUnlockPack, "Sorters", Sorters);
			OverrideVariable(modularUnlockPack, "ConditionalOptions", ConditionalOptions);
				
			#endregion
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);
			
			if (!(gameDataObject is ModularUnlockPack modularUnlockPack))
				return;
			
		}
	}
}