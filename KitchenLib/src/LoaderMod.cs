#if MELONLOADER
using MelonLoader;
#endif
#if BEPINEX
using BepInEx;
#endif
#if WORKSHOP
using KitchenMods;
using Kitchen;
#endif

namespace KitchenLib
{
#if MELONLOADER
    public abstract class LoaderMod : MelonMod { }
#endif

#if BEPINEX
    public abstract class LoaderMod : BaseUnityPlugin { }
#endif
#if WORKSHOP
	public abstract class LoaderMod : IModInitializer
	{
		public abstract void PostActivate(Mod mod);
		public abstract void PostInject();
		public abstract void PreInject();
	}
#endif
}