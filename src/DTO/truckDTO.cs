using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src.Data;
using TMS.src;
using System.ComponentModel.DataAnnotations;

namespace TMS.src
{
    public class CreateTruckDTO
    {
        public string? plate_number{get;set;}
        public string? model{get;set;}
        public string? make{get;set;}
        public string? year{get;set;}
        public string? type{get;set;}       // owned || outsourced
    }


        public class TruckDetailsDTO
    {
        public int? id{get;set;}
        public string? plate_number{get;set;}
        public string? model{get;set;}
        public string? make{get;set;}
        public string? year{get;set;}
        public string? type{get;set;}       // owned || outsourced
        public string? status{get;set;} //active || inactive|| in maintenance
        public string? sub_status{get;set;} // available || onroute
        public double? rating{get;set;} 
        public double? image_url{get;set;} 
        public Specifications? specifications{get;set;} 
        public Compliance? compliance{get;set;} 
        public AssignedPersonnel? assigned_personnel_info{get;set;} 
        public AssignedCoDriver? assigned_co_driver{get;set;} 
        public bool? is_active{get;set;}
        public DateTime created_at{get;set;}
        public DateTime updated_at{get;set;}

    }
    public class AssignedPersonnel
    {
         public int id {get;set;}
         public string? name {get;set;}
         public string? cnic {get;set;}
         public string? phone {get;set;}
         public string? email {get;set;}
         public string? address {get;set;}
         public string? emergency_contact {get;set;}
         public string? is_verified {get;set;}
         public string? verification_status {get;set;}
    }
    public class AssignedCoDriver
    {
         public int id {get;set;}
         public string? name {get;set;}
         public string? cnic {get;set;}
         public string? phone {get;set;}
         public string? email {get;set;}
         public string? address {get;set;}
         public string? emergency_contact {get;set;}
         public string? is_verified {get;set;}
         public string? verification_status {get;set;}


        
    }

    public class Specifications
    {
        public string? fuel_percentage{get;set;}
        public int? mileage_km{get;set;}
        public string? tire_condition{get;set;} // good, fair, poor
        public string? engine_capacity{get;set;} 
        public int? max_load_tons{get;set;} 
    }
    public class Compliance
    {
        public string? registration_expiry{get;set;} 
        public string? insurance_type{get;set;} 
        public string? permit_type{get;set;} 
        public string? fitness_certificate_expiry{get;set;} 
        public string? cnic_status{get;set;} // erified | pending | rejected
    }





    public class TruckCreatedDTO
     {
        public int? id{get;set;}
        public string? plate_number{get;set;}
        public string? model{get;set;}
        public string? make{get;set;}
        public string? year{get;set;}
        public string? type{get;set;}       // owned || outsourced

        
    }

    public class TruckListDTO
    {
        public int? id{get;set;}
        public string? plate_number{get;set;}
        
        public string? model{get;set;}
        public string? make{get;set;}
        public string? year{get;set;}
        public string? type{get;set;}       // owned || outsourced
        public string? status{get;set;} //active || inactive|| in maintenance
        public string? sub_status{get;set;} // available || onroute
        public Driver? driver{get;set;}
        
    }



    public class TruckInfoDTO
    {
        public int? id{get;set;}
        public string? plate_number{get;set;}
        
        public string? model{get;set;}
        public string? make{get;set;}
        public string? year{get;set;}
        public string? type{get;set;}       // owned || outsourced
        public string? status{get;set;} //active || inactive|| in maintenance
        public string? sub_status{get;set;} // available || onroute
        
        
    }

    public class Truck
    {
        public int? truck_id{get;set;}
        public string? plate_number{get;set;}
        public string? model{get;set;}
   
        
    }


    public class Driver
    {
        public int? driver_id{get;set;}
        public string? driver_name{get;set;}
        public string? driver_image_url{get;set;}
        public string? cnic{get;set;}
        public string? age{get;set;}
        public string? license_number{get;set;}
    }

}