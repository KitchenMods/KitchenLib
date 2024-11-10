using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace KitchenLib.Customs
{
	public abstract class CustomRecipeLocalisation : CustomLocalisationSet<RecipeLocalisation, RecipeInfo>
	{
		// Base-Game Variables
		public virtual LocalisationObject<RecipeInfo> Info { get; protected set; }

		public virtual Dictionary<Dish, string> Text { get; protected set; } = new SerializedDictionary<Dish, string>();

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			RecipeLocalisation result = ScriptableObject.CreateInstance<RecipeLocalisation>();
			
			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "Info", Info);
			OverrideVariable(result, "Text", Text);
			
			gameDataObject = result;
		}
	}
}
