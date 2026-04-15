using System.Collections.Generic;
using KitchenData;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomItemGroup : CustomItem<ItemGroup>
	{
		#region Base Game Variables
		
		public virtual List<ItemGroup.ItemSet> Sets { get; protected set; }
		public virtual bool CanContainSide { get; protected set; }
		public virtual bool ApplyProcessesToComponents { get; protected set; }
		public virtual bool AllowLooseComponentSplitting { get; protected set; }
		public virtual bool AutoCollapsing { get; protected set; }
		public virtual List<ItemGroup.ItemReward> Rewards { get; protected set; }
		
		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is ItemGroup itemGroup)
			{
				#region Apply Properties

				OverrideVariable(itemGroup, "CanContainSide", CanContainSide);
				OverrideVariable(itemGroup, "ApplyProcessesToComponents", ApplyProcessesToComponents);
				OverrideVariable(itemGroup, "AllowLooseComponentSplitting", AllowLooseComponentSplitting);
				OverrideVariable(itemGroup, "AutoCollapsing", AutoCollapsing);
				
				#endregion
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);
			
			if (gameDataObject is ItemGroup itemGroup)
			{
				#region Apply Properties

				OverrideVariable(itemGroup, "Sets", Sets);
				OverrideVariable(itemGroup, "Rewards", Rewards);
				
				#endregion
			}
		}
	}
}