﻿using KitchenLib.Views;

namespace KitchenLib.Utils
{
	/// <summary>
	/// Utilities related to views. See also AddViewType in BaseMod.
	/// </summary>
	public class ViewUtils
	{
		/// <summary>
		/// Find a custom view type by mod ID and view ID.
		/// </summary>
		/// <param name="modId">The registering mod ID.</param>
		/// <param name="viewId">The view's ID.</param>
		/// <returns>The CustomViewType corresponding to the ID.</returns>
		public static CustomViewType GetViewType(string modId, string viewId)
		{
			return GetViewType(StringUtils.GetInt32HashCode(modId + ":" + viewId));
		}

		/// <summary>
		/// Find a custom view type by mod ID and view ID.
		/// </summary>
		/// <param name="id">The view's ID.</param>
		/// <returns>The CustomViewType corresponding to the ID.</returns>
		public static CustomViewType GetViewType(int id)
		{
			if (CustomViewType.Values.TryGetValue(id, out var viewType))
			{
				return viewType;
			}

			return CustomViewType.None;
		}
	}
}
