using KitchenData;
using KitchenLib.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomRandomUpgradeSet
	{
        public virtual string Name { get {return "New Layout";} }
        public virtual UpgradeRewardTier Tier { get; internal set; }
        public virtual List<IUpgrade> Rewards { get; internal set; }

        public virtual int ID { get; internal set; }
        public string ModName { get; internal set; }
        public virtual int BaseRandomUpgradeSetId { get { return -1; } }
        public RandomUpgradeSet RandomUpgradeSet{ get; internal set; }
        public virtual void OnRegister(RandomUpgradeSet randomUpgradeSet) { }
        public int GetHash() { return StringUtils.GetInt32HashCode($"{ModName}:{Name}"); }
    }
}