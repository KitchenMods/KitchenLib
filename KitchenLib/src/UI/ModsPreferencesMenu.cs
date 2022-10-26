using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using System.Collections.Generic;
using System;
using System.Reflection;
using KitchenLib.Event;
using KitchenLib.Utils;

namespace KitchenLib
{
    public partial class ModsPreferencesMenu<T> : Menu<T>
    {
        public ModsPreferencesMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        private static Dictionary<string, List<BasePreference>> ModIDS = new Dictionary<string, List<BasePreference>>();

        public override void Setup(int player_id) {
            AddLabel("Mod Preferences");
            New<SpacerElement>(true);

            MethodInfo mInfo = this.GetType().GetMethod("AddSubmenuButton", BindingFlags.NonPublic | BindingFlags.Instance);
            EventUtils.InvokeEvent(nameof(Events.PreferenceMenu_SetupEvent), Events.PreferenceMenu_SetupEvent?.GetInvocationList(), null, new PreferenceMenu_SetupArgs(this, mInfo));

            New<SpacerElement>(true);
            New<SpacerElement>(true);
            AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate(int i)
			{
				this.RequestPreviousMenu();
			}, 0, 1f, 0.2f);
        }

        public void AddNewButton(Type type, string name)
        {
            AddButton(name, delegate
            {
                RequestSubMenu(type);
            });
        }

        public override void CreateSubmenus(ref Dictionary<Type, Menu<T>> menus)
        {
            //menus.Add(typeof(KLSettingsMenu<T>), new KLSettingsMenu<T>(this.Container, this.ModuleList));
            EventUtils.InvokeEvent(nameof(Events.PreferenceMenu_CreateSubmenusEvent), Events.PreferenceMenu_CreateSubmenusEvent?.GetInvocationList(), null, new PreferenceMenu_CreateSubmenusArgs(this, menus, this.Container, this.ModuleList));
        }
    }
}