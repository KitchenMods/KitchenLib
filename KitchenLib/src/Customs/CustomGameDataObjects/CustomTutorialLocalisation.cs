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
			
			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "Info", Info);
			OverrideVariable(result, "Text", Text);
			
			gameDataObject = result;
		}
	}
}
