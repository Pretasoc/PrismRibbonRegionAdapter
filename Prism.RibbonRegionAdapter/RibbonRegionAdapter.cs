using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Prism.RibbonRegionAdapter
{
	public class RibbonRegionAdapter : RegionAdapterBase<Ribbon>
	{

		/// <summary>
		/// Initializes a new instance of <see cref="ContentControlRegionAdapter"/>.
		/// </summary>
		/// <param name="regionBehaviorFactory">The factory used to create the region behaviors to attach to the created regions.</param>
		public RibbonRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
			: base(regionBehaviorFactory)
		{
		}

		/// <summary>
		/// Adapts a <see cref="ContentControl"/> to an <see cref="IRegion"/>.
		/// </summary>
		/// <param name="region">The new region being used.</param>
		/// <param name="regionTarget">The object to adapt.</param>
		protected override void Adapt(IRegion region, Ribbon regionTarget)
		{
			if (regionTarget == null) throw new ArgumentNullException("regionTarget");

			region.ActiveViews.CollectionChanged += (o, e) => OnActiveViewsChanged(o, e, region, regionTarget);
			region.Views.CollectionChanged += (o, e) => OnViewsChanged(o, e, region, regionTarget);
		}

		private void OnActiveViewsChanged(object o, NotifyCollectionChangedEventArgs e, IRegion region, Ribbon ribbon)
		{
			ribbon.SelectedItem = region.ActiveViews.FirstOrDefault();
		}

		private void OnViewsChanged(object o, NotifyCollectionChangedEventArgs e, IRegion region, Ribbon ribbon)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					MergeViews(e.NewItems, ribbon);
					break;
				case NotifyCollectionChangedAction.Remove:
					RemoveViews(e.NewItems, ribbon);
					break;
			}
		}

		protected virtual internal void MergeViews(System.Collections.IList list, Ribbon ribbon)
		{
			foreach (var view in list)
			{
				var ribbonToMerge = GetRibbon((UIElement)view);
				MergeRibbon(ribbonToMerge, ribbon);
			}
		}

		protected virtual internal Ribbon GetRibbon(UIElement element)
		{
			if (element is Ribbon)
				return (Ribbon)element;
			else if (element is UserControl)
			{
				var uc = (UserControl)element;
				if (uc.Content is Ribbon)
					return (Ribbon)uc.Content;
				else
					throw new NotSupportedException(string.Format("UserControl.Content of {0} is not a ribbon", element.GetType().FullName));
			}
			throw new NotSupportedException(string.Format("Cannot merge view of type {0} with a ribbon", element.GetType().FullName));
		}

		protected virtual internal void MergeRibbon(Ribbon moduleRibbon, Ribbon ribbon)
		{
			MergeApplicationMenu(moduleRibbon, ribbon);
			MergeQuickAccessToolbar(moduleRibbon, ribbon);
			MergeContextualTabGroups(moduleRibbon, ribbon);
			MergeItemsControl(moduleRibbon, ribbon);
		}

		protected virtual internal void MergeQuickAccessToolbar(Ribbon moduleRibbon, Ribbon ribbon)
		{
			if (moduleRibbon.QuickAccessToolBar != null)
			{
				if (ribbon.QuickAccessToolBar == null)
					ribbon.QuickAccessToolBar = new RibbonQuickAccessToolBar();
				MergeItemsControl(moduleRibbon.QuickAccessToolBar, ribbon.QuickAccessToolBar);
			}
		}

		protected virtual internal void MergeApplicationMenu(Ribbon moduleRibbon, Ribbon ribbon)
		{
			if (moduleRibbon.ApplicationMenu != null)
			{
				if (ribbon.ApplicationMenu == null)
					ribbon.ApplicationMenu = new RibbonApplicationMenu();
				MergeItemsControl(moduleRibbon.ApplicationMenu, ribbon.ApplicationMenu);
			}
		}

		protected virtual internal void MergeContextualTabGroups(Ribbon moduleRibbon, Ribbon ribbon)
		{
			foreach (RibbonContextualTabGroup group in moduleRibbon.ContextualTabGroups)
			{
				if (!ribbon.ContextualTabGroups.Any(t => ItemsMatch(t, group)))
				{
					group.DisconnectFromParent();
					ribbon.ContextualTabGroups.Add(group);
				}
			}
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

		protected virtual internal void RemoveViews(System.Collections.IList list, Ribbon ribbon)
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
