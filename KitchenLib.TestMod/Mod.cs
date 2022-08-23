using KitchenLib.Appliances;

namespace KitchenLib.TestMod
{
	public class Mod : BaseMod
	{
		public Mod() : base() { }
		public override void OnApplicationStart() {
			ModName = Info.Name;
			ModVersion = Info.Version;
			base.OnApplicationStart();
			RegisterCustomAppliance(new CustomApplianceInfo() {
				Name                    = "Testing Terminal",
				Description             = "This is how we test things, from a terminal!",
				OnCheckInteractPossible = (data) => true,
				OnInteract              = (data) => Mod.Log($"Interacting! {data.Attempt.Mode} {data.Attempt.Type}")
			});
		}
	}
}