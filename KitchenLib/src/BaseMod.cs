using MelonLoader;
using System.Runtime.CompilerServices;
using KitchenLib.Registry;
using KitchenLib.Appliances;
using Kitchen;
using HarmonyLib;
using UnityEngine;
using KitchenLib.Utils;

namespace KitchenLib
{
	public abstract class BaseMod : MelonMod
	{
		public string ModName { get { return Info.Name; } }
		public string ModVersion { get { return Info.Version; } }
        
        public BaseMod() : base() {
            ModRegistery.Register(this);
        }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Log(string message) {
			MelonLogger.Msg(message);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Error(string message) {
			MelonLogger.Error(message);
		}

		public CustomAppliance RegisterCustomAppliance<T>() where T : CustomAppliance, new()  {
			T appliance = new T();
			appliance.ModName = Info.Name;
			if (CustomAppliances.Register(appliance))
				return appliance;
			else return null;
		}
    }
}
