using System;
using SportGround.Data.Entities;
using System.Data.Entity.ModelConfiguration;


namespace SportGround.Data.Configurations
{
	public class CourtConfiguration : EntityTypeConfiguration<CourtEntity>
	{
		public CourtConfiguration()
		{
			ToTable("Courts");

			HasKey(x => x.Id);

			HasOptional(x => x.WorkingHours)
				.WithMany();

			HasOptional(x => x.BookingCourt)
				.WithMany();

			Property(x => x.Name)
				.HasMaxLength(255)
				.IsOptional();
		}
	}
}