using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace TestApplication.Module1
{
	[Module(ModuleName = "Module1", OnDemand = true)]
	class Module1 : IModule
	{

    private readonly IRegionManager _regionManager;

		public Module1(IRegionManager regionManager)
    {
      _regionManager = regionManager;
    }

		public void Initialize()
		{
			_regionManager.RegisterViewWithRegion(ShellRegions.EditorContextMenu, GetContextMenu);
			//_regionManager.RegisterViewWithRegion(ShellRegions.MainMenu, typeof(Ribbon));
		}

		private object GetContextMenu()
		{
			var cmv = ServiceLocator.Current.GetInstance<EditorContextMenuView>();
			return cmv.ContextMenu;
		}
	}
}
