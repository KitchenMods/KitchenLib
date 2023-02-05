using KitchenData;

namespace KitchenLib.src.ContentPack.Models.Containers
{
    public class UnlockEffectContainer
    {
        public UnlockEffectContext Type { get; set; }
        public UnlockEffect Effect { get; set; }
    }

    public enum UnlockEffectContext
    {
        FranchiseEffect,
        GlobalEffect,
        ParameterEffect,
        ShopEffect,
        StartBonusEffect,
        StatusEffect,
        ThemeAddEffect
    }
}
