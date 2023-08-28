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

			Main.LogDebug($"[CustomPopupTextLocalisation.Convert] [1.1] Converting Base");

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<PopupTextLocalisation>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.Info != Info) result.Info = Info;
			if (result.Text != Text) result.Text = Text;
			
			gameDataObject = result;
		}
	}
}
