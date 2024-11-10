using System;
using System.Collections.Generic;
using System.IO;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Registry;
using KitchenMods;
using UnityEngine;

namespace KitchenLib
{
	public class ModsMenu : KLMenu
	{
		public ModsMenu(Transform container, ModuleList module_list) : base(container, module_list) { }
		
		private Option<int> pages;
		
		private List<string> kitchenLibModNames = new List<string>();
		private List<string> nonKitchenLibModNames = new List<string>();
		
		private List<string> blockedModNames = new List<string>
		{
			"Mono.Cecil.dll",
			"Mono.Cecil.Mdb.dll",
			"Mono.Cecil.Pdb.dll",
			"Mono.Cecil.Rocks.dll",
			"MonoMod.RuntimeDetour.dll",
			"MonoMod.Utils.dll",
			"UniverseLib.Mono.dll",
			"MonoMod.Backports.dll",
			"MonoMod.Core.dll",
			"MonoMod.Iced.dll",
			"MonoMod.ILHelpers.dll",
		};
		
		private string modToModNameAndVersion(BaseMod mod) => $"{mod.ModName}     v{mod.ModVersion}{mod.BetaVersion}";
		
		public override void Setup(int player_id)
		{
			base.Setup(player_id);

			kitchenLibModNames.Clear();
			nonKitchenLibModNames.Clear();
			
			foreach (Type modType in ModRegistery.Registered.Keys)
			{
				BaseMod mod = ModRegistery.Registered[modType];
				kitchenLibModNames.Add(modToModNameAndVersion(mod));
				blockedModNames.Add(ModRegistery.keyValuePairs[modType].GetName().Name + ".dll");
			}

			foreach (Mod mod in ModPreload.Mods)
			{
				foreach (AssemblyModPack pack in mod.GetPacks<AssemblyModPack>())
				{
					if (blockedModNames.Contains(pack.Name))
					{
						continue;
					}
					nonKitchenLibModNames.Add(pack.Name);
				}
			}

			pages = CreatePageSelector(new Dictionary<int, PageDetails>
			{
				{
					0, new PageDetails
					{
						title = "KitchenLib Mods",
						action = DrawKitchenLibMods
					}
				},
				{
					1, new PageDetails
					{
						title = "Non-KitchenLib Mods",
						action = DrawNonKitchenLibMods
					}
				}
			});

			DrawKitchenLibMods();
		}

		protected override void Redraw()
		{
			ModuleList.Clear();
			AddSelect(pages);
			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate(int i)
			{
				this.RequestPreviousMenu();
			}, 0, 1f, 0.2f);
		}

		private void DrawKitchenLibMods()
		{
			Redraw();
			CreateModLabels(AddInfo("").Position, kitchenLibModNames, 3, 0.3f, 6);
			ResetPanel();
		}
		
		private void DrawNonKitchenLibMods()
		{
			Redraw();
			CreateModLabels(AddInfo("").Position, nonKitchenLibModNames, 3, 0.3f, 6);
			ResetPanel();
		}
	}
}