using KitchenLib.Utils;
using KitchenLib.Systems;

namespace KitchenLib
{
	public class Mod : BaseMod
	{
		public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
			//SystemUtils.AddSystem<CustomRotateSystem>(); //Commenting this out as we can't get the CustomRotateSystem to work
			SystemUtils.AddSystem<CustomInteractionSystem>();
		}
	}
}
