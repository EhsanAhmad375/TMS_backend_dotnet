using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace TMS.src
{
    public interface ITripService
    {
        Task<TripModel> createTripService(CreateTripDTO createTrip);

        Task<List<GetAllTripsDTO>> getAllTripService(string? type,string? truckId,string? driverId,string? date);
        Task<getTripDetailsByIdDTO> getTripDetailsById(int id);
    }
    public class TripService : ITripService
    {
        private readonly ITripRepo _tripRepo;

        public TripService(ITripRepo tripRepo)
        {
            _tripRepo=tripRepo;
        }
       public async Task<TripModel> createTripService(CreateTripDTO createTrip)
        {
            var trip=new TripModel
            {
                truck_id=createTrip.truck_id,
                driver_id=createTrip.driver_id,
                co_driver_id=createTrip.co_driver_id,
                trip_type=createTrip.type,
                pickup_location=createTrip.pickup_location,
                destination=createTrip.destination,
                allowance=createTrip.allowance,
                scheduled_date=createTrip.scheduled_date,
                client_Name=createTrip.client_name,
                client_contact=createTrip.client_contact,
                client_company=createTrip.client_company,

            };
            return await _tripRepo.createTripRepo(trip);
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
        status=t.TripStatusId,
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
            var trip=await _tripRepo.getTripByIdRepo(id);
            if (trip == null)
            {
                throw new ApiException("message",$"this trip id: {id} is not exist");
            }

            var tripDetail=new getTripDetailsByIdDTO
            {
                trip_id=trip.tripId,
                status=trip.TripStatusId,
                type=trip.trip_type,
                truck=trip.truck!=null? new Truck
                {
                    truck_id=trip.truck.tripId,
                    plate_number=trip.truck.plate_number,
                    model=trip.truck.model
                }:null,
                driver=trip.driver!=null?new Driver
                {
                    driver_id=trip.driver.userId,
                    driver_name=trip.driver.f_Name+" "+trip.driver.l_Name,
                    cnic=trip.driver.cnic,
                    age=trip.driver.age,
                    license_number=trip.driver.license_number,
                    driver_image_url=trip.driver.profile_image,
                }:null,
                co_driver=trip.co_driver!=null? new Driver
                {
                    driver_id=trip.co_driver.userId,
                    driver_name=trip.co_driver.f_Name+" "+trip.driver.l_Name,
                    cnic=trip.co_driver.cnic,
                    age=trip.co_driver.age,
                    license_number=trip.co_driver.license_number,
                    driver_image_url=trip.co_driver.profile_image,
                }:null,
                client=new Client
                {
                    name=trip.client_Name,
                    company=trip.client_company,
                    contact=trip.client_contact,
                },
                route=new Route
                {
                    from=trip.pickup_location,
                    to=trip.destination,
                    distance_km=trip.distance_km,
                    estimated_time=trip.estimated_time_min,
                    lats=null,
                    longs=null,
                },
                allowance=new Allowance
                {
                    total=trip.allowance,
                    used=null,
                    remaining=null,

                },
                createdAt=trip.created_at


            };
    
    
        return tripDetail;
    
        }
    
    
    
    
    
    }
    }