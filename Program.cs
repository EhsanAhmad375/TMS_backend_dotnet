using Microsoft.EntityFrameworkCore;
using TMS.src.Data;
using TMS.Middlewares;
using TMS.src;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. RENDER PORT CONFIGURATION ---
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// --- 2. CORS CONFIGURATION (Flutter Web Fix) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- 3. DATABASE CONFIGURATION ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (!string.IsNullOrEmpty(connectionString))
    {
        // var serverVersion = ServerVersion.AutoDetect(connectionString);
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 30));
        options.UseMySql(connectionString, serverVersion, mysqlOptions => 
            mysqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            ));
    }
});

// --- 4. JWT AUTHENTICATION ---
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

// --- 5. DEPENDENCY INJECTION (Fixes the Crash) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddControllers();

// Repositories
// Note: UserService direct UserRepo mang raha hai, isliye dono register karne hain
builder.Services.AddScoped<UserRepo>(); 
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<ITruckRepo, TruckRepo>();
builder.Services.AddScoped<ITripRepo, TripRepo>();
builder.Services.AddScoped<IExpenseRepo, ExpenseRepo>();
builder.Services.AddScoped<IncomeRepo>();

// Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ITruckService, TruckService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build(); 

// --- 6. MIDDLEWARE PIPELINE ---

// IMPORTANT: UseCors hamesha Authentication se PEHLE hona chahiye
app.UseCors("AllowAll");

app.UseMiddleware<MyCustomMiddleware>();

// Swagger (Always ON for testing on Render)
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TMS API V1");
    c.RoutePrefix = string.Empty; 
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health Check
app.MapGet("/health", () => Results.Ok(new { status = "Running", port = port }));

app.Run();