using KitchenData;
using KitchenLib.Utils;
namespace KitchenLib.Customs
{
	public abstract class CustomEffectRepresentation
	{
        public virtual string Name { get; internal set; }
        public virtual string Description { get; internal set; }
        public virtual string Icon { get; internal set; } 

        public virtual int ID { get; internal set; }
        public string ModName { get; internal set; }
        public virtual int BaseEffectRepresentationId { get { return -1; } }
        public EffectRepresentation EffectRepresentation{ get; internal set; }
        public virtual void OnRegister(EffectRepresentation effectRepresentation) { }
        public int GetHash() { return StringUtils.GetInt32HashCode($"{ModName}:{Name}"); }
    }
}