using KitchenData;
using KitchenLib.DevUI;
using KitchenLib.Views;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace KitchenLib.UI
{
	public class FunMenu : BaseUI
	{
		public FunMenu()
		{
			ButtonName = "Fun";
		}

		public override void OnInit()
		{
			Mode = FunMode.None;
		}
		private static Vector2 blueprintSelector;
		private static Vector2 outfitScroll;
		private static Vector2 cosmeticScroll;
		private static Vector2 playerScroll;
		private static string blueprintSearchBar = "";
		public override void Setup()
		{
			//795, 1100
			
			GUILayout.BeginArea(new Rect(0, 0, 100, 25), GetBackground(100, 25, Color.blue)); //Disable Effects
			if (GUILayout.Button("None"))
			{
				Mode = FunMode.None;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(100, 0, 100, 25), GetBackground(100, 25, Color.blue)); //Burn Appliances
			if (GUILayout.Button("Fire"))
			{
				Mode = FunMode.Fire;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(200, 0, 100, 25), GetBackground(100, 25, Color.blue)); //Chop Itesm
			if (GUILayout.Button("Chop"))
			{
				Mode = FunMode.Chop;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(300, 0, 100, 25), GetBackground(100, 25, Color.blue)); //Clean Items
			if (GUILayout.Button("Clean"))
			{
				Mode = FunMode.Clean;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(400, 0, 100, 25), GetBackground(100, 25, Color.blue)); //Cook Items
			if (GUILayout.Button("Cook"))
			{
				Mode = FunMode.Cook;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(500, 0, 100, 25), GetBackground(100, 25, Color.blue)); //Knead Items
			if (GUILayout.Button("Knead"))
			{
				Mode = FunMode.Knead;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(0, 25, 100, 25), GetBackground(100, 25, Color.blue)); //Trigger Blindness
			if (GUILayout.Button("Blindness"))
			{
				Mode = FunMode.Blindness;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(100, 25, 100, 25), GetBackground(100, 25, Color.blue)); //Garbage Filler
			if (GUILayout.Button("Garbage"))
			{
				Mode = FunMode.Garbage;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(200, 25, 100, 25), GetBackground(100, 25, Color.blue)); //ItemProvider
			if (GUILayout.Button("ItemProvider"))
			{
				Mode = FunMode.ItemProvider;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(600, 0, 195, 125), GetBackground(195, 125, Color.red)); //Customer Spawner
			GUILayout.Label("Customer Caller");
			GUILayout.Label("Customers: " + CommandView.customerCount);
			CommandView.customerCount = Mathf.Round(GUILayout.HorizontalSlider(CommandView.customerCount, 0, 10));
			CommandView.isCat = GUILayout.Toggle(CommandView.isCat, "IsCat");
			if (GUILayout.Button("Customer"))
			{
				Mode = FunMode.Customer;
			}
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(600, 125, 195, 175), GetBackground(195, 175, Color.blue)); //Blueprint Spawner
			GUILayout.Label("Blueprint Spawner");
			
			GUILayout.BeginArea(new Rect(0, 25, 100, 25)); //Manual: Label
			GUILayout.Label("Manual: ");
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(100, 25, 100, 25)); //Manual ID Selector
			CommandView.manualBlueprint = int.Parse(GUILayout.TextArea(CommandView.manualBlueprint.ToString()));
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(0, 50, 195, 100)); //Scroll Box
			blueprintSearchBar = GUILayout.TextArea(blueprintSearchBar);
			blueprintSelector = GUILayout.BeginScrollView(blueprintSelector, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			foreach (Appliance appliance in GameData.Main.Get<Appliance>())
			{
				if (appliance.Name.ToLower().Contains(blueprintSearchBar.ToLower()) && !string.IsNullOrEmpty(appliance.Name))
				{
					if (GUILayout.Button(appliance.Name))
					{
						CommandView.selectedBlueprint = appliance.ID;
					}
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(0, 150, 75, 25)); //Checkbox
			CommandView.useRedBlueprint = GUILayout.Toggle(CommandView.useRedBlueprint, "Red");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(75, 150, 120, 25)); //Button
			if (GUILayout.Button("Spawn Blueprint"))
			{
				Mode = FunMode.SpawnBlueprint;
			}
			GUILayout.EndArea();
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(600, 300, 195, 75), GetBackground(195, 75, Color.red)); //Mess Creator
			GUILayout.Label("Mess Spawner");
			CommandView.messType = (int)Mathf.Round(GUILayout.HorizontalSlider(CommandView.messType, 1, 3));

			GUILayout.BeginArea(new Rect(0, 50, 75, 25)); //Checkbox
			CommandView.isKitchenMess = GUILayout.Toggle(CommandView.isKitchenMess, "Kitchen");
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(75, 50, 120, 25)); //Button
			if (GUILayout.Button("Mess"))
			{
				Mode = FunMode.Mess;
			}
			GUILayout.EndArea();
			
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(600, 375, 195, 300), GetBackground(195, 300, Color.blue)); //Decor Generator
			GUILayout.Label("Theme Builder");
			GUILayout.Label("Exclusive: " + CommandView.exclusiveLevel);
			CommandView.exclusiveLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(CommandView.exclusiveLevel, 0, 9));
			GUILayout.Label("Affordable: " + CommandView.affordableLevel);
			CommandView.affordableLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(CommandView.affordableLevel, 0, 9));
			GUILayout.Label("Charming: " + CommandView.charmingLevel);
			CommandView.charmingLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(CommandView.charmingLevel, 0, 9));
			GUILayout.Label("Formal: " + CommandView.formalLevel);
			CommandView.formalLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(CommandView.formalLevel, 0, 9));
			GUILayout.Label("Kitchen: " + CommandView.kitchenLevel);
			CommandView.kitchenLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(CommandView.kitchenLevel, 0, 9));
			if (GUILayout.Button("Theme"))
			{
				Mode = FunMode.Theme;
			}
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(600, 675, 195, 350), GetBackground(195, 350, Color.red)); //Outfit Changer
			GUILayout.Label("Outfit Editor");
			GUILayout.Label("Players:");
			GUILayout.BeginArea(new Rect(0, 50, 195, 75)); //Player Selector
			playerScroll = GUILayout.BeginScrollView(playerScroll, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			foreach (int player in CommandView.players.Keys)
			{
				if (GUILayout.Button(CommandView.players[player]))
				{
					CommandView.selectedPlayer = player;
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
			GUILayout.BeginArea(new Rect(0, 125, 195, 100)); //Outfit Selector
			GUILayout.Label("Outfits:");
			outfitScroll = GUILayout.BeginScrollView(outfitScroll, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			foreach (PlayerCosmetic outfit in GameData.Main.Get<PlayerCosmetic>())
			{
				if (outfit.CosmeticType == CosmeticType.Outfit)
				{
					if (GUILayout.Button(outfit.name))
					{
						CommandView.selectedOutfit = outfit.ID;
					}
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
			GUILayout.BeginArea(new Rect(0, 225, 195, 100)); //Hat Selector
			GUILayout.Label("Hats:");
			cosmeticScroll = GUILayout.BeginScrollView(cosmeticScroll, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			foreach (PlayerCosmetic hat in GameData.Main.Get<PlayerCosmetic>())
			{
				if (hat.CosmeticType == CosmeticType.Hat)
				{
					if (GUILayout.Button(hat.name))
					{
						CommandView.selectedHat = hat.ID;
					}
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
			GUILayout.BeginArea(new Rect(0, 325, 195, 25)); //Button
			if (GUILayout.Button("Cosmetics"))
			{
				Mode = FunMode.Cosmetics;
			}
			GUILayout.EndArea();
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(400, 675, 195, 75), GetBackground(195, 75, Color.blue)); //Outfit Changer
			GUILayout.Label("Color Editor");
			CommandView.color = GUILayout.TextArea(CommandView.color);
			if (GUILayout.Button("Color"))
			{
				Mode = FunMode.Color;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(400, 750, 195, 75), GetBackground(195, 75, Color.blue)); //Speed Changer
			GUILayout.Label("Speed Multiplier: " + CommandView.speed);
			CommandView.speed = GUILayout.HorizontalSlider(CommandView.speed, 0, 5);
			if (GUILayout.Button("Speed"))
			{
				Mode = FunMode.Speed;
			}
			GUILayout.EndArea();
		}

		public override void Disable()
		{
			Mode = FunMode.None;
		}

		public static FunMode Mode = FunMode.None;

		private static Texture2D tex = new Texture2D(1, 1);
		private Texture2D GetBackground(int width, int height, Color32 color)
		{
			/*
			Texture2D tex = new Texture2D(width, height);
			Color32 resetColor = color;
			Color32[] resetColorArray = tex.GetPixels32();
			for (int i = 0; i < resetColorArray.Length; i++)
			{
				resetColorArray[i] = resetColor;
			}

			tex.SetPixels32(resetColorArray);
			tex.Apply();
			*/
			return tex;
		}
	}

	public enum FunMode
	{
		None,
		Fire,
		Chop,
		Clean,
		Cook,
		Knead,
		Customer,
		SpawnBlueprint,
		Mess,
		Theme,
		Cosmetics,
		Color,
		Speed,
		Blindness,
		Garbage,
		ItemProvider
	}
}
