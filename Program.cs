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

// --- RENDER PORT CONFIGURATION (REQUIRED) --- updaye tje code
// Render 'PORT' environment variable deta hai, humein usay sunna hoga.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// 1. Database Connection String
// Render par aap ise Environment Variables mein "ConnectionStrings__DefaultConnection" ke naam se denge
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. DbContext Configuration (MySQL)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (!string.IsNullOrEmpty(connectionString))
    {
        var serverVersion = ServerVersion.AutoDetect(connectionString);
        options.UseMySql(connectionString, serverVersion, mysqlOptions => 
            mysqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            ));
    }
});

// 3. JWT Authentication Setup
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "YourFallbackSuperSecretKey123"))
        };
    });

// 4. Swagger & API Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddControllers();

// 5. Dependency Injection (Services & Repos)
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

// Trucks
builder.Services.AddScoped<ITruckRepo, TruckRepo>();
builder.Services.AddScoped<ITruckService, TruckService>();

// Trip
builder.Services.AddScoped<ITripRepo, TripRepo>();
builder.Services.AddScoped<ITripService, TripService>();

// Expense
builder.Services.AddScoped<IExpenseRepo, ExpenseRepo>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();

// Dashboard
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build(); 

// 6. Middleware Pipeline
app.UseMiddleware<MyCustomMiddleware>();

// Render par testing ke liye hum Swagger hamesha on rakh sakte hain
// Agar aapko hide karna ho to wapis condition laga dein
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TMS API V1");
    c.RoutePrefix = string.Empty; // Is se URL ke end mein /swagger nahi likhna parega
});

// Render/Docker mein HTTPS redirection aksar issues deta hai agar SSL termination bahar ho rahi ho
// app.UseHttpsRedirection(); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok("Server is running!"));

app.MapGet("/weatherforecast", () => {
    return new[] { "Sunny", "Cloudy", "Rainy" };
})
.WithName("GetWeatherForecast");

app.Run();

// using Microsoft.EntityFrameworkCore;
// using TMS.src.Data;
// using TMS.Middlewares;
// using TMS.src;

// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;
// using System.Security.Claims;
// using System.IdentityModel.Tokens.Jwt;

// var builder = WebApplication.CreateBuilder(args);

// // 1. Database Connection String
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// // 2. DbContext Configuration
// // 2. DbContext Configuration
// builder.Services.AddDbContext<AppDbContext>(options =>
// {
//     var serverVersion = ServerVersion.AutoDetect(connectionString);
//     options.UseMySql(connectionString, serverVersion, mysqlOptions => 
//         mysqlOptions.EnableRetryOnFailure(
//             maxRetryCount: 10,
//             maxRetryDelay: TimeSpan.FromSeconds(30),
//             errorNumbersToAdd: null
//         ));
// });


//     builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["Jwt:Issuer"],
//             ValidAudience = builder.Configuration["Jwt:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
//         };
//     });

// // Middleware pipeline mein (UseRouting aur UseAuthorization ke darmiyan)


// // 3. Swagger/OpenAPI Services
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(); // Default setup use karen takay 'Models' ka error khatam ho jaye
// builder.Services.AddControllers();
// builder.Services.AddScoped<UserService>();
// builder.Services.AddScoped<UserRepo>();
// builder.Services.AddScoped<IUserRepo,UserRepo>();
// // Trucks
// builder.Services.AddScoped<ITruckRepo, TruckRepo>();
// builder.Services.AddScoped<ITruckService, TruckService>();
// // Trip
// builder.Services.AddScoped<ITripRepo, TripRepo>();
// builder.Services.AddScoped<ITripService, TripService>();
// //expense
// builder.Services.AddScoped<IExpenseRepo, ExpenseRepo>();
// builder.Services.AddScoped<IExpenseService, ExpenseService>();
// //dashboard
// builder.Services.AddScoped<IDashboardService,DashboardService>();


// // builder.Services.AddOpenApi();

// var app = builder.Build(); 

// app.MapControllers();
// // 4. Middleware Pipeline
// app.UseMiddleware<MyCustomMiddleware>();

// if (app.Environment.IsDevelopment())
// {
//     // Swagger UI ko enable karne ke liye ye do lines lazmi hain
//     app.UseSwagger();
//     app.UseSwaggerUI();
    
//     // app.MapOpenApi();
// }

// app.UseHttpsRedirection();
// app.UseAuthentication();
// app.UseAuthorization();

// app.MapGet("/weatherforecast", () => {
//     return new[] { "Sunny", "Cloudy", "Rainy" };
// })
// .WithName("GetWeatherForecast");
// // .WithOpenApi();

// app.Run();