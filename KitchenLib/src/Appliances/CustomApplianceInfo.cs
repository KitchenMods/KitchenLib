using KitchenData;
using KitchenLib.Utils;

namespace KitchenLib.Appliances
{
	public class CustomApplianceInfo
	{
		public int ID;
		public string ModName;
		public string Name;
		public string Description;
		public int BaseApplianceId = -1248669347;

		public Appliance Appliance;

		public int GetHash() {
			return StringUtils.GetInt32HashCode($"{ModName}:{Name}");
		}
	}
}
