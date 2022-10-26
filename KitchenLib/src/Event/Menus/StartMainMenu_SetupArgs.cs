using Kitchen;
using Kitchen.Modules;
using KitchenLib.Utils;
using System;
using System.Reflection;

namespace KitchenLib.Event
{
    public class StartMainMenu_SetupArgs : EventArgs
    {
        internal StartMainMenu instance;
        internal MethodInfo addActionButton;
        internal MethodInfo addSubmenuButton;
        internal MethodInfo addSpacer;

        internal StartMainMenu_SetupArgs(StartMainMenu instance, MethodInfo addActionButton, MethodInfo addSubmenuButton, MethodInfo addSpacer)
        {
            this.instance = instance;
            this.addActionButton = addActionButton;
            this.addSubmenuButton = addSubmenuButton;
            this.addSpacer = addSpacer;
        }

        public void AddSubmenuButton(object[] parameters)
        {
            addSubmenuButton.Invoke(instance, parameters);
        }

        public void AddSpacer(object[] parameters)
        {
            addSpacer.Invoke(instance, parameters);
        }

        public void AddActionButtion(object[] parameters)
        {
            addActionButton.Invoke(instance, parameters);
        }
    }
}