using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomProcess : CustomGameDataObject<Process>
	{

		#region Base Game Variables

		public virtual GameDataObject BasicEnablingAppliance { get; protected set; }
		public virtual int EnablingApplianceCount { get; protected set; } = 1;
		public virtual Process IsPseudoprocessFor { get; protected set; }
		public virtual Process IsCounteractedBy { get; protected set; }
		public virtual bool CanObfuscateProgress { get; protected set; }
		public virtual bool ReverseProgressBar { get; protected set; }
		public virtual bool DrawBadProcessAsMinor { get; protected set; }
		public virtual ProcessColourSet OverrideProgressColour { get; protected set; }
		public virtual LocalisationObject<ProcessInfo> Info { get; protected set; } = new();

		#endregion
		
		#region KitchenLib Variables

		public virtual List<(Locale, ProcessInfo)> InfoList { get; protected set; } = new();

		#endregion

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not Process process) return;
			
			#region Apply Properties

			OverrideVariable(process, "BasicEnablingAppliance", BasicEnablingAppliance);
			OverrideVariable(process, "IsPseudoprocessFor", IsPseudoprocessFor);
			OverrideVariable(process, "IsCounteractedBy", IsCounteractedBy);
			OverrideVariable(process, "EnablingApplianceCount", EnablingApplianceCount);
			OverrideVariable(process, "CanObfuscateProgress", CanObfuscateProgress);
			OverrideVariable(process, "ReverseProgressBar", ReverseProgressBar);
			OverrideVariable(process, "DrawBadProcessAsMinor", DrawBadProcessAsMinor);
			OverrideVariable(process, "OverrideProgressColour", OverrideProgressColour);
			OverrideVariable(process, "Info", Info);
				
			#endregion
			
			if (InfoList != null && InfoList.Count > 0)
			{
				ConvertInfoListToLocalisationObject(InfoList, ref process.Info);
			}
			else
			{
				OverrideVariable(process, "Info", Info);
			}
			
		}
	}
}