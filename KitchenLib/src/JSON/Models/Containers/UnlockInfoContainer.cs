using KitchenData;
using KitchenLib.Utils;

namespace KitchenLib.JSON.Models.Containers
{
	public class UnlockInfoContainer
	{
		public Locale locale;
		public string name;
		public string description;
		public string flavourText;

		public UnlockInfo Convert()
		{
			return LocalisationUtils.CreateUnlockInfo(name, description, flavourText);
		}
	}
}
