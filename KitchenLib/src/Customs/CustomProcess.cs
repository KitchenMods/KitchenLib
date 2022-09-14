using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomProcess
	{
		public virtual string ProcessName { get; internal set; }
		public virtual GameDataObject BasicEnablingAppliance { get; internal set; }
		public virtual int EnablingApplianceCount { get; internal set; }
		public virtual Process IsPseudoprocessFor { get; internal set; }
		public virtual bool CanObfuscateProgress { get; internal set; }
		public virtual string Icon { get; internal set; }
		public virtual LocalisationObject<ProcessInfo> Info { get; internal set; }
		public virtual int ID { get; internal set; }
		public virtual AudioClip ProcessAudioClip { get; internal set; }

		public Process Process{ get; internal set; }
		public string ModName { get; internal set; }
		public virtual int BaseProcessId { get { return -1; } }

		public virtual void OnRegister(Process process) {}
		
		public int GetHash()
		{
			return StringUtils.GetInt32HashCode($"{ModName}:{ProcessName}");
		}
	}
}
