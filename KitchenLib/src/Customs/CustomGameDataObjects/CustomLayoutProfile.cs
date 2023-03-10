using Kitchen.Layouts;
using KitchenData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomLayoutProfile : CustomLocalisedGameDataObject<BasicInfo>
	{
		public virtual LayoutGraph Graph { get; protected set; }
		public virtual int MaximumTables { get; protected set; } = 3;
		public virtual List<GameDataObject> RequiredAppliances { get; protected set; } = new List<GameDataObject>();
		public virtual GameDataObject Table { get; protected set; }
		public virtual GameDataObject Counter { get; protected set; }
		public virtual Appliance ExternalBin { get; protected set; }
		public virtual Appliance WallPiece { get; protected set; }
		public virtual Appliance InternalWallPiece { get; protected set; }
		public virtual Appliance StreetPiece { get; protected set; }

		[Obsolete("Please set your Name in Info")]
		public virtual string Name { get; protected set; } = "New Layout";

		[Obsolete("Please set your Description in Info")]
		public virtual string Description { get; protected set; } = "A new layout type for your restaurants!";

		//private static readonly LayoutProfile empty = ScriptableObject.CreateInstance<LayoutProfile>();
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			LayoutProfile result = ScriptableObject.CreateInstance<LayoutProfile>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<LayoutProfile>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.Graph != Graph) result.Graph = Graph;
			if (result.MaximumTables != MaximumTables) result.MaximumTables = MaximumTables;
			if (result.Info != Info) result.Info = Info;

			if (InfoList.Count > 0)
			{
				result.Info = new LocalisationObject<BasicInfo>();
				foreach ((Locale, BasicInfo) info in InfoList)
					result.Info.Add(info.Item1, info.Item2);
			}

			if (result.Info == null)
			{
				result.Info = new LocalisationObject<BasicInfo>();
				if (!result.Info.Has(Locale.English))
				{
					result.Info.Add(Locale.English, new BasicInfo
					{
						Name = Name,
						Description = Description
					});
				}
			}

			gameDataObject = result;
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			LayoutProfile result = (LayoutProfile)gameDataObject;

			if (result.RequiredAppliances != RequiredAppliances) result.RequiredAppliances = RequiredAppliances;
			if (result.Table != Table) result.Table = Table;
			if (result.Counter != Counter) result.Counter = Counter;
			if (result.ExternalBin != ExternalBin) result.ExternalBin = ExternalBin;
			if (result.WallPiece != WallPiece) result.WallPiece = WallPiece;
			if (result.InternalWallPiece != InternalWallPiece) result.InternalWallPiece = InternalWallPiece;
			if (result.StreetPiece != StreetPiece) result.StreetPiece = StreetPiece;
		}
	}
}