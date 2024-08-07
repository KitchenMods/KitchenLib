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
			
			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "Info", Info);
			OverrideVariable(result, "Text", Text);
			
			gameDataObject = result;
		}
	}
}
