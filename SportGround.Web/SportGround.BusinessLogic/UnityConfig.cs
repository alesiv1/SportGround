using System;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Operations;
using Unity;

namespace SportGround.BusinessLogic
{
	public class UnityConfig
	{
		public static void RegisterTypes(IUnityContainer container)
		{
			container.RegisterType<ICourtOperations, CourtOperations>();
			container.RegisterType<IUserOperations, UserOperations>();
		}
	}
}
