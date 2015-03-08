namespace TestApplication
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow() : this(null)
		{
		}
		public MainWindow(MainWindowViewModel viewModel)
		{
			DataContext = viewModel;
			InitializeComponent();
		}
	}
}
