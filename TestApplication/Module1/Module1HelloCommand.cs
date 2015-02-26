using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TestApplication.Module1
{
	public class Module1HelloCommand : ShellHelloCommand
	{

		protected override string GetMessagePrefix()
		{
			return "Hello from Module-1";
		}
	}
}
