using KitchenData;
using KitchenLib.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomResearch
	{
        public virtual string Name { get; internal set; }
        public virtual int RequiredResearch { get; internal set; }
        public virtual List<IUpgrade> Rewards { get; internal set; }
        public virtual List<Research> EnablesResearchOf { get; internal set; }
        public virtual List<Research> RequiresForResearch { get; internal set; }

        public virtual int ID { get; internal set; }
        public string ModName { get; internal set; }
        public virtual int BaseResearchId { get { return -1; } }
        public Research Research{ get; internal set; }
        public virtual void OnRegister(Research research) { }
        public int GetHash() { return StringUtils.GetInt32HashCode($"{ModName}:{Name}"); }
    }
}