using Unity.Entities;
using KitchenLib.Appliances;
using KitchenLib.Utils;
using KitchenLib.Registry;

namespace KitchenLib
{
	public class Mod : BaseMod
	{
		public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
			SystemUtils.NewSystem<CustomApplianceInteractionSystem>();
		}
  }
}
