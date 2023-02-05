using KitchenData;
using System;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomEffectRepresentation : CustomLocalisedGameDataObject<EffectInfo>
    {
		[Obsolete("Please set your Name in Info")]
		public virtual string Name { get; protected set; }

		[Obsolete("Please set your Description in Info")]
		public virtual string Description { get; protected set; }

		[Obsolete("Please set your Icon in Info")]
		public virtual string Icon { get; protected set; }

        private static readonly EffectRepresentation empty = ScriptableObject.CreateInstance<EffectRepresentation>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            EffectRepresentation result = ScriptableObject.CreateInstance<EffectRepresentation>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<EffectRepresentation>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (empty.ID != ID) result.ID = ID;
            //if (empty.Name != Name) result.Name = Name;
            //if (empty.Description != Description) result.Description = Description;
            //if (empty.Icon != Icon) result.Icon = Icon;
			if (empty.Info != Info) result.Info = Info;

			gameDataObject = result;
        }
    }
}