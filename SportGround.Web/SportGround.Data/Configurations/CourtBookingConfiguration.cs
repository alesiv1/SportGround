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

			HasOptional(x => x.Court)
				.WithRequired();

			Property(x => x.BookingDate)
				.IsRequired();
		}
	}
}
