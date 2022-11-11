#if MELONLOADER
using MelonLoader;
#endif
#if BEPINEX
using BepInEx;
#endif

namespace KitchenLib
{
    #if MELONLOADER
    public abstract class LoaderMod : MelonMod
    {
    }
#endif

#if BEPINEX
    public abstract class LoaderMod : BaseUnityPlugin
    {
    }
#endif
}