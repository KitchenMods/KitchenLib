using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Registry;
using System.Collections.Generic;


namespace KitchenLib
{
    public partial class ModsMenu<T> : Menu<T>
    {
        public ModsMenu(Transform container, ModuleList module_list) : base(container, module_list) { }
        private List<string> modNames = new List<string>();

        public override void Setup(int player_id) {
            AddLabel("Loaded Mods");
            foreach (BaseMod mod in ModRegistery.Registered.Values)
            {
                if (mod.ModName != null && mod.ModVersion != null)
                    if (ModRegistery.isModSafeForVersion(mod))
                    {
                        AddInfo(mod.ModName + "     v" + mod.ModVersion);
                        modNames.Add(mod.ModName);
                    }
            }

            AddLabel("Untested Mods");
            foreach (BaseMod mod in ModRegistery.Registered.Values)
            {
                if (mod.ModName != null && mod.ModVersion != null)
                    if (!ModRegistery.isModSafeForVersion(mod))
                    {
                        AddInfo(mod.ModName + "     v" + mod.ModVersion);
                        modNames.Add(mod.ModName);
                    }
            }

            AddLabel("Non-KitchenLib Mods");
            System.Collections.ObjectModel.ReadOnlyCollection<MelonLoader.MelonMod> mods = MelonLoader.MelonMod.RegisteredMelons;
			foreach (MelonLoader.MelonMod mod in mods)
			{
                if (!modNames.Contains(mod.Info.Name))
                {
                    AddInfo(mod.Info.Name + "     v" + mod.Info.Version);
                }
			}
            New<SpacerElement>(true);
            New<SpacerElement>(true);
            AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate(int i)
			{
				this.RequestPreviousMenu();
			}, 0, 1f, 0.2f);
        }

    }
}