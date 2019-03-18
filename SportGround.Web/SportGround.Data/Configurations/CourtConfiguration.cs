using SportGround.Data.Entities;
using System;
using System.Data.Entity.ModelConfiguration;


namespace SportGround.Data.Configurations
{
	public class CourtConfiguration : EntityTypeConfiguration<CourtEntity>
	{
		public CourtConfiguration()
		{
			ToTable("Courts");

			HasKey(x => x.Id);

			Property(x => x.Name)
				.HasMaxLength(255)
				.IsOptional();
		}
	}
}