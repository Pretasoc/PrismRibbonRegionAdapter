using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;

namespace TestApplication
{
	public class ShellHelloCommand : ICommand
	{

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			string msg = GetMessagePrefix();
			if (parameter is RibbonButton)
				msg += " button: " + ((RibbonButton)parameter).Label;
			else if (parameter is MenuItem)
				msg += " menu-item: " + ((MenuItem)parameter).Header;
			else if (parameter != null)
				msg += " " + parameter.ToString();

			MessageBox.Show(msg);
		}

		protected virtual string GetMessagePrefix()
		{
			string msg = "Hello from Shell";
			return msg;
		}

		protected virtual void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
				CanExecuteChanged(this, EventArgs.Empty);
		}

	}
}
