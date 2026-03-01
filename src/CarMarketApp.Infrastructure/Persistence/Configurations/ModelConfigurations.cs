using CarMarketApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarMarketApp.Infrastructure.Persistence.Configurations;

public sealed class ModelConfigurations : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder.HasQueryFilter(b => !b.IsDeleted);
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => new { b.BrandId, b.NormalizedName }).IsUnique();

        builder.Property(b => b.Name).IsRequired().HasMaxLength(20);
        builder.Property(b => b.NormalizedName).IsRequired().HasMaxLength(20);
    }
}
