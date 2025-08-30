using EBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBook.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(r => r.ReviewId);

        builder.Property(r => r.Content)
            .IsRequired(true)
            .HasMaxLength(200);

        builder.Property(r => r.Rating)
            .IsRequired(true);
    }
}
