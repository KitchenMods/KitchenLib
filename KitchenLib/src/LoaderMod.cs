#if MELONLOADER
using MelonLoader;
#endif
#if BEPINEX
using BepInEx;
#endif
#if WORKSHOP
using KitchenMods;
#endif

using Kitchen;
using System.Reflection;
using System.IO;
using System;

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
#if WORKSHOP
	public abstract class LoaderMod : GenericSystemBase, IModSystem
	{
	}
#endif
}