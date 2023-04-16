using Kitchen;
using KitchenData;
using KitchenMods;

namespace KitchenLib.Systems
{
	public class GenerateCustomers : CreateCustomerGroup, IModSystem
	{
		public static int AddCustomer = 0;
		public static bool isCat = false;

		protected override void OnUpdate()
		{
			if (AddCustomer == 0)
				return;

			NewGroup(GameData.Main.Get<CustomerType>(-260015680), AddCustomer, isCat);
			AddCustomer = 0;
		}
	}
}
