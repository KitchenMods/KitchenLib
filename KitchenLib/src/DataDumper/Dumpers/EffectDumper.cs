using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class EffectDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder effects = new StringBuilder();
			StringBuilder effectProperties = new StringBuilder();
			effects.AppendLine("ID,Type,EffectRange,EffectCondition,EffectType,EffectInformation");
			effectProperties.AppendLine("ID,Type,IEffectProperty");
			foreach (Effect effect in GameData.Main.Get<Effect>())
			{
				effects.AppendLine($"{effect.ID},{effect.name},{effect.EffectRange},{effect.EffectCondition},{effect.EffectType},{effect.EffectInformation}");
				foreach (IEffectProperty property in effect.Properties)
				{
					effectProperties.AppendLine($"{effect.ID},{effect.name},{property}");
				}
			}

			SaveCSV("Effect", "Effects", effects);
			SaveCSV("Effect", "EffectProperties", effectProperties);
		}
	}
}
