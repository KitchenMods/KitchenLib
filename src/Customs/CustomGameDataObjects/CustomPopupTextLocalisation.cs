using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace KitchenLib.Customs
{
	public abstract class CustomPopupTextLocalisation : CustomLocalisationSet<PopupTextLocalisation, PopupText>
	{
		// Base-Game Variables
		public virtual LocalisationObject<PopupText> Info { get; protected set; }

		public virtual Dictionary<PopupType, PopupDetails> Text { get; protected set; } = new SerializedDictionary<PopupType, PopupDetails>();

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			PopupTextLocalisation result = ScriptableObject.CreateInstance<PopupTextLocalisation>();

			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "Info", Info);
			OverrideVariable(result, "Text", Text);
			
			gameDataObject = result;
		}
	}
}
