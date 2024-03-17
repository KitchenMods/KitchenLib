using System;
using Kitchen;
using KitchenMods;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Systems
{
	[UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
	[UpdateBefore(typeof(PerformSaveRequests))]
	[UpdateInGroup(typeof(SimulationSystemGroup))]
	public class LogSaveDetails : GameSystemBase
	{
		private EntityQuery SaveRequests;

		protected override void Initialise()
		{
			SaveRequests = GetEntityQuery(typeof(CRequestSave));
		}

		protected override void OnUpdate()
		{
			return;
			Entity entity = this.SaveRequests.First();
			CRequestSave crequestSave = this.SaveRequests.First<CRequestSave>();

			if (crequestSave.SaveType == SaveType.AutoFull)
			{
				string path = Path.Combine(Application.persistentDataPath, "Full", GetSingleton<SSelectedLocation>().Selected.Slot.ToString());
				List<ModDetails> saveDetails = new();
				foreach (Mod mod in ModPreload.Mods)
				{
					if (mod.ID == 0) continue;

					ModDetails modDetails = new();
					modDetails.ModID = mod.ID;
					foreach (AssemblyModPack assemblyModPack in mod.GetPacks<AssemblyModPack>())
					{
						foreach (Type type in assemblyModPack.Asm.GetTypes())
						{
							Main.Logger.LogInfo("Type : " + type.Name);
							if (!typeof(IComponentData).IsAssignableFrom(type) || type.IsInterface) continue;
							

							Main.Logger.LogInfo("IsComponent!");

							ulong hash = TypeManager.GetTypeInfo(TypeManager.GetTypeIndex(type)).StableTypeHash;
							modDetails.ComponentHashes.Add(new ComponentDetails
							{
								Hash = hash,
								Name = type.Name
							});
						}
					}

					saveDetails.Add(modDetails);
				}
 
				File.WriteAllText(path + "/details.json", JsonConvert.SerializeObject(saveDetails, Formatting.Indented));
			}
		}

		internal class ModDetails
		{
			public ulong ModID;
			public List<ComponentDetails> ComponentHashes = new();
		}

		internal class ComponentDetails
		{
			public ulong Hash;
			public string Name;
		}
	}
}