using KitchenData;
using System;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomProcess : CustomGameDataObject
    {
        public virtual GameDataObject BasicEnablingAppliance { get; protected set; }
        public virtual int EnablingApplianceCount { get; protected set; } = 1;
        public virtual Process IsPseudoprocessFor { get; protected set; }
        public virtual bool CanObfuscateProgress { get; protected set; }

		[Obsolete("Please set your Icon in Info")]
		public virtual string Icon { get; protected set; } = "!";
        public virtual LocalisationObject<ProcessInfo> Info { get; protected set; }

        private static readonly Process empty = ScriptableObject.CreateInstance<Process>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Process result = ScriptableObject.CreateInstance<Process>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Process>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (empty.ID != ID) result.ID = ID;
            if (empty.EnablingApplianceCount != EnablingApplianceCount) result.EnablingApplianceCount = EnablingApplianceCount;
            if (empty.CanObfuscateProgress != CanObfuscateProgress) result.CanObfuscateProgress = CanObfuscateProgress;
            if (empty.Info != Info) result.Info = Info;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Process result = (Process)gameDataObject;

            if (empty.BasicEnablingAppliance != BasicEnablingAppliance) result.BasicEnablingAppliance = BasicEnablingAppliance;
            if (empty.IsPseudoprocessFor != IsPseudoprocessFor) result.IsPseudoprocessFor = IsPseudoprocessFor;
        }
    }
}