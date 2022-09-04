using Unity.Entities;
using KitchenLib.Appliances;
using KitchenLib.Utils;
using KitchenLib.Registry;

namespace KitchenLib
{
	public class Mod : BaseMod
	{
		public Mod() : base("kitchenlib", ">=1.0.0 <=1.0.5") { }
		public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
			//SystemUtils.NewSystem<CustomApplianceInteractionSystem>();
		}
  }
}
