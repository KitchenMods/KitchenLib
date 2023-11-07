using Kitchen;
using KitchenLib.Components;
using KitchenLib.Utils;
using KitchenMods;
using Unity.Entities;

namespace KitchenLib.Systems
{
	public class ViewCreator : GameSystemBase, IModSystem
	{
		protected override void OnUpdate()
		{
			EnsureView<SModSync>("KitchenLib.Views.SyncMods");
		}

		private void EnsureView<T>(ViewType viewType) where T : IComponentData, new()
		{
			if (!HasSingleton<T>())
			{
				Entity entity = EntityManager.CreateEntity(typeof(T), typeof(CRequiresView), typeof(CDoNotPersist));
				EntityManager.SetComponentData(entity, new CRequiresView
				{
					Type = viewType
				});
			}
		}

		private void EnsureView<T>(int viewType) where T : IComponentData, new()
		{
			EnsureView<T>((ViewType)viewType);
		}

		private void EnsureView<T>(string viewType) where T : IComponentData, new()
		{
			EnsureView<T>((ViewType)VariousUtils.GetID(viewType));
		}
	}
}