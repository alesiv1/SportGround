using System;
using SportGround.Data.entities;
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
				.IsRequired();

			Property(x => x.LastName)
				.HasMaxLength(255)
				.IsOptional();

			Property(x => x.Email)
				.IsRequired();

			Property(x => x.Role)
				.IsRequired();

			Property(x => x.Password)
				.IsRequired();

			Property(x => x.Salt)
				.IsRequired();
		}
	}
}