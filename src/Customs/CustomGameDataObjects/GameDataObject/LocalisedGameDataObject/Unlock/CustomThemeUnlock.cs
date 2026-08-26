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

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not ThemeUnlock themeUnlock) return;

			#region Apply Properties

			OverrideVariable(themeUnlock, "IsPrimary", IsPrimary);
			OverrideVariable(themeUnlock, "Type", Type);
			OverrideVariable(themeUnlock, "ParentTheme1", ParentTheme1);
			OverrideVariable(themeUnlock, "ParentTheme2", ParentTheme2);

			#endregion
		}
	}
}