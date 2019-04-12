using System;
using SportGround.Data.Interfaces;
using SportGround.Data.Repositories;
using Unity;

namespace SportGround.Data
{
	public class UnityConfig
	{
		public static void RegisterTypes(IUnityContainer container)
		{
			container.RegisterType<ICourtRepository, CourtRepository>();
			container.RegisterType<IUserRepository, UserRepository>();
			container.RegisterType<ICourtWorkingDaysRepository, CourtWorkingDaysRepository>();
			container.RegisterType<ICourtBookingRepository, CourtBookingRepository>();
		}
	}
}
