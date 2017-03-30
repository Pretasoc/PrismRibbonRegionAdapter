using System;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using Prism.Regions;

namespace TestApplication
{
	public class LoadModule1RibbonCommand : ICommand
	{
		private readonly IRegionManager _regionManager;

		public event EventHandler CanExecuteChanged;

		public LoadModule1RibbonCommand(IRegionManager regionManager)
		{
			_regionManager = regionManager;
			if (_regionManager.Regions.ContainsRegionWithName(ShellRegions.MainMenu))
				_regionManager.Regions[ShellRegions.MainMenu].Views.CollectionChanged += (o, e) => RaiseCanExecuteChanged();
			else
				_regionManager.Regions.CollectionChanged += OnRegionsChanged;
		}

		public virtual bool CanExecute(object parameter)
		{
			var region = GetRegion();
			return region != null && region.GetView("Module1") == null;
		}

		public virtual void Execute(object parameter)
		{
			var view = GetView();
			if (view != null)
			{
				var region = GetRegion();
				region.Add(view, "Module1");
			}
		}

		protected void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
				CanExecuteChanged(this, EventArgs.Empty);
		}

		private void OnRegionsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (_regionManager.Regions.ContainsRegionWithName(ShellRegions.MainMenu))
			{
				_regionManager.Regions.CollectionChanged -= OnRegionsChanged;
				_regionManager.Regions[ShellRegions.MainMenu].Views.CollectionChanged += (o, e2) => RaiseCanExecuteChanged();
				RaiseCanExecuteChanged();
			}
		}

		private IRegion GetRegion()
		{
			if (_regionManager.Regions.ContainsRegionWithName(ShellRegions.MainMenu))
				return _regionManager.Regions[ShellRegions.MainMenu];
			return null;
		}

		private object GetView()
		{
			var view = ServiceLocator.Current.GetInstance<Module1.Ribbon>();
			return view;
		}
	}
}
