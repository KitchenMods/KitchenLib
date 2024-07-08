using System;
using System.Collections.Generic;
using Kitchen;
using KitchenLib.Achievements;
using KitchenLib.Components;
using KitchenMods;
using MessagePack;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Views
{
	public class AchievementTicketView : UpdatableObjectView<AchievementTicketView.ViewData>
	{
		public class UpdateView : ViewSystemBase, IModSystem
		{
			private EntityQuery views;
			
			protected override void Initialise()
			{
				base.Initialise();
				views = GetEntityQuery(new QueryHelper().All(typeof(SAchievementTicketView.Marker), typeof(CLinkedView)));
			}

			protected override void OnUpdate()
			{
				using NativeArray<Entity> entities = views.ToEntityArray(Allocator.Temp);
				foreach (Entity entity in entities)
				{
					if (Require(entity, out CLinkedView cLinkedView))
					{
						if (!HasBuffer<SAchievementTicketView>(entity))
						{
							EntityManager.AddBuffer<SAchievementTicketView>(entity);
						}

						DynamicBuffer<SAchievementTicketView> buffer = GetBuffer<SAchievementTicketView>(entity);
						
						if (buffer.Length == 0) continue;
						
						SAchievementTicketView ticket = buffer[0];
						
						SendUpdate(cLinkedView.Identifier, new ViewData
						{
							modId = ticket.modId,
							achievementKey = ticket.achivementKey
						});
						
						buffer.RemoveAt(0);
					}
				}
			}
		}
		[MessagePackObject(false)]
		public struct ViewData : IViewData, IViewData.ICheckForChanges<ViewData>
		{
			public bool IsChangedFrom(ViewData check)
			{
				return modId != check.modId || achievementKey != check.achievementKey;
			}
			[Key(0)] public FixedString32 modId;
			[Key(0)] public FixedString32 achievementKey;
		}

		public Animator animator;
		public TextMeshPro Title;
		public TextMeshPro Description;
		private static int TriggerAnimation = Animator.StringToHash("TriggerAnimation");
		
		private readonly List<PendingNotification> pendingNotifications = new List<PendingNotification>();
		
		protected override void UpdateData(ViewData data)
		{
			pendingNotifications.Add(new PendingNotification
			{
				modId = data.modId.ToString(),
				achievementKey = data.achievementKey.ToString()
			});
		}

		private void Update()
		{
			if (animator == null) return;
			if (Title == null) return;
			if (Description == null) return;
			
			int state = GetTicketState();
			
			if (state == 0)
			{
				if (pendingNotifications.Count == 0) return;
				PendingNotification notification = pendingNotifications[0];
				pendingNotifications.RemoveAt(0);
				
				AchievementsManager manager = AchievementsManager.GetManager(notification.modId);
				if (manager == null) return;
				
				Achievement achievement = manager.GetAchievement(notification.achievementKey);
				if (achievement == null) return;

				long unlock = achievement.UnlockDate;
				long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
				
				if (now - unlock > 20000) return;
				
				Title.text = achievement.Name;
				Description.text = achievement.Description;
				
				animator.SetBool(TriggerAnimation, true);
			}
			else if (state == 1)
			{
				animator.SetBool(TriggerAnimation, false);
			}
		}

		private int GetTicketState()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !animator.GetBool(TriggerAnimation))
			{
				return 0;
			}
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("Display") ||  animator.GetBool(TriggerAnimation))
			{
				return 1;
			}

			return -1;
		}
	}
	
	public struct PendingNotification
	{
		public string modId;
		public string achievementKey;
	}
}