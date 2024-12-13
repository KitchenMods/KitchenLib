using UnityEngine;

namespace KitchenLib.Interfaces
{
	public interface IMaterialEditor
	{
		public void GUI(Material material);
		public string Export(Material material);
	}
}