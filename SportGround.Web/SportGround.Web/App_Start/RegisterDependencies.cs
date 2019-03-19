using System;
using Microsoft.Extensions.DependencyInjection;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Operations;
using Sitecore.DependencyInjection;
using Sitecore.Foundation.DependencyInjection;

namespace SportGround.Web.App_Start
{
	public class RegisterDependencies : IServicesConfigurator
	{
		public void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<ICourtOperations, CourtOperations>();
			// TODO: Add any other registrations here
			serviceCollection.AddMvcControllersInCurrentAssembly();
		}
	}
}