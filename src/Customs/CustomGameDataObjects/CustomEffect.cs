using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomEffect : CustomGameDataObject<Effect>
    {
	    // Base-Game Variables
        public virtual List<IEffectProperty> Properties { get; protected set; } = new List<IEffectProperty>();
        public virtual IEffectRange EffectRange { get; protected set; }
        public virtual IEffectCondition EffectCondition { get; protected set; }
        public virtual IEffectType EffectType { get; protected set; }
        public virtual EffectRepresentation EffectInformation { get; protected set; }
		
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Effect result = ScriptableObject.CreateInstance<Effect>();
            
            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "Properties", Properties);
            OverrideVariable(result, "EffectRange", EffectRange);
            OverrideVariable(result, "EffectCondition", EffectCondition);
            OverrideVariable(result, "EffectType", EffectType);

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Effect result = (Effect)gameDataObject;

			OverrideVariable(result, "EffectInformation", EffectInformation);
        }
    }
}