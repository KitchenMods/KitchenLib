using Kitchen;
using KitchenMods;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using KitchenLib.Utils;
using Newtonsoft.Json;
using Unity.Entities;

namespace KitchenLib.Systems
{
	public class SaveComponentsList : GameSystemBase, IModSystem
	{
		private Dictionary<ulong, ModComponents> _componentsMap = new Dictionary<ulong, ModComponents>();
		protected override void Initialise()
		{
			string directoryPath = Path.Combine(Application.persistentDataPath, "UserData", "KitchenLib");
			string filePath = Path.Combine(directoryPath, "componentHistory.json");
			
			if (!Directory.Exists(directoryPath))
				Directory.CreateDirectory(directoryPath);
			
			if (File.Exists(filePath)) 
				_componentsMap = JsonConvert.DeserializeObject<Dictionary<ulong, ModComponents>>(File.ReadAllText(filePath));
			
			FieldInfo packsInfo = ReflectionUtils.GetField<Mod>("Packs");
			foreach (Mod mod in ModPreload.Mods)
			{
				List<ModPack> packs = (List<ModPack>)packsInfo.GetValue(mod);

				foreach (ModPack modPack in packs)
				{
					if (modPack.GetType().Equals(typeof(AssemblyModPack)))
					{
						AssemblyModPack assemblyModPack = (AssemblyModPack)modPack;
						foreach (Type type in assemblyModPack.Asm.GetTypes())
						{
							if (typeof(IComponentData).IsAssignableFrom(type) && !type.IsInterface)
							{
								if (mod.ID != 0) 
									if (!_componentsMap.ContainsKey(mod.ID)) 
										_componentsMap.Add(mod.ID, new ModComponents(mod.Name, new List<(ulong, string)>()));

								ulong hash = TypeManager.GetTypeInfo(TypeManager.GetTypeIndex(type)).StableTypeHash;

								if (mod.ID != 0)
									_componentsMap[mod.ID].Components.Add((hash, type.FullName));
							}
						}
					}
				}
			}
			File.WriteAllText(filePath, JsonConvert.SerializeObject(_componentsMap, Formatting.Indented));
		}

		protected override void OnUpdate()
		{
			
		}
	}

	public class ModComponents
	{
		public readonly string ModName;
		public readonly List<(ulong, string)> Components;

		public ModComponents(string modName, List<(ulong, string)> components)
		{
			ModName = modName;
			Components = components;
		}
	}
}