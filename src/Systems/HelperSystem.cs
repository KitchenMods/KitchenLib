using Kitchen;
using KitchenMods;
using Unity.Entities;

namespace KitchenLib.Systems
{
	public class HelperSystem : GameSystemBase, IModSystem
	{
		/// <summary>
		/// The helper belonging to the world that is running right now, or null when there is no world to
		/// work on, such as in the main menu. Looked up on each use rather than kept in a static field,
		/// because a world is torn down and rebuilt for every session and a system held over from a dead
		/// one still answers a null check as though it were alive.
		/// </summary>
		public static HelperSystem Instance
		{
			get
			{
				// Fully qualified because the World inherited from the system base hides the type here.
				World world = World.DefaultGameObjectInjectionWorld;
				if (world == null || !world.IsCreated)
					return null;

				return world.GetExistingSystem<HelperSystem>();
			}
		}

		protected override void OnUpdate()
		{
		}

		public bool _TryGetSingletonEntity<T>(out Entity entity) where T : struct, IComponentData
		{
			return TryGetSingletonEntity<T>(out entity);
		}
		
		public bool _Has<T>(Entity entity) where  T : struct, IComponentData
		{
			return Has<T>(entity);
		}

		public bool _Require<T>(Entity entity, out T component) where T : struct, IComponentData
		{
			return Require<T>(entity, out component);
		}

		public void _Set<T>(Entity entity, T component) where T : struct, IComponentData
		{
			Set(entity, component);
		}

		public void _Unset<T>(Entity entity) where T : struct, IComponentData
		{
			Unset<T>(entity);
		}
	}
}