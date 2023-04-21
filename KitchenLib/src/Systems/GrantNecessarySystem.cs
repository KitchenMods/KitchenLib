using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using KitchenMods;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Systems
{
	[UpdateAfter(typeof(GrantNecessaryAppliances))]
	internal class GrantNecessarySystem : NightSystem, IModSystem
	{
		private static EntityQuery Unlocks;
		private EntityQuery CreateAppliances;
		private EntityQuery Providers;
		private EntityQuery Parcels;
		protected override void Initialise()
		{
			Unlocks = GetEntityQuery(new QueryHelper().All(typeof(CProgressionUnlock)));
			CreateAppliances = GetEntityQuery(new QueryHelper().All(typeof(CCreateAppliance)));
			Providers = GetEntityQuery(new QueryHelper().All(typeof(CAppliance)).Any(typeof(CItemProvider)));
			Parcels = GetEntityQuery(new QueryHelper().All(typeof(CLetterAppliance)));
		}

		protected override void OnUpdate()
		{
			if (!CreateAppliances.IsEmpty)
				return;

			using var providers = Providers.ToComponentDataArray<CAppliance>(Allocator.Temp);
			using var letters = Parcels.ToComponentDataArray<CLetterAppliance>(Allocator.Temp);

			Dictionary<int, int> ProvidersOfType = new();
			foreach (var appliance in providers)
			{
				if (!ProvidersOfType.TryGetValue(appliance.ID, out int current))
					ProvidersOfType[appliance.ID] = 0;
				ProvidersOfType[appliance.ID]++;
			}
			foreach (var letter in letters)
			{
				if (!ProvidersOfType.TryGetValue(letter.ApplianceID, out int current))
					ProvidersOfType[letter.ApplianceID] = 0;
				ProvidersOfType[letter.ApplianceID]++;
			}

			Dictionary<int, int> CountedItems = new();
			foreach (var pair in ProvidersOfType)
			{
				if (pair.Value <= 0 || !GameData.Main.TryGet<Appliance>(pair.Key, out var appliance, false) || !appliance.GetProperty<CItemProvider>(out var provider))
					continue;

				if (GDOUtils.AutomatedParcelItems.Contains(provider.DefaultProvidedItem))
				{
					if (!CountedItems.TryGetValue(provider.DefaultProvidedItem, out var itemCount))
						CountedItems[provider.DefaultProvidedItem] = 0;
					CountedItems[provider.DefaultProvidedItem] += provider.Maximum * pair.Value;
				}
			}

			var maxSize = GetOrDefault<SKitchenParameters>().Parameters.MaximumGroupSize;
			int offset = 0;
			foreach (var pair in CountedItems)
			{
				if (!GameRequiresItem(pair.Key))
					continue;

				if (pair.Value < maxSize)
				{
					var postTiles = GetPostTiles(false);
					var parcelTile = GetParcelTile(postTiles, ref offset);
					if (GameData.Main.TryGet<Item>(pair.Key, out var item, false) && item.DedicatedProvider != null)
					{
						PostHelpers.CreateApplianceParcel(EntityManager, parcelTile, item.DedicatedProvider.ID);
					}
				}
			}
		}

		private Vector3 GetParcelTile(List<Vector3> tiles, ref int offset)
		{
			Vector3 vector = Vector3.zero;
			bool flag = false;
			while (!flag && offset < tiles.Count)
			{
				int num = offset;
				offset = num + 1;
				vector = tiles[num];
				flag |= GetOccupant(vector, OccupancyLayer.Default) == default(Entity) && !GetTile(vector).HasFeature;
			}
			return flag ? vector : GetFallbackTile();
		}

		internal static bool GameRequiresItem(int itemID)
		{
			using var unlocks = Unlocks.ToComponentDataArray<CProgressionUnlock>(Allocator.Temp);

			foreach (var unlock in unlocks)
			{
				if (unlock.Type != CardType.Default)
					continue;

				if (GameData.Main.TryGet<Dish>(unlock.ID, out var dish, false) && dish.MinimumIngredients.Any(item => item.ID == itemID))
					return true;
			}

			return false;
		}
	}
}
