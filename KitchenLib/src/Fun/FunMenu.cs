using Kitchen.Transports;
using Kitchen;
using KitchenLib.DevUI;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Reflection;
using KitchenLib.Utils;
using KitchenData;

namespace KitchenLib.Fun
{
	public class FunMenu : BaseUI
	{
		public FunMenu()
		{
			ButtonName = "Fun";
		}

		public override void OnInit()
		{
			RefVars.AvailableColors.Add(Color.red, "Red");
			RefVars.AvailableColors.Add(Color.green, "Green");
			RefVars.AvailableColors.Add(Color.blue, "Blue");
			RefVars.AvailableColors.Add(Color.white, "White");
			RefVars.AvailableColors.Add(Color.black, "Black");
			RefVars.AvailableColors.Add(Color.yellow, "Yellow");
			RefVars.AvailableColors.Add(Color.cyan, "Cyan");
			RefVars.AvailableColors.Add(Color.magenta, "Magenta");
			RefVars.AvailableColors.Add(Color.gray, "Gray");
		}
		private readonly int defaultFontSize = 16;
		private readonly TextAnchor defaultTextAnchor = TextAnchor.MiddleLeft;
		private MenuPage Page = MenuPage.Player;

		//Player Page Specifics
		private Vector2 playerSelectorScrollPosition;
		private Vector2 outfitSelectorScrollPosition;
		private Vector2 hatSelectorScrollPosition;
		private Vector2 colorSelectorScrollPosition;
		private Vector2 blueprintSelectorScrollPosition;
		private Vector2 unlockSelectorScrollPosition;
		private Vector2 processScrollPosition;
		private Vector2 orderchangerScrollPosition;
		private string ApplianceSearch = "";
		private string UnlockSearch = "";
		private string ItemSearch = "";
		private string LobbyID = "";
		private int Money = 0;
		public static bool isOpen = false;
		public override void Setup()
		{
			BuildHeader();
			BuildNavigation();
			BuildContent();
			isOpen = true;
		}

		private Dictionary<(int, int), Texture2D> cachedTextures = new Dictionary<(int, int), Texture2D>();

		private Texture2D GetBackground(int x, int y, Color color)
		{
			if (cachedTextures.ContainsKey((x, y)))
				return cachedTextures[(x, y)];

			Texture2D tex = new Texture2D(x, y);
			Color32 resetColor = color;
			Color32[] resetColorArray = tex.GetPixels32();
			for (int i = 0; i < resetColorArray.Length; i++)
				resetColorArray[i] = resetColor;
			tex.SetPixels32(resetColorArray);
			tex.Apply();

			cachedTextures.Add((x, y), tex);
			return tex;
		}

		public override void Disable()
		{
			isOpen = false;
		}

		private void ResetDefaults()
		{
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
		}

		private void BuildHeader()
		{
			GUILayout.BeginArea(new Rect(0, 0, 795, 70));
			GUI.skin.label.fontSize = 25;
			GUI.skin.label.alignment = TextAnchor.UpperCenter;
			GUILayout.Label("StarFlux's Kitchen Nightmare", new GUILayoutOption[] { GUILayout.Height(40) });
			GUI.skin.label.fontSize = 15;
			GUILayout.Label("Currently Selected: " + RefVars.CurrentMode.ToString(), new GUILayoutOption[] { GUILayout.Height(50) });
			GUILayout.EndArea();
			ResetDefaults();
		}

		private void BuildNavigation()
		{
			GUILayout.BeginArea(new Rect(0, 70, 795, 20));

			GUILayout.BeginArea(new Rect(10, 0, 139, 20));
			if (GUILayout.Button("Player"))
				Page = MenuPage.Player;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 0, 139, 20));
			if (GUILayout.Button("Appliance"))
				Page = MenuPage.Appliance;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(328, 0, 139, 20));
			if (GUILayout.Button("Item"))
				Page = MenuPage.Item;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(487, 0, 139, 20));
			if (GUILayout.Button("World"))
				Page = MenuPage.World;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 0, 139, 20));
			if (GUILayout.Button("Misc"))
				Page = MenuPage.Misc;
			GUILayout.EndArea();

			GUILayout.EndArea();
		}

		private void BuildContent()
		{
			GUILayout.BeginArea(new Rect(0, 90, 795, 960));
			switch (Page)
			{
				case MenuPage.Player:
					BuildPlayerPage();
					break;
				case MenuPage.Appliance:
					BuildAppliancePage();
					break;
				case MenuPage.Item:
					BuildItemPage();
					break;
				case MenuPage.World:
					BuildWorldPage();
					break;
				case MenuPage.Misc:
					BuildMiscPage();
					break;
			}
			GUILayout.EndArea();
		}
		private void BuildPlayerPage()
		{
			PlayerSelector();
			OutfitSelector();
			HatSelector();
			ColorSelector();
			HexColorSelector();
			SpeedSelector();
			BuildRainbowToggle();
		}

		#region BuildPlayerPage
		private void PlayerSelector()
		{
			GUILayout.BeginArea(new Rect(10, 10, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label("Player Selector");
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 40, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			if (RefVars.SelectedPlayer != 0)
				GUILayout.Label(RefVars.CurrentPlayers[RefVars.SelectedPlayer]);
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 70, 139, 200));
			playerSelectorScrollPosition = GUILayout.BeginScrollView(playerSelectorScrollPosition, false, false, GUIStyle.none, GUIStyle.none);
			foreach (int playerID in RefVars.CurrentPlayers.Keys)
			{
				if (GUILayout.Button(RefVars.CurrentPlayers[playerID]))
				{
					RefVars.SelectedPlayer = playerID;
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 280, 139, 20));
			if (GUILayout.Button("Clear", new GUILayoutOption[] { GUILayout.Height(19) })) { }
			GUILayout.EndArea();
		}
		private void OutfitSelector()
		{
			GUILayout.BeginArea(new Rect(169, 10, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label("Outfit Selector");
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 40, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			if (RefVars.SelectedOutfit != 0)
				GUILayout.Label(RefVars.GetKeyPair(RefVars.SelectedOutfit));
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 70, 139, 200));
			outfitSelectorScrollPosition = GUILayout.BeginScrollView(outfitSelectorScrollPosition, false, false, GUIStyle.none, GUIStyle.none);
			foreach (int cosmeticID in RefVars.AvailableOutfits.Keys)
			{
				if (GUILayout.Button(RefVars.GetKeyPair(cosmeticID)))
				{
					RefVars.SelectedOutfit = cosmeticID;
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 280, 298, 20));
			if (GUILayout.Button("Apply Costume", new GUILayoutOption[] { GUILayout.Height(19) }))
			{
				RefVars.ForceUpdate(FunMode.Outfit);
			}
			GUILayout.EndArea();
		}
		private void HatSelector()
		{
			GUILayout.BeginArea(new Rect(328, 10, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label("Hat Selector");
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(328, 40, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			if (RefVars.SelectedHat != 0)
				GUILayout.Label(RefVars.GetKeyPair(RefVars.SelectedHat));
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(328, 70, 139, 200));
			hatSelectorScrollPosition = GUILayout.BeginScrollView(hatSelectorScrollPosition, false, false, GUIStyle.none, GUIStyle.none);
			foreach (int cosmeticID in RefVars.AvailableHats.Keys)
			{
				if (GUILayout.Button(RefVars.GetKeyPair(cosmeticID)))
				{
					RefVars.SelectedHat = cosmeticID;
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
		private void ColorSelector()
		{
			GUILayout.BeginArea(new Rect(487, 10, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label("Color Selector");
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(487, 40, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label(RefVars.AvailableColors[RefVars.SelectedPlayerColor]);
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(487, 70, 139, 200));
			colorSelectorScrollPosition = GUILayout.BeginScrollView(colorSelectorScrollPosition, false, false, GUIStyle.none, GUIStyle.none);
			foreach (Color color in RefVars.AvailableColors.Keys)
			{
				if (GUILayout.Button(RefVars.AvailableColors[color]))
				{
					RefVars.SelectedPlayerColor = color;
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(487, 280, 139, 20));
			if (GUILayout.Button("Apply", new GUILayoutOption[] { GUILayout.Height(19) }))
			{
				RefVars.ForceUpdate(FunMode.Color);
			}
			GUILayout.EndArea();
		}
		private void HexColorSelector()
		{
			GUILayout.BeginArea(new Rect(646, 10, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label("Hex Color Selector");
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 70, 139, 20));
			RefVars.SelectedPlayerHex = GUILayout.TextField(RefVars.SelectedPlayerHex);
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 100, 139, 20));
			if (GUILayout.Button("Apply", new GUILayoutOption[] { GUILayout.Height(19) }))
			{
				RefVars.ForceUpdate(FunMode.HexColor);
			}
			GUILayout.EndArea();
		}
		private void SpeedSelector()
		{
			GUILayout.BeginArea(new Rect(646, 160, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label("Speed: " + RefVars.SelectedPlayerSpeed);
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 190, 139, 20));
			if (!Input.GetKey(KeyCode.LeftShift))
				RefVars.SelectedPlayerSpeed = GUILayout.HorizontalSlider(RefVars.SelectedPlayerSpeed, 0, 10);
			else
				RefVars.SelectedPlayerSpeed = (int)Mathf.Round(GUILayout.HorizontalSlider(RefVars.SelectedPlayerSpeed, 0, 10));
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 220, 139, 20));
			if (GUILayout.Button("Apply", new GUILayoutOption[] { GUILayout.Height(19) }))
			{
				RefVars.ForceUpdate(FunMode.Speed);
			}
			GUILayout.EndArea();
		}
		private void BuildRainbowToggle()
		{
			GUILayout.BeginArea(new Rect(646, 280, 139, 20));
			if (GUILayout.Button("Rainbow"))
			{
				RefVars.ForceUpdate(FunMode.Rainbow);
			}
			GUILayout.EndArea();
		}
		#endregion

		private void BuildAppliancePage()
		{
			BuildFire();
			BuildThemeBuilder();
			BuildArsonist();
			BuildFireFighter();
			BuildResetOrder();
			BuildApplianceDestroyer();
			//BuildOrderAdder();
		}

		#region BuildAppliancePage
		private void BuildFire()
		{
			GUILayout.BeginArea(new Rect(10, 10, 139, 20));
			if (GUILayout.Button("Fire"))
			{
				RefVars.ToggleFunMode(FunMode.Fire);
			}
			GUILayout.EndArea();
		}
		private void BuildArsonist()
		{
			GUILayout.BeginArea(new Rect(10, 40, 139, 20));
			if (GUILayout.Button("Arsonist"))
			{
				RefVars.ForceUpdate(FunMode.Arsonist);
			}
			GUILayout.EndArea();
		}
		private void BuildFireFighter()
		{
			GUILayout.BeginArea(new Rect(10, 70, 139, 20));
			if (GUILayout.Button("FireFighter"))
			{
				RefVars.ForceUpdate(FunMode.FireFighter);
			}
			GUILayout.EndArea();
		}
		private void BuildResetOrder()
		{
			GUILayout.BeginArea(new Rect(10, 100, 139, 20));
			if (GUILayout.Button("Reset Order"))
			{
				RefVars.ToggleFunMode(FunMode.ResetOrder);
			}
			GUILayout.EndArea();
		}
		private void BuildThemeBuilder()
		{
			GUILayout.BeginArea(new Rect(646, 10, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label("Theme Builder");
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 40, 139, 20));
			GUILayout.Label("Exclusive: " + RefVars.SelectedExclusiveLevel);
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 70, 139, 20));
			RefVars.SelectedExclusiveLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(RefVars.SelectedExclusiveLevel, 0, 9));
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 100, 139, 20));
			GUILayout.Label("Affordable: " + RefVars.SelectedAffordableLevel);
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 130, 139, 20));
			RefVars.SelectedAffordableLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(RefVars.SelectedAffordableLevel, 0, 9));
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 160, 139, 20));
			GUILayout.Label("Charming: " + RefVars.SelectedCharmingLevel);
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 190, 139, 20));
			RefVars.SelectedCharmingLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(RefVars.SelectedCharmingLevel, 0, 9));
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 220, 139, 20));
			GUILayout.Label("Formal: " + RefVars.SelectedFormalLevel);
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 250, 139, 20));
			RefVars.SelectedFormalLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(RefVars.SelectedFormalLevel, 0, 9));
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 280, 139, 20));
			GUILayout.Label("Kitchen: " + RefVars.SelectedKitchenLevel);
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 310, 139, 20));
			RefVars.SelectedKitchenLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(RefVars.SelectedKitchenLevel, 0, 9));
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 340, 139, 20));
			if (GUILayout.Button("Toggle", new GUILayoutOption[] { GUILayout.Height(19) }))
			{
				RefVars.ToggleFunMode(FunMode.Theme);
			}
			GUILayout.EndArea();
		}
		private void BuildApplianceDestroyer()
		{
			GUILayout.BeginArea(new Rect(169, 10, 139, 20));
			if (GUILayout.Button("Destroy"))
			{
				RefVars.ToggleFunMode(FunMode.Destroy);
				//CommandViewHelpers.Main.test();
			}
			GUILayout.EndArea();
		}
		private void BuildOrderAdder()
		{
			GUILayout.BeginArea(new Rect(417, 10, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label("Order Adder");
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(347, 40, 279, 20));
			if (RefVars.SelectedOrder != 0)
				GUILayout.Label(RefVars.GetKeyPair(RefVars.SelectedOrder));
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(347, 70, 279, 200));
			orderchangerScrollPosition = GUILayout.BeginScrollView(orderchangerScrollPosition, false, false, GUIStyle.none, GUIStyle.none);
			GUIStyle tmp = new GUIStyle("button");
			tmp.alignment = TextAnchor.MiddleLeft;
			foreach (int itemID in RefVars.AvailableItems.Keys)
			{
				if (RefVars.GetKeyPair(itemID).ToLower().Contains(ItemSearch.ToLower()))
				{
					if (GUILayout.Button(RefVars.GetKeyPair(itemID), tmp))
					{
						RefVars.SelectedOrder = itemID;
					}
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(348, 280, 129, 20));
			ItemSearch = GUILayout.TextField(ItemSearch);
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(497, 280, 129, 20));
			if (GUILayout.Button("Add Order"))
			{
				RefVars.ToggleFunMode(FunMode.ChangeOrder);
			}
			GUILayout.EndArea();
		}
		#endregion

		private void BuildItemPage()
		{
			//BuildChop();
			//BuildClean();
			//BuildCook();
			//BuildExtinguishFire();
			//BuildFillCoffee();
			//BuildKnead();
			BuildProcessList();
		}

		#region BuildItemPage
		private void BuildChop()
		{
			GUILayout.BeginArea(new Rect(10, 10, 139, 20));
			if (GUILayout.Button("Chop"))
			{
				RefVars.ToggleProcessMode(ProcessType.Chop);
			}
			GUILayout.EndArea();
		}
		private void BuildClean()
		{
			GUILayout.BeginArea(new Rect(169, 10, 139, 20));
			if (GUILayout.Button("Clean"))
			{
				RefVars.ToggleProcessMode(ProcessType.Clean);
			}
			GUILayout.EndArea();
		}
		private void BuildCook()
		{
			GUILayout.BeginArea(new Rect(328, 10, 139, 20));
			if (GUILayout.Button("Cook"))
			{
				RefVars.ToggleProcessMode(ProcessType.Cook);
			}
			GUILayout.EndArea();
		}
		private void BuildExtinguishFire()
		{
			GUILayout.BeginArea(new Rect(487, 10, 139, 20));
			if (GUILayout.Button("ExtinguishFire"))
			{
				RefVars.ToggleProcessMode(ProcessType.ExtinguishFire);
			}
			GUILayout.EndArea();
		}
		private void BuildFillCoffee()
		{
			GUILayout.BeginArea(new Rect(646, 10, 139, 20));
			if (GUILayout.Button("FillCoffee"))
			{
				RefVars.ToggleProcessMode(ProcessType.FillCoffee);
			}
			GUILayout.EndArea();
		}
		private void BuildKnead()
		{
			GUILayout.BeginArea(new Rect(10, 40, 139, 20));
			if (GUILayout.Button("Knead"))
			{
				RefVars.ToggleProcessMode(ProcessType.Knead);
			}
			GUILayout.EndArea();
		}
		private void BuildProcessList()
		{
			GUILayout.BeginArea(new Rect(10, 10, 278, 200));
			processScrollPosition = GUILayout.BeginScrollView(processScrollPosition, false, false, GUIStyle.none, GUIStyle.none);
			
			foreach (int processID in RefVars.AvailableProcesses.Keys)
			{
				if (GUILayout.Button(RefVars.AvailableProcesses[processID]))
				{
					RefVars.SelectedProcess = processID;
				}
			}
			
			GUILayout.EndScrollView();
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 220, 278, 20));
			if (RefVars.SelectedProcess != 0)
				GUILayout.Label($"Selected Process: {RefVars.GetKeyPair(RefVars.SelectedProcess)}");
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 250, 129, 20));
			RefVars.ReversedProcess = GUILayout.Toggle(RefVars.ReversedProcess, "Reversed");
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(159, 250, 129, 20));
			if (GUILayout.Button("Toggle"))
			{
				RefVars.ToggleFunMode(FunMode.NewProcess);
			}
			GUILayout.EndArea();
		}
		#endregion

		private void BuildWorldPage()
		{
			BuildMessMaker();
			BuildBlindnessToggle();
			BuildGarbageChanger();
			BuildProviderChanger();
		}

		#region BuildWorldPage
		private void BuildMessMaker()
		{
			GUILayout.BeginArea(new Rect(10, 10, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label("Mess Spawner");
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 40, 139, 20));
			RefVars.SelectedMessLevel = (int)Mathf.Round(GUILayout.HorizontalSlider(RefVars.SelectedMessLevel, 1, 3));
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 70, 139, 20));
			RefVars.IsMessKitchen = GUILayout.Toggle(RefVars.IsMessKitchen, "IsKitchen");
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 100, 139, 20));
			if (GUILayout.Button("Mess", new GUILayoutOption[] { GUILayout.Height(19) }))
			{
				RefVars.ToggleFunMode(FunMode.Mess);
			}
			GUILayout.EndArea();
		}
		private void BuildBlindnessToggle()
		{
			GUILayout.BeginArea(new Rect(169, 10, 139, 20));
			if (GUILayout.Button("Blindness", new GUILayoutOption[] { GUILayout.Height(19) }))
			{
				RefVars.ForceUpdate(FunMode.Blindness);
			}
			GUILayout.EndArea();
		}
		private void BuildGarbageChanger()
		{
			GUILayout.BeginArea(new Rect(169, 40, 139, 20));
			if (GUILayout.Button("Garbage", new GUILayoutOption[] { GUILayout.Height(19) }))
			{
				RefVars.ToggleFunMode(FunMode.Garbage);
			}
			GUILayout.EndArea();
		}
		private void BuildProviderChanger()
		{
			GUILayout.BeginArea(new Rect(169, 70, 139, 20));
			if (GUILayout.Button("ItemProvider", new GUILayoutOption[] { GUILayout.Height(19) }))
			{
				RefVars.ToggleFunMode(FunMode.ItemProvider);
			}
			GUILayout.EndArea();
		}
		#endregion

		private void BuildMiscPage()
		{
			BuildCustomerSpawner();
			BuildBlueprintSpawner();
			BuildDishSelector();
			BuildLobbyJoiner();
			BuildMoneyEditor();
		}

		#region BuildMiscPage
		private void BuildCustomerSpawner()
		{
			GUILayout.BeginArea(new Rect(10, 10, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label("Customer Spawner");
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 40, 139, 20));
			GUILayout.Label("Customers: " + RefVars.SelectedCustomerCount);
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 70, 139, 20));
			RefVars.SelectedCustomerCount = (int)Mathf.Round(GUILayout.HorizontalSlider(RefVars.SelectedCustomerCount, 0, 10));
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 100, 139, 20));
			RefVars.SelectedIsCustomerCat = GUILayout.Toggle(RefVars.SelectedIsCustomerCat, "isCat");
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 130, 139, 20));
			if (GUILayout.Button("Apply", new GUILayoutOption[] { GUILayout.Height(19) }))
			{
				RefVars.ForceUpdate(FunMode.CustomerSpawn);
			}
			GUILayout.EndArea();
		}
		private void BuildBlueprintSpawner()
		{
			GUILayout.BeginArea(new Rect(169, 10, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label("Blueprint Spawner");
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 40, 139, 20));
			GUILayout.Label("Manual ID");
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 70, 139, 20));
			try
			{
				RefVars.SelectedManualBlueprint = int.Parse(GUILayout.TextField(RefVars.SelectedManualBlueprint.ToString()));
			}
			catch
			{
				RefVars.SelectedManualBlueprint = 0;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 100, 139, 20));
			GUILayout.Label("Forced Price");
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 130, 139, 20));
			try
			{
				RefVars.ForcedAppliancePrice = int.Parse(GUILayout.TextField(RefVars.ForcedAppliancePrice.ToString()));
			}
			catch
			{
				RefVars.ForcedAppliancePrice = 0;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 160, 139, 20));
			ApplianceSearch = GUILayout.TextField(ApplianceSearch);
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(135, 190, 208, 200));
			blueprintSelectorScrollPosition = GUILayout.BeginScrollView(blueprintSelectorScrollPosition, false, false, GUIStyle.none, GUIStyle.none);
			foreach (int applianceID in RefVars.AvailableAppliances.Keys)
			{
				if (RefVars.GetKeyPair(applianceID).ToLower().Contains(ApplianceSearch.ToLower()) && !string.IsNullOrEmpty(RefVars.GetKeyPair(applianceID)))
				{
					if (GUILayout.Button(RefVars.GetKeyPair(applianceID)))
					{
						RefVars.SelectedAppliance = applianceID;
					}
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 400, 139, 20));
			RefVars.IsRedAppliance = GUILayout.Toggle(RefVars.IsRedAppliance, "Is Red");
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 430, 139, 20));
			if (GUILayout.Button("Spawn", new GUILayoutOption[] { GUILayout.Height(19) }))
			{
				RefVars.ToggleFunMode(FunMode.BlueprintSpawn);
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 460, 139, 20));
			if (GUILayout.Button("Spawn Manual", new GUILayoutOption[] { GUILayout.Height(19) }))
			{
				RefVars.ToggleFunMode(FunMode.ManualBlueprintSpawn);
			}
			GUILayout.EndArea();
		}

		private void BuildDishSelector()
		{
			GUILayout.BeginArea(new Rect(398, 10, 139, 20));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label("Unlock Selector");
			GUI.skin.label.fontSize = defaultFontSize;
			GUI.skin.label.alignment = defaultTextAnchor;
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(398, 40, 139, 20));
			UnlockSearch = GUILayout.TextField(UnlockSearch);
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(363, 70, 208, 200));
			unlockSelectorScrollPosition = GUILayout.BeginScrollView(unlockSelectorScrollPosition, false, false, GUIStyle.none, GUIStyle.none);
			foreach (int unlockID in RefVars.AvailableUnlocks.Keys)
			{
				string name = RefVars.AvailableUnlocks[unlockID];
				if (RefVars.ActiveUnlocks.Contains(unlockID))
				{
					name = $"*{RefVars.AvailableUnlocks[unlockID]}*";
				}
				if (name.ToLower().Contains(UnlockSearch.ToLower()))
				{
					if (GUILayout.Button(name))
					{
						RefVars.SelectedUnlock = unlockID;
						RefVars.ForceUpdate(FunMode.Unlock);
					}
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
		
		private void BuildLobbyJoiner()
		{
			GUILayout.BeginArea(new Rect(646, 10, 139, 20));
			LobbyID = GUILayout.TextArea(LobbyID);
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(646, 40, 139, 20));
			if (GUILayout.Button("Join Lobby"))
			{
				if (!LobbyID.IsNullOrEmpty())
				{
					Session.JoinGame(new SteamNetworkTarget(new SteamId
					{
						Value = ulong.Parse(LobbyID)
					}), true);
				}
			}
			GUILayout.EndArea();
		}

		private void BuildMoneyEditor()
		{
			GUILayout.BeginArea(new Rect(10, 490, 139, 20));
			GUILayout.Label("Money: " + Money);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(10, 520, 298, 20));
			Money = (int)Mathf.Round(GUILayout.HorizontalSlider(Money, 0, 10000));
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 550, 139, 20));
			if (GUILayout.Button("Update"))
			{
				Money = RefVars.CurrentMoney;
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 550, 139, 20));
			if (GUILayout.Button("Apply"))
			{
				RefVars.SelectedMoney = (int)Mathf.Round(Money);
				RefVars.ForceUpdate(FunMode.Money);
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(10, 580, 59, 20));
			if (GUILayout.Button("-100"))
			{
				RefVars.SelectedMoney = RefVars.CurrentMoney - 100;
				RefVars.ForceUpdate(FunMode.Money);
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(89, 580, 60, 20));
			if (GUILayout.Button("-10"))
			{
				RefVars.SelectedMoney = RefVars.CurrentMoney - 10;
				RefVars.ForceUpdate(FunMode.Money);
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(169, 580, 59, 20));
			if (GUILayout.Button("+10"))
			{
				RefVars.SelectedMoney = RefVars.CurrentMoney + 10;
				RefVars.ForceUpdate(FunMode.Money);
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(248, 580, 60, 20));
			if (GUILayout.Button("+100"))
			{
				RefVars.SelectedMoney = RefVars.CurrentMoney + 100;
				RefVars.ForceUpdate(FunMode.Money);
			}
			GUILayout.EndArea();
		}
		#endregion
	}

	public enum MenuPage
	{
		Player,
		Appliance,
		Item,
		World,
		Misc
	}
}
