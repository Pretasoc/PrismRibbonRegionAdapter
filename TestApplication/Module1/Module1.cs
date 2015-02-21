using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication.Module1
{
	class Module1 : IModule
	{

    private readonly IRegionManager _regionManager;

		public Module1(IRegionManager regionManager)
    {
      _regionManager = regionManager;
    }

		public void Initialize()
		{
			_regionManager.RegisterViewWithRegion(ShellRegions.MainMenu, typeof(Ribbon));
		}
	}
}
