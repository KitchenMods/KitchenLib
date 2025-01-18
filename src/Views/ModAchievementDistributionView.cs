using Kitchen;
using KitchenLib.Achievements;
using KitchenLib.Components;
using MessagePack;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Views
{
	public class ModAchievementDistributionView : UpdatableObjectView<ModAchievementDistributionView.ViewData>
	{
		public class UpdateView : ViewSystemBase
		{
			private EntityQuery views;
			
			protected override void Initialise()
			{
				base.Initialise();
				views = GetEntityQuery(new QueryHelper().All(typeof(CRequestAchievementUnlock), typeof(CLinkedView)));
			}

			protected override void OnUpdate()
			{
				using NativeArray<Entity> entities = views.ToEntityArray(Allocator.Temp);
				foreach (Entity entity in entities)
				{
					if (Require(entity, out CLinkedView cLinkedView) && Require(entity, out CRequestAchievementUnlock cRequestAchievementUnlock))
					{
						SendUpdate(cLinkedView.Identifier, new ViewData
						{
							modId = cRequestAchievementUnlock.modId,
							achievementKey = cRequestAchievementUnlock.achivementKey
						});
					}
				}
			}
		}
		[MessagePackObject(false)]
		public struct ViewData : IViewData, IViewResponseData
		{
			[Key(0)] public FixedString64 modId;
			[Key(1)] public FixedString64 achievementKey;
		}
		
		private bool IsRemoved;
		private bool IsGranted;

		protected override void UpdateData(ViewData data)
		{
			AchievementsManager manager = AchievementsManager.GetManager(data.modId.ToString());
			if (manager == null) return;
			
			manager.CompleteAchievement(data.achievementKey.ToString());
			IsGranted = true;
			CheckForRemoval();
		}
		
		private void CheckForRemoval()
		{
			if (IsRemoved && IsGranted)
			{
				base.Remove();
			}
		}

		public override void Remove()
		{
			IsRemoved = true;
			CheckForRemoval();
		}
	}
}