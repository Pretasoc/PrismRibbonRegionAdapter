using System.Linq;
using System.Windows;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApplication;
using TestApplication.Module1;

namespace Prism.RibbonRegionAdapter.Tests
{
	[TestClass]
	public class MergingItemsControlRegionAdapterTests : BasePrismTest
	{

		[TestMethod]
		public void MergeContextMenu_KeepContext()
		{
			var ctx = ServiceLocator.Current;
			var target = ctx.GetInstance<MergingItemsControlRegionAdapter>();
			var targetMenu = ctx.GetInstance<MainWindow>().CtxMenu;
			var view = ctx.GetInstance<EditorContextMenuView>();
			var moduleMenu = view.ContextMenu;

			target.MergeItemsControl(view, moduleMenu, targetMenu);
			var insertedItems = target.GetMergedItemsByView(view);
			Assert.AreEqual(4, insertedItems.Count);
			Assert.IsTrue(insertedItems.Cast<FrameworkElement>().All(i => i.DataContext is EditorContextMenuViewModel));
		}
	}
}
