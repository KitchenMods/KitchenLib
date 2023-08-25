using KitchenData;
using Kitchen;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace KitchenLib.Customs
{
	public abstract class CustomStartDayWarningLocalisation : CustomLocalisationSet<StartDayWarningLocalisation, StartDayWarningInfo>
	{
		// Base-Game Variables
		public virtual LocalisationObject<StartDayWarningInfo> Info { get; protected set; }

		public virtual Dictionary<StartDayWarning, GenericLocalisationStruct> Text { get; protected set; } = new SerializedDictionary<StartDayWarning, GenericLocalisationStruct>();

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			StartDayWarningLocalisation result = ScriptableObject.CreateInstance<StartDayWarningLocalisation>();

			Main.LogDebug($"[CustomRestaurantSetting.Convert] [1.1] Converting Base");

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<StartDayWarningLocalisation>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.Info != Info) result.Info = Info;
			if (result.Text != Text) result.Text = Text;
			
			gameDataObject = result;
		}
	}
}
