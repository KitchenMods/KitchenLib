using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomResearch : CustomLocalisedGameDataObject<Research, ResearchLocalisation>
    {
	    // Base-Game Variables
        public virtual int RequiredResearch { get; protected set; }
        public virtual List<IUpgrade> Rewards { get; protected set; } = new List<IUpgrade>();
        public virtual List<Research> EnablesResearchOf { get; protected set; } = new List<Research>();
        public virtual List<Research> RequiresForResearch { get; protected set; } = new List<Research>();
        
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Research result = ScriptableObject.CreateInstance<Research>();

			Main.LogDebug($"[CustomResearch.Convert] [1.1] Converting Base");

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Research>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.RequiredResearch != RequiredResearch) result.RequiredResearch = RequiredResearch;
            if (result.Info != Info) result.Info = Info;

            if (InfoList.Count > 0)
            {
                result.Info = new LocalisationObject<ResearchLocalisation>();
                foreach ((Locale, ResearchLocalisation) info in InfoList)
                    result.Info.Add(info.Item1, info.Item2);
            }

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Research result = ScriptableObject.CreateInstance<Research>();

			Main.LogDebug($"[CustomResearch.AttachDependentProperties] [1.1] Converting Base");

			if (result.Rewards != Rewards) result.Rewards = Rewards;
            if (result.EnablesResearchOf != EnablesResearchOf) result.EnablesResearchOf = EnablesResearchOf;
            if (result.RequiresForResearch != RequiresForResearch) result.RequiresForResearch = RequiresForResearch;
        }
    }
}