using KitchenData;
using System;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomEffectRepresentation : CustomLocalisedGameDataObject<EffectRepresentation, EffectInfo>
    {
	    // Base-Game Variables
        [Obsolete("Please set your Name in Info")]
        public virtual string Name { get; protected set; }

        [Obsolete("Please set your Description in Info")]
        public virtual string Description { get; protected set; }

        [Obsolete("Please set your Icon in Info")]
        public virtual string Icon { get; protected set; }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            EffectRepresentation result = ScriptableObject.CreateInstance<EffectRepresentation>();
            
            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "Info", Info);
            
			if (InfoList.Count > 0)
			{
				SetupLocalisation<EffectInfo>(InfoList, ref result.Info);
			}
			else
			{
				if (result.Info == null)
				{
					Main.LogDebug($"Setting up fallback localisation");
					result.Info = new LocalisationObject<EffectInfo>();
					if (!result.Info.Has(Locale.English))
					{
						EffectInfo effectInfo = ScriptableObject.CreateInstance<EffectInfo>();
						effectInfo.Name = Name;
						effectInfo.Description = Description;
						effectInfo.Icon = Icon;
						result.Info.Add(Locale.English, effectInfo);
					}
				}
			}

            gameDataObject = result;
        }
    }
}