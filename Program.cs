using Microsoft.EntityFrameworkCore;
using TMS.src.Data;
using TMS.Middlewares;
using TMS.src;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. DbContext Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Middleware pipeline mein (UseRouting aur UseAuthorization ke darmiyan)


// 3. Swagger/OpenAPI Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Default setup use karen takay 'Models' ka error khatam ho jaye
builder.Services.AddControllers();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<IUserRepo,UserRepo>();
// Trucks
builder.Services.AddScoped<ITruckRepo, TruckRepo>();
builder.Services.AddScoped<ITruckService, TruckService>();

// builder.Services.AddOpenApi();

var app = builder.Build(); 

app.MapControllers();
// 4. Middleware Pipeline
app.UseMiddleware<MyCustomMiddleware>();

if (app.Environment.IsDevelopment())
{
    // Swagger UI ko enable karne ke liye ye do lines lazmi hain
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/weatherforecast", () => {
    return new[] { "Sunny", "Cloudy", "Rainy" };
})
.WithName("GetWeatherForecast");
// .WithOpenApi();

app.Run();