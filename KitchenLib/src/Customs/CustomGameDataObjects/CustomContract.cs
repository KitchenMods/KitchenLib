using KitchenData;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomContract : CustomContractLocalisation<Contract>
	{
		// Base-Game Variables
		public virtual RestaurantStatus Status { get; protected set; }
		public virtual int ExperienceMultiplier { get; protected set; } = 1;

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			Contract result = ScriptableObject.CreateInstance<Contract>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<Contract>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.Status != Status) result.Status = Status;
			if (result.ExperienceMultiplier != ExperienceMultiplier) result.ExperienceMultiplier = ExperienceMultiplier;
			if (result.Info != Info) result.Info = Info;

			if (InfoList.Count > 0)
			{
				result.Info = new LocalisationObject<ContractInfo>();
				foreach ((Locale, ContractInfo) info in InfoList)
					result.Info.Add(info.Item1, info.Item2);
			}
			gameDataObject = result;
		}
	}
}
