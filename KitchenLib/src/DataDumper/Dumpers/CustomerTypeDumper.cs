using KitchenData;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class CustomerTypeDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder sb = new StringBuilder();
			StringBuilder cosmetics = new StringBuilder();
			StringBuilder properties = new StringBuilder();
			sb.AppendLine("ID,Type,IsGenericGroup,RelativeGroupSize,MinGroupSize,MaxGroupSize,PatienceModifiers,OrderingModifiers");
			cosmetics.AppendLine("ID,Type,Cosmetic ID, Cosmetic Type");
			properties.AppendLine("ID,Type,Property Type");
			foreach (CustomerType decor in GameData.Main.Get<CustomerType>())
			{
				sb.AppendLine($"{decor.ID},{decor.name},{decor.IsGenericGroup},{decor.RelativeGroupSize},{decor.MinGroupSize},{decor.MaxGroupSize},{decor.PatienceModifiers},{decor.OrderingModifiers}");
				foreach (PlayerCosmetic cosmetic in decor.Cosmetics)
				{
					cosmetics.AppendLine($"{decor.ID},{decor.name},{cosmetic.ID},{cosmetic.name}");
				}
				foreach (ICustomerProperty property in decor.Properties)
				{
					properties.AppendLine($"{decor.ID},{decor.name},{property.GetType()}");
				}
			}

			SaveCSV("CustomerType", "CustomerTypes", sb);
			SaveCSV("CustomerType", "CustomerTypeCosmetics", cosmetics);
			SaveCSV("CustomerType", "CustomerTypeProperties", properties);
		}
	}
}
