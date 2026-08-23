using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomThemeUnlock : CustomUnlock<ThemeUnlock>
	{

		#region Base Game Variables

		public virtual bool IsPrimary { get; protected set; } = true;
		public virtual DecorationType Type { get; protected set; }
		public virtual ThemeUnlock ParentTheme1 { get; protected set; }
		public virtual ThemeUnlock ParentTheme2 { get; protected set; }

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is ThemeUnlock themeUnlock)
			{
				#region Apply Properties

				OverrideVariable(themeUnlock, "IsPrimary", IsPrimary);
				OverrideVariable(themeUnlock, "Type", Type);

				#endregion
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is ThemeUnlock themeUnlock)
			{
				#region Apply Properties

				OverrideVariable(themeUnlock, "ParentTheme1", ParentTheme1);
				OverrideVariable(themeUnlock, "ParentTheme2", ParentTheme2);

				#endregion
			}
		}
	}
}