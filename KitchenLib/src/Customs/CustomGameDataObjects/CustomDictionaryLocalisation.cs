using KitchenData;
using Kitchen;
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

			Main.LogDebug($"[CustomDictionaryLocalisation.Convert] [1.1] Converting Base");

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<DictionaryLocalisation>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.Info != Info) result.Info = Info;
			if (result.Text != Text) result.Text = Text;
			
			gameDataObject = result;
		}
	}
}
