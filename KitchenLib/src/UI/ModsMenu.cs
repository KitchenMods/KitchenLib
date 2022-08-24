using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Registry;

namespace KitchenLib
{
    public partial class ModsMenu : StartGameMainMenu
    {
        public ModsMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int player_id) {
            AddLabel("Loaded Mods");

            foreach (BaseMod mod in ModRegistery.Registered.Values)
            {
                if (mod.ModName != null && mod.ModVersion != null)
                    AddInfo(mod.ModName + "     v" + mod.ModVersion);
            }
            

            New<SpacerElement>(true);
            New<SpacerElement>(true);
            AddActionButton("Back", MainMenuAction.Back, ElementStyle.MainMenuBack);
        }
    }
}