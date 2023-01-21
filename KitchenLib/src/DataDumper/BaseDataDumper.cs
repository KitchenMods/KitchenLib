using System.IO;
using System.Text;
using UnityEngine;

namespace KitchenLib.DataDumper
{
	public abstract class BaseDataDumper
	{
		public abstract void Dump();

		protected void SaveCSV(string path, string name, StringBuilder sb)
		{
			var dumpFolder = Path.Combine(Application.persistentDataPath, "DataDump");
			if (!Directory.Exists(dumpFolder))
				Directory.CreateDirectory(dumpFolder);
			if (!Directory.Exists(Path.Combine(dumpFolder, path)))
				Directory.CreateDirectory(Path.Combine(dumpFolder, path));

			File.WriteAllText(Path.Combine(dumpFolder, path, $"{name}.csv"), sb.ToString());
		}
	}
}
