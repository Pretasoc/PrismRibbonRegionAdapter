using Autofac;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Prism.AutofacExtension;
using System.Windows;
using TestApplication;
using System.Windows.Controls.Ribbon;
using System.Windows.Controls;

namespace Prism.RibbonRegionAdapter.Tests
{
	[TestClass]
	public class AdapterTests : BasePrismTest
	{

		[TestMethod]
		public void GetMergeKey_Explicit()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var targetRibbon = ctx.GetInstance<TestApplication.MainWindow>().MainMenu;
			var moduleRibbon = ctx.GetInstance<TestApplication.Module1.Ribbon>().MainMenu;

			string key;
			RibbonTab tab;
			tab = (RibbonTab)targetRibbon.Items[0];
			key = target.GetMergeKey(tab);
			Assert.AreEqual(MainMenuMergeKeys.GeneralTab, key);
		}

		[TestMethod]
		public void GetMergeKey_FallbackToHeader()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var targetRibbon = ctx.GetInstance<TestApplication.MainWindow>().MainMenu;
			var moduleRibbon = ctx.GetInstance<TestApplication.Module1.Ribbon>().MainMenu;

			string key;
			RibbonTab tab;
			tab = (RibbonTab)moduleRibbon.Items[0];
			key = target.GetMergeKey(tab);
			Assert.AreEqual(tab.Header, key);
		}

		[TestMethod]
		public void GetRibbon_Success()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var moduleRibbon = ctx.GetInstance<TestApplication.Module1.Ribbon>();

			var ribbon = target.GetRibbon(moduleRibbon);
		}

		[TestMethod, ExpectedException(typeof(NotSupportedException))]
		public void GetRibbon_NoRibbon()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var moduleRibbon = new UserControl();

			var ribbon = target.GetRibbon(moduleRibbon);
		}

		[TestMethod]
		public void ItemsMatch()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<RibbonRegionAdapter>();
			var targetRibbon = ctx.GetInstance<TestApplication.MainWindow>().MainMenu;
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
			var targetRibbon = ctx.GetInstance<TestApplication.MainWindow>().MainMenu;
			var moduleRibbon = ctx.GetInstance<TestApplication.Module1.Ribbon>().MainMenu;

			var sourceTab = (RibbonTab)moduleRibbon.Items[1];
			var targetTab = (RibbonTab)targetRibbon.Items[0];

			target.MergeItemsControl(sourceTab, targetTab);
		}

	}
}
