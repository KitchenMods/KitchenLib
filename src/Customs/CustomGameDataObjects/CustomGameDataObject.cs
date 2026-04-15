using KitchenData;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
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
	    [Obsolete("BaseGameDataObject is no longer in use.")]
	    public virtual int BaseGameDataObjectID { get; protected set; } = -1;

	    public string ModID = "";
	    public string ModName = "";
	    public Mod mod;
	    public GameDataObject GameDataObject;

	    #endregion

	    #region KitchenLib Methods
	    
	    public abstract void Convert(GameData gameData, out GameDataObject gameDataObject);
	    public abstract void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject);
	    
	    #endregion

	    #region Utility Methods
	    
	    protected void OverrideVariable(object result, string varName, object value, bool supressError = false)
	    {
		    try
		    {
			    FieldInfo fieldInfo = ReflectionUtils.GetField(result.GetType(), varName);
			    Main.LogDebug($"Assigning : {value} >> {varName}");
			    fieldInfo.SetValue(result, value);
		    }
		    catch (Exception e)
		    {
			    if (!supressError)
			    {
				    Main.LogError($"Failed to assign : {value} >> {varName}");
				    Main.LogError(e);
			    }
		    }
	    }
	    
	    protected void ConvertInfoListToLocalisationObject<L>(List<(Locale, L)> InfoList, ref LocalisationObject<L> result) where L : Localisation
	    {
		    Main.LogDebug("Setting up localisation");
		    result = new LocalisationObject<L>();

		    L fallback = default;
		    foreach ((Locale, L) info in InfoList)
		    {
			    if (info.Item1 == Locale.English)
			    {
				    fallback = info.Item2;
			    }
			    result.Add(info.Item1, info.Item2);
		    }

		    if (fallback != null)
			    foreach (Locale locale in Enum.GetValues(typeof(Locale)))
				    if (!result.Has(locale))
					    result.Add(locale, fallback);
	    }

	    #endregion
        
        [Obsolete("Use OnRegister(SpecificGDOType) instead")]
        public virtual void OnRegister(GameDataObject gameDataObject) { }

        public int GetHash()
        {
            return StringUtils.GetInt32HashCode($"{ModID}:{UniqueNameID}");
        }
        public int GetLegacyHash()
        {
            return StringUtils.GetInt32HashCode($"{ModName}:{UniqueNameID}");
        }
    }
    
    public abstract class CustomGameDataObject<T> : CustomGameDataObject where T : GameDataObject
    {
        [Obsolete("Use OnRegister(SpecificGDOType) instead")]
        public override void OnRegister(GameDataObject gameDataObject)
        {
            OnRegister(gameDataObject as T);
        }
        public virtual void OnRegister(T gameDataObject) { }


        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
	        GameDataObject = ScriptableObject.CreateInstance<T>();
	        gameDataObject = GameDataObject;
	        OverrideVariable(gameDataObject, "ID", ID);
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject) { }
    }
}