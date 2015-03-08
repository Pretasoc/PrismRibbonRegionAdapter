using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Specialized;
using System.Linq;
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

		// ReSharper disable UnusedParameter.Local
		private void OnActiveViewsChanged(object o, NotifyCollectionChangedEventArgs e, IRegion region, ItemsControl target)
		{
			var sel = target as Selector;
			if (sel != null)
				sel.SelectedItem = region.ActiveViews.FirstOrDefault();
		}

		private void OnViewsChanged(object o, NotifyCollectionChangedEventArgs e, IRegion region, ItemsControl target)
		{
			if (e.NewItems != null)
				MergeItems(e.NewItems, target);
			if (e.OldItems != null)
				UnmergeItems(e.OldItems, target);
		}
		// ReSharper restore UnusedParameter.Local

		protected virtual void MergeItems(IList list, ItemsControl target)
		{
			if (list == null)
				return;

			foreach (object item in list)
			{
				if (item is ItemsControl)
					MergeItemsControl(item, (ItemsControl)item, target);
				else
					MergeNonItemsControl(item as UIElement, target);
			}
		}

		protected virtual internal void MergeNonItemsControl(UIElement item, ItemsControl target)
		{
			if (item == null)
				return;
			InsertItem(item, item, target.Items);
		}

		protected virtual internal void MergeItemsControl(object sourceView, ItemsControl source, ItemsControl target)
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
						InsertItem(sourceView, item, target.Items);
					else
						MergeItemsControl(sourceView, (ItemsControl)item, existingItem);
				}
				else
				{
					InsertItem(sourceView, item, target.Items);
				}
			}
		}

		private readonly Dictionary<object, List<UIElement>> _mergedItemsByView = new Dictionary<object, List<UIElement>>();

		protected void InsertItem(object sourceView, UIElement item, IList target)
		{
			RememberMergedItem(sourceView, item);
			item.DisconnectFromParent();
			UIElementExtension.InsertSorted(item, target);
		}

		private void RememberMergedItem(object sourceView, UIElement item)
		{
			if (!_mergedItemsByView.ContainsKey(sourceView))
				_mergedItemsByView.Add(sourceView, new List<UIElement>());
			var list = _mergedItemsByView[sourceView];
			list.Add(item);
		}

		protected virtual internal void UnmergeItems(IList list, ItemsControl target)
		{
			if (list == null)
				return;

			foreach (var item in list)
			{
				Unmerge(item, target);
			}
		}

		protected virtual void Unmerge(object view, ItemsControl target)
		{
			var list = GetMergedItemsByView(view);
			if (list == null)
				return;
			list.ForEach(i => i.DisconnectFromParent(false));
			_mergedItemsByView.Remove(view);
		}

		protected virtual internal List<UIElement> GetMergedItemsByView(object view)
		{
			if (!_mergedItemsByView.ContainsKey(view))
				return null;
			var list = _mergedItemsByView[view];
			return list;
		}

		/// <summary>
		/// Creates a new instance of <see cref="SingleActiveRegion"/>
		/// </summary>
		/// <returns>A new instance of <see cref="SingleActiveRegion"/></returns>
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
