using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src.Data;
namespace TMS.src
{
    public interface IDashboardService
    {
        Task<DashboardDTO> getDashbaordService(string? date);
    }

    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;
        private readonly ITripRepo _tripRepo;
        public DashboardService(AppDbContext context)
        {
            _context=context;
         
        }

        public async Task<DashboardDTO> getDashbaordService(string? date)
{
    bool isFilterByDate = !string.IsNullOrEmpty(date);

    // 1. Trips Summary - AsNoTracking() added for performance
    var tripsData = await _context.trips
        .AsNoTracking() 
        .Where(c => !isFilterByDate || c.scheduled_date == date)
        .GroupBy(c => 1)
        .Select(g => new {
            Total = g.Count(),
            Import = g.Count(c => c.trip_type == "import"),
            Export = g.Count(c => c.trip_type == "export"),
            Local = g.Count(c => c.trip_type == "local"),
            InProgress = g.Count(c => c.TripStatusId == 1),
            Completed = g.Count(c => c.TripStatusId == 2),
            NotStarted = g.Count(c => c.TripStatusId == 3),
            ready = g.Count(c => c.TripStatusId == 4)
        }).FirstOrDefaultAsync();

    // 2. Trucks Data - AsNoTracking() added
    var trucksData = await _context.trucks
        .AsNoTracking()
        .GroupBy(t => 1)
        .Select(g => new {
            Total = g.Count(),
            Owned = g.Count(t => t.type == "owned"),
            Outsourced = g.Count(t => t.type == "outsourced"),
            Available = g.Count(t => t.sub_status == "available"),
            OnTrip = g.Count(t => t.sub_status == "on_trip"),
            Maintenance = g.Count(t => t.type == "maintenance")
        }).FirstOrDefaultAsync();

    // 3. Detailed Trips List - Optimized projection
    List<GetTripsListDTO> tripsListDTOs = new List<GetTripsListDTO>();

    if (isFilterByDate)
    {
        tripsListDTOs = await _context.trips
            .AsNoTracking()
            .Where(t => t.scheduled_date == date)
            // .Include ko hata diya, Select khud handle karega
            .Select(t => new GetTripsListDTO
            {
                tripId = t.tripId,
                pickupPoint = t.pickup_location,
                destination = t.destination,
                type = t.trip_type,
                date = t.scheduled_date,
                status=t.TripStatusId,
                created_at = t.created_at,
                truck = t.truck != null ? new Truck
                {
                    truck_id = t.truck_id, // Check karein agar ye truckId hai
                    plate_number = t.truck.plate_number,
                    model = t.truck.model 
                } : null
            }).ToListAsync();
    }

    // Mapping and Calculation
    int totalOwned = trucksData?.Owned ?? 0;
    int onTrip = trucksData?.OnTrip ?? 0;
    int loadPercentage = totalOwned > 0 ? (int)((double)onTrip / totalOwned * 100) : 0;

    return new DashboardDTO
    {
        date = date ?? "All Time",
        load_percentage = loadPercentage,
        trips = new TripsInfoDTO {
            total = tripsData?.Total ?? 0,
            import = tripsData?.Import ?? 0,
            export = tripsData?.Export ?? 0,
            local = tripsData?.Local ?? 0,
            in_progress = tripsData?.InProgress ?? 0,
            completed = tripsData?.Completed ?? 0,
            not_started = tripsData?.NotStarted ?? 0,
            ready=tripsData?.ready??0
        },
        vihicles = new VihiclesInfoDTO {
            total = trucksData?.Total ?? 0,
            owned = totalOwned,
            outsourced = trucksData?.Outsourced ?? 0,
            available = trucksData?.Available ?? 0,
            in_progress = onTrip,
            in_maintenance = trucksData?.Maintenance ?? 0
        },
        tripList = tripsListDTOs 
    };
}


    
    }
    
}