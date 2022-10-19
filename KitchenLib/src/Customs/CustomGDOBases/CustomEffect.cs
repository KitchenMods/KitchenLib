using KitchenData;
using KitchenLib.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomEffect
	{
        public virtual string Name { get; internal set; }
        public virtual List<IEffectProperty> Properties { get; internal set; }
        public virtual IEffectRange EffectRange { get; internal set; }
        public virtual IEffectCondition EffectCondition { get; internal set; }
        public virtual IEffectType EffectType { get; internal set; }
        public EffectRepresentation EffectInformation { get; internal set; }

        public virtual int ID { get; internal set; }
        public string ModName { get; internal set; }
        public virtual int BaseEffectId { get { return -1; } }
        public Effect Effect{ get; internal set; }
        public virtual void OnRegister(Effect effect) { }
        public int GetHash() { return StringUtils.GetInt32HashCode($"{ModName}:{Name}"); }
    }
}