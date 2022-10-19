using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomItemGroup
	{
        public virtual string Name { get; internal set; }
        public virtual List<ItemGroup.ItemSet> DerivedSets { get; internal set; }
        public virtual bool CanContainSide { get; internal set; }
        public virtual bool ApplyProcessesToComponents { get; internal set; }
        public virtual bool AutoCollapsing { get; internal set; }

        public virtual int ID { get; internal set; }
        public string ModName { get; internal set; }
        public virtual int BaseItemGroupId { get { return -1; } }
        public ItemGroup ItemGroup{ get; internal set; }
        public virtual void OnRegister(ItemGroup itemGroup) { }
        public int GetHash() { return StringUtils.GetInt32HashCode($"{ModName}:{Name}"); }
    }
}