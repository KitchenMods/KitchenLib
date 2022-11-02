#if MelonLoader
using MelonLoader;
#endif
#if BepInEx
using BepInEx;
#endif

namespace KitchenLib
{
    #if MelonLoader
    public abstract class LoaderMod : MelonMod
    {
    }
    #endif

    #if BepInEx
    public abstract class LoaderMod : BaseUnityPlugin
    {
    }
    #endif
}