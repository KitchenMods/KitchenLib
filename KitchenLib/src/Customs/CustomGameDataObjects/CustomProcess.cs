using KitchenData;
using System;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomProcess : CustomLocalisedGameDataObject<Process, ProcessInfo>
    {
        public virtual GameDataObject BasicEnablingAppliance { get; protected set; }
        public virtual int EnablingApplianceCount { get; protected set; } = 1;
        public virtual Process IsPseudoprocessFor { get; protected set; }
        public virtual bool CanObfuscateProgress { get; protected set; }

        [Obsolete("Please set your Icon in Info")]
        public virtual string Icon { get; protected set; } = "!";

        //private static readonly Process result = ScriptableObject.CreateInstance<Process>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Process result = ScriptableObject.CreateInstance<Process>();

			Main.LogDebug($"[CustomProcess.Convert] [1.1] Convering Base");

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Process>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.EnablingApplianceCount != EnablingApplianceCount) result.EnablingApplianceCount = EnablingApplianceCount;
            if (result.CanObfuscateProgress != CanObfuscateProgress) result.CanObfuscateProgress = CanObfuscateProgress;

            if (result.Info != Info) result.Info = Info;

            if (InfoList.Count > 0)
            {
                result.Info = new LocalisationObject<ProcessInfo>();
                foreach ((Locale, ProcessInfo) info in InfoList)
                    result.Info.Add(info.Item1, info.Item2);
            }

            if (result.Info == null)
            {
                result.Info = new LocalisationObject<ProcessInfo>();
                if (!result.Info.Has(Locale.English))
                {
					ProcessInfo processInfo = ScriptableObject.CreateInstance<ProcessInfo>();
					processInfo.Name = Icon;
					processInfo.Icon = Icon;
					result.Info.Add(Locale.English, processInfo);
				}
            }

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Process result = (Process)gameDataObject;

			Main.LogDebug($"[CustomProcess.AttachDependentProperties] [1.1] Convering Base");

			if (result.BasicEnablingAppliance != BasicEnablingAppliance) result.BasicEnablingAppliance = BasicEnablingAppliance;
            if (result.IsPseudoprocessFor != IsPseudoprocessFor) result.IsPseudoprocessFor = IsPseudoprocessFor;
        }
    }
}