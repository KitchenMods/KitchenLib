using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class GardenProfileDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder gardenProfileDump = new StringBuilder();
			StringBuilder gardenProfileSpawnsDump = new StringBuilder();
			gardenProfileDump.AppendLine("ID,Type,SpawnHolder");
			gardenProfileSpawnsDump.AppendLine("ID,Type,Item,Chance");
			foreach (GardenProfile gardenProfile in GameData.Main.Get<GardenProfile>())
			{
				if (gardenProfile.SpawnHolder == null)
					gardenProfileDump.AppendLine($"{gardenProfile.ID},{gardenProfile.name},null");
				else
					gardenProfileDump.AppendLine($"{gardenProfile.ID},{gardenProfile.name},{gardenProfile.SpawnHolder}");
				if (gardenProfile.Spawns != null)
				{
					foreach (GardenProfile.SpawnProbability spawnPropability in gardenProfile.Spawns)
					{
						if (spawnPropability.Item == null)
							gardenProfileSpawnsDump.AppendLine($"{gardenProfile.ID},{gardenProfile.name},null,{spawnPropability.Chance}");
						else
							gardenProfileSpawnsDump.AppendLine($"{gardenProfile.ID},{gardenProfile.name},{spawnPropability.Item},{spawnPropability.Chance}");
					}
				}
			}

			SaveCSV("GardenProfile", "GardenProfiles", gardenProfileDump);
			SaveCSV("GardenProfile", "GardenProfileSpawns", gardenProfileSpawnsDump);
		}
	}
}
