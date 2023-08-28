using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace KitchenLib.Customs
{
	public abstract class CustomRarityTierLocalisation : CustomLocalisationSet<RarityTierLocalisation, RarityTierInfo>
	{
		// Base-Game Variables
		public virtual LocalisationObject<RarityTierInfo> Info { get; protected set; }

		public virtual Dictionary<RarityTier, string> Text { get; protected set; } = new SerializedDictionary<RarityTier, string>();

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			RarityTierLocalisation result = ScriptableObject.CreateInstance<RarityTierLocalisation>();

			Main.LogDebug($"[CustomRarityTierLocalisation.Convert] [1.1] Converting Base");

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<RarityTierLocalisation>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.Info != Info) result.Info = Info;
			if (result.Text != Text) result.Text = Text;
			
			gameDataObject = result;
		}
	}
}
