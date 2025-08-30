using EBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBook.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(b => b.BookId);

        builder.Property(b => b.Title).IsRequired(true).HasMaxLength(50);

        builder.Property(b => b.Author).IsRequired(true).HasMaxLength(100);  

        builder.Property(b => b.Description).IsRequired(true).HasMaxLength(300); 
        
        builder.Property(b => b.Published).IsRequired(true);

        builder.Property(b => b.Pages).IsRequired(true);


        builder.HasMany(b => b.Reviews)
            .WithOne(r => r.Book)
            .HasForeignKey(b => b.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
