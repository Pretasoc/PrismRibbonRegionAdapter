using Microsoft.Practices.Prism.Regions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;

namespace Prism.RibbonRegionAdapter
{
	public class RibbonRegionAdapter : MergingItemsControlRegionAdapter
	{

		/// <summary>
		/// Initializes a new instance of <see cref="RibbonRegionAdapter"/>.
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
		protected override void Adapt(IRegion region, ItemsControl regionTarget)
		{
			Adapt(regionTarget);
			base.Adapt(region, regionTarget);
		}

		internal void Adapt(ItemsControl regionTarget)
		{
			if (!(regionTarget is Ribbon))
				throw new ArgumentNullException("target must be of Type " + typeof(Ribbon).FullName);
		}

		protected internal override void MergeNonItemsControl(UIElement item, ItemsControl target)
		{
			if (item == null)
				return;

			var ribbon = (Ribbon)target;
			var ribbonToMerge = GetRibbon(item);
			MergeRibbon(item, ribbonToMerge, ribbon);
		}

		protected override void Unmerge(object view, ItemsControl target)
		{
			var ribbon = (Ribbon) target;
			var list = GetMergedItemsByView(view);
			if (list == null)
				return;
			/* RibbonContextualTabGroup items are not part of the visual tree (if they are not visible), 
			 * so we must handle them separately */
			list.OfType<RibbonContextualTabGroup>().ToList()
				.ForEach(i => ribbon.ContextualTabGroups.Remove((i)));
			base.Unmerge(view, target);
		}

		protected virtual internal Ribbon GetRibbon(UIElement element)
		{
			if (element is Ribbon)
				return (Ribbon)element;
			if (element is UserControl)
			{
				var uc = (UserControl)element;
				if (uc.Content is Ribbon)
					return (Ribbon)uc.Content;
				throw new NotSupportedException(string.Format("UserControl.Content of {0} is not a ribbon", element.GetType().FullName));
			}
			throw new NotSupportedException(string.Format("Cannot merge view of type {0} with a ribbon", element.GetType().FullName));
		}

		protected virtual internal void MergeRibbon(object sourceView, Ribbon moduleRibbon, Ribbon ribbon)
		{
			MergeApplicationMenu(sourceView, moduleRibbon, ribbon);
			MergeQuickAccessToolbar(sourceView, moduleRibbon, ribbon);
			MergeContextualTabGroups(sourceView, moduleRibbon, ribbon);
			MergeItemsControl(sourceView, moduleRibbon, ribbon);
		}

		protected virtual internal void MergeQuickAccessToolbar(object sourceView, Ribbon moduleRibbon, Ribbon ribbon)
		{
			if (moduleRibbon.QuickAccessToolBar != null)
			{
				if (ribbon.QuickAccessToolBar == null)
					ribbon.QuickAccessToolBar = new RibbonQuickAccessToolBar();
				MergeItemsControl(sourceView, moduleRibbon.QuickAccessToolBar, ribbon.QuickAccessToolBar);
			}
		}

		protected virtual internal void MergeApplicationMenu(object sourceView, Ribbon moduleRibbon, Ribbon ribbon)
		{
			if (moduleRibbon.ApplicationMenu != null)
			{
				if (ribbon.ApplicationMenu == null)
					ribbon.ApplicationMenu = new RibbonApplicationMenu();
				MergeItemsControl(sourceView, moduleRibbon.ApplicationMenu, ribbon.ApplicationMenu);
			}
		}

		protected virtual internal void MergeContextualTabGroups(object sourceView, Ribbon moduleRibbon, Ribbon ribbon)
		{
			foreach (RibbonContextualTabGroup group in moduleRibbon.ContextualTabGroups)
			{
				if (!ribbon.ContextualTabGroups.Any(t => ItemsMatch(t, group)))
				{
					group.DisconnectFromParent();
					if (group.DataContext == null)
						group.DataContext = moduleRibbon.DataContext;
					InsertItem(sourceView, group, ribbon.ContextualTabGroups);
				}
			}
		}

		protected virtual internal void RemoveViews(System.Collections.IList list, Ribbon ribbon)
		{
			throw new NotImplementedException();
		}

	}
}
