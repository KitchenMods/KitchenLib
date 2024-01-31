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
			
			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "Info", Info);
			OverrideVariable(result, "Text", Text);
			
			gameDataObject = result;
		}
	}
}
