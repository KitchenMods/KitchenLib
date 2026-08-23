using System;
using System.Collections.Generic;
using KitchenData;
using KitchenLib.Utils;
using KitchenMods;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomGameDataObject
	{
		#region Base Game Variables

		public virtual int ID { get; internal set; }

		#endregion

		#region KitchenLib Variables

		public int LegacyID { get; internal set; }
		public abstract string UniqueNameID { get; }
		public virtual int BaseGameDataObjectID { get; protected set; } = -1;

		public string ModID = "";
		public string ModName = "";
		public Mod mod;
		public GameDataObject GameDataObject;

		#endregion

		#region Helper Methods

		public int GetHash()
		{
			return StringUtils.GetInt32HashCode($"{ModID}:{UniqueNameID}");
		}

		public int GetLegacyHash()
		{
			return StringUtils.GetInt32HashCode($"{ModName}:{UniqueNameID}");
		}

		protected void OverrideVariable(object result, string varName, object value, bool supressError = false)
		{
			try
			{
				var fieldInfo = ReflectionUtils.GetField(result.GetType(), varName);
				BaseMod.InternalLogger.LogDebug($"Assigning : {value} >> {varName}");
				fieldInfo.SetValue(result, value);
			}
			catch (Exception e)
			{
				if (!supressError)
				{
					BaseMod.InternalLogger.LogError($"Failed to assign : {value} >> {varName}");
					BaseMod.InternalLogger.LogError(e);
				}
			}
		}

		protected void ConvertInfoListToLocalisationObject<L>(List<(Locale, L)> InfoList, ref LocalisationObject<L> result) where L : Localisation
		{
			BaseMod.InternalLogger.LogDebug("Setting up localisation");
			result = new LocalisationObject<L>();

			L fallback = default;
			foreach (var info in InfoList)
			{
				if (info.Item1 == Locale.English)
				{
					fallback = info.Item2;
				}

				result.Add(info.Item1, info.Item2);
			}

			if (fallback != null)
			{
				foreach (Locale locale in Enum.GetValues(typeof(Locale)))
				{
					if (!result.Has(locale))
					{
						result.Add(locale, fallback);
					}
				}
			}
		}

		#endregion
		
		public virtual void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
		}

		[Obsolete("Use OnRegister(SpecificGDOType) instead")]
		public virtual void OnRegister(GameDataObject gameDataObject)
		{
		}

		public virtual void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			gameDataObject = null;
		}

		
	}

	public abstract class CustomGameDataObject<T> : CustomGameDataObject where T : GameDataObject
	{
		public new T GameDataObject => base.GameDataObject as T;

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);
			base.GameDataObject = ScriptableObject.CreateInstance<T>();
			gameDataObject = base.GameDataObject;
			OverrideVariable(gameDataObject, "ID", ID);
		}

		[Obsolete("Use OnRegister(SpecificGDOType) instead")]
		public override void OnRegister(GameDataObject gameDataObject)
		{
			OnRegister(gameDataObject as T);
		}

		public virtual void OnRegister(T gameDataObject)
		{
		}
	}
}