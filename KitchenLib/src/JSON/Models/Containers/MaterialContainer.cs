namespace KitchenLib.JSON.Models.Containers
{
	public class MaterialContainer
	{
		public MaterialType Type { get; set; }
		public string Name { get; set; }
	}

	public enum MaterialType
	{
		Existing,
		Custom
	}
}
