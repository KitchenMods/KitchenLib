using KitchenData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Utils
{
	public class LocalisationUtils
	{

		#region Obsolete

		[Obsolete("Use LocalisationBuilder")]
		public static ApplianceInfo CreateApplianceInfo(string name, string description, List<Appliance.Section> sections, List<string> tags)
		{
			return LocalisationBuilder.NewBuilder<ApplianceInfo>().SetName(name).SetDescription(description).SetSections(sections).SetTags(tags);
		}

		[Obsolete("Use LocalisationBuilder")]
		public static CosmeticInfo CreateCosmeticInfo(string name, string description)
		{
			return LocalisationBuilder.NewBuilder<CosmeticInfo>().SetName(name).SetDescription(description);
		}

		[Obsolete("Use LocalisationBuilder")]
		public static EffectInfo CreateEffectInfo(string name, string description, string icon)
		{
			return LocalisationBuilder.NewBuilder<EffectInfo>().SetName(name).SetDescription(description).SetInfoIcon(icon);
		}

		[Obsolete("Use LocalisationBuilder")]
		public static ProcessInfo CreateProcessInfo(string name, string icon)
		{
			return LocalisationBuilder.NewBuilder<ProcessInfo>().SetName(name).SetIcon(icon);
		}

		[Obsolete("Use LocalisationBuilder")]
		public static ResearchLocalisation CreateResearchLocalisation(string name, string description, string flavourText)
		{
			return LocalisationBuilder.NewBuilder<ResearchLocalisation>().SetName(name).SetDescription(description).SetFlavourText(flavourText);
		}

		[Obsolete("Use LocalisationBuilder")]
		public static UnlockInfo CreateUnlockInfo(string name, string description, string flavourText)
		{
			return LocalisationBuilder.NewBuilder<UnlockInfo>().SetName(name).SetDescription(description).SetFlavourText(flavourText);
		}
		
		[Obsolete("Use LocalisationBuilder")]
		public static BasicInfo CreateBasicInfo(string name, string description)
		{
			return LocalisationBuilder.NewBuilder<BasicInfo>().SetName(name).SetDescription(description);
		}

		[Obsolete("Use LocalisationBuilder")]
		public static ContractInfo CreateContractInfo(string name, string description)
		{
			return LocalisationBuilder.NewBuilder<ContractInfo>().SetName(name).SetDescription(description);
		}

		[Obsolete("Use LocalisationBuilder")]
		public static DecorationBonusInfo CreateDecorationBonusInfo(Dictionary<DecorationType, string> icons, Dictionary<DecorationBonus, string> text)
		{
			return LocalisationBuilder.NewBuilder<DecorationBonusInfo>().SetIcons(icons).SetText(text);
		}

		[Obsolete("Use LocalisationBuilder")]
		public static DictionaryInfo CreateDictionaryInfo(Dictionary<string, string> text)
		{
			return LocalisationBuilder.NewBuilder<DictionaryInfo>().SetText(text);
		}

		[Obsolete("Use LocalisationBuilder")]
		public static PopupText CreatePopupText(Dictionary<PopupType, PopupDetails> text)
		{
			return LocalisationBuilder.NewBuilder<PopupText>().SetText(text);
		}

		[Obsolete("Use LocalisationBuilder")]
		public static RecipeInfo CreateRecipeInfo(Dictionary<Dish, string> text)
		{
			return LocalisationBuilder.NewBuilder<RecipeInfo>().SetText(text);
		}

		[Obsolete("Use LocalisationBuilder")]
		public static TutorialText CreateTutorialText(Dictionary<TutorialMessage, TutorialDetails> text)
		{
			return LocalisationBuilder.NewBuilder<TutorialText>().SetText(text);
		}

		#endregion

		#region Active
		
		public static EnumInfo<T> CreateEnumInfo<T>(Dictionary<T, string> name) where T : Enum
		{
			var enumInfo = ScriptableObject.CreateInstance<EnumInfo<T>>();

			enumInfo.Name = name;

			return enumInfo;
		}

		public static EnumBasicInfo<T> CreateEnumBasicInfo<T>(Dictionary<T, GenericLocalisationStruct> text) where T : Enum
		{
			var enumBasicInfo = ScriptableObject.CreateInstance<EnumBasicInfo<T>>();

			enumBasicInfo.Text = text;

			return enumBasicInfo;
		}

		#endregion
	}
}
