using System.Text.Json.Serialization;
using Shrak.Models;
using Shrak.DatabaseContexts;
using Shrak.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<IShipmentDbContext, ShipmentDbContext>(option =>
{
    option.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
});

builder.Services.AddScoped<IShipmentService, ShipmentService>();

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);
builder.Services.AddSingleton(settings);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.AllowTrailingCommas = true;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
});


// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
// {
//     o.TokenValidationParameters = new TokenValidationParameters
//     {
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.BearerKey)),
//         ValidateIssuerSigningKey = true,
//         ValidateAudience = false,
//         ValidateIssuer = false,
//     };
// });

builder.Services.AddOpenApi();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<IShipmentDbContext>();
        dbContext.Database.EnsureCreated();
    }
}

app.UseHttpsRedirection();

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();