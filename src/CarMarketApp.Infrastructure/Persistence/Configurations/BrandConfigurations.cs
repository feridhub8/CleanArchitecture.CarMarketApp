using CarMarketApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarMarketApp.Infrastructure.Persistence.Configurations;

public sealed class BrandConfigurations : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasQueryFilter(b => !b.IsDeleted);
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.NormalizedName).IsUnique();

        builder.Property(b => b.Name).IsRequired().HasMaxLength(20);
        builder.Property(b => b.NormalizedName).IsRequired().HasMaxLength(20);
    }
}
