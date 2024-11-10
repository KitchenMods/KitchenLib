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

            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "RequiredResearch", RequiredResearch);
            OverrideVariable(result, "RequiredResearch", RequiredResearch);

            if (InfoList.Count > 0)
            {
	            SetupLocalisation<ResearchLocalisation>(InfoList, ref result.Info);
            }

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Research result = ScriptableObject.CreateInstance<Research>();
            
            OverrideVariable(result, "Rewards", Rewards);
            OverrideVariable(result, "EnablesResearchOf", EnablesResearchOf);
            OverrideVariable(result, "RequiresForResearch", RequiresForResearch);
        }
    }
}