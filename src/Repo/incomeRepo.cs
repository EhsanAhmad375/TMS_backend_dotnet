using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src;
using TMS.src.Data;
namespace TMS.src
{
    public interface InicomeRepo
    {
        Task<IncomeModel> createIncomeRepo(IncomeModel incomeModel);
        IQueryable<IncomeModel> getAllIncomeRepo();
        IQueryable<IncomeSource> getAllIncomeSourcesRepo();
        Task<IncomeModel> getIncomeByIdRepo(int id);
        Task SaveIncomeChanges();
    }
    public class IncomeRepo : InicomeRepo
    {
        private readonly AppDbContext _appDbContext;
        public IncomeRepo(AppDbContext appDbContext)
           {
            _appDbContext=appDbContext;
            }

            public async Task SaveIncomeChanges()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IncomeModel> createIncomeRepo(IncomeModel incomeModel)
        {
            var income= await _appDbContext.Incomes.AddAsync(incomeModel);
            await SaveIncomeChanges();
            return income.Entity;
        }

        public IQueryable<IncomeModel> getAllIncomeRepo()
        {
            var income= _appDbContext.Incomes.Include(i=>i.incomeSource).Include(i=>i.trip);
            return income;
        }

        public async Task<IncomeModel> getIncomeByIdRepo(int id)
        {
            var income=await _appDbContext.Incomes.
            Include(i=>i.incomeSource).
            Include(i=>i.trip).
            FirstOrDefaultAsync(i=>i.incomeId==id);
            return income;
        }

        public IQueryable<IncomeSource> getAllIncomeSourcesRepo()
{
    return _appDbContext.IncomeSources;
}

}
}