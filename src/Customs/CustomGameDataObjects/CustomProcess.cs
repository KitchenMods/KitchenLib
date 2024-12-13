using KitchenData;
using System;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomProcess : CustomLocalisedGameDataObject<Process, ProcessInfo>
    {
	    // Base-Game Variables
        public virtual GameDataObject BasicEnablingAppliance { get; protected set; }
        public virtual int EnablingApplianceCount { get; protected set; } = 1;
        public virtual Process IsPseudoprocessFor { get; protected set; }
        public virtual bool CanObfuscateProgress { get; protected set; }

        [Obsolete("Please set your Icon in Info")]
        public virtual string Icon { get; protected set; } = "!";
		
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Process result = ScriptableObject.CreateInstance<Process>();
            
            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "EnablingApplianceCount", EnablingApplianceCount);
            OverrideVariable(result, "CanObfuscateProgress", CanObfuscateProgress);
            OverrideVariable(result, "Info", Info);
            
            if (InfoList.Count > 0)
            {
	            SetupLocalisation<ProcessInfo>(InfoList, ref result.Info);
            }
            else
            {
	            if (result.Info == null)
	            {
		            Main.LogDebug($"Setting up fallback localisation");
		            result.Info = new LocalisationObject<ProcessInfo>();
		            if (!result.Info.Has(Locale.English))
		            {
			            ProcessInfo processInfo = ScriptableObject.CreateInstance<ProcessInfo>();
			            processInfo.Name = Icon;
			            processInfo.Icon = Icon;
			            result.Info.Add(Locale.English, processInfo);
		            }
	            }
            }
            
            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Process result = (Process)gameDataObject;

            OverrideVariable(result, "BasicEnablingAppliance", BasicEnablingAppliance);
            OverrideVariable(result, "IsPseudoprocessFor", IsPseudoprocessFor);
        }
    }
}