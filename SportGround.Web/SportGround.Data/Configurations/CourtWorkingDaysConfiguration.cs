using System;
using System.Data.Entity.ModelConfiguration;
using SportGround.Data.Entities;

namespace SportGround.Data.Configurations
{
	public class CourtWorkingDaysConfiguration : EntityTypeConfiguration<CourtWorkingDaysEntity>
	{
		public CourtWorkingDaysConfiguration()
		{
			ToTable("CourtWorkingHours");

			HasKey(x => x.Id);

			HasOptional(x => x.Court)
				.WithRequired();

			Property(x => x.Day)
				.IsRequired();

			Property(x => x.StartTimeOfDay)
				.IsRequired();

			Property(x => x.EndTimeOfDay)
				.IsRequired();
		}
	}
}
