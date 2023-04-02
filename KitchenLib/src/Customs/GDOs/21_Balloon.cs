using Kitchen;
using KitchenData;
using KitchenLib.References;
using UnityEngine;
using Shapes;

namespace KitchenLib.Customs.GDOs
{
	public class _21_Balloon : CustomAppliance
	{
		public override string UniqueNameID => "_21_Balloon";
		public override int BaseGameDataObjectID => ApplianceReferences.HeartBalloons;
		public override GameObject Prefab => Main.bundle.LoadAsset<GameObject>("21_Balloon");
		public override string Name => "21st Balloon";

		public override void OnRegister(Appliance gameDataObject)
		{
			HoldPointContainer container = gameDataObject.Prefab.AddComponent<HoldPointContainer>();
			container.HoldPoint = gameDataObject.Prefab.transform.Find("HoldPoint");
			GameObject lineGO = gameDataObject.Prefab.transform.Find("Line").gameObject;
			GameObject lineGO2 = gameDataObject.Prefab.transform.Find("Line2").gameObject;
			Line line = lineGO.AddComponent<Line>();
			Line line2 = lineGO2.AddComponent<Line>();
			LineBetween lineBetween = lineGO.AddComponent<LineBetween>();
			LineBetween lineBetween2 = lineGO2.AddComponent<LineBetween>();
			lineBetween.Line = line;
			lineBetween2.Line = line2;
			line.Thickness = 0.03f;
			line.Color = Color.yellow;
			lineBetween.Target1 = gameDataObject.Prefab.transform.Find("21").Find("LineAnchor1");
			lineBetween.Target2 = gameDataObject.Prefab.transform.Find("Anchor");

			line2.Thickness = 0.03f;
			line2.Color = Color.yellow;
			lineBetween2.Target1 = gameDataObject.Prefab.transform.Find("21").Find("LineAnchor2");
			lineBetween2.Target2 = gameDataObject.Prefab.transform.Find("Anchor");
		}
	}
}
