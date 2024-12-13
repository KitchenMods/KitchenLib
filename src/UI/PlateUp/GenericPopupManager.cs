using HarmonyLib;
using Kitchen;
using Kitchen.Modules;
using KitchenData;
using KitchenLib.Utils;
using KitchenMods;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.UI
{
	public class GenericPopupManager : GenericChoicePopupManager, IModSystem
	{
		public const ViewType KL_POPUP_VIEW_TYPE = (ViewType)6288800;
		public const PopupType KL_POPUP_TYPE = (PopupType)6288800;
		public const float MAX_WIDTH = 6f;
		public const float MAX_HEIGHT = 6f;
		public const float MIN_WIDTH = 3f;
		public const float MIN_HEIGHT = 1f;

		public bool WaitingForInjection = false;
		public bool FoundInjection = false;

		public struct Popup
		{
			public string Title;
			public string Description;
			public GenericChoiceType Type;
			public Action OnSuccess;
			public Action OnCancel;
			public TextAlignmentOptions TitleAlignment;
			public TextAlignmentOptions DescriptionAlignment;
			public Color Color;
		}

		public class Colors
		{
			public static readonly Color Default = new Color(0.37f, 0.56f, 0.91f);
			public static readonly Color Cancel = new Color(0.5f, 0.11f, 0.1f);
		}

		private static GenericPopupManager Instance;
		private readonly List<Popup> PopupQueue = new();
		private Popup? ActivePopup = null;

		protected override void Initialise()
		{
			base.Initialise();
			Instance = this;
		}

		protected override void OnUpdate()
		{
			if (PopupQueue.Count > 0 && ActivePopup == null)
			{
				NextPopup();
			}
		}

		public override PopupType ManagedType => KL_POPUP_TYPE;

		public override Entity CreateNewPopup(Entity request)
		{
			return PopupUtilities.CreateGenericPopup(GenericChoiceType.AcceptOrConsentCancel, ManagedType, PopupLocation.Centre);
		}

		protected override bool HandleDecision(Entity popup, GenericChoiceDecision decision)
		{
			if (decision == GenericChoiceDecision.Accept)
			{
				ActivePopup?.OnSuccess?.Invoke();
			}
			else if (decision == GenericChoiceDecision.Cancel)
			{
				ActivePopup?.OnCancel?.Invoke();
			}

			NextPopup();

			return true;
		}

		private void NextPopup()
		{
			ActivePopup = null;
			if (PopupQueue == null)
				return;
			if (PopupQueue.Count == 0)
				return;

			ActivePopup = PopupQueue.First();
			PopupQueue.RemoveAt(0);

			if (ActivePopup == null)
			{
				return;
			}

			PopupUtilities.CreateGenericPopup(ActivePopup?.Type ?? GenericChoiceType.OnlyAccept, KL_POPUP_TYPE, PopupLocation.Centre);
		}

		/// <summary>
		/// Add a popup to the popup queue.
		/// </summary>
		/// <param name="title">The title of the popup.</param>
		/// <param name="description">The description of the popup.</param>
		/// <param name="type">How the user(s) close the popup.</param>
		/// <param name="onSuccess">Action to run when the popup closes successfully.</param>
		/// <param name="onCancel">Action to run when the popup closes by being cancelled.</param>
		/// <param name="titleAlignment">The alignment of the title.</param>
		/// <param name="descriptionAlignment">The alignment of the description.</param>
		/// <param name="color">The color of the popup.</param>
		public static void CreatePopup(string title, string description, GenericChoiceType type = GenericChoiceType.OnlyAccept, Action onSuccess = null, Action onCancel = null, TextAlignmentOptions? titleAlignment = null, TextAlignmentOptions? descriptionAlignment = null, Color? color = null)
		{
			Instance.PopupQueue.Add(new Popup
			{
				Title = title,
				Description = description,
				Type = type,
				OnSuccess = onSuccess,
				OnCancel = onCancel,
				TitleAlignment = titleAlignment ?? TextAlignmentOptions.Left,
				DescriptionAlignment = descriptionAlignment ?? TextAlignmentOptions.Left,
				Color = color ?? Colors.Default
			});
		}

		[HarmonyPatch(typeof(GenericChoiceView), "FirstUpdate")]
		internal class GenericChoiceViewFirstUpdatePatch
		{
			static void Prefix(GenericChoiceView.ViewData view_data)
			{
				if (view_data.TextSet == KL_POPUP_TYPE)
				{
					Instance.WaitingForInjection = true;
				}
			}

			static void Postfix()
			{
				Instance.WaitingForInjection = false;
				Instance.FoundInjection = false;
			}
		}

		[HarmonyPatch(typeof(TextPanelElement), "SetStyle")]
		internal class TextPanelElementSetStylePatch
		{
			static void Postfix(Element.ThemeColour colour, ref TextPanelElement __result, ref TextPanelElement __instance)
			{
				if (Instance.WaitingForInjection && colour == Element.ThemeColour.Default)
				{
					Instance.WaitingForInjection = false;
					Instance.FoundInjection = true;
				}
			}
		}

		[HarmonyPatch(typeof(TextPanelElement), "SetText")]
		internal class TextPanelElementSetTextPatch
		{
			static void Prefix(ref string title, ref string text, ref TextPanelElement __instance)
			{
				if (Instance.FoundInjection)
				{
					__instance.Resize(MAX_WIDTH, MAX_HEIGHT);

					var fTitle = ReflectionUtils.GetField<TextPanelElement>("Title");
					var tmp1 = (TextMeshPro)fTitle.GetValue(__instance);
					tmp1.alignment = TextAlignmentOptions.Left;

					var fText = ReflectionUtils.GetField<TextPanelElement>("Text");
					var tmp2 = (TextMeshPro)fText.GetValue(__instance);
					tmp2.alignment = TextAlignmentOptions.Left;

					tmp1.text = Instance.ActivePopup?.Title ?? "";
					tmp2.text = Instance.ActivePopup?.Description ?? "";
					//tmp2.text = Instance.LeftAlignedDescription;

					// Resize to fit
					tmp2.ForceMeshUpdate(true, true);
					var computedWidth = tmp2.preferredWidth + 0.25f;
					var computedHeight = tmp2.preferredHeight + 0.25f;
					__instance.Resize(Mathf.Max(MIN_WIDTH, Mathf.Min(MAX_WIDTH, computedWidth)), Mathf.Max(MIN_HEIGHT, Mathf.Min(MAX_HEIGHT, computedHeight)));

					tmp1.alignment = Instance.ActivePopup?.TitleAlignment ?? TextAlignmentOptions.Left;
					tmp2.alignment = Instance.ActivePopup?.DescriptionAlignment ?? TextAlignmentOptions.Left;

					title = Instance.ActivePopup?.Title ?? "";
					text = Instance.ActivePopup?.Description ?? "";

					Instance.FoundInjection = false;
				}
			}
		}
	}
}
