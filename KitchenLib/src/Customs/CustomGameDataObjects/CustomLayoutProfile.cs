using KitchenData;
using System.Collections.Generic;
using Kitchen.Layouts;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomLayoutProfile : CustomGameDataObject
    {
		public virtual LayoutGraph Graph { get; internal set; }
		public virtual int MaximumTables { get { return 3;}}
		public virtual List<GameDataObject> RequiredAppliances { get { return new List<GameDataObject>(); } }
		public virtual GameDataObject Table { get; internal set; }
		public virtual GameDataObject Counter { get; internal set; }
		public virtual Appliance ExternalBin { get; internal set; }
		public virtual Appliance WallPiece { get; internal set; }
		public virtual Appliance InternalWallPiece { get; internal set; }
		public virtual Appliance StreetPiece { get; internal set; }
		public virtual LocalisationObject<BasicInfo> Info { get; internal set; }
		public virtual string Name { get { return "New Layout";}}
		public virtual string Description { get { return "A new layout type for your restaurants!";}}

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            LayoutProfile result = ScriptableObject.CreateInstance<LayoutProfile>();
			LayoutProfile empty = ScriptableObject.CreateInstance<LayoutProfile>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<LayoutProfile>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (empty.ID != ID) result.ID = ID;
            if (empty.Graph != Graph) result.Graph = Graph;
            if (empty.MaximumTables != MaximumTables) result.MaximumTables = MaximumTables;
            if (empty.RequiredAppliances != RequiredAppliances) result.RequiredAppliances = RequiredAppliances;
            if (empty.Table != Table) result.Table = Table;
            if (empty.Counter != Counter) result.Counter = Counter;
            if (empty.ExternalBin != ExternalBin) result.ExternalBin = ExternalBin;
            if (empty.WallPiece != WallPiece) result.WallPiece = WallPiece;
            if (empty.InternalWallPiece != InternalWallPiece) result.InternalWallPiece = InternalWallPiece;
            if (empty.StreetPiece != StreetPiece) result.StreetPiece = StreetPiece;
            if (empty.Info != Info) result.Info = Info;
            if (empty.Name != Name) result.Name = Name;
            if (empty.Description != Description) result.Description = Description;

            gameDataObject = result ;
        }
    }
}