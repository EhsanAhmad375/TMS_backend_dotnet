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
        IQueryable<TripModel> getAllTripByDriverIdRepo(int driverId);
        int getAllTripCountByDriverIdRepo(int driverId);
        Task<TripModel> getTripByIdRepo(int id);
        Task UpdateTruckCurrentLocation(int tripId, string lat, string lng);
        Task<bool> deleteTripByIdRepo(int id);
        IQueryable<TripStatus> getAllTripStatusRepo();
        Task UpdateTripStatusRepo(int tripId, int statusId);
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
        public IQueryable<TripModel> getAllTripByDriverIdRepo(int driverId)
        {
            var trip= _appDbContext.trips.Where(t=>t.driver_id==driverId).Include(t=>t.driver).Include(t=>t.truck);
            return trip;
        }
        public int getAllTripCountByDriverIdRepo(int driverId)
        {
        var tripCount = _appDbContext.trips.Count(t => t.driver_id == driverId);
        return tripCount;
        }

        public async Task UpdateTruckCurrentLocation(int tripId, string lat, string lng)
    {
        var trip = await _appDbContext.trips.FindAsync(tripId);
        if (trip != null)
        {
            trip.curr_lat = lat;
            trip.curr_lng = lng;
        
        }

        await SaveTripChanges();

    }
            public async Task PicPointLoc(int tripId, string lat, string lng)
    {
        var trip = await _appDbContext.trips.FindAsync(tripId);
        if (trip != null)
        {
            trip.pic_lat = lat;
            trip.pic_lng = lng;
        
        }

        await SaveTripChanges();

    }
            public async Task DestiniationPointLoc(int tripId, string lat, string lng)
    {
        var trip = await _appDbContext.trips.FindAsync(tripId);
        if (trip != null)
        {
            trip.des_lat = lat;
            trip.des_lng = lng;
        
        }

        await SaveTripChanges();

    }


        public IQueryable<TripStatus> getAllTripStatusRepo()
        {
            var tripStatus= _appDbContext.tripStatuses;
            return tripStatus;
        }

        public async Task UpdateTripStatusRepo(int tripId, int statusId)
        {
            var trip = await _appDbContext.trips.FindAsync(tripId);
            if (trip != null)
            {
                trip.TripStatusId = statusId;
                await SaveTripChanges();
            }
        }

        public async Task<bool> deleteTripByIdRepo(int id){
            var trip = await getTripByIdRepo(id);
            if(trip == null){
                return false;
            }

            // Remove child expenses and incomes first to avoid FK constraint failures
            var expenses = _appDbContext.expenses.Where(e => e.trip_id == id);
            _appDbContext.expenses.RemoveRange(expenses);

            var incomes = _appDbContext.Incomes.Where(i => i.trip_id == id);
            _appDbContext.Incomes.RemoveRange(incomes);

            // Reset current assigned trip for driver and co-driver if they still exist
            if (trip.driver_id.HasValue)
            {
                var driver = await _appDbContext.users.FindAsync(trip.driver_id.Value);
                if (driver != null && driver.current_assigned_tripId == id)
                {
                    driver.current_assigned_tripId = null;
                    _appDbContext.users.Update(driver);
                }
            }

            if (trip.co_driver_id.HasValue)
            {
                var coDriver = await _appDbContext.users.FindAsync(trip.co_driver_id.Value);
                if (coDriver != null && coDriver.current_assigned_tripId == id)
                {
                    coDriver.current_assigned_tripId = null;
                    _appDbContext.users.Update(coDriver);
                }
            }

            _appDbContext.trips.Remove(trip);
            await SaveTripChanges();
            return true;
        }



    
}
}