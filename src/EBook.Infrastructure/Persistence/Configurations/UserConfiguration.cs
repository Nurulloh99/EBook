using EBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBook.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.UserId);

        builder.Property(u => u.FirstName).IsRequired(true).HasMaxLength(150);

        builder.Property(u => u.LastName).IsRequired(true).HasMaxLength(150);

        builder.Property(u => u.UserName).IsRequired(true).HasMaxLength(150);
        builder.HasIndex(u => u.UserName).IsUnique();

        builder.Property(u => u.Salt).IsRequired(true).HasMaxLength(40);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(350);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Password).IsRequired(true).HasMaxLength(1000);
        builder.HasIndex(u => u.Password).IsUnique();

        builder.Property(u => u.PhoneNumber).IsRequired(true).HasMaxLength(17);
        builder.HasIndex(u => u.PhoneNumber).IsUnique();

        builder.Property(u => u.ConfirmerId).IsRequired(false);

        builder.HasOne(u => u.Confirmer)
            .WithOne(c => c.User)
            .HasForeignKey<User>(u => u.ConfirmerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Books)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Reviews)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
