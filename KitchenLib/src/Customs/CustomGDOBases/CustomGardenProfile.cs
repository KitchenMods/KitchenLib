using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;


namespace KitchenLib.Customs
{
	public abstract class CustomGardenProfile
	{
		public virtual string Name { get; internal set; }
		public virtual Appliance SpawnHolder { get; internal set; }
		public virtual List<Item> Spawns { get; internal set; }

		public virtual int ID { get; internal set; }
		public GardenProfile GardenProfile{ get; internal set; }
		public string ModName { get; internal set; }
		public virtual int BaseGardenProfileId { get { return -1; } }

		public virtual void OnRegister(GardenProfile gardenProfile) {}
		
		public int GetHash()
		{
			return StringUtils.GetInt32HashCode($"{ModName}:{Name}");
		}
	}
}
