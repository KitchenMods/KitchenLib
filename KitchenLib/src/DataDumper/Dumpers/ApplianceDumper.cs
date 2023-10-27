using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class ApplianceDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder applianceDump = new StringBuilder();
			StringBuilder applianceProcessesDump = new StringBuilder();
			StringBuilder appliancePropertiesDump = new StringBuilder();
			StringBuilder applianceRequiresForShopDump = new StringBuilder();
			StringBuilder applianceRequiresProcessForShopDump = new StringBuilder();
			StringBuilder applianceUpgradesDump = new StringBuilder();
			StringBuilder applianceEnchantmentsDump = new StringBuilder();
			StringBuilder applianceSectionDump = new StringBuilder();
			StringBuilder applianceRequiresPhaseForShopDump = new StringBuilder();

			applianceDump.AppendLine("ID,Type,Prefab,HeldAppliancePrefab,EffectRange," +
				"EffectCondition,EffectType,EffectRepresentation,IsNonInteractive,Layer," +
				"ForceHighInteractionPriority,PurchaseCost,EntryAnimation,ExitAnimation,SkipRotationAnimation," +
				"IsPurchasable,IsPurchasableAsUpgrade,ThemeRequired,ShoppingTags,RarityTier," +
				"ShopRequirementFilter,StapleWhenMissing,SellOnlyAsDuplicate,SellOnlyAsUnique,PreventSale,IsAnUpgrade," +
				"IsNonCrated,CrateItem,Name,Description");
			applianceProcessesDump.AppendLine("ID,Type,Process,IsAutomatic,Speed,Validity");
			appliancePropertiesDump.AppendLine("ID,Type,IApplianceProperty");
			applianceRequiresForShopDump.AppendLine("ID,Type,Appliance");
			applianceRequiresProcessForShopDump.AppendLine("ID,Type,Process");
			applianceUpgradesDump.AppendLine("ID,Type,Upgrade");
			applianceEnchantmentsDump.AppendLine("ID,Type,Enchantment");
			applianceSectionDump.AppendLine("ID,Type,Title,Description,RangeDescription");
			applianceRequiresPhaseForShopDump.AppendLine("ID,Type,Phase");

			foreach (Appliance appliance in GameData.Main.Get<Appliance>())
			{
				applianceDump.AppendLine($"{appliance.ID},{appliance.name},{appliance.Prefab},{appliance.HeldAppliancePrefab},{appliance.EffectRange}," +
					$"{appliance.EffectCondition},{appliance.EffectType},{appliance.EffectRepresentation},{appliance.IsNonInteractive},{appliance.Layer}," +
					$"{appliance.ForceHighInteractionPriority},{appliance.PurchaseCost},{appliance.EntryAnimation},{appliance.ExitAnimation},{appliance.SkipRotationAnimation}," +
					$"{appliance.IsPurchasable},{appliance.IsPurchasableAsUpgrade},{appliance.ThemeRequired},{appliance.ShoppingTags.ToString().Replace(",", ";")},{appliance.RarityTier}," +
					$"{appliance.ShopRequirementFilter},{appliance.StapleWhenMissing},{appliance.SellOnlyAsDuplicate},{appliance.SellOnlyAsUnique},{appliance.PreventSale},{appliance.IsAnUpgrade}," +
					$"{appliance.IsNonCrated},{appliance.CrateItem},{appliance.Name},{appliance.Description.Replace(",", "")}");

				foreach (Appliance.ApplianceProcesses process in appliance.Processes)
					applianceProcessesDump.AppendLine($"{appliance.ID},{appliance.name},{process.Process},{process.IsAutomatic},{process.Speed},{process.Validity}");

				foreach (IApplianceProperty property in appliance.Properties)
					appliancePropertiesDump.AppendLine($"{appliance.ID},{appliance.name},{property}");

				foreach (Appliance appliance2 in appliance.RequiresForShop)
					applianceRequiresForShopDump.AppendLine($"{appliance.ID},{appliance.name},{appliance2}");

				foreach (Process process in appliance.RequiresProcessForShop)
					applianceRequiresProcessForShopDump.AppendLine($"{appliance.ID},{appliance.name},{process}");

				foreach (Appliance upgrade in appliance.Upgrades)
					applianceUpgradesDump.AppendLine($"{appliance.ID},{appliance.name},{upgrade}");

				foreach (Appliance enchantment in appliance.Enchantments)
					applianceEnchantmentsDump.AppendLine($"{appliance.ID},{appliance.name},{enchantment}");

				foreach (Appliance.Section section in appliance.Sections)
					applianceSectionDump.AppendLine($"{appliance.ID},{appliance.name},{section.Title},{section.Description},{section.RangeDescription}");

				foreach (MenuPhase phase in appliance.RequiresPhaseForShop)
					applianceRequiresPhaseForShopDump.AppendLine($"{appliance.ID},{appliance.name},{phase}");
			}

			SaveCSV("Appliance", "Appliances", applianceDump);
			SaveCSV("Appliance", "ApplianceProcesses", applianceProcessesDump);
			SaveCSV("Appliance", "ApplianceProperties", appliancePropertiesDump);
			SaveCSV("Appliance", "ApplianceRequiresForShop", applianceRequiresForShopDump);
			SaveCSV("Appliance", "ApplianceRequiresProcessForShop", applianceRequiresProcessForShopDump);
			SaveCSV("Appliance", "ApplianceUpgrades", applianceUpgradesDump);
			SaveCSV("Appliance", "ApplianceEnchantments", applianceEnchantmentsDump);
			SaveCSV("Appliance", "ApplianceSections", applianceSectionDump);
			SaveCSV("Appliance", "ApplianceRequiresPhaseForShop", applianceRequiresPhaseForShopDump);
		}
	}
}
