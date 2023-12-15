using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace KitchenLib.Customs
{
	public abstract class CustomTutorialLocalisation : CustomLocalisationSet<TutorialLocalisation, TutorialText>
	{
		// Base-Game Variables
		public virtual LocalisationObject<TutorialText> Info { get; protected set; }

		public virtual Dictionary<TutorialMessage, TutorialDetails> Text { get; protected set; } = new SerializedDictionary<TutorialMessage, TutorialDetails>();

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			TutorialLocalisation result = ScriptableObject.CreateInstance<TutorialLocalisation>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<TutorialLocalisation>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.Info != Info) result.Info = Info;
			if (result.Text != Text) result.Text = Text;
			
			gameDataObject = result;
		}
	}
}
