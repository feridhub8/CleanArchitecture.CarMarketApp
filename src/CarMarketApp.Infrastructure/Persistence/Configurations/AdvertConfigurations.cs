using CarMarketApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarMarketApp.Infrastructure.Persistence.Configurations;

public sealed class AdvertConfigurations : IEntityTypeConfiguration<Advert>
{
    public void Configure(EntityTypeBuilder<Advert> builder)
    {
        builder.HasQueryFilter(a => !a.IsDeleted);
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Description)
               .IsRequired()
               .HasMaxLength(1000);
    }
}
