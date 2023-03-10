using Kitchen;
using System;
using System.Reflection;

namespace KitchenLib.Event
{
	public class MainMenu_SetupArgs : EventArgs
	{
		public readonly StartMainMenu instance;
		public readonly MethodInfo addActionButton;
		public readonly MethodInfo addSubmenuButton;
		public readonly MethodInfo addSpacer;

		internal MainMenu_SetupArgs(StartMainMenu instance, MethodInfo addActionButton, MethodInfo addSubmenuButton, MethodInfo addSpacer)
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