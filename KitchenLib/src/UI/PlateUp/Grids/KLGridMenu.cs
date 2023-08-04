using System.Collections.Generic;
using Kitchen;
using Kitchen.Modules;
using UnityEngine;

namespace KitchenLib.UI.PlateUp.Grids
{

	public abstract class KLPageGridMenu<TItem> : GridMenu
	{
		protected virtual int RowLength
		{
			get
			{
				return 5;
			}
		}

		protected virtual int ColumnLength
		{
			get
			{
				return 4;
			}
		}

		protected int MaxPerGroup
		{
			get
			{
				return RowLength * ColumnLength;
			}
		}

		protected virtual float ElementWidth
		{
			get
			{
				return 0.5f;
			}
		}

		protected virtual float ElementHeight
		{
			get
			{
				return 0.5f;
			}
		}

		protected virtual float Padding
		{
			get
			{
				return 0.1f;
			}
		}

		protected virtual GridMenuElement GetPrefab()
		{
			return ModuleDirectory.Main.GetPrefab<GridMenuElement>();
		}

		private List<TItem> Items;
		private bool HasBack;

		public KLPageGridMenu(List<TItem> items, Transform container, int player, bool has_back)
		{
			Container = container;
			Player = player;
			Panel = UnityEngine.Object.Instantiate<PanelElement>(ModuleDirectory.Main.GetPrefab<PanelElement>(), container, false);
			Items = items;
			HasBack = has_back;
			CreateElements(Items, HasBack);
		}

		protected void Redraw(List<TItem> list, bool has_back, int page = 0, bool isForward = false)
		{
			Grid.Destroy();
			CreateElements(Items, HasBack, page, isForward);
		}

		protected void CreateElements(List<TItem> list, bool has_back, int page = 0, bool isForward = false)
		{
			GridMenuElement prefab = GetPrefab();

			Grid = new ModuleGrid
			{
				RowLength = RowLength,
				ColumnLength = ColumnLength,
				XSpacing = ElementWidth,
				YSpacing = ElementHeight,
				Padding = Padding
			};


			// Setup Back Button
			GridMenuElement backButton = UnityEngine.Object.Instantiate(prefab, this.Container, false);
			backButton.SetAsBack();
			backButton.Set(Main.bundle.LoadAsset<Texture2D>("previousPage"));
			if (page == 0)
			{
				backButton.OnActivate += RequestGoBack;
			}
			else
			{
				backButton.OnActivate += () =>
				{
					Redraw(list, has_back, --page, false);
				};
			}
			Grid.AddModule(backButton);

			// Add Blank Spaces
			for (int i = 0; i < RowLength-2; i++)
			{
				Grid.AddModule(UnityEngine.Object.Instantiate(prefab, this.Container, false));
			}


			// Setup Icons

			int AvailableSlots = ((RowLength * ColumnLength) - RowLength);
			int LoadedCosmetics = 0;
			int StartingIndex = 0;
			if (page == 0)
				StartingIndex = 0;
			else
				StartingIndex = page * AvailableSlots;

			// Setup Next Page Button
			if ((list.Count - StartingIndex) > AvailableSlots)
			{
				GridMenuElement nextButton = UnityEngine.Object.Instantiate(prefab, this.Container, false);
				nextButton.OnActivate += () =>
				{
					Redraw(list, has_back, ++page, true);
				};
				nextButton.Set(Main.bundle.LoadAsset<Texture2D>("nextPage"));
				Grid.AddModule(nextButton);
			}
			else
			{
				Grid.AddModule(UnityEngine.Object.Instantiate(prefab, this.Container, false));
			}


			for (int i = StartingIndex; list.Count > 0; i++)
			{
				if (list.Count >= i + 1)
				{
					TItem item = list[i];
					CreateItem(item);
					LoadedCosmetics++;
					if (LoadedCosmetics >= AvailableSlots)
					{
						break;
					}
				}
				else
				{
					break;
				}
			}

			// Pre-Select

			if (isForward)
			{
				Grid.Select(Grid.Modules[RowLength-1].Module);
			}

			// Setup Blanks

			int BlankCount = AvailableSlots - LoadedCosmetics;

			for (int i = 0; i < BlankCount; i++)
			{
				Grid.AddModule(UnityEngine.Object.Instantiate(prefab, this.Container, false));
			}
			this.Panel.SetTarget(this.Grid);
			this.Panel.SetColour(this.Player);
		}

		private void CreateItem(TItem item)
		{
			GridMenuElement gridMenuElement2 = UnityEngine.Object.Instantiate<GridMenuElement>(GetPrefab(), Container, false);
			SetupElement(item, gridMenuElement2, Player);
			gridMenuElement2.OnActivate += delegate ()
			{
				OnSelect(item);
			};
			Grid.AddModule(gridMenuElement2);
		}

		protected abstract void SetupElement(TItem item, GridMenuElement element, int playerID = 0);

		protected abstract void OnSelect(TItem item);
	}
}
