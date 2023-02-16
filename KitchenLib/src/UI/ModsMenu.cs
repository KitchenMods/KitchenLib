using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Registry;
using System.Collections.Generic;
using KitchenMods;
using System;
using System.Reflection;
using System.Linq;

namespace KitchenLib
{
    public partial class ModsMenu<T> : KLMenu<T>
    {
        public ModsMenu(Transform container, ModuleList module_list) : base(container, module_list) { }
        private List<string> modNames = new List<string>();

		private static List<ModPage> modPages = new List<ModPage>
		{
			ModPage.LoadedMods,
			ModPage.UntestedMods,
			ModPage.NonKitchenLibMods
		};

        private static readonly float columnWidth = 6f;
        private static readonly int modsPerColumn = 25;
        private static readonly Vector2 selectPosition = new Vector2(1, 4);
        private static readonly Vector2 backButtonPosition = new Vector2(1, 3.5f);
        private static readonly List<string> modsToFilterOut = new List<string> {
            "Mono.Cecil.dll",
            "Mono.Cecil.Mdb.dll",
            "Mono.Cecil.Pdb.dll",
            "Mono.Cecil.Rocks.dll",
            "MonoMod.RuntimeDetour.dll",
            "MonoMod.Utils.dll",
            "UniverseLib.Mono.dll",
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
			AddSelect<ModPage>(PageSelector).Position = selectPosition;
			New<SpacerElement>(true);
			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i) {
				this.RequestPreviousMenu();
			}, 0, 1f, 0.2f).Position = backButtonPosition;

			if (mod_page == ModPage.LoadedMods)
			{
				createModLabels(loadedMods.Values.Select(modToModNameAndVersion).ToList());
			}
			else if (mod_page == ModPage.UntestedMods)
			{
				createModLabels(untestedMods.Values.Select(modToModNameAndVersion).ToList());
			}
			else if (mod_page == ModPage.NonKitchenLibMods)
			{
				List<string> nonKlMods = ModPreload.Mods
					.Where(mod => mod.State == ModState.PostActivated)
					.SelectMany(mod => mod.GetPacks<AssemblyModPack>())
					.Where(pack => !modAssemblies.ContainsValue(pack.Name) && !modsToFilterOut.Contains(pack.Name))
					.Select(pack => pack.Name.Replace(".dll", "")).ToList();
				createModLabels(nonKlMods);
			}
		}

        private string modToModNameAndVersion(BaseMod mod) => $"{mod.ModName}     v{mod.ModVersion}{mod.BetaVersion}";

        private void createModLabels(List<string> modNames) {
            int columns = modNames.Count / modsPerColumn;
            int i = 0;

            modNames.OrderBy(x => x).ToList().ForEach(modName => {
                InfoBoxElement infoBoxElement = AddInfo(modName);
                infoBoxElement.SetSize(columnWidth, 1f);
                infoBoxElement.Position = new Vector2(
                    Mathf.Floor(i / modsPerColumn) * columnWidth - (columns * columnWidth / 2),
                    i % modsPerColumn * -0.25f + 3f);
                i++;
            });
        }
    }

	public enum ModPage
	{
		LoadedMods,
		UntestedMods,
		NonKitchenLibMods
	}
}