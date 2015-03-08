using System.Windows.Input;
using System.Windows.Media;

namespace TestApplication.Module1
{
	public class RibbonViewModel
	{
		public RibbonViewModel(Module1HelloCommand cmd)
		{
			HelloCommand = cmd;
		    ContextualBackground = new SolidColorBrush(Colors.Orange);
		}
		public ICommand HelloCommand { get; set; }

	    public Brush ContextualBackground { get; set; }
	}
}
