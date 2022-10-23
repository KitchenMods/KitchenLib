using KitchenData;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
    public abstract class CustomResearch : CustomGameDataObject
    {
		public virtual int RequiredResearch { get; internal set; }
		public virtual List<IUpgrade> Rewards { get { return new List<IUpgrade>(); } }
		public virtual List<Research> EnablesResearchOf  { get { return new List<Research>(); } }
		public virtual List<Research> RequiresForResearch { get { return new List<Research>(); } }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Research result = new Research();
            Research empty = new Research();
            
            if (empty.ID != ID) result.ID = ID;
            if (empty.RequiredResearch != RequiredResearch) result.RequiredResearch = RequiredResearch;
            if (empty.Rewards != Rewards) result.Rewards = Rewards;
            if (empty.EnablesResearchOf != EnablesResearchOf) result.EnablesResearchOf = EnablesResearchOf;
            if (empty.RequiresForResearch != RequiresForResearch) result.RequiresForResearch = RequiresForResearch;

            gameDataObject = result;
        }
    }
}