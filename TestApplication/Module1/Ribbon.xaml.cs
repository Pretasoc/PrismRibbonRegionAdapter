namespace TestApplication.Module1
{
	/// <summary>
	/// Interaction logic for Ribbon.xaml
	/// </summary>
	public partial class Ribbon
	{
		public Ribbon()
			: this(null)
		{
		}
		public Ribbon(RibbonViewModel viewModel)
		{
			DataContext = viewModel;
			InitializeComponent();
		}
	}
}
