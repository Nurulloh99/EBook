using EBook.Api.Configurations;
using EBook.Api.Endpoints;
using EBook.Presentation.Configurations;
using EBook.Server.Configurations;
using EBook.Api.Extensions;
using EBook.Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);

ServiceCollectionExtensions.AddSwaggerWithJwt(builder.Services);
// Add services to the container.

builder.ConfigureSerilog(); // SeriLog ning registratsiya qlingani

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.ConfigureDependencies(); // Dependency Injection configuration
builder.Configuration(); // DB ConnectionString configuration
builder.ConfigureJwtSettings(); // JWT settings configuration
builder.ConfigurationJwtAuth();


var app = builder.Build();

app.MapAuthEndpoints();
app.MapUserEndpoints();
app.MapBookEndpoints();
app.MapGenreEndpoints();
app.MapLanguageEndpoints();
app.MapReviewEndpoints();
app.MapAdminEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
