using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace KitchenLib.Customs
{
	public abstract class CustomDictionaryLocalisation<T> : CustomLocalisationSet<T, DictionaryInfo> where T : GameDataObject
	{
		// Base-Game Variables
		public virtual LocalisationObject<DictionaryInfo> Info { get; protected set; }

		public virtual Dictionary<string, string> Text { get; protected set; } = new SerializedDictionary<string, string>();

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			DictionaryLocalisation result = ScriptableObject.CreateInstance<DictionaryLocalisation>();
			
			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "Info", Info);
			OverrideVariable(result, "Text", Text);
			
			gameDataObject = result;
		}
	}
}
