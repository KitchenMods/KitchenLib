using System;
using Controllers;
using Kitchen.Modules;
using KitchenData;
using Shapes;
using TMPro;
using UnityEngine;
using Font = KitchenData.Font;

namespace KitchenLib
{
	public class AchivementElement : MouseElement
	{
		public AchivementElement SetUnlockDate(string label)
		{
			UnlockDate.text = label;
			return this;
		}

		public AchivementElement SetTitle(string label)
		{
			Title.text = label;
			return this;
		}

		public AchivementElement SetDescription(string label)
		{
			Description.text = label;
			return this;
		}

		public override bool HandleInteraction(InputState state)
		{
			if (state.MenuSelect == ButtonState.Pressed)
			{
				OnActivate();
				return true;
			}
			return false;
		}

		private static readonly int Image = Shader.PropertyToID("_Image");
		
		public AchivementElement SetIcon(Texture2D icon)
		{
			if (icon == null)
				icon = Main.bundle.LoadAsset<Texture2D>("defaultAchievementIcon");
			
			Icon.material.SetTexture(Image, icon);
			return this;
		}
		
		
		private static Color defaultUnlockColor = new Color(0.5921569f, 0.6666667f, 0.8078432f);
		private static Color defaultTitleColor = new Color(0.1215686f, 0.2941177f, 0.7019608f);
		private static Color defaultDescriptionColor = new Color(0.3686365f, 0.4721985f, 0.7169812f);
		
		private static Color greyscaleUnlockColor = new Color(0.6588235f, 0.6588235f, 0.6588235f);
		private static Color greyscaleTitleColor = new Color(0.2901961f, 0.2901961f, 0.2901961f);
		private static Color greyscaleDescriptionColor = new Color(0.4666667f, 0.4666667f, 0.4666667f);
		
		public AchivementElement SetUnlocked(bool isGrayscale)
		{
			Icon.material.SetFloat("_IsBlowout", isGrayscale ? 1 : 0);
			Icon.material.SetFloat("_BlowoutScale1", 0);
			Icon.material.SetFloat("_BlowoutOffset1", 0);
			
			UnlockDate.color = isGrayscale ? greyscaleUnlockColor : defaultUnlockColor;
			Title.color = isGrayscale ? greyscaleTitleColor : defaultTitleColor;
			Description.color = isGrayscale ? greyscaleDescriptionColor : defaultDescriptionColor;
			
			return this;
		}

		public override Bounds BoundingBox
		{
			get
			{
				return new Bounds
				{
					center = transform.localPosition,
					size = new Vector3(4.4f, 1.2f, 0.1f)
				};
			}
		}

		public override void OnMouseUIUp(Vector3 position)
		{
			if (!IsSelectable || !gameObject.activeInHierarchy)
			{
				return;
			}
			base.OnMouseUIUp(position);
			OnActivate();
		}

		public AchivementElement SetStyle(ElementStyle style)
		{
			FontStyles fontStyles;
			if (style == ElementStyle.MainMenu)
			{
				fontStyles = FontStyles.Normal;
			}
			else
			{
				fontStyles = FontStyles.Normal;
			}
			
			TMP_FontAsset tmp_FontAsset;
			if (style != ElementStyle.MainMenu)
			{
				if (style != ElementStyle.MainMenuBack)
				{
					tmp_FontAsset = GameData.Main.GlobalLocalisation.Fonts[Font.Default];
				}
				else
				{
					tmp_FontAsset = GameData.Main.GlobalLocalisation.Fonts[Font.MainMenu];
				}
			}
			else
			{
				tmp_FontAsset = GameData.Main.GlobalLocalisation.Fonts[Font.MainMenu];
			}
			
			Color black;
			if (style != ElementStyle.MainMenu)
			{
				if (style != ElementStyle.MainMenuBack)
				{
					black = Color.black;
				}
				else
				{
					black = new Color(0.35f, 0.36f, 0.41f);
				}
			}
			else
			{
				black = new Color(0.35f, 0.36f, 0.41f);
			}
			
			
			UnlockDate.fontStyle = fontStyles;
			Title.fontStyle = fontStyles;
			Description.fontStyle = fontStyles;
			UnlockDate.font = tmp_FontAsset;
			Title.font = tmp_FontAsset;
			Description.font = tmp_FontAsset;
			BackingBorder.Color = black;
			return this;
		}

		public Rectangle BackingBorder;
		public Rectangle MouseBackingBorder;
		public TextMeshPro UnlockDate;
		public TextMeshPro Title;
		public TextMeshPro Description;
		public MeshRenderer Icon;
		public event Action OnActivate = delegate
		{
		};
	}
}
