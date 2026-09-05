using System.Collections.Generic;
using Kitchen.Layouts;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomLayoutProfile : CustomLocalisedGameDataObject<LayoutProfile, BasicInfo>
	{

		#region Base Game Variables

		public virtual LayoutGraph Graph { get; protected set; }
		public virtual int MaximumTables { get; protected set; } = 3;
		public virtual List<GameDataObject> RequiredAppliances { get; protected set; } = new();
		public virtual GameDataObject Table { get; protected set; }
		public virtual GameDataObject Counter { get; protected set; }
		public virtual Appliance ExternalBin { get; protected set; }
		public virtual Appliance WallPiece { get; protected set; }
		public virtual Appliance InternalWallPiece { get; protected set; }
		public virtual Appliance StreetPiece { get; protected set; }
		public virtual Season FixedRunSeason { get; protected set; }

		#endregion

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not LayoutProfile layoutProfile) return;

			#region Apply Properties

			OverrideVariable(layoutProfile, "Graph", Graph);
			OverrideVariable(layoutProfile, "MaximumTables", MaximumTables);
			OverrideVariable(layoutProfile, "FixedRunSeason", FixedRunSeason);
			OverrideVariable(layoutProfile, "RequiredAppliances", RequiredAppliances);
			OverrideVariable(layoutProfile, "Table", Table);
			OverrideVariable(layoutProfile, "Counter", Counter);
			OverrideVariable(layoutProfile, "ExternalBin", ExternalBin);
			OverrideVariable(layoutProfile, "WallPiece", WallPiece);
			OverrideVariable(layoutProfile, "InternalWallPiece", InternalWallPiece);
			OverrideVariable(layoutProfile, "StreetPiece", StreetPiece);

			#endregion
		}
	}
}