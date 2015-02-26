using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestApplication
{
	public class MainWindowViewModel
	{
		public MainWindowViewModel(ShellHelloCommand cmd)
		{
			HelloCommand = cmd;
		}
		public ICommand HelloCommand { get; set; }
	}
}
