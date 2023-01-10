using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomUnlockCard : CustomUnlock
    {
		public virtual List<UnlockEffect> Effects { get { return new List<UnlockEffect>(); } }
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            UnlockCard result = ScriptableObject.CreateInstance<UnlockCard>();
			UnlockCard empty = ScriptableObject.CreateInstance<UnlockCard>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<UnlockCard>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (empty.ID != ID) result.ID = ID;
            if (empty.Effects != Effects) result.Effects = Effects;

			if (empty.ExpReward != ExpReward) result.ExpReward = ExpReward;
			if (empty.IsUnlockable != IsUnlockable) result.IsUnlockable = IsUnlockable;
			if (empty.UnlockGroup != UnlockGroup) result.UnlockGroup = UnlockGroup;
			if (empty.CardType != CardType) result.CardType = CardType;
			if (empty.MinimumFranchiseTier != MinimumFranchiseTier) result.MinimumFranchiseTier = MinimumFranchiseTier;
			if (empty.IsSpecificFranchiseTier != IsSpecificFranchiseTier) result.IsSpecificFranchiseTier = IsSpecificFranchiseTier;
			if (empty.CustomerMultiplier != CustomerMultiplier) result.CustomerMultiplier = CustomerMultiplier;
			if (empty.SelectionBias != SelectionBias) result.SelectionBias = SelectionBias;
			
			if (empty.Info != Info) result.Info = Info;

			gameDataObject = result;
        }

        public override void AttachDependentProperties(GameDataObject gameDataObject)
		{
			UnlockCard result = (UnlockCard)gameDataObject;
			UnlockCard empty = ScriptableObject.CreateInstance<UnlockCard>();

			FieldInfo hardcodedRequirements = ReflectionUtils.GetField<Unlock>("HardcodedRequirements");
			FieldInfo hardcodedBlockers = ReflectionUtils.GetField<Unlock>("HardcodedBlockers");

			if (hardcodedRequirements.GetValue(empty) != HardcodedRequirements) hardcodedRequirements.SetValue(result, HardcodedRequirements);
			if (hardcodedBlockers.GetValue(empty) != HardcodedBlockers) hardcodedBlockers.SetValue(result, HardcodedBlockers);
		}
    }
}