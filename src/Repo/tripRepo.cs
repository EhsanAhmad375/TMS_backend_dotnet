using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src;
using TMS.src.Data;
namespace TMS.src
{
    public interface ITripRepo
    {
        Task<TripModel> createTripRepo(TripModel tripModel);
        IQueryable<TripModel> getAllTripRepo();
        Task<TripModel> getTripByIdRepo(int id);
        Task SaveTripChanges();

    }
    public class TripRepo : ITripRepo
    {
        private readonly AppDbContext _appDbContext;
        public TripRepo(AppDbContext appDbContext)
        {
            _appDbContext=appDbContext;
        }

        public async Task SaveTripChanges()
        {
             await _appDbContext.SaveChangesAsync();
        }

        public async Task<TripModel> createTripRepo(TripModel tripModel)
        {
            var trips= await _appDbContext.trips.AddAsync(tripModel);
            await SaveTripChanges();
            return trips.Entity;
        }

        public IQueryable<TripModel> getAllTripRepo()
        {
            var trip= _appDbContext.trips.Include(t=>t.driver).Include(t=>t.truck);
            return trip;
        }
        public async Task<TripModel> getTripByIdRepo(int id)
        {
            var trip=await _appDbContext.trips.
            Include(t=>t.truck).
            Include(t=>t.driver).
            Include(t=>t.co_driver).
            Include(t=>t.tripStatus).
            FirstOrDefaultAsync(t=>t.tripId==id);
            return trip;
        }
    }
}