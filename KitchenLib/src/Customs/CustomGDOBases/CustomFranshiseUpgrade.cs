using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomFranchiseUpgrade
	{
        public virtual string Name { get; internal set; }
        public virtual int MaximumUpgradeCount { get {return 1;} }
        public virtual List<IFranchiseUpgrade> Upgrades { get; internal set; }


        public virtual int ID { get; internal set; }
        public string ModName { get; internal set; }
        public virtual int BaseFranchiseUpgradeId { get { return -1; } }
        public FranchiseUpgrade FranchiseUpgrade{ get; internal set; }
        public virtual void OnRegister(FranchiseUpgrade franchiseUpgrade) { }
        public int GetHash() { return StringUtils.GetInt32HashCode($"{ModName}:{Name}"); }
    }
}