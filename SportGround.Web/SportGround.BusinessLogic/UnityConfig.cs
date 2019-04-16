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
			container.RegisterType<ICourtService, CourtService>();
			container.RegisterType<IUserService, UserService>();
			container.RegisterType<ICourtWorkingDaysService, CourtService>();
			container.RegisterType<IBookingService, BookingService>();
		}
	}
}
