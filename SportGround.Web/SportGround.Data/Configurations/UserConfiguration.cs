using SportGround.Data.entities;
using System;
using System.Data.Entity.ModelConfiguration;

namespace SportGround.Data.Configurations
{
	public class UserConfiguration : EntityTypeConfiguration<UserEntity>
	{
		public UserConfiguration()
		{
			ToTable("Users");

			HasKey(x => x.Id);

			Property(x => x.FirstName)
				.HasMaxLength(255)
				.IsOptional();

			Property(x => x.LastName)
				.HasMaxLength(255)
				.IsOptional();

			Property(x => x.Role)
				.IsRequired();
		}
	}
}