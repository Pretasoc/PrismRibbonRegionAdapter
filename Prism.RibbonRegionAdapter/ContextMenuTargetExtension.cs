using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Prism.RibbonRegionAdapter
{
	/// <summary>
	/// Xaml shortcut Markup-Extension for the binding-expression 
	/// <![CDATA[ {Binding DataContext, RelativeSource={RelativeSource FindAncestor, AncestorType=ContextMenu}} ]]>
	/// </summary>
	/// <example>
	/// <![CDATA[<MenuItem Header="Bound to Context-Menu Target (Module1) (10)" Command="{Binding MyCommand}" CommandParameter="{prism:ContextMenuTarget}" />]]>
	/// </example>
	public class ContextMenuTargetExtension : MarkupExtension
	{
		/// <summary>
		/// Creates a default instance with <see cref="PropertyName"/> &quot;DataContext&quot;
		/// </summary>
		public ContextMenuTargetExtension()
		{
		}

		public Binding TargetBinding { get; set; }

		/// <summary>
		/// Returns a binding to the DataContext of the first ContextMenu parent of the target-control, on which this extension is applied
		/// </summary>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			var target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
			var depObj = target.TargetObject as FrameworkElement;

			var binding = GetBinding();
			var output = binding.ProvideValue(serviceProvider);
			return output;
		}

		private Binding GetBinding()
		{
			Binding binding;
			if (TargetBinding != null)
			{
				binding = TargetBinding;
			}
			else
			{
				binding = new Binding("DataContext")
				{
					Mode = BindingMode.OneWay,
				};
			}
			//binding.Source = null;
			binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ContextMenu), 1);
			return binding;
		}
	}
}
