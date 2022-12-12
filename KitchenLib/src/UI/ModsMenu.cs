using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Registry;
using System.Collections.Generic;
using KitchenMods;
using System;

namespace KitchenLib
{
    public partial class ModsMenu<T> : Menu<T>
    {
        public ModsMenu(Transform container, ModuleList module_list) : base(container, module_list) { }
        private List<string> modNames = new List<string>();

        public override void Setup(int player_id) {
            
            AddLabel("Loaded Mods");

			foreach (Type modType in ModRegistery.Registered.Keys)
			{
				BaseMod mod = ModRegistery.Registered[modType];
				if (mod.ModName != null && mod.ModVersion != null)
					if (ModRegistery.isModSafeForVersion(mod))
					{
						AddInfo(mod.ModName + "     v" + mod.ModVersion);
						modNames.Add(mod.ModName);
						modNames.Add(ModRegistery.keyValuePairs[modType].GetName().Name);
					}
			}

            AddLabel("Untested Mods");

			foreach (Type modType in ModRegistery.Registered.Keys)
			{
				BaseMod mod = ModRegistery.Registered[modType];
				if (mod.ModName != null && mod.ModVersion != null)
					if (!ModRegistery.isModSafeForVersion(mod))
					{
						AddInfo(mod.ModName + "     v" + mod.ModVersion);
						modNames.Add(mod.ModName);
						modNames.Add(ModRegistery.keyValuePairs[modType].GetName().Name);
					}
			}

			AddLabel("Non-KitchenLib Mods");
#if MELONLOADER
            System.Collections.ObjectModel.ReadOnlyCollection<MelonLoader.MelonMod> mods = MelonLoader.MelonMod.RegisteredMelons;
			foreach (MelonLoader.MelonMod mod in mods)
			{
                if (!modNames.Contains(mod.Info.Name))
                {
                    AddInfo(mod.Info.Name + "     v" + mod.Info.Version);
                }
			}
#endif
#if BEPINEX
            Dictionary<string, BepInEx.PluginInfo> plugins = BepInEx.Bootstrap.Chainloader.PluginInfos;
            foreach (BepInEx.PluginInfo plugin in plugins.Values)
            {
                if (!modNames.Contains(plugin.Metadata.Name))
                {
                    AddInfo(plugin.Metadata.Name + "     v" + plugin.Metadata.Version);
                }
            }
#endif
#if WORKSHOP
			//AddInfo("Workshop Mods are unable to be detected by KitchenLib");
			List<Mod> mods = ModPreload.Mods;
			foreach (Mod mod in mods)
			{
				if (mod.State == ModState.PostActivated)
				{
					if (mod.Packs[0].Name != "KitchenLib-Workshop.dll")
					{
						if (!modNames.Contains(mod.Packs[0].Name.Replace(".dll", "")))
						{
							AddInfo(mod.Packs[0].Name.Replace(".dll", ""));
						}
					}
				}
			}
#endif

			New<SpacerElement>(true);
            New<SpacerElement>(true);
            AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate(int i)
			{
				this.RequestPreviousMenu();
			}, 0, 1f, 0.2f);
        }

    }
}