using Unity.Entities;
using KitchenLib.Customs;
using KitchenLib.Utils;
using KitchenLib.Registry;

namespace KitchenLib
{
	public class Mod : BaseMod
	{
		public Mod() : base("kitchenlib", ">=1.0.0 <=1.0.5") { }
		public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
		}
  }
}
