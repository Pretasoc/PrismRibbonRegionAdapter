using System.Windows.Input;

namespace TestApplication
{
	public class MainWindowViewModel
	{
		public MainWindowViewModel(ShellHelloCommand cmd, 
			LoadModule1RibbonCommand loadModule1RibbonCommand,
			UnloadModule1RibbonCommand unloadModule1RibbonCommand)
		{
			HelloCommand = cmd;
			LoadModule1RibbonCommand = loadModule1RibbonCommand;
			UnloadModule1RibbonCommand = unloadModule1RibbonCommand;
		}

		public ICommand HelloCommand { get; set; }
		public ICommand LoadModule1RibbonCommand { get; set; }
		public ICommand UnloadModule1RibbonCommand { get; set; }
	}
}
