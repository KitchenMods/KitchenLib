using KitchenData;
using KitchenLib.Utils;

namespace KitchenLib.Customs
{
    public abstract class CustomGameDataObject
    {
        public virtual int ID { get; internal set; }
        public virtual string UniqueNameID { get; internal set; }
        public virtual int BaseGameDataObjectID { get; internal set; } = -1;
        public string ModName = "";
        public GameDataObject GameDataObject;
        public abstract void Convert(GameData gameData, out GameDataObject gameDataObject);
        public virtual void OnRegister(GameDataObject gameDataObject) { }

        public int GetHash()
        {
            return StringUtils.GetInt32HashCode($"{ModName}:{UniqueNameID}");
        }
    }
}