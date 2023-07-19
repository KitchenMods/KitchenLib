using Kitchen;
using KitchenMods;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			Entity entity = this.SaveRequests.First();
			CRequestSave crequestSave = this.SaveRequests.First<CRequestSave>();

			if (crequestSave.SaveType == SaveType.AutoFull)
			{
				EntityContext entityContext = new EntityContext(base.World.EntityManager);
				string path = Path.Combine(Application.persistentDataPath, "Full", entityContext.Get<SSelectedLocation>().Selected.Slot.ToString());
				SaveDetails saveDetails = new SaveDetails();

				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

				saveDetails.Mods = ModPreload.Mods
					.Where(mod => mod.State == ModState.PostActivated)
					.SelectMany(mod => mod.GetPacks<AssemblyModPack>())
					.Select(pack => pack.Name.Replace(".dll", "")).ToList();

				saveDetails.Components = EntityManager
					.GetAllEntities()
					.SelectMany(_ => EntityManager.GetChunk(_).Archetype.GetComponentTypes())
					.ToDictionary(_ => _.GetManagedType().FullName, _ => TypeManager.GetTypeInfo(_.TypeIndex).StableTypeHash.ToString());

				File.WriteAllText(path + "/details.json", JsonConvert.SerializeObject(saveDetails, Formatting.Indented));
			}
		}
	}

	internal class SaveDetails
	{
		public List<string> Mods = new();
		public Dictionary<string, string> Components = new();
	}
}
