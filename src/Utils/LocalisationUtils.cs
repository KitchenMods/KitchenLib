using KitchenData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Utils
{
	public class LocalisationUtils
	{
		#region Helpers for Custom GDOs

		public static ApplianceInfo CreateApplianceInfo(string name, string description, List<Appliance.Section> sections, List<string> tags)
		{
			var applianceInfo = ScriptableObject.CreateInstance<ApplianceInfo>();

			applianceInfo.Name = name;
			applianceInfo.Description = description;
			applianceInfo.Sections = sections;
			applianceInfo.Tags = tags;

			return applianceInfo;
		}

		public static CosmeticInfo CreateCosmeticInfo(string name, string description)
		{
			var cosmeticInfo = ScriptableObject.CreateInstance<CosmeticInfo>();

			cosmeticInfo.Name = name;
			cosmeticInfo.Description = description;

			return cosmeticInfo;
		}

		public static EffectInfo CreateEffectInfo(string name, string description, string icon)
		{
			var effectInfo = ScriptableObject.CreateInstance<EffectInfo>();

			effectInfo.Name = name;
			effectInfo.Description = description;
			effectInfo.Icon = icon;

			return effectInfo;
		}

		public static ProcessInfo CreateProcessInfo(string name, string icon)
		{
			var processInfo = ScriptableObject.CreateInstance<ProcessInfo>();

			processInfo.Name = name;
			processInfo.Icon = icon;

			return processInfo;
		}

		public static ResearchLocalisation CreateResearchLocalisation(string name, string description, string flavourText)
		{
			var researchLocalisation = ScriptableObject.CreateInstance<ResearchLocalisation>();

			researchLocalisation.Name = name;
			researchLocalisation.Description = description;
			researchLocalisation.FlavourText = flavourText;

			return researchLocalisation;
		}

		public static UnlockInfo CreateUnlockInfo(string name, string description, string flavourText)
		{
			var unlockInfo = ScriptableObject.CreateInstance<UnlockInfo>();

			unlockInfo.Name = name;
			unlockInfo.Description = description;
			unlockInfo.FlavourText = flavourText;

			return unlockInfo;
		}

		#endregion

		#region Helpers for Other Localisation Subclasses

		public static BasicInfo CreateBasicInfo(string name, string description)
		{
			var basicInfo = ScriptableObject.CreateInstance<BasicInfo>();

			basicInfo.Name = name;
			basicInfo.Description = description;

			return basicInfo;
		}

		public static ContractInfo CreateContractInfo(string name, string description)
		{
			var contractInfo = ScriptableObject.CreateInstance<ContractInfo>();

			contractInfo.Name = name;
			contractInfo.Description = description;

			return contractInfo;
		}

		public static DecorationBonusInfo CreateDecorationBonusInfo(Dictionary<DecorationType, string> icons, Dictionary<DecorationBonus, string> text)
		{
			var decorationBonusInfo = ScriptableObject.CreateInstance<DecorationBonusInfo>();

			decorationBonusInfo.Icons = icons;
			decorationBonusInfo.Text = text;

			return decorationBonusInfo;
		}

		public static DictionaryInfo CreateDictionaryInfo(Dictionary<string, string> text)
		{
			var dictionaryInfo = ScriptableObject.CreateInstance<DictionaryInfo>();

			dictionaryInfo.Text = text;

			return dictionaryInfo;
		}

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

		public static PopupText CreatePopupText(Dictionary<PopupType, PopupDetails> text)
		{
			var popupText = ScriptableObject.CreateInstance<PopupText>();

			popupText.Text = text;

			return popupText;
		}

		public static RecipeInfo CreateRecipeInfo(Dictionary<Dish, string> text)
		{
			var recipeInfo = ScriptableObject.CreateInstance<RecipeInfo>();

			recipeInfo.Text = text;

			return recipeInfo;
		}

		public static TutorialText CreateTutorialText(Dictionary<TutorialMessage, TutorialDetails> text)
		{
			var tutorialText = ScriptableObject.CreateInstance<TutorialText>();

			tutorialText.Text = text;

			return tutorialText;
		}

		#endregion
	}
}
