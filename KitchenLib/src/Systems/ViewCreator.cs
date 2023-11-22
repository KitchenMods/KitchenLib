using System;
using System.Collections.Generic;
using Kitchen;
using KitchenLib.Components;
using KitchenLib.Utils;
using KitchenLib.Views;
using KitchenMods;
using Unity.Entities;

namespace KitchenLib.Systems
{
	public class ViewCreator : GameSystemBase, IModSystem
	{
		internal static Dictionary<ViewType, (Type, Type)> RegisteredViews = new Dictionary<ViewType, (Type, Type)>();

		public static void RegisterView(ViewType viewType, Type singleton, Type component)
		{
			RegisteredViews.Add(viewType, (singleton, component));
		}
		
		public static void RegisterView(string viewType, Type singleton, Type component)
		{
			RegisterView((ViewType)VariousUtils.GetID(viewType), singleton, component);
		}

		protected override void OnUpdate()
		{
			foreach (ViewType viewType in RegisteredViews.Keys)
			{
				EnsureView(RegisteredViews[viewType].Item1, viewType);
			}
		}

		internal void EnsureView<T>(ViewType viewType) where T : IComponentData, new()
		{
			EnsureView(typeof(T), viewType);
		}

		internal void EnsureView(Type type, ViewType viewType)
		{
			if (EntityManager.CreateEntityQuery(type).CalculateEntityCount() != 1)
			{
				Entity entity = EntityManager.CreateEntity(type, typeof(CRequiresView), typeof(CDoNotPersist), typeof(CPersistThroughSceneChanges));
				EntityManager.SetComponentData(entity, new CRequiresView
				{
					Type = viewType
				});
			}
		}

		internal void EnsureView<T>(int viewType) where T : IComponentData, new()
		{
			EnsureView<T>((ViewType)viewType);
		}

		internal void EnsureView<T>(string viewType) where T : IComponentData, new()
		{
			EnsureView<T>((ViewType)VariousUtils.GetID(viewType));
		}

		internal void EnsureView(Type type, int viewType)
		{
			EnsureView(type, (ViewType)viewType);
		}

		internal void EnsureView(Type type, string viewType)
		{
			EnsureView(type, (ViewType)VariousUtils.GetID(viewType));
		}
	}
}