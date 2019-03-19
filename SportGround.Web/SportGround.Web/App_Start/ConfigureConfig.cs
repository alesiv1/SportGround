using Microsoft.Extensions.DependencyInjection;
using System;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Operations;

namespace SportGround.Web.App_Start
{
	public class ConfigureConfig
	{
		public static void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<ICourtOperations, CourtOperations>();
			serviceCollection.AddScoped<CourtOperations, CourtOperations>();
		}
	}
}