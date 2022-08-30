using Unity.Entities;
using KitchenLib.Appliances;
using KitchenLib.Utils;

namespace KitchenLib
{
	public class Mod : BaseMod
	{
		public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
			SystemUtils.AddSystem<CustomApplianceInteractionSystem>();
			SystemUtils.AddSystem<CustomApplianceRotationSystem>();
		}
	}
}
