using Kitchen;
using System;
using System.Reflection;

namespace KitchenLib.Event
{
    public class MainMenuViewEventArgs : EventArgs
    {
        internal MainMenuView instance;
        internal MethodInfo addMenu;

        internal MainMenuViewEventArgs(MainMenuView instance, MethodInfo addMenu)
        {
            this.instance = instance;
            this.addMenu = addMenu;
        }

        public void AddMenu(object[] parameters)
        {
            addMenu.Invoke(instance, parameters);
        }
    }
}