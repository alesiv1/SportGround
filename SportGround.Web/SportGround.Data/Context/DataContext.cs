﻿using System;
using System.Data.Entity;
using SportGround.Data.entities;
using SportGround.Data.Entities;

namespace SportGround.Data.Context
{
	public class DataContext : DbContext
	{
		public DataContext()
			: base("LocalDB")
		{
		}

		public DbSet<UserEntity> Users { get; set; }
		public DbSet<CourtEntity> Courts { get; set; }

		//protected override void OnModelCreating(DbModelBuilder modelBuilder)
		//{
		//	modelBuilder.Entity<UserEntity>().ToTable("Users");
		//	modelBuilder.Entity<CourtEntity>().ToTable("Courts");
		//	base.OnModelCreating(modelBuilder);
		//}
	}
}
