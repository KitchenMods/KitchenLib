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

			Main.LogDebug($"[CustomRecipeLocalisation.Convert] [1.1] Converting Base");

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<RecipeLocalisation>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.Info != Info) result.Info = Info;
			if (result.Text != Text) result.Text = Text;
			
			gameDataObject = result;
		}
	}
}
