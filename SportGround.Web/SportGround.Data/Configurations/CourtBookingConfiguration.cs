using System;
using SportGround.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace SportGround.Data.Configurations
{
	public class CourtBookingConfiguration : EntityTypeConfiguration<CourtBookingEntity>
	{
		public CourtBookingConfiguration()
		{
			ToTable("CourtWorkingHours");

			HasKey(x => x.Id);

			HasOptional(x => x.User)
				.WithRequired();

			Property(x => x.CourtId)
				.IsRequired();

			Property(x => x.Date)
				.IsRequired();
		}
	}
}
