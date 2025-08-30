using EBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EBook.Presentation.Configurations;

public static class DataBaseConfiguration
{
    public static void Configuration(this WebApplicationBuilder builder)
    {
        var MSConnectionString = builder.Configuration.GetConnectionString("MSDataBaseConnection");

        builder.Services.AddDbContext<AppDbContext>(options =>
          options.UseSqlServer(MSConnectionString));
    }
}
