using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomCrateSet : CustomGameDataObject<CrateSet>
	{
		#region Base Game Variables

		public virtual List<Appliance> Options { get; protected set; } = new List<Appliance>();
		
		#endregion
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is CrateSet crateSet))
				return;
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);
			
			if (!(gameDataObject is CrateSet crateSet))
				return;
			
			#region Apply Properties

				OverrideVariable(crateSet, "Options", Options);
				
				#endregion
			
		}
	}
}