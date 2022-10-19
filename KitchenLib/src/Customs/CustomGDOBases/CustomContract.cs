using KitchenData;
using KitchenLib.Utils;

namespace KitchenLib.Customs
{
	public abstract class CustomContract
	{
        public virtual string Name { get; internal set; }
        public virtual string Description { get; internal set; }
        public virtual RestaurantStatus Status { get; internal set; }
        public virtual float ExperienceMultiplier { get { return 1f; } }

        public virtual int ID { get; internal set; }

        public string ModName { get; internal set; }
        public virtual int BaseContractId { get { return -1; } }

        public Contract Contract{ get; internal set; }

        public virtual void OnRegister(Contract contract) { }

        public int GetHash()
        {
            return StringUtils.GetInt32HashCode($"{ModName}:{Name}");
        }
    }
}