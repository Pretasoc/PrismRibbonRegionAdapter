using Autofac;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
	partial class UIBootstrapper
	{
		//must be constructed here, as the container is not available yet

		protected override IContainer CreateContainer()
		{
			var container = base.CreateContainer();
			return container;
		}

	}
}
