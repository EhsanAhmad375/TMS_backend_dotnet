using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using TMS.src.Data;
namespace TMS.src
{
    public interface ITripService
    {
        Task<TripModel> createTripService(CreateTripDTO createTrip);

        Task<List<GetAllTripsDTO>> getAllTripService(string? type,string? truckId,string? driverId,string? date);
        Task<getTripDetailsByIdDTO> getTripDetailsById(int id);

        Task<List<TripStatus>> getAllTripStatus();

        Task<bool> deleteTripById(int id);

        Task<bool> UpdateTripStatusService(int tripId, int statusId);

        Task<bool> addTripLocationService(addCurrentLocationDTO addCurrentLocationDTO);
    }
    public class TripService : ITripService
    {
        private readonly ITripRepo _tripRepo;
        private readonly IExpenseRepo _expenseRepo;
        private readonly IncomeRepo _incomeRepo;
        private readonly AppDbContext _context; // Add your DbContext here

        public TripService(ITripRepo tripRepo, IExpenseRepo expenseRepo, IncomeRepo incomeRepo, AppDbContext context)
        {
            _tripRepo=tripRepo;
            _expenseRepo=expenseRepo;
            _incomeRepo=incomeRepo;
            _context = context; 
        }
        public async Task<TripModel> createTripService(CreateTripDTO createTrip)
        {
        if (_context == null) throw new Exception("Database context is not initialized.");

        // Define the strategy for MySQL retries
        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
        // Start transaction INSIDE the execution strategy
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var trip = new TripModel
            {
                truck_id = createTrip.truck_id,
                driver_id = createTrip.driver_id,
                co_driver_id = createTrip.co_driver_id,
                trip_type = createTrip.type,
                pickup_location = createTrip.pickup_location,
                destination = createTrip.destination,
                allowance = createTrip.allowance,
                scheduled_date = createTrip.scheduled_date,
                client_Name = createTrip.client_name,
                client_contact = createTrip.client_contact,
                client_company = createTrip.client_company,
                scheduled_time = createTrip.scheduled_time,
                
                // Null-safe mapping
                pic_lat = createTrip.location?.pic_lat ?? "",
                pic_lng = createTrip.location?.pic_lng ?? "",
                des_lat = createTrip.location?.des_lat ?? "",
                des_lng = createTrip.location?.des_lng ?? ""
            };

            // 1. Save Trip (This populates savedTrip.tripId)
            var savedTrip = await _tripRepo.createTripRepo(trip);

            // 2. Save Income only if it exists in DTO
            if (createTrip.income != null)
            {
                var income = new IncomeModel
                {
                    trip_id = savedTrip.tripId, 
                    total_amount = createTrip.income.total_amount,
                    reveived_amount = createTrip.income.received_amount,
                    income_source_id = createTrip.income.sourceId,
                    remaining_amount = (createTrip.income.total_amount ?? 0) - (createTrip.income.received_amount ?? 0),
                    notes = createTrip.income.notes ?? $"Income for Trip ID: {savedTrip.tripId}",
                    added_by = createTrip.income.added_by??0
                };
                
                await _incomeRepo.createIncomeRepo(income);                
            }

            // 3. Update driver and co-driver assigned trip id
            if (savedTrip.driver_id.HasValue)
            {
                var driver = await _context.users.FindAsync(savedTrip.driver_id.Value);
                if (driver != null)
                {
                    driver.current_assigned_tripId = savedTrip.tripId;
                    _context.users.Update(driver);
                }
            }

            if (savedTrip.co_driver_id.HasValue)
            {
                var coDriver = await _context.users.FindAsync(savedTrip.co_driver_id.Value);
                if (coDriver != null)
                {
                    coDriver.current_assigned_tripId = savedTrip.tripId;
                    _context.users.Update(coDriver);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return savedTrip;
        } 
        catch (Exception)
        {
            // Rollback only if transaction is still active
            if (transaction.TransactionId != null)
            {
                await transaction.RollbackAsync();
            }
            throw; 
        }
        });
        }


        public async Task<List<GetAllTripsDTO>> getAllTripService(string? type,string? truckId,string? driverId,string? date)
        {
        var query =  _tripRepo.getAllTripRepo();

            if (!string.IsNullOrEmpty(type))
            {
                query=query.Where(q=>q.trip_type==type);
            }
            if (!string.IsNullOrEmpty(truckId))
            {
                query=query.Where(q=>q.truck_id==int.Parse(truckId));
            }
            if (!string.IsNullOrEmpty(driverId))
            {
                query=query.Where(q=>q.driver_id==int.Parse(driverId));
            }
            if (!string.IsNullOrEmpty(date))
            {
                query=query.Where(q=>q.scheduled_date==date);
            }


         var trips = query.Select(t => new GetAllTripsDTO
        {
        tripId = t.tripId,
        pickupPoint = t.pickup_location,
        destination = t.destination,
        allowance = t.allowance,
        type = t.trip_type,
        status=t.tripStatus!.statusName,
        driver = t.driver != null ? new GetUserRegisterDTo
        {
            userId = t.driver.userId,
            f_name = t.driver.f_Name,
            L_name = t.driver.l_Name,
            email = t.driver.email,
        } : null,
        truck = t.truck != null ? new TruckInfoDTO
        {
            id = t.truck.truckId,
            plate_number = t.truck.plate_number,
            make = t.truck.make,
            model = t.truck.model,
            year = t.truck.year,
            type = t.truck.type,
            status = t.truck.status,
            sub_status = t.truck.sub_status,
        } : null

        }).ToList();

        return trips; 
        }
   

        public async Task<getTripDetailsByIdDTO> getTripDetailsById(int id)
        {
        var trip = await _tripRepo.getTripByIdRepo(id);
        if (trip == null)
        {
        throw new ApiException("message", $"this trip id: {id} is not exist");
        }

        var expenses = _expenseRepo.getExpenseListByTripId(trip.tripId) ;
        var totalExpense = expenses.Sum(e => e.amount);
        var remainingAllowance = trip.allowance - totalExpense;

        var tripDetail = new getTripDetailsByIdDTO
        {
        trip_id = trip.tripId,
        // FIX 1: Added ?. and ?? for status
        status = trip.tripStatus?.statusName ?? "Unknown", 
        type = trip.trip_type,
        
        truck = trip.truck != null ? new Truck
        {
            truck_id = trip.truck.tripId,
            plate_number = trip.truck.plate_number,
            model = trip.truck.model
        } : null,

        driver = trip.driver != null ? new Driver
        {
            driver_id = trip.driver.userId,
            driver_name = $"{trip.driver.f_Name} {trip.driver.l_Name}",
            cnic = trip.driver.cnic,
            age = trip.driver.age,
            license_number = trip.driver.license_number,
            driver_image_url = trip.driver.profile_image,
        } : null,

        // FIX 2: Corrected co_driver logic to use trip.co_driver everywhere
        co_driver = trip.co_driver != null ? new Driver
        {
            driver_id = trip.co_driver.userId,
            driver_name = $"{trip.co_driver.f_Name} {trip.co_driver.l_Name}",
            cnic = trip.co_driver.cnic,
            age = trip.co_driver.age,
            license_number = trip.co_driver.license_number,
            driver_image_url = trip.co_driver.profile_image,
        } : null,

        client = new Client
        {
            name = trip.client_Name,
            company = trip.client_company,
            contact = trip.client_contact,
        },

        route = new Route
        {
            from = trip.pickup_location,
            to = trip.destination,
            distance_km = trip.distance_km,
            estimated_time = trip.estimated_time_min,
            lats = null,
            longs = null,
        },

        allowance = new Allowance
        {
            total = trip.allowance,
            used = totalExpense,
            remaining = remainingAllowance,
        },

        location = new Location
        {
            pic_lat = trip.pic_lat,
            pic_lng = trip.pic_lng,
            des_lat = trip.des_lat,
            des_lng = trip.des_lng,
        },

        expenses = expenses.Select(e => new TripExpense
        {
            expense_id = e.expenseId,
            amount = e.amount,
            date = e.created_at.ToString("yyyy-MM-dd"),
            note = e.notes,
            receiptImage = e.receipt_url,
            expense_category_id = e.e_c_id,
            // FIX 3: Added safety for category name
            expense_category_name = e.expenseCategory!.name ?? "No Defined Category",
            created_at = e.created_at.ToString("yyyy-MM-dd HH:mm:ss")
        }).ToList(),

        createdAt = trip.created_at
        };

        return tripDetail;
        }
 
    
    
    
   
        public async Task<List<TripStatus>> getAllTripStatus()
        {
        var tripStatus= await _tripRepo.getAllTripStatusRepo().ToListAsync();
        return tripStatus;
   
        }
    
    
         public async Task<bool> UpdateTripStatusService(int tripId, int statusId)
        {
            await _tripRepo.UpdateTripStatusRepo(tripId,statusId);

            return true;
            
        }


        public async Task<bool> addTripLocationService(addCurrentLocationDTO addCurrentLocationDTO)
        {
        await _tripRepo.UpdateTruckCurrentLocation(addCurrentLocationDTO.tripId.Value, addCurrentLocationDTO.current_lat, addCurrentLocationDTO.current_long);
        return true;
    
        }
        public async Task<bool> deleteTripById(int id)
        {
        var result = await _tripRepo.deleteTripByIdRepo(id);
        return result;
        } 
    
        }
        



}