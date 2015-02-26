using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestApplication.Module1
{
	public class RibbonViewModel
	{
		public RibbonViewModel(Module1HelloCommand cmd)
		{
			HelloCommand = cmd;
		}
		public ICommand HelloCommand { get; set; }
	}
}
