using Kitchen;
using Kitchen.Modules;
using KitchenData;
using KitchenLib.Preferences;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.UI.PlateUp.Grids
{
	public class KLCosmeticGridMenu : KLPageGridMenu<PlayerCosmetic>
	{
		public KLCosmeticGridMenu(List<PlayerCosmetic> cosmetics, Transform container, int player, bool has_back) : base(cosmetics, container, player, has_back)
		{
		}

		protected override int RowLength => Main.manager.GetPreference<PreferenceInt>("cosmeticWidth").Value;
		protected override int ColumnLength => Main.manager.GetPreference<PreferenceInt>("cosmeticHeight").Value;

		protected override void SetupElement(PlayerCosmetic item, GridMenuElement element, int playerID)
		{
			element.Set(item);
			//FieldInfo info = ReflectionUtils.GetField<GridMenuElement>("CosmeticMaterial");
			//((Material)info.GetValue(element)).SetTexture(Shader.PropertyToID("_Image"), Main.GetCosmeticSnapshot(item, Players.Main.Get(playerID).Profile.Colour, 512, 512));

		}

		protected override void OnSelect(PlayerCosmetic item)
		{
			if (Player != 0 && item != null)
			{
				//Players.Main.SetCosmetic(this.Player, item);
				ProfileAccessor.SetCosmetic(Player, item);
			}
		}
	}

	public class KLGridMenuCosmeticConfig : GridMenuConfig
	{
		public override GridMenu Instantiate(Transform container, int player, bool has_back)
		{
			return new KLCosmeticGridMenu(this.Cosmetics, container, player, has_back);
		}

		public List<PlayerCosmetic> Cosmetics;
	}

	public class KLGridMenuHatConfig : GridMenuConfig
	{
		public override GridMenu Instantiate(Transform container, int player, bool has_back)
		{
			return new KLCosmeticGridMenu(this.Cosmetics, container, player, has_back);
		}

		public List<PlayerCosmetic> Cosmetics;
	}
}
