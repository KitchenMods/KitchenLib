using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using Kitchen.Layouts;

namespace KitchenLib.Customs
{
	public abstract class CustomLevelUpgradeSet
	{
        public virtual string Name { get; internal set ; }
        public virtual List<LevelUpgrade> Upgrades { get; internal set; }

        public virtual int ID { get; internal set; }
        public string ModName { get; internal set; }
        public virtual int BaseLevelUpgradeSetId { get { return -1; } }
        public LevelUpgradeSet LevelUpgradeSet{ get; internal set; }
        public virtual void OnRegister(LevelUpgradeSet levelUpgradeSet) { }
        public int GetHash() { return StringUtils.GetInt32HashCode($"{ModName}:{Name}"); }
    }
}