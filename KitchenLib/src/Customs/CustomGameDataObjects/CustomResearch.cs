using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomResearch : CustomLocalisedGameDataObject<ResearchLocalisation>
    {
        public virtual int RequiredResearch { get; protected set; }
        public virtual List<IUpgrade> Rewards { get; protected set; } = new List<IUpgrade>();
        public virtual List<Research> EnablesResearchOf { get; protected set; } = new List<Research>();
        public virtual List<Research> RequiresForResearch { get; protected set; } = new List<Research>();

        private static readonly Research empty = ScriptableObject.CreateInstance<Research>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Research result = ScriptableObject.CreateInstance<Research>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Research>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (empty.ID != ID) result.ID = ID;
            if (empty.RequiredResearch != RequiredResearch) result.RequiredResearch = RequiredResearch;
            if (empty.Info != Info) result.Info = Info;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameDataObject gameDataObject)
        {
            Research result = ScriptableObject.CreateInstance<Research>();

            if (empty.Rewards != Rewards) result.Rewards = Rewards;
            if (empty.EnablesResearchOf != EnablesResearchOf) result.EnablesResearchOf = EnablesResearchOf;
            if (empty.RequiresForResearch != RequiresForResearch) result.RequiresForResearch = RequiresForResearch;
        }
    }
}