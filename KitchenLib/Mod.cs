using Unity.Entities;
using KitchenLib.Appliances;
using KitchenLib.Utils;
using KitchenLib.Registry;

namespace KitchenLib
{
	public class Mod : BaseMod
	{
        public Mod() : base("kitchenlib", new string[] { "49FD", "294C" }) { }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
			SystemUtils.AddSystem<CustomApplianceInteractionSystem>();
		}
    }
}
