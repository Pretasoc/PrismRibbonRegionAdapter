using System.Windows;
using System.Windows.Media;

namespace Prism.RibbonRegionAdapter
{
	/// <summary>
	/// Extensions to <see cref="DependencyObject"/> instances
	/// </summary>
	public static class DependencyObjectExtension
	{
		/// <summary>
		/// Returns the first parent matching the supplied type
		/// </summary>
		/// <typeparam name="T">the parent's type to search for</typeparam>
		/// <param name="child">the instance for which to search for a parent</param>
		/// <returns>the first parent matching the supplied type, or null</returns>
		public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
		{
			if (child is T)
				return child as T;

			//get parent item
			DependencyObject parentObject = VisualTreeHelper.GetParent(child);

			while (parentObject != null)
			{
				var typedParent = parentObject as T;
				if (typedParent != null)
					return typedParent;
				parentObject = VisualTreeHelper.GetParent(parentObject);
			}
			return null;
		}

		/// <summary>
		/// Returns the first child matching the supplied type
		/// </summary>
		/// <typeparam name="T">the child's type to search for</typeparam>
		/// <param name="depObj">the instance from which to start searching the child elements</param>
		/// <returns>the first child matching the supplied type, or null</returns>
		public static T GetChildOfType<T>(this DependencyObject depObj)
				where T : DependencyObject
		{
			if (depObj == null) return null;

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
			{
				var child = VisualTreeHelper.GetChild(depObj, i);

				var result = (child as T) ?? GetChildOfType<T>(child);
				if (result != null) return result;
			}
			return null;
		}

		/// <summary>
		/// Detaches the supplied <see cref="UIElement">element</see> from it's parent
		/// </summary>
		public static void DisconnectFromParent(this UIElement child, bool preserveDataContext = true)
		{
			DependencyObject parent = null;
			if (child is FrameworkElement)
				parent = ((FrameworkElement)child).Parent;
			if (parent == null)
				parent = VisualTreeHelper.GetParent(child);
			if (parent == null)
				return;

			object dc = null;
			if (preserveDataContext && child is FrameworkElement)
			{
				dc = ((FrameworkElement) child).DataContext;
				if (dc == null && parent is FrameworkElement)
					dc = ((FrameworkElement) parent).DataContext;
			}

			try
			{
				var parentAsPresenter = parent as System.Windows.Controls.ContentPresenter;
				if (parentAsPresenter != null)
				{
					parentAsPresenter.Content = null;
					return;
				}
				var parentAsPanel = parent as System.Windows.Controls.Panel;
				if (parentAsPanel != null)
				{
					parentAsPanel.Children.Remove(child);
					return;
				}
				var parentAsContentControl = parent as System.Windows.Controls.ContentControl;
				if (parentAsContentControl != null)
				{
					parentAsContentControl.Content = null;
					return;
				}
				var parentAsDecorator = parent as System.Windows.Controls.Decorator;
				if (parentAsDecorator != null)
				{
					parentAsDecorator.Child = null;
					return;
				}
				var parentAsItemsControl = parent as System.Windows.Controls.ItemsControl;
				if (parentAsItemsControl != null)
				{
					parentAsItemsControl.Items.Remove(child);
				}
			}
			finally
			{
				if (preserveDataContext && child is FrameworkElement && dc != null)
					((FrameworkElement)child).DataContext = dc;
			}
		}

	}
}
