using EBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBook.Infrastructure.Persistence.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Languages");

        builder.HasKey(l => l.LanguageId);

        builder.Property(r => r.LanguageName).IsRequired(true).HasMaxLength(50);

        builder.HasMany(b => b.Books)
            .WithOne(l => l.Language)
            .HasForeignKey(l => l.LanguageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
