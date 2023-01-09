using KitchenData;
using KitchenLib.Utils;

namespace KitchenLib.Customs
{
    public abstract class CustomGameDataObject
    {
        public virtual int ID {get; internal set; }
        public virtual string UniqueNameID { get; internal set;}
        public virtual int BaseGameDataObjectID {get { return -1; } }
        public string ModName = "";
        public GameDataObject GameDataObject;
        public abstract void Convert(GameData gameData, out GameDataObject gameDataObject);
        /// <summary>
        /// Override this method to modify the final GameDataObject that is constructed according to your CustomGameDataObject implementation.
        /// If you need to reference other modded GameDataObjects within this method, it is recommended to use OnPostRegister() instead.
        /// </summary>
        /// <param name="gameDataObject">the GameDataObject corresponding this CustomGameDataObject implementation</param>
        public virtual void OnRegister(GameDataObject gameDataObject) { }
        /// <summary>
        /// Override this method to modify the final GameDataObject that is constructed according to your CustomGameDataObject implementation.
        /// This callback is fired after ALL modded CustomGameDataObjects have been registered. Unless you need this specific behavior, it is
        /// recommended to use OnRegister() instead.
        /// </summary>
        /// <param name="gameDataObject">the GameDataObject corresponding this CustomGameDataObject implementation</param>
        public virtual void OnPostRegister(GameDataObject gameDataObject) { }

        public int GetHash()
        {
            return StringUtils.GetInt32HashCode($"{ModName}:{UniqueNameID}");
        }
    }
}