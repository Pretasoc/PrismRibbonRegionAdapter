namespace TestApplication.Module1
{
	/// <summary>
	/// Interaction logic for EditorContextMenuView.xaml
	/// </summary>
	public partial class EditorContextMenuView
	{
		public EditorContextMenuView()
			: this(null)
		{
		}
		public EditorContextMenuView(EditorContextMenuViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
			ContextMenu.DataContext = viewModel;
		}
	}
}
