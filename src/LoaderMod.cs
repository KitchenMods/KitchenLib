using Kitchen;
using KitchenMods;

namespace KitchenLib
{
	public abstract class LoaderMod : GenericSystemBase, IModInitializer, IModSystem
	{
		public abstract void PostActivate(Mod mod);
		public abstract void PostInject();
		public abstract void PreInject();
		protected override void OnUpdate() { }
		protected override void Initialise() { }
	}
}