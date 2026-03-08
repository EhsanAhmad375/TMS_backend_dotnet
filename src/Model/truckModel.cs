using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.src
{
    public class TruckModel
    {
        [Key]
        public int truckId{get;set;}
        public string? plate_number{get;set;}
        public string? model{get;set;}
        public string? make{get;set;}
        public string? year{get;set;}
        public string? type{get;set;}       // owned || outsourced
        public string? status{get;set;} //active || inactive|| in maintenance
        public string? sub_status{get;set;} // available || onroute
        public string? fuel_percentage{get;set;}
        public int? mileage_km{get;set;}
        public string? tire_condition{get;set;} // good, fair, poor
        public string? engine_capacity{get;set;} 
        public int? max_load_tons{get;set;} 
        public double? rating{get;set;} 
        public double? image_url{get;set;} 
        public int? tripId { get; set; } // Agar ye kisi aur table ki ID hai toh isay nullable rakhein
        [ForeignKey("tripId")]
        public TripModel trip{get;set;}
        public string? fuel_type { get; set; } // e.g., Diesel, Petrol, Electric

        public int? driver_id{get;set;} 
        [ForeignKey("driver_id")]
        public UserModel driver{get;set;}

        public int? co_driver_id{get;set;} 
        [ForeignKey("co_driver_id")]
        public UserModel co_driver{get;set;}

        public string? registration_expiry{get;set;} 
        public string? insurance_type{get;set;} 
        public string? permit_type{get;set;} 
        public string? fitness_cert_expiry{get;set;} 
        public string? cnic_status{get;set;} // erified | pending | rejected

        public bool? is_active{get;set;}

        public DateTime created_at{get;set;}=DateTime.UtcNow;
        public DateTime updated_at{get;set;}=DateTime.UtcNow;






    }
}