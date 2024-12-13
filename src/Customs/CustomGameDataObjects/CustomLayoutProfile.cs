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
	    // Base-Game Variables
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
        
        // KitchenLib Variables
		public virtual List<NodeConnection> NodeConnections { get; protected set; } = new List<NodeConnection>();

        [Obsolete("Please set your Name in Info")]
        public virtual string Name { get; protected set; } = "New Layout";

        [Obsolete("Please set your Description in Info")]
        public virtual string Description { get; protected set; } = "A new layout type for your restaurants!";
		
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            LayoutProfile result = ScriptableObject.CreateInstance<LayoutProfile>();

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<LayoutProfile>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			OverrideVariable(result, "ID", ID);
			OverrideVariable(result, "Graph", Graph);
			OverrideVariable(result, "MaximumTables", MaximumTables);
			OverrideVariable(result, "Info", Info);
			
			if (NodeConnections.Count > 0)
			{
				Main.LogDebug($"Setting up NodeConnections");
				LayoutGraph layoutGraph = ScriptableObject.CreateInstance<LayoutGraph>();
				layoutGraph.nodes = new List<Node>();
				PopulateConnections(ref result.Graph, NodeConnections);
			}
			

            if (InfoList.Count > 0)
            {
	            SetupLocalisation<BasicInfo>(InfoList, ref result.Info);
            }
            else
            {
	            if (result.Info == null)
	            {
		            Main.LogDebug($"Setting up fallback localisation");
		            result.Info = new LocalisationObject<BasicInfo>();
		            if (!result.Info.Has(Locale.English))
		            {
			            BasicInfo basicInfo = ScriptableObject.CreateInstance<BasicInfo>();
			            basicInfo.Name = Name;
			            basicInfo.Description = Description;
			            result.Info.Add(Locale.English, basicInfo);
		            }
	            }
            }
            
			gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            LayoutProfile result = (LayoutProfile)gameDataObject;
            
            OverrideVariable(result, "RequiredAppliances", RequiredAppliances);
            OverrideVariable(result, "Table", Table);
            OverrideVariable(result, "Counter", Counter);
            OverrideVariable(result, "ExternalBin", ExternalBin);
            OverrideVariable(result, "WallPiece", WallPiece);
            OverrideVariable(result, "InternalWallPiece", InternalWallPiece);
            OverrideVariable(result, "StreetPiece", StreetPiece);
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
			}
		}
	}
}