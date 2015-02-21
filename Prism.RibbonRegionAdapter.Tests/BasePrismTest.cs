using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApplication;

namespace Prism.RibbonRegionAdapter.Tests
{
	public abstract class BasePrismTest : UIBootstrapper
	{

		public TestContext TestContext { get; set; }

		protected BasePrismTest()
		{
			Run();
		}

		protected override System.Windows.DependencyObject CreateShell()
		{
			return null;
		}
	}
}
