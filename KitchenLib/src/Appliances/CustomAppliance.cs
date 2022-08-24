using System;
using Kitchen;
using KitchenData;
using KitchenLib.Utils;

namespace KitchenLib.Appliances
{
	public abstract class CustomAppliance
	{
		public string ModName { get; internal set; }
		
		public virtual int ID { get; internal set; }
		public virtual string Name { get { return "Custom Appliance"; } }
		public virtual string Description { get { return ""; } }
		public virtual int BaseApplianceId { get { return -1248669347; } }
		public virtual int BasePrefabId { get { return -1248669347; } }

		public Appliance Appliance { get; internal set; }

		public virtual void OnRegister(Appliance appliance) { }

		public virtual void OnInteract(InteractionData data) { }

		public virtual bool OnCheckInteractPossible(InteractionData data) {
			return false;
		}

		public int GetHash() {
			return StringUtils.GetInt32HashCode($"{ModName}:{Name}");
		}
	}
}