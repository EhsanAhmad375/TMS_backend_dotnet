using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.src
{
    public class TripModel
    {
        [Key]
        public int tripId{get;set;}
        public string? trip_number{get;set;}
        public int? truck_id{get;set;}
        [ForeignKey("truck_id")]
        public TruckModel? truck{get;set;}
        public int? driver_id{get;set;}
        [ForeignKey("driver_id")]
        public UserModel? driver{get;set;}
        public int? co_driver_id{get;set;}

        [ForeignKey("co_driver_id")]
        public UserModel? co_driver{get;set;}
        public string? client_Name{get;set;}
        public string? client_contact{get;set;}
        public string? client_company{get;set;}
        public string? trip_type{get;set;} // import | export | local
        public int? TripStatusId { get; set; }


// 2. The Navigation Property
        [ForeignKey("TripStatusId")]
        public TripStatus? tripStatus{get;set;}
        
        
        public string? pickup_location{get;set;}
        public string? destination{get;set;}
        public double? distance_km{get;set;}
        public int? estimated_time_min{get;set;}
        public double? allowance{get;set;}
        public DateTime? actual_start_time{get;set;}
        public DateTime? actual_end_time{get;set;}
        public int? odometer_start{get;set;}
        public int? odometer_end{get;set;}
        public string? notes{get;set;}
        public string? scheduled_date{get;set;}
        public string? scheduled_time{get;set;}
        public bool? is_active{get;set;}

        public string? curr_lat{get;set;}
        public string? curr_lng{get;set;}

        public string? pic_lat{get;set;}
        public string? pic_lng{get;set;}

        public string? des_lat{get;set;}
        public string? des_lng{get;set;}
        public DateTime created_at{get;set;}=DateTime.UtcNow;
        public DateTime updated_at{get;set;}=DateTime.UtcNow;
    }


    public class TripStatus
{
    [Key]
    public int tripStatusId { get; set; }
    public string statusName { get; set; } = string.Empty; 
    

}
}