using System;
using System.Data.Entity.ModelConfiguration;
using SportGround.Data.Entities;

namespace SportGround.Data.Configurations
{
	public class CourtWorkingHoursConfiguration : EntityTypeConfiguration<CourtWorkingHoursEntity>
	{
		public CourtWorkingHoursConfiguration()
		{
			ToTable("CourtWorkingHours");

			HasKey(x => x.Id);

			HasOptional(x => x.CourtId)
				.WithRequired();

			Property(x => x.Day)
				.IsRequired();

			Property(x => x.StartTime)
				.IsRequired();

			Property(x => x.EndTime)
				.IsRequired();
		}
	}
}
