using Unity.Entities;
using KitchenLib.Appliances;

namespace KitchenLib
{
	public class Mod : BaseMod
	{

		public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
			World.DefaultGameObjectInjectionWorld.AddSystem<CustomApplianceInteractionSystem>(new CustomApplianceInteractionSystem());
		}
	}
}
