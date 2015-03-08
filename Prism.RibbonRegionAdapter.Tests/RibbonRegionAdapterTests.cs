using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceLocation;
using TestApplication;
using System.Windows.Controls.Ribbon;
using System.Windows.Controls;

namespace Prism.RibbonRegionAdapter.Tests
{
	[TestClass]
	public class RibbonRegionAdapterTests : BasePrismTest
	{

		[TestMethod]
		public void GetMergeKey_Explicit()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var targetRibbon = ctx.GetInstance<MainWindow>().MainMenu;

			var tab = (RibbonTab)targetRibbon.Items[0];
			var key = target.GetMergeKey(tab);
			Assert.AreEqual(MainMenuMergeKeys.GeneralTab, key);
		}

		[TestMethod]
		public void GetMergeKey_FallbackToHeader()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var moduleRibbon = ctx.GetInstance<TestApplication.Module1.Ribbon>().MainMenu;

			var tab = (RibbonTab)moduleRibbon.Items[0];
			var key = target.GetMergeKey(tab);
			Assert.AreEqual(tab.Header, key);
		}

		[TestMethod]
		public void GetRibbon_Success()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var moduleRibbon = ctx.GetInstance<TestApplication.Module1.Ribbon>();

			target.GetRibbon(moduleRibbon);
		}

		[TestMethod, ExpectedException(typeof(NotSupportedException))]
		public void GetRibbon_NoRibbon()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var moduleRibbon = new UserControl();

			target.GetRibbon(moduleRibbon);
		}

		[TestMethod]
		public void MergeContextualTabGroups_KeepContext()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var targetRibbon = ctx.GetInstance<MainWindow>().MainMenu;
			var module = ctx.GetInstance<TestApplication.Module1.Ribbon>();
			var moduleRibbon = module.MainMenu;

			var tab = moduleRibbon.ContextualTabGroups.Single();
			target.MergeContextualTabGroups(module, moduleRibbon, targetRibbon);
			Assert.IsNotNull(tab.DataContext);
			Assert.IsTrue(targetRibbon.ContextualTabGroups.Contains(tab));
		}

		[TestMethod]
		public void ItemsMatch()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var targetRibbon = ctx.GetInstance<MainWindow>().MainMenu;
			var moduleRibbon = ctx.GetInstance<TestApplication.Module1.Ribbon>().MainMenu;

			RibbonTab tab1, tab2;
			RibbonGroup group1, group2;
			bool result;

			tab1 = (RibbonTab)targetRibbon.Items[0];
			tab2 = (RibbonTab)moduleRibbon.Items[1];
			result = target.ItemsMatch(tab1, tab2);
			Assert.IsTrue(result);
			Assert.AreNotEqual(tab1.Header, tab2.Header);

			tab1 = (RibbonTab)targetRibbon.Items[0];
			tab2 = (RibbonTab)moduleRibbon.Items[1];
			group1 = (RibbonGroup)tab1.Items[0];
			group2 = (RibbonGroup)tab2.Items[0];
			result = target.ItemsMatch(group1, group2);
			Assert.IsFalse(result);

			tab1 = (RibbonTab)targetRibbon.Items[0];
			tab2 = (RibbonTab)moduleRibbon.Items[1];
			group1 = (RibbonGroup)tab1.Items[1];
			group2 = (RibbonGroup)tab2.Items[1];
			result = target.ItemsMatch(group1, group2);
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void MergeItemsControl()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var targetRibbon = ctx.GetInstance<MainWindow>().MainMenu;
			var module = ctx.GetInstance<TestApplication.Module1.Ribbon>();
			var moduleRibbon = module.MainMenu;

			var sourceTab = (RibbonTab)moduleRibbon.Items[1];
			var targetTab = (RibbonTab)targetRibbon.Items[0];

			target.MergeItemsControl(module, sourceTab, targetTab);
		}

		[TestMethod]
		public void MergeRibbon()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var targetRibbon = ctx.GetInstance<MainWindow>().MainMenu;
			var module = ctx.GetInstance<TestApplication.Module1.Ribbon>();

			Assert.AreEqual(2, targetRibbon.Items.Count);
			Assert.IsNull(targetRibbon.ApplicationMenu);
			Assert.AreEqual(0, targetRibbon.ContextualTabGroups.Count);

			target.MergeNonItemsControl(module, targetRibbon);
			Assert.AreEqual(3, targetRibbon.Items.Count);
			Assert.IsNull(targetRibbon.ApplicationMenu);
			Assert.AreEqual(1, targetRibbon.ContextualTabGroups.Count);
		}

		[TestMethod]
		public void UnmergeRibbon()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var targetRibbon = ctx.GetInstance<MainWindow>().MainMenu;
			var module = ctx.GetInstance<TestApplication.Module1.Ribbon>();

			target.MergeNonItemsControl(module, targetRibbon);
			Assert.AreEqual(3, targetRibbon.Items.Count);
			Assert.IsNull(targetRibbon.ApplicationMenu);
			Assert.AreEqual(1, targetRibbon.ContextualTabGroups.Count);

			target.UnmergeItems(new[] { module }, targetRibbon);
			Assert.AreEqual(2, targetRibbon.Items.Count);
			Assert.IsNull(targetRibbon.ApplicationMenu);
			Assert.AreEqual(0, targetRibbon.ContextualTabGroups.Count);
		}

	}
}
