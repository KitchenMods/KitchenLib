using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Registry;
using System.Collections.Generic;
using KitchenMods;
using System;
using System.Reflection;

namespace KitchenLib
{
    public partial class ModsMenu<T> : Menu<T>
    {
        public ModsMenu(Transform container, ModuleList module_list) : base(container, module_list) { }
        private List<string> modNames = new List<string>();

		private static List<ModPage> modPages = new List<ModPage>
		{
			ModPage.LoadedMods,
			ModPage.UntestedMods,
			ModPage.NonKitchenLibMods
		};

		private static List<string> modPagesNames = new List<string>
		{
			"Loaded Mods",
			"Untested Mods",
			"Non-KitchenLib Mods"
		};

		private Option<ModPage> PageSelector = new Option<ModPage>(modPages, ModPage.LoadedMods, modPagesNames);

		public override void Setup(int player_id) {

			Dictionary<Type, BaseMod> loadedMods = new Dictionary<Type, BaseMod>();
			Dictionary<Type, BaseMod> untestedMods = new Dictionary<Type, BaseMod>();
			Dictionary<Assembly, string> modAssemblies = new Dictionary<Assembly, string>();

			foreach (Type modType in ModRegistery.Registered.Keys)
			{
				BaseMod mod = ModRegistery.Registered[modType];
				if (ModRegistery.isModSafeForVersion(mod))
					loadedMods.Add(modType, mod);
				else
					untestedMods.Add(modType, mod);
				modNames.Add(mod.ModName);
				modAssemblies.Add(ModRegistery.keyValuePairs[modType], ModRegistery.keyValuePairs[modType].GetName().Name + ".dll");
			}
			PageSelector.OnChanged += delegate (object _, ModPage result)
			{
				Redraw(player_id, result, modAssemblies, untestedMods, loadedMods);
			};
			Redraw(player_id, ModPage.LoadedMods, modAssemblies, untestedMods, loadedMods);
		}


		private void Redraw(int player_id, ModPage mod_page, Dictionary<Assembly, string> modAssemblies, Dictionary<Type, BaseMod> untestedMods, Dictionary<Type, BaseMod> loadedMods)
		{
			ModuleList.Clear();
			AddSelect<ModPage>(PageSelector);
			if (mod_page == ModPage.LoadedMods)
			{
				AddLabel("Loaded Mods");

				foreach (Type modType in loadedMods.Keys)
				{
					BaseMod mod = loadedMods[modType];
					AddInfo(mod.ModName + "     v" + mod.ModVersion);
				}
			}
			else if (mod_page == ModPage.UntestedMods)
			{

				AddLabel("Untested Mods");

				foreach (Type modType in untestedMods.Keys)
				{
					BaseMod mod = untestedMods[modType];
					AddInfo(mod.ModName + "     v" + mod.ModVersion);
				}
			}
			else if (mod_page == ModPage.NonKitchenLibMods)
			{

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

				List<Mod> mods = ModPreload.Mods;
				foreach (Mod mod in mods)
				{
					if (mod.State == ModState.PostActivated)
					{
						foreach (AssemblyModPack pack in mod.GetPacks<AssemblyModPack>())
						{
							if (!modAssemblies.ContainsValue(pack.Name))
							{
								AddInfo(pack.Name.Replace(".dll", ""));
							}
						}
					}
				}
#endif
			}

			New<SpacerElement>(true);
			New<SpacerElement>(true);
			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i)
			{
				this.RequestPreviousMenu();
			}, 0, 1f, 0.2f);
		}

    }

	public enum ModPage
	{
		LoadedMods,
		UntestedMods,
		NonKitchenLibMods
	}
}