using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace KitchenLib.Customs
{
	public abstract class CustomDecorationLocalisation : CustomLocalisationSet<DecorationLocalisation, DecorationBonusInfo>
	{
		// Base-Game Variables
		public virtual LocalisationObject<DecorationBonusInfo> Info { get; protected set; }

		public virtual Dictionary<DecorationBonus, string> Text { get; protected set; } = new SerializedDictionary<DecorationBonus, string>();

		public virtual Dictionary<DecorationType, string> Icons { get; protected set; } = new SerializedDictionary<DecorationType, string>();

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			DecorationLocalisation result = ScriptableObject.CreateInstance<DecorationLocalisation>();
			
			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "Info", Info);
			OverrideVariable(result, "Text", Text);
			OverrideVariable(result, "Icons", Icons);
			
			gameDataObject = result;
		}
	}
}
