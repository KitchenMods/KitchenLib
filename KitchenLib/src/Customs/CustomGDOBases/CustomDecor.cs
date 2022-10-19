using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomDecor
	{
        public virtual string Name { get; internal set; }
        public virtual Material Material { get; internal set; }
        public virtual Appliance ApplicatorAppliance { get; internal set; }
        public virtual LayoutMaterialType Type { get; internal set; }
        public virtual bool IsAvailable { get { return true; } }

        public virtual int ID { get; internal set; }
        public string ModName { get; internal set; }
        public virtual int BaseDecorId { get { return -1; } }
        public Decor Decor{ get; internal set; }
        public virtual void OnRegister(Decor decor) { }
        public int GetHash() { return StringUtils.GetInt32HashCode($"{ModName}:{Name}"); }
    }
}