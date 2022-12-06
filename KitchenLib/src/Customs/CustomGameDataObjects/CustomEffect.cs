using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomEffect : CustomGameDataObject
    {
        public virtual List<IEffectProperty> Properties { get { return new List<IEffectProperty>(); } }
        public virtual IEffectRange EffectRange { get; internal set; }
        public virtual IEffectCondition EffectCondition { get; internal set; }
        public virtual IEffectType EffectType { get; internal set; }
        public virtual EffectRepresentation EffectInformation { get; internal set; }


        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Effect result = ScriptableObject.CreateInstance<Effect>();
			Effect empty = ScriptableObject.CreateInstance<Effect>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<Effect>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (empty.ID != ID) result.ID = ID;
            if (empty.Properties != Properties) result.Properties = Properties;
            if (empty.EffectRange != EffectRange) result.EffectRange = EffectRange;
            if (empty.EffectCondition != EffectCondition) result.EffectCondition = EffectCondition;
            if (empty.EffectType != EffectType) result.EffectType = EffectType;
            if (empty.EffectInformation != EffectInformation) result.EffectInformation = EffectInformation;

            gameDataObject = result;
        }
    }
}