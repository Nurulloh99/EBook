using CloudinaryDotNet;
using EBook.Application.Interfaces;
using EBook.Application.Services.Helpers;
using EBook.Application.Services.ServiceImplementations;
using EBook.Application.Services.ServiceInterfaces;
using EBook.Infrastructure.Persistence.Repositories;

namespace EBook.Server.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddSingleton<Cloudinary>();


            //services.AddScoped<IValidator<UserCreateDto>, UserCreateDtoValidator>();
            //services.AddScoped<IValidator<LoginDto>, UserLoginDtoValidator>();
            //services.AddScoped<IValidator<ContactCreateDto>, ContactCreateDtoValidator>();
            //services.AddScoped<IValidator<ContactDto>, ContactDtoValidator>();
        }
    }
}
