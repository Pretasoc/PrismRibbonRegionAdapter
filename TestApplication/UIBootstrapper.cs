using Autofac;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Prism.AutofacExtension;
using Prism.RibbonRegionAdapter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;

namespace TestApplication
{
	public partial class UIBootstrapper : AutofacBootstrapper
	{

		protected override void ConfigureContainer(ContainerBuilder builder)
		{
			base.ConfigureContainer(builder);

			builder.RegisterType<RibbonRegionAdapter>().SingleInstance();
			builder.RegisterType<MainWindow>().SingleInstance();
			builder.RegisterType<Module1.Module1>().SingleInstance();
			builder.RegisterType<Module1.Ribbon>().SingleInstance();
		}

		protected override Microsoft.Practices.Prism.Regions.RegionAdapterMappings ConfigureRegionAdapterMappings()
		{
			var mappings = base.ConfigureRegionAdapterMappings();

			mappings.RegisterMapping(typeof(Ribbon), ServiceLocator.Current.GetInstance<RibbonRegionAdapter>());

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
			ModuleInfo module = new ModuleInfo()
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

		private void EnsureDirectoryExists(string directory)
		{
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);
		}

	}
}
