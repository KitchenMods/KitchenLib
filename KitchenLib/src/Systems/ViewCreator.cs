using System;
using System.Collections.Generic;
using Kitchen;
using KitchenLib.Utils;
using KitchenMods;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Systems
{
	public class ViewCreator : GameSystemBase, IModSystem
	{
		internal static Dictionary<ViewType, (Type, Type)> RegisteredViews = new Dictionary<ViewType, (Type, Type)>();
		internal static Dictionary<ViewType, (ViewMode, Vector3)> ModesAndPositions = new Dictionary<ViewType, (ViewMode, Vector3)>();

		public static void RegisterView(ViewType viewType, Type singleton, Type component)
		{
			RegisteredViews.Add(viewType, (singleton, component));
		}
		
		public static void RegisterView(string viewType, Type singleton, Type component)
		{
			RegisterView((ViewType)VariousUtils.GetID(viewType), singleton, component);
		}

		public static void RegisterView(ViewType viewType, Type singleton, Type component, ViewMode mode, Vector3 position)
		{
			RegisteredViews.Add(viewType, (singleton, component));
			ModesAndPositions.Add(viewType, (mode, position));
		}
		
		public static void RegisterView(string viewType, Type singleton, Type component, ViewMode mode, Vector3 position)
		{
			RegisterView((ViewType)VariousUtils.GetID(viewType), singleton, component, mode, position);
		}

		protected override void OnUpdate()
		{
			foreach (ViewType viewType in RegisteredViews.Keys)
			{
				if (ModesAndPositions.ContainsKey(viewType))
				{
					EnsureView(RegisteredViews[viewType].Item1, viewType, ModesAndPositions[viewType].Item1, ModesAndPositions[viewType].Item2);
				}
				else
				{
					EnsureView(RegisteredViews[viewType].Item1, viewType, ViewMode.World, Vector3.zero);
				}
			}
		}

		internal void EnsureView<T>(ViewType viewType, ViewMode mode, Vector3 position) where T : IComponentData, new()
		{
			EnsureView(typeof(T), viewType, mode, position);
		}

		internal void EnsureView(Type type, ViewType viewType, ViewMode mode, Vector3 position)
		{
			if (EntityManager.CreateEntityQuery(type).CalculateEntityCount() != 1)
			{
				Entity entity = EntityManager.CreateEntity(type, typeof(CDoNotPersist), typeof(CPersistThroughSceneChanges));
				EntityManager.AddComponentData(entity, new CRequiresView
				{
					Type = viewType,
					ViewMode = mode
				});
				EntityManager.AddComponentData(entity, new CPosition
				{
					Position = position
				});
			}
		}

		internal void EnsureView<T>(int viewType, ViewMode mode, Vector3 position) where T : IComponentData, new()
		{
			EnsureView<T>((ViewType)viewType, mode, position);
		}

		internal void EnsureView<T>(string viewType, ViewMode mode, Vector3 position) where T : IComponentData, new()
		{
			EnsureView<T>((ViewType)VariousUtils.GetID(viewType), mode, position);
		}

		internal void EnsureView(Type type, int viewType, ViewMode mode, Vector3 position)
		{
			EnsureView(type, (ViewType)viewType, mode, position);
		}

		internal void EnsureView(Type type, string viewType, ViewMode mode, Vector3 position)
		{
			EnsureView(type, (ViewType)VariousUtils.GetID(viewType), mode, position);
		}
	}
}