using KitchenData;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomDecor : CustomGameDataObject<Decor>
	{
		#region Base Game Variables

		public virtual Material Material { get; protected set; }
		public virtual Appliance ApplicatorAppliance { get; protected set; }
		public virtual LayoutMaterialType Type { get; protected set; }
		public virtual bool IsAvailable { get; protected set; } = true;

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is Decor decor)
			{
				#region Apply Properties

				OverrideVariable(decor, "Material", Material);
				OverrideVariable(decor, "Type", Type);
				OverrideVariable(decor, "IsAvailable", IsAvailable);

				#endregion
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is Decor decor)
			{
				#region Apply Properties

				OverrideVariable(decor, "ApplicatorAppliance", ApplicatorAppliance);

				#endregion
			}
		}
	}
}