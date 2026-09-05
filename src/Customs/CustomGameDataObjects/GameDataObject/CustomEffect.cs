using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomEffect : CustomGameDataObject<Effect>
	{

		#region Base Game Variables

		public virtual List<IEffectProperty> Properties { get; protected set; } = new();
		public virtual IEffectRange EffectRange { get; protected set; }
		public virtual IEffectCondition EffectCondition { get; protected set; }
		public virtual IEffectType EffectType { get; protected set; }
		public virtual EffectRepresentation EffectInformation { get; protected set; }

		#endregion

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not Effect effect) return;

			#region Apply Properties
				
			OverrideVariable(effect, "Properties", Properties);
			OverrideVariable(effect, "EffectRange", EffectRange);
			OverrideVariable(effect, "EffectCondition", EffectCondition);
			OverrideVariable(effect, "EffectType", EffectType);
			OverrideVariable(effect, "EffectInformation", EffectInformation);

			#endregion
		}
	}
}