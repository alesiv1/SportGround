using System;
using SportGround.Data.entities;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;
using SportGround.Data.Repositories;
using Unity;

namespace SportGround.Data
{
	public class UnityConfig
	{
		public static void RegisterTypes(IUnityContainer container)
		{
			container.RegisterType<IDataRepository<CourtEntity>, CourtDataRepository>();
			container.RegisterType<IDataRepository<UserEntity>, UserDataRepository>();
		}
	}
}
