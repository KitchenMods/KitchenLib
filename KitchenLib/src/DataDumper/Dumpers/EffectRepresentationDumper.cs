using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class EffectRepresentationDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("ID,Type,Name,Description,Icon");


			foreach (EffectRepresentation effectRepresentation in GameData.Main.Get<EffectRepresentation>())
			{
				sb.AppendLine($"{effectRepresentation.ID},{effectRepresentation.name},{effectRepresentation.Name},{effectRepresentation.Description},{effectRepresentation.Icon}");
			}

			SaveCSV("EffectRepresentation", "EffectRepresentations", sb);
		}
	}
}
