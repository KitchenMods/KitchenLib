using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.DevUI
{
	public class DevUIController : MonoBehaviour
	{
		public static List<BaseUI> _uiList = new List<BaseUI>();
		private bool isEnabled = false;
		private bool hasBeenLoaded = false;
		//Menu Controller
		public void Start()
		{
			for (int i = 0; i < _uiList.Count; i++)
			{
				_uiList[i].OnInit();
			}
		}
		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.T))
			{
				if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
				{
					isEnabled = !isEnabled;
				}
			}
		}
		public void OnGUI()
		{
			if (isEnabled)
			{
				WindowRect = GUILayout.Window(VariousUtils.GetID("kitchenlibdevui"), WindowRect, DevWindow, "KitchenLib DevUI", GUILayout.Width(900), GUILayout.Height(1100));
			}
			else
			{
				if (hasBeenLoaded)
				{
					for (int i = 0; i < _uiList.Count; i++)
					{
						if (_uiList[i].IsEnabled)
						{
							_uiList[i].Disable();
							_uiList[i].IsEnabled = false;
						}
					}
					hasBeenLoaded = false;
				}
			}
		}

		//Base Menu

		private Rect WindowRect;
		private static Vector2 scrollPosition;

		private void DevWindow(int id)
		{
			hasBeenLoaded = true;
			GUILayout.Space(2);
			GUILayout.BeginArea(new Rect(0, 20, 100, 1100));
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			foreach (var menu in _uiList)
			{
				if (GUILayout.Button(menu.ButtonName, GUILayout.Width(95), GUILayout.Height(20)))
				{
					for (int i = 0; i < _uiList.Count; i++)
					{
						if (_uiList[i].IsEnabled)
						{
							_uiList[i].Disable();
						}
						_uiList[i].IsEnabled = false;
					}

					menu.IsEnabled = true;
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(100, 20, 795, 1100));
			foreach (var menu in _uiList)
			{
				if (menu.IsEnabled)
				{
					menu.Setup();
				}
			}
			GUILayout.EndArea();
			GUI.DragWindow();
		}
	}

	public abstract class BaseUI
	{
		public string ButtonName = "Button";
		public bool IsEnabled = false;
		public virtual void Setup()
		{
			GUILayout.Label("HAHA Menu");
		}
		public virtual void Disable() { }
		public virtual void OnInit() { }
	}
}
