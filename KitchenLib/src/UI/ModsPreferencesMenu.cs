using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Registry;
using KitchenLib.Event;
using KitchenLib.Utils;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace KitchenLib
{
    public partial class ModsPreferencesMenu : StartGameMainMenu
    {
        public ModsPreferencesMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        private static Dictionary<string, List<BasePreference>> ModIDS = new Dictionary<string, List<BasePreference>>();

        public override void Setup(int player_id) {
            AddLabel("Mod Preferences");
            New<SpacerElement>(true);
            
            EventUtils.InvokeEvent(nameof(Events.SetupEvent), Events.SetupEvent?.GetInvocationList(), null, new SetupEventArgs(player_id, this));

            New<SpacerElement>(true);
            New<SpacerElement>(true);
            AddActionButton("Back", MainMenuAction.Back, ElementStyle.MainMenuBack);
        }

        public void AddNewButton(Type type, string name)
        {
            AddButton(name, delegate
            {
                RequestSubMenu(type);
            });
        }

        public override void CreateSubmenus(ref Dictionary<Type, Menu<MainMenuAction>> menus)
        {
            EventUtils.InvokeEvent(nameof(Events.CreateSubMenusEvent), Events.CreateSubMenusEvent?.GetInvocationList(), null, new CreateSubMenusEventArgs(menus, Container, ModuleList));
        }
    }
}