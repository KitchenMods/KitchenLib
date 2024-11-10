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
	public class AchievementNotification : UpdatableObjectView<AchievementNotification.ViewData>
	{
		public class UpdateView : ViewSystemBase, IModSystem
		{
			private EntityQuery views;
			
			protected override void Initialise()
			{
				base.Initialise();
				views = GetEntityQuery(new QueryHelper().All(typeof(SAchievementDisplayView.Marker), typeof(CLinkedView)));
			}

			protected override void OnUpdate()
			{
				using NativeArray<Entity> entities = views.ToEntityArray(Allocator.Temp);
				foreach (Entity entity in entities)
				{
					if (Require(entity, out CLinkedView cLinkedView))
					{
						if (!HasBuffer<SAchievementDisplayView>(entity))
						{
							EntityManager.AddBuffer<SAchievementDisplayView>(entity);
						}

						DynamicBuffer<SAchievementDisplayView> buffer = GetBuffer<SAchievementDisplayView>(entity);
						
						if (buffer.Length == 0) continue;
						
						SAchievementDisplayView display = buffer[0];
						
						SendUpdate(cLinkedView.Identifier, new ViewData
						{
							modId = display.modId,
							achievementKey = display.achivementKey
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
		
		public Animator Animator;
		public TextMeshPro Title;
		public TextMeshPro Description;
		public Renderer Icon;

		private static readonly int Image = Shader.PropertyToID("_Image");
		
		
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
			if (Animator== null) return;
			
			if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
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

				if (Title != null)
				{
					Title.text = achievement.Name;
				}

				if (Description != null)
				{
					Description.text = achievement.Description;
				}

				if (Icon != null)
				{
					Icon.material.SetTexture(Image, achievement.Icon);
				}

				Animator.Play("Idle", 0);
				Animator.Play("Display", 0);
			}
		}
	}
	
	public struct PendingNotification
	{
		public string modId;
		public string achievementKey;
	}
}