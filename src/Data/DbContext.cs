using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
namespace TMS.src.Data
{
    
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        public DbSet<UserModel> users{get;set;}
        public DbSet<TripModel> trips{get;set;}
        public DbSet<ExpenseModel> expenses{get;set;}
        public DbSet<TruckModel> trucks{get;set;}
        public DbSet<TripStatus> tripStatuses{get;set;}
        public DbSet<ExpenseCategory> expenseCategories{get;set;}
        public DbSet<IncomeSource> IncomeSources { get; set; }
        public DbSet<IncomeModel> Incomes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
    }
}
 