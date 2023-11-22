using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace KitchenLib.Customs
{
	public abstract class CustomNewsItemFallbackLocalisation : CustomLocalisationSet<NewsItemFallbackLocalisation, NewsItemFallbackInfo>
	{
		// Base-Game Variables
		public virtual LocalisationObject<NewsItemFallbackInfo> Info { get; protected set; }

		public virtual Dictionary<NewsItemType, GenericLocalisationStruct> Text { get; protected set; } = new SerializedDictionary<NewsItemType, GenericLocalisationStruct>();

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			NewsItemFallbackLocalisation result = ScriptableObject.CreateInstance<NewsItemFallbackLocalisation>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<NewsItemFallbackLocalisation>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.Info != Info) result.Info = Info;
			if (result.Text != Text) result.Text = Text;
			
			gameDataObject = result;
		}
	}
}
