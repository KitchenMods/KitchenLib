using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomPlayerCosmetic
	{
        public virtual string Name { get; internal set; }
        public virtual CosmeticType CosmeticType { get; internal set; }
        public virtual GameObject Visual { get; internal set; }

        public virtual int ID { get; internal set; }
        public string ModName { get; internal set; }
        public virtual int BasePlayerCosmeticId { get { return -1; } }
        public PlayerCosmetic PlayerCosmetic{ get; internal set; }
        public virtual void OnRegister(PlayerCosmetic playerCosmetic) { }
        public int GetHash() { return StringUtils.GetInt32HashCode($"{ModName}:{Name}"); }
    }
}