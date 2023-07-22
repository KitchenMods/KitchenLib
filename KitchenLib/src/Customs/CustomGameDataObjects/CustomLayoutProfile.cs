using Kitchen.Layouts;
using Kitchen.Layouts.Modules;
using KitchenData;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using XNode;

namespace KitchenLib.Customs
{
    public abstract class CustomLayoutProfile : CustomLocalisedGameDataObject<LayoutProfile, BasicInfo>
    {
		[Obsolete("Please use NodeConnections")]
		public virtual LayoutGraph Graph { get; protected set; } = null;
        public virtual int MaximumTables { get; protected set; } = 3;
        public virtual List<GameDataObject> RequiredAppliances { get; protected set; } = new List<GameDataObject>();
        public virtual GameDataObject Table { get; protected set; }
        public virtual GameDataObject Counter { get; protected set; }
        public virtual Appliance ExternalBin { get; protected set; }
        public virtual Appliance WallPiece { get; protected set; }
        public virtual Appliance InternalWallPiece { get; protected set; }
        public virtual Appliance StreetPiece { get; protected set; }
		public virtual List<NodeConnection> NodeConnections { get; protected set; } = new List<NodeConnection>();

        [Obsolete("Please set your Name in Info")]
        public virtual string Name { get; protected set; } = "New Layout";

        [Obsolete("Please set your Description in Info")]
        public virtual string Description { get; protected set; } = "A new layout type for your restaurants!";

        //private static readonly LayoutProfile empty = ScriptableObject.CreateInstance<LayoutProfile>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            LayoutProfile result = ScriptableObject.CreateInstance<LayoutProfile>();

			Main.LogDebug($"[CustomLayoutProfile.Convert] [1.1] Convering Base");

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<LayoutProfile>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;


			if (result.Graph != Graph) result.Graph = Graph;
			if (NodeConnections.Count > 0)
			{
				result.Graph = new LayoutGraph
				{
					nodes = new List<Node>()
				};
				PopulateConnections(ref result.Graph, NodeConnections);
			}



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

			Main.LogDebug($"[CustomLayoutProfile.AttachDependentProperties] [1.1] Convering Base");

			if (result.RequiredAppliances != RequiredAppliances) result.RequiredAppliances = RequiredAppliances;
            if (result.Table != Table) result.Table = Table;
            if (result.Counter != Counter) result.Counter = Counter;
            if (result.ExternalBin != ExternalBin) result.ExternalBin = ExternalBin;
            if (result.WallPiece != WallPiece) result.WallPiece = WallPiece;
            if (result.InternalWallPiece != InternalWallPiece) result.InternalWallPiece = InternalWallPiece;
            if (result.StreetPiece != StreetPiece) result.StreetPiece = StreetPiece;
        }
		public struct NodeConnection
		{
			public Node FromIndex;
			public string FromPortName;

			public Node ToIndex;
			public string ToPortName;

			public NodeConnection(Node fromIndex, Node toIndex, string fromPortName = "Output", string toPortName = "Input")
			{
				FromIndex = fromIndex;
				FromPortName = fromPortName;
				ToIndex = toIndex;
				ToPortName = toPortName;
			}
		}

		static FieldInfo f_ports = ReflectionUtils.GetField<Node>("ports");
		private void PopulateConnections(ref LayoutGraph layoutGraph, List<NodeConnection> connections)
		{
			foreach (NodeConnection connection in connections)
			{
				if (!layoutGraph.nodes.Contains(connection.FromIndex))
					layoutGraph.nodes.Add(connection.FromIndex);
				if (!layoutGraph.nodes.Contains(connection.ToIndex))
					layoutGraph.nodes.Add(connection.ToIndex);
			}
			List<Node> nodes = layoutGraph.nodes;

			foreach (Node node in nodes)
			{
				if (node is LayoutModule layoutModule)
				{
					layoutModule.graph = layoutGraph;
				}
			}

			foreach (NodeConnection connection in connections)
			{
				if (!TryGetNodePort(connection.FromIndex, connection.FromPortName, out NodePort fromPort))
					break;
				if (!TryGetNodePort(connection.ToIndex, connection.ToPortName, out NodePort toPort))
					break;

				fromPort.Connect(toPort);

				bool TryGetNodePort(Node nodeIndex, string portName, out NodePort nodePort)
				{
					nodePort = null;
					if (!nodes.Contains(nodeIndex))
					{
						return false;
					}
					Node node = nodeIndex;
					object obj = f_ports.GetValue(node);
					if (obj == null || !(obj is Dictionary<string, NodePort> nodeDictionary))
					{
						return false;
					}
					if (!nodeDictionary.TryGetValue(portName, out nodePort))
					{
						return false;
					}
					return true;
				}

				void LogNodeError(object msg)
				{
					Main.LogError($"{GetType().FullName} error! {msg}");
				}
			}
		}
	}
}