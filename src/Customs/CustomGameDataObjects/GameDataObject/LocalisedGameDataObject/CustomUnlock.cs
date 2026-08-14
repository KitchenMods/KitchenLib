using System.Collections.Generic;
using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomUnlock<T> : CustomLocalisedGameDataObject<T, UnlockInfo> where T : GameDataObject
	{

		#region Base Game Variables

		public virtual Unlock.RewardLevel ExpReward { get; protected set; } = Unlock.RewardLevel.Medium;
		public virtual Factor ExpMult { get; protected set; } = 0;
		public virtual DishCustomerChange CustomerMultiplier { get; protected set; }
		public virtual bool IsUnlockable { get; protected set; } = true;
		public virtual bool HasSubOptions { get; protected set; }
		public virtual Unlock OptionCard1 { get; protected set; }
		public virtual Unlock OptionCard2 { get; protected set; }
		public virtual Unlock ParentOption { get; protected set; }
		public virtual UnlockGroup UnlockGroup { get; protected set; }
		public virtual CardType CardType { get; protected set; }
		public virtual int MinimumFranchiseTier { get; protected set; }
		public virtual bool IsSpecificFranchiseTier { get; protected set; }
		public virtual float SelectionBias { get; protected set; }
		public virtual List<Unlock> HardcodedRequirements { get; protected set; } = new();
		public virtual List<Unlock> HardcodedBlockers { get; protected set; } = new();
		public virtual bool BlocksAllOtherFood { get; protected set; }
		public virtual List<Unlock> AllowedFoods { get; protected set; } = new();
		public virtual RestaurantSetting ForceFranchiseSetting { get; protected set; }
		public virtual List<Unlock.ItemReward> ItemRewards { get; protected set; } = new();
		public virtual int ItemRHeatCardDisplayedNumberewards { get; protected set; }

		#endregion

		#region KitchenLib Variables

		public virtual string IconOverride { get; protected set; }
		public virtual Color ColourOverride { get; protected set; }

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is Unlock unlock)
			{
				#region Apply Properties

				OverrideVariable(unlock, "ExpReward", ExpReward);
				OverrideVariable(unlock, "ExpMult", ExpMult);
				OverrideVariable(unlock, "CustomerMultiplier", CustomerMultiplier);
				OverrideVariable(unlock, "IsUnlockable", IsUnlockable);
				OverrideVariable(unlock, "HasSubOptions", HasSubOptions);
				OverrideVariable(unlock, "UnlockGroup", UnlockGroup);
				OverrideVariable(unlock, "CardType", CardType);
				OverrideVariable(unlock, "MinimumFranchiseTier", MinimumFranchiseTier);
				OverrideVariable(unlock, "IsSpecificFranchiseTier", IsSpecificFranchiseTier);
				OverrideVariable(unlock, "SelectionBias", SelectionBias);
				OverrideVariable(unlock, "BlocksAllOtherFood", BlocksAllOtherFood);
				OverrideVariable(unlock, "ItemRHeatCardDisplayedNumberewards", ItemRHeatCardDisplayedNumberewards);

				#endregion

				if (!string.IsNullOrEmpty(IconOverride))
				{
					Main.LogDebug($"Assigning : {IconOverride} >> IconOverride");
					UnlockOverrides.AddIconOverride(unlock.ID, IconOverride);
				}

				if (ColourOverride != new Color())
				{
					Main.LogDebug($"Assigning : {ColourOverride} >> ColourOverride");
					UnlockOverrides.AddColourOverride(unlock.ID, ColourOverride);
				}
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is Unlock unlock)
			{
				#region Apply Properties

				OverrideVariable(unlock, "OptionCard1", OptionCard1);
				OverrideVariable(unlock, "OptionCard2", OptionCard2);
				OverrideVariable(unlock, "ParentOption", ParentOption);
				OverrideVariable(unlock, "HardcodedRequirements", HardcodedRequirements);
				OverrideVariable(unlock, "HardcodedBlockers", HardcodedBlockers);
				OverrideVariable(unlock, "AllowedFoods", AllowedFoods);
				OverrideVariable(unlock, "ForceFranchiseSetting", ForceFranchiseSetting);
				OverrideVariable(unlock, "ItemRewards", ItemRewards);

				#endregion
			}
		}
	}
}