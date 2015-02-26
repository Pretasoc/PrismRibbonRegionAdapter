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

		Ribbon _targetRibbon;

		/// <summary>
		/// Adapts a <see cref="ContentControl"/> to an <see cref="IRegion"/>.
		/// </summary>
		/// <param name="region">The new region being used.</param>
		/// <param name="regionTarget">The object to adapt.</param>
		protected override void Adapt(IRegion region, ItemsControl regionTarget)
		{
			_targetRibbon = regionTarget as Ribbon;
			if (_targetRibbon == null)
				throw new ArgumentNullException("regionTarget must be of Type " + typeof(Ribbon).FullName);
			base.Adapt(region, regionTarget);
		}

		protected override void MergeNonItemsControl(UIElement item, ItemsControl regionTarget)
		{
			if (item == null)
				return;

			var ribbon = _targetRibbon;
			var ribbonToMerge = GetRibbon((UIElement)item);
			MergeRibbon(ribbonToMerge, ribbon);
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

		protected virtual internal void RemoveViews(System.Collections.IList list, Ribbon ribbon)
		{
			throw new NotImplementedException();
		}

	}
}
