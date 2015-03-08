using Autofac;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Prism.AutofacExtension;
using Prism.RibbonRegionAdapter;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;

namespace TestApplication
{
	public class UIBootstrapper : AutofacBootstrapper
	{

		protected override void ConfigureContainer(ContainerBuilder builder)
		{
			base.ConfigureContainer(builder);

			builder.RegisterAssemblyTypes(typeof(RibbonRegionAdapter).Assembly)
				.InNamespaceOf<RibbonRegionAdapter>()
				.AsSelf()
				.SingleInstance();

			builder.RegisterAssemblyTypes(typeof(UIBootstrapper).Assembly)
				.InNamespaceOf<MainWindowViewModel>()
				.AsSelf()
				.SingleInstance();

			builder.RegisterAssemblyTypes(typeof(UIBootstrapper).Assembly)
				.InNamespaceOf<Module1.Module1HelloCommand>()
				.AsSelf()
				.InstancePerDependency();
		}

		protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
		{
			var mappings = base.ConfigureRegionAdapterMappings();

			mappings.RegisterMapping(typeof(Ribbon), ServiceLocator.Current.GetInstance<RibbonRegionAdapter>());
			mappings.RegisterMapping(typeof(ContextMenu), ServiceLocator.Current.GetInstance<MergingItemsControlRegionAdapter>());

			return mappings;
		}

		protected override void ConfigureModuleCatalog()
		{
			base.ConfigureModuleCatalog();
			AddModule<Module1.Module1>();
		}

		private void AddModule<T>(string moduleName = null) where T : class, IModule
		{
			var moduleType = typeof(T);
			var module = new ModuleInfo
			{
				Ref = moduleType.Assembly.CodeBase,
				ModuleName = moduleName ?? moduleType.Name,
				ModuleType = moduleType.AssemblyQualifiedName,
				InitializationMode = InitializationMode.WhenAvailable,
			};
			ModuleCatalog.AddModule(module);
		}

		protected override DependencyObject CreateShell()
		{
			var shell = ServiceLocator.Current.GetInstance<MainWindow>();
			var regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
			RegionManager.SetRegionManager(shell, regionManager);
			RegionManager.UpdateRegions();
			return shell;
		}

		protected override void InitializeShell()
		{
			base.InitializeShell();
			var shell = (MainWindow)Shell;
			Application.Current.MainWindow = shell;
			Application.Current.MainWindow.Show();
		}

	}
}
