using EBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBook.Infrastructure.Persistence.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");

        builder.HasKey(g => g.GenreId);

        builder.Property(g => g.GenreName).IsRequired(true).HasMaxLength(100);

        builder.Property(g => g.GenreDescription).IsRequired().HasMaxLength(200);


        builder.HasMany(g => g.Books)
            .WithOne(b => b.Genre)
            .HasForeignKey(b => b.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
