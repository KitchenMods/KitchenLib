using KitchenData;
using KitchenLib.DevUI;
using KitchenLib.Views;
using UnityEngine;

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
		private static string blueprintSearchBar = "";
		public override void Setup()
		{
			//795, 1100
			GUILayout.BeginArea(new Rect(0, 0, 198, 25));
			if (GUILayout.Button("None"))
			{
				Mode = FunMode.None;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(198, 0, 198, 25));
			if (GUILayout.Button("Fire"))
			{
				Mode = FunMode.Fire;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(396, 0, 198, 25));
			if (GUILayout.Button("Chop"))
			{
				Mode = FunMode.Chop;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(0, 25, 198, 25));
			if (GUILayout.Button("Clean"))
			{
				Mode = FunMode.Clean;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(198, 25, 198, 25));
			if (GUILayout.Button("Cook"))
			{
				Mode = FunMode.Cook;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(396, 25, 198, 25));
			if (GUILayout.Button("Knead"))
			{
				Mode = FunMode.Knead;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(594, 0, 198, 125));
			GUILayout.Label("Customer Caller");
			CommandView.isCat = GUILayout.Toggle(CommandView.isCat, "IsCat");
			GUILayout.Label("Customers: " + CommandView.customerCount);
			CommandView.customerCount = Mathf.Round(GUILayout.HorizontalSlider(CommandView.customerCount, 0, 10));
			if (GUILayout.Button("Customer"))
			{
				Mode = FunMode.Customer;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(594, 125, 198, 250));
			GUILayout.Label("Blueprint Spawner");
			blueprintSearchBar = GUILayout.TextArea(blueprintSearchBar);
			blueprintSelector = GUILayout.BeginScrollView(blueprintSelector, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			CommandView.manualBlueprint = int.Parse(GUILayout.TextArea(CommandView.manualBlueprint.ToString()));
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
			CommandView.useRedBlueprint = GUILayout.Toggle(CommandView.useRedBlueprint, "Use Red Blueprint");
			if (GUILayout.Button("Spawn Blueprint"))
			{
				Mode = FunMode.SpawnBlueprint;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(594, 400, 198, 100));
			GUILayout.Label("Mess Creator");
			CommandView.messType = (int)Mathf.Round(GUILayout.HorizontalSlider(CommandView.messType, 1, 3));
			CommandView.isKitchenMess = GUILayout.Toggle(CommandView.isKitchenMess, "Kitchen Mess");
			if (GUILayout.Button("Mess"))
			{
				Mode = FunMode.Mess;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(396, 50, 198, 250));
			GUILayout.Label("Theme Builder");
			GUILayout.Label("Exclusive");
			CommandView.exclusiveLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(CommandView.exclusiveLevel, 0, 9));
			GUILayout.Label("Affordable");
			CommandView.affordableLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(CommandView.affordableLevel, 0, 9));
			GUILayout.Label("Charming");
			CommandView.charmingLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(CommandView.charmingLevel, 0, 9));
			GUILayout.Label("Formal");
			CommandView.formalLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(CommandView.formalLevel, 0, 9));
			GUILayout.Label("Kitchen");
			CommandView.kitchenLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(CommandView.kitchenLevel, 0, 9));
			if (GUILayout.Button("Theme"))
			{
				Mode = FunMode.Theme;
			}
			GUILayout.EndArea();
		}

		public override void Disable()
		{
			Mode = FunMode.None;
		}

		public static FunMode Mode = FunMode.None;
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
		Theme
	}
}
