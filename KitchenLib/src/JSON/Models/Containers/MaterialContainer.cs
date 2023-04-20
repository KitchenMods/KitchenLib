namespace KitchenLib.JSON.Models.Containers
{
	public class MaterialContainer
	{
		public MaterialType Type { get; set; }
		public string name { get; set; }
	}

	public enum MaterialType
	{
		Existing,
		Custom
	}
}
