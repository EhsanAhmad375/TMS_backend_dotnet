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
    public interface ITruckService{
        Task<TruckCreatedDTO> createTruckService(CreateTruckDTO createTruckDTO);
        Task<List<TruckListDTO>> GetAllTruckListService();
        Task<TruckDetailsDTO> getTruckByIdService(int id);
        // Task<TruckModel> getTruckByNumberPlateService(string number_plate);
    }

    public class TruckService:ITruckService
    {
        private readonly ITruckRepo _truckRepo;
        public TruckService(ITruckRepo truckRepo)
        {
            _truckRepo=truckRepo;
        }
  
        public async Task<TruckCreatedDTO> createTruckService(CreateTruckDTO createTruckDTO)
        {
            var isExistTruck=await _truckRepo.getTruckByNumberPlateRepo(createTruckDTO.plate_number);
            if (isExistTruck != null)
            {
                throw new ApiException("message",$"this truck {createTruckDTO.plate_number} already exist");
            }

            var truck=new TruckModel{
            plate_number = createTruckDTO.plate_number,
            model=createTruckDTO.model,
            make=createTruckDTO.make,
            year=createTruckDTO.year,
            type=createTruckDTO.type,
            
         };

         var newTruck=await _truckRepo.createTruckRepo(truck);
         var truckDetails=new TruckCreatedDTO
         {
           id=newTruck.truckId,
           plate_number=newTruck.plate_number,
           model=newTruck.model,
           make=newTruck.make,
           year=newTruck.year,
           type=newTruck.type,  
         };
         return truckDetails;
        }

        public async Task<List<TruckListDTO>> GetAllTruckListService()
        {
        
            var query=await _truckRepo.getAllTruckRepo();

            var trucks=query.Select(t=>new TruckListDTO
            {
                id=t.truckId,
                plate_number=t.plate_number,
                make=t.make,
                model=t.model,
                year=t.year, 
                type=t.type,
                status=t.status,
                sub_status=t.sub_status,
                driver =new Driver{
                driver_id=t.driver?.userId,
                driver_name=t.driver?.f_Name+" "+t.driver?.l_Name,
                driver_image_url=t.driver?.profile_image    
                }
                

            }).ToList();
            
            return trucks;
        }


        public async Task<TruckDetailsDTO> getTruckByIdService(int id)
        {
            var truck=await _truckRepo.getTruckByIdRepo(id);
            if (truck == null)
            {
                throw new ApiException("message",$"this truck id {id} is not found ");
            }

            var truckDetails=new TruckDetailsDTO
            {
                id=truck.truckId,
                plate_number=truck.plate_number,
                model=truck.plate_number,
                make=truck.plate_number,
                year=truck.plate_number,
                type=truck.plate_number,
                status=truck.plate_number,
                sub_status=truck.plate_number,
                rating=truck.rating,
                is_active=truck.is_active,
                specifications=new Specifications
                {
                    fuel_percentage=truck.fuel_percentage,
                    mileage_km=truck.mileage_km,
                    tire_condition=truck.tire_condition,
                    engine_capacity=truck.engine_capacity,
                    max_load_tons=truck.max_load_tons,
                    
                },
                compliance=new Compliance
                {
                    registration_expiry=truck.registration_expiry,
                    insurance_type=truck.registration_expiry,
                    permit_type=truck.registration_expiry,
                    fitness_certificate_expiry=truck.registration_expiry,
                    cnic_status=truck.registration_expiry,
                },
                assigned_personnel_info =new AssignedPersonnel{
                    id=truck.driver.userId,
                    name=truck.driver.f_Name+" "+truck.driver.l_Name,
                    phone=truck.driver.phone_no,
                    email=truck.driver.email,
                    address=truck.driver.address,
                    emergency_contact=truck.driver.emergency_contact,
                    is_verified=truck.driver.is_verified,
                    verification_status=truck.driver.verification_status,
                },
                assigned_co_driver=new AssignedCoDriver
                {
                    id=truck.co_driver.userId,
                    name=truck.co_driver.f_Name+" "+truck.co_driver.l_Name,
                    phone=truck.co_driver.phone_no,
                    email=truck.co_driver.email,
                    address=truck.co_driver.address,
                    emergency_contact=truck.co_driver.emergency_contact,
                    is_verified=truck.co_driver.is_verified,
                    verification_status=truck.co_driver.verification_status,
                },
                created_at=truck.created_at,
                updated_at=truck.updated_at,
            };
            return truckDetails;
        }
  
  
    }

}