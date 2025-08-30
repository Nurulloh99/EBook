using EBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBook.Infrastructure.Persistence.Configurations;

public class UserConfirmerConfiguration : IEntityTypeConfiguration<UserConfirmer>
{
    public void Configure(EntityTypeBuilder<UserConfirmer> builder)
    {
        builder.ToTable("UserConfirmer");

        builder.HasKey(uc => uc.ConfirmerId);

        builder.Property(uc => uc.Gmail).IsRequired(true).HasMaxLength(150);

        builder.Property(uc => uc.IsConfirmed).IsRequired(true);

        builder.Property(uc => uc.ConfirmingCode).IsRequired(false).HasMaxLength(6);

        builder.Property(uc => uc.ExpiredDate).IsRequired(true);
    }
}
