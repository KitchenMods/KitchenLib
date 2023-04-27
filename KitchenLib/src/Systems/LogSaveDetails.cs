using Kitchen;
using KitchenMods;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.src.Systems
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
			Entity entity = this.SaveRequests.First();
			CRequestSave crequestSave = this.SaveRequests.First<CRequestSave>();
			if (crequestSave.SaveType == SaveType.AutoFull)
			{
				EntityContext entityContext = new EntityContext(base.World.EntityManager);
				string path = Path.Combine(Application.persistentDataPath, "Full", entityContext.Get<SSelectedLocation>().Selected.Slot.ToString());
				SaveDetails saveDetails = null;

				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

				if (File.Exists(path + "/details.json"))
					saveDetails = JsonConvert.DeserializeObject<SaveDetails>(File.ReadAllText(path + "/details.json"));

				if (saveDetails == null)
					saveDetails = new SaveDetails();

				foreach (string x in GetModList())
				{
					if (!saveDetails.Mods.Contains(x))
					{
						saveDetails.Mods.Add(x);
					}
				}
				foreach (Entity e in EntityManager.GetAllEntities())
				{
					foreach (var component in EntityManager.GetChunk(e).Archetype.GetComponentTypes().Select(c => c.GetManagedType()))
					{
						if (!saveDetails.Components.ContainsKey(component.ToString()))
						{
							saveDetails.Components.Add(component.ToString(), component.GetHashCode().ToString());
						}
					}
				}

				File.WriteAllText(path + "/details.json", JsonConvert.SerializeObject(saveDetails, Formatting.Indented));
			}
		}

		private string[] GetModList()
		{
			List<string> nonKlMods = ModPreload.Mods
					.Where(mod => mod.State == ModState.PostActivated)
					.SelectMany(mod => mod.GetPacks<AssemblyModPack>())
					.Select(pack => pack.Name.Replace(".dll", "")).ToList();

			return nonKlMods.ToArray();
		}

		private Dictionary<string, string> GetComponents()
		{
			return new Dictionary<string, string>();
		}
	}

	internal class SaveDetails
	{
		public List<string> Mods = new List<string>();
		public Dictionary<string, string> Components = new Dictionary<string, string>();
	}
}
