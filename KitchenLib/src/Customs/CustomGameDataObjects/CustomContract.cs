using KitchenData;
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
			
			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "Status", Status);
			OverrideVariable(result, "ExperienceMultiplier", ExperienceMultiplier);
			OverrideVariable(result, "Info", Info);

			if (InfoList.Count > 0)
			{
				Main.LogDebug($"Setting up localisation");
				result.Info = new LocalisationObject<ContractInfo>();
				foreach ((Locale, ContractInfo) info in InfoList)
					result.Info.Add(info.Item1, info.Item2);
			}
			gameDataObject = result;
		}
	}
}
