using System.Collections.Generic;
using Kitchen;
using KitchenData;
using UnityEngine;

namespace KitchenLib.Utils
{
    public static class LocalisationBuilder
    {
        public static T NewBuilder<T>(Locale locale = Locale.English) where T : Localisation
        {
            T result = ScriptableObject.CreateInstance<T>();
            result.Locale = locale;
            return result;
        }

        #region ApplianceInfo
        
        public static ApplianceInfo SetName(this ApplianceInfo localisation, string name)
        {
            localisation.Name = name;
            return localisation;
        }

        public static ApplianceInfo SetDescription(this ApplianceInfo localisation, string description)
        {
            localisation.Description = description;
            return localisation;
        }

        public static ApplianceInfo SetSections(this ApplianceInfo localisation, List<Appliance.Section> sections)
        {
            localisation.Sections = sections;
            return localisation;
        }

        public static ApplianceInfo SetTags(this ApplianceInfo localisation, List<string> tags)
        {
            localisation.Tags = tags;
            return localisation;
        }        

        #endregion

        #region BasicInfo

        public static BasicInfo SetName(this BasicInfo localisation, string name)
        {
            localisation.Name = name;
            return localisation;
        }

        public static BasicInfo SetDescription(this BasicInfo localisation, string description)
        {
            localisation.Description = description;
            return localisation;
        }

        #endregion

        #region ContractInfo

        public static ContractInfo SetName(this ContractInfo localisation, string name)
        {
            localisation.Name = name;
            return localisation;
        }

        public static ContractInfo SetDescription(this ContractInfo localisation, string description)
        {
            localisation.Description = description;
            return localisation;
        }

        #endregion

        #region CosmeticInfo

        public static CosmeticInfo SetName(this CosmeticInfo localisation, string name)
        {
            localisation.Name = name;
            return localisation;
        }

        public static CosmeticInfo SetDescription(this CosmeticInfo localisation, string description)
        {
            localisation.Description = description;
            return localisation;
        }

        #endregion
        
        #region DecorationBonusInfo
        
        public static DecorationBonusInfo SetIcons(this DecorationBonusInfo localisation, Dictionary<DecorationType, string> icons)
        {
            localisation.Icons = icons;
            return localisation;
        }
        
        public static DecorationBonusInfo SetText(this DecorationBonusInfo localisation, Dictionary<DecorationBonus, string> text)
        {
            localisation.Text = text;
            return localisation;
        }
        
        #endregion

        #region DictionaryInfo

        public static DictionaryInfo SetText(this DictionaryInfo localisation, Dictionary<string, string> text)
        {
            localisation.Text = text;
            return localisation;
        }

        #endregion

        #region EffectInfo

        public static EffectInfo SetInfoIcon(this EffectInfo localisation, string icon)
        {
            localisation.Icon = icon;
            return localisation;
        }

        public static EffectInfo SetName(this EffectInfo localisation, string name)
        {
            localisation.Name = name;
            return localisation;
        }

        public static EffectInfo SetDescription(this EffectInfo localisation, string description)
        {
            localisation.Description = description;
            return localisation;
        }

        #endregion

        #region NewsItemFallbackInfo

        public static NewsItemFallbackInfo SetText(this NewsItemFallbackInfo localisation, Dictionary<NewsItemType, GenericLocalisationStruct> text)
        {
            localisation.Text = text;
            return localisation;
        }

        #endregion

        #region PopupText

        public static PopupText SetText(this PopupText localisation, Dictionary<PopupType, PopupDetails> text)
        {
            localisation.Text = text;
            return localisation;
        }

        #endregion

        #region ProcessInfo

        public static ProcessInfo SetName(this ProcessInfo localisation, string name)
        {
            localisation.Name = name;
            return localisation;
        }

        public static ProcessInfo SetIcon(this ProcessInfo localisation, string icon)
        {
            localisation.Icon = icon;
            return localisation;
        }

        #endregion

        #region RarityTierInfo

        public static RarityTierInfo SetName(this RarityTierInfo localisation, Dictionary<RarityTier, string> name)
        {
            localisation.Name = name;
            return localisation;
        }

        #endregion

        #region RecipeInfo

        public static RecipeInfo SetText(this RecipeInfo localisation, Dictionary<Dish, string> text)
        {
            localisation.Text = text;
            return localisation;
        }

        #endregion

        #region ResearchLocalisation

        public static ResearchLocalisation SetName(this ResearchLocalisation localisation, string name)
        {
            localisation.Name = name;
            return localisation;
        }

        public static ResearchLocalisation SetFlavourText(this ResearchLocalisation localisation, string flavourText)
        {
            localisation.FlavourText = flavourText;
            return localisation;
        }

        public static ResearchLocalisation SetDescription(this ResearchLocalisation localisation, string description)
        {
            localisation.Description = description;
            return localisation;
        }

        #endregion

        #region StartDayWarningInfo

        public static StartDayWarningInfo SetText(this StartDayWarningInfo localisation, Dictionary<StartDayWarning, GenericLocalisationStruct> text)
        {
            localisation.Text = text;
            return localisation;
        }

        #endregion

        #region TutorialText

        public static TutorialText SetText(this TutorialText localisation, Dictionary<TutorialMessage, TutorialDetails> text)
        {
            localisation.Text = text;
            return localisation;
        }

        #endregion

        #region UnlockInfo

        public static UnlockInfo SetName(this UnlockInfo localisation, string name)
        {
            localisation.Name = name;
            return localisation;
        }

        public static UnlockInfo SetFlavourText(this UnlockInfo localisation, string flavourText)
        {
            localisation.FlavourText = flavourText;
            return localisation;
        }

        public static UnlockInfo SetDescription(this UnlockInfo localisation, string description)
        {
            localisation.Description = description;
            return localisation;
        }

        #endregion
    }
}