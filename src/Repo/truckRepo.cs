using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using TMS.src;
using TMS.src.Data;

namespace TMS.src
{
    public interface ITruckRepo
    {
        Task<TruckModel> createTruckRepo(TruckModel truck);
        Task<List<TruckModel>> getAllTruckRepo();
        Task<TruckModel> getTruckByIdRepo(int id);
        Task<TruckModel> getTruckByNumberPlateRepo(string number_plate);
        Task saveChnagesRepo();
    }
    public class TruckRepo : ITruckRepo
    {
        private readonly AppDbContext _appDbContext;
        public TruckRepo(AppDbContext appDbContext)
        {
            _appDbContext=appDbContext;
        }
        public async Task saveChnagesRepo()
        {
            await _appDbContext.SaveChangesAsync();
        }


        public async Task<TruckModel> getTruckByNumberPlateRepo(string number_plate)
        {
            var truck=await _appDbContext.trucks.FirstOrDefaultAsync(t=>t.plate_number==number_plate);
            if (truck == null)
            {
                return null;
            }
            return truck;
        }
        public async Task<TruckModel> createTruckRepo(TruckModel truck)
        {
         await _appDbContext.trucks.AddAsync(truck);
         await saveChnagesRepo();
            
            return truck;
        }

        public async Task<TruckModel> getTruckByIdRepo(int id)
        {
            var truck=await _appDbContext.trucks.
            Include(t=>t.driver).Include(t=>t.co_driver)
            .FirstOrDefaultAsync(t=>t.truckId==id);
            if (truck == null)
            {
                return null;
            }
            return truck;
        }

        public async Task<List<TruckModel>> getAllTruckRepo()
        {
            return await _appDbContext.trucks.Include(t=>t.driver).ToListAsync();
        }
    }
}