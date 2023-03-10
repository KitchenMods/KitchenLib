using KitchenData;
using KitchenLib.Utils;

namespace KitchenLib.Customs
{
	public abstract class CustomGameDataObject
	{
		public virtual int ID { get; internal set; }
		public int LegacyID { get; internal set; }
		public virtual string UniqueNameID { get; protected set; }
		public virtual int BaseGameDataObjectID { get; protected set; } = -1;

		public string ModID = "";
		public string ModName = "";
		public GameDataObject GameDataObject;

		public abstract void Convert(GameData gameData, out GameDataObject gameDataObject);
		public virtual void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject) { }
		public virtual void OnRegister(GameDataObject gameDataObject) { }

		public int GetHash()
		{
			return StringUtils.GetInt32HashCode($"{ModID}:{UniqueNameID}");
		}
		public int GetLegacyHash()
		{
			return StringUtils.GetInt32HashCode($"{ModName}:{UniqueNameID}");
		}
	}
}