using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using Kitchen.Layouts;

namespace KitchenLib.Customs
{
	public abstract class CustomLayoutProfile
	{
        public virtual string Name { get {return "New Layout";} }
        public virtual string Description { get {return "A new layout type for your restaurants!";} }
        public virtual LayoutGraph Graph { get; internal set; }
        public virtual int MaximumTables { get {return 3;}}
        public virtual List<GameDataObject> RequiredAppliances { get; internal set; }
        public virtual GameDataObject Table { get; internal set; }
        public virtual GameDataObject Counter { get; internal set; }
        public virtual Appliance ExternalBin { get; internal set; }
        public virtual Appliance WallPiece { get; internal set; }
        public virtual Appliance InternalWallPiece { get; internal set; }
        public virtual Appliance StreetPiece { get; internal set; }

        public virtual int ID { get; internal set; }
        public string ModName { get; internal set; }
        public virtual int BaseLayoutProfileId { get { return -1; } }
        public LayoutProfile LayoutProfile{ get; internal set; }
        public virtual void OnRegister(LayoutProfile layoutProfile) { }
        public int GetHash() { return StringUtils.GetInt32HashCode($"{ModName}:{Name}"); }
    }
}