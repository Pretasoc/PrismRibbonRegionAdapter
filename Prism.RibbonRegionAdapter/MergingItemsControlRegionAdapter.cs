using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Prism.RibbonRegionAdapter
{
	public class MergingItemsControlRegionAdapter : RegionAdapterBase<ItemsControl>
	{

		/// <summary>
		/// Initializes a new instance of <see cref="MergingItemsControlRegionAdapter"/>.
		/// </summary>
		/// <param name="regionBehaviorFactory">The factory used to create the region behaviors to attach to the created regions.</param>
		public MergingItemsControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
			: base(regionBehaviorFactory)
		{
		}

		/// <summary>
		/// Adapts a <see cref="ContentControl"/> to an <see cref="IRegion"/>.
		/// </summary>
		/// <param name="region">The new region being used.</param>
		/// <param name="regionTarget">The object to adapt.</param>
		protected override void Adapt(IRegion region, ItemsControl regionTarget)
		{
			if (regionTarget == null) throw new ArgumentNullException("regionTarget");

			region.ActiveViews.CollectionChanged += (o, e) => OnActiveViewsChanged(o, e, region, regionTarget);
			region.Views.CollectionChanged += (o, e) => OnViewsChanged(o, e, region, regionTarget);
		}

		private void OnActiveViewsChanged(object o, NotifyCollectionChangedEventArgs e, IRegion region, ItemsControl regionTarget)
		{
			var sel = regionTarget as Selector;
			if (sel != null)
				sel.SelectedItem = region.ActiveViews.FirstOrDefault();
		}

		private void OnViewsChanged(object o, NotifyCollectionChangedEventArgs e, IRegion region, ItemsControl regionTarget)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					MergeItems(e.NewItems, regionTarget);
					break;
				case NotifyCollectionChangedAction.Remove:
					RemoveItems(e.NewItems, regionTarget);
					break;
			}
		}

		protected virtual void MergeItems(System.Collections.IList list, ItemsControl regionTarget)
		{
			foreach (object item in list)
			{
				if (item is ItemsControl)
					MergeItemsControl((ItemsControl)item, regionTarget);
				else
					MergeNonItemsControl(item as UIElement, regionTarget);
			}
		}

		protected virtual void MergeNonItemsControl(UIElement item, ItemsControl regionTarget)
		{
			if (item == null)
				return;
			InsertItem(item, regionTarget);
		}

		protected virtual internal void MergeItemsControl(ItemsControl source, ItemsControl target)
		{
			var items = source.Items.Cast<UIElement>().ToArray();
			foreach (UIElement item in items)
			{
				if (item is ItemsControl)
				{
					var existingItem = target.Items
						.OfType<ItemsControl>()
						.FirstOrDefault(t => ItemsMatch(t, item));
					if (existingItem == null)
						InsertItem(item, target);
					else
						MergeItemsControl((ItemsControl)item, existingItem);
				}
				else
				{
					InsertItem(item, target);
				}
			}
		}

		private void InsertItem(UIElement item, ItemsControl target)
		{
			item.DisconnectFromParent();
			UIElementExtension.InsertSorted(item, target.Items);
		}

		protected virtual internal void RemoveItems(System.Collections.IList list, ItemsControl target)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a new instance of <see cref="SingleActiveRegion"/>.
		/// </summary>
		/// <returns>A new instance of <see cref="SingleActiveRegion"/>.</returns>
		protected override IRegion CreateRegion()
		{
			return new SingleActiveRegion();
		}

		protected virtual internal bool ItemsMatch(UIElement item1, UIElement item2)
		{
			var tab1Id = GetMergeKey(item1);
			var tab2Id = GetMergeKey(item2);

			return tab1Id.Equals(tab2Id);
		}

		protected virtual internal string GetMergeKey(UIElement item)
		{
			var key = UIElementExtension.GetMergeKey(item);
			if (string.IsNullOrEmpty(key))
			{
				if (item is HeaderedItemsControl)
					key = ((HeaderedItemsControl)item).Header as string;
				if (string.IsNullOrEmpty(key) && item is FrameworkElement)
					key = ((FrameworkElement)item).Name;
				if (string.IsNullOrEmpty(key))
					key = Guid.NewGuid().ToString();
				UIElementExtension.SetMergeKey(item, key);
			}
			return key;
		}

	}
}
