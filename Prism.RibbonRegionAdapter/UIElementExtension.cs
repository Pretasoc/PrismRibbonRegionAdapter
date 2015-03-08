using System;
using System.Collections;
using System.Windows;

namespace Prism.RibbonRegionAdapter
{
	/// <summary>
	/// Extensions of <see cref="UIElement"/> instances
	/// </summary>
	public static class UIElementExtension
	{

		#region MergeKeyProperty

		/// <summary>
		/// The property definition of the <see cref="MergeKeyProperty">MergeKey</see>
		/// attached property
		/// </summary>
		public static readonly DependencyProperty MergeKeyProperty = DependencyProperty.RegisterAttached("MergeKey",
			typeof(string), typeof(UIElementExtension), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange));

		/// <summary>
		/// Sets the <see cref="MergeKeyProperty">MergeKey</see> of the supplied <paramref name="element"/>
		/// </summary>
		public static void SetMergeKey(UIElement element, string value)
		{
			element.SetValue(MergeKeyProperty, value);
		}

		/// <summary>
		/// Returns the <see cref="MergeKeyProperty">MergeKey</see> of the supplied <paramref name="element"/>
		/// </summary>
		public static string GetMergeKey(UIElement element)
		{
			return (string)element.GetValue(MergeKeyProperty);
		}

		#endregion

		#region MergeOrderProperty

		/// <summary>
		/// The default <see cref="MergeOrderProperty">MergeOrder</see>
		/// of a <see cref="UIElement"/>, which has not been assigned this property
		/// </summary>
		public const double DefaultMergeOrder = 10000d;

		/// <summary>
		/// The property definition of the <see cref="MergeOrderProperty">MergeOrder</see>
		/// attached property
		/// </summary>
		public static readonly DependencyProperty MergeOrderProperty = DependencyProperty.RegisterAttached("MergeOrder",
			typeof(double), typeof(UIElementExtension), new FrameworkPropertyMetadata(DefaultMergeOrder, FrameworkPropertyMetadataOptions.AffectsArrange));

		/// <summary>
		/// Sets the <see cref="MergeOrderProperty">MergeOrder</see> of the supplied <paramref name="element"/>
		/// </summary>
		public static void SetMergeOrder(UIElement element, double value)
		{
			element.SetValue(MergeOrderProperty, value);
		}

		/// <summary>
		/// Returns the <see cref="MergeOrderProperty">MergeOrder</see> of the supplied <paramref name="element"/>
		/// </summary>
		public static double GetMergeOrder(UIElement element)
		{
			return (double)element.GetValue(MergeOrderProperty);
		}

		/// <summary>
		/// Inserts the supplied <paramref name="item"/> into the given <paramref name="collection"/>
		/// </summary>
        public static void InsertSorted(UIElement item, IList collection, double fallbackOrder = DefaultMergeOrder)
		{
			var order = GetMergeOrder(item);
			if (Math.Abs(order - DefaultMergeOrder) < 0.001)
			{
				order = fallbackOrder;
				SetMergeOrder(item, order);
			}

			int insertPosition = 0;
			foreach (UIElement t in collection)
			{
				var curOrder = GetMergeOrder(t);
				if (curOrder > order)
					break;
				insertPosition++;
			}
			item.DisconnectFromParent();
			collection.Insert(insertPosition, item);
		}
		#endregion

	}
}
