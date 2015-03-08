using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Regions;

namespace TestApplication
{
	public class UnloadModule1RibbonCommand : ICommand
	{
		private readonly IRegionManager _regionManager;

		public event EventHandler CanExecuteChanged;

		public UnloadModule1RibbonCommand(IRegionManager regionManager)
		{
			_regionManager = regionManager;
			if (_regionManager.Regions.ContainsRegionWithName(ShellRegions.MainMenu))
				_regionManager.Regions[ShellRegions.MainMenu].Views.CollectionChanged += (o, e) => RaiseCanExecuteChanged();
			else
				_regionManager.Regions.CollectionChanged += OnRegionsChanged;
		}

		public virtual bool CanExecute(object parameter)
		{
			var view = GetView();
			return view != null;
		}

		public virtual void Execute(object parameter)
		{
			var view = GetView();
			if (view != null)
			{
				var region = GetRegion();
				region.Remove(view);
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
			var region = GetRegion();
			if (region == null)
				return null;
			var view = region.GetView("Module1");
			return view;
		}
	}
}
