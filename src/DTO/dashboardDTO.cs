
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src;

namespace TMS.src
{
    public class DashboardDTO
    {
        public string? date{get;set;}
        public int? load_percentage{get;set;}
        public TripsInfoDTO? trips{get;set;}
        public VihiclesInfoDTO? vihicles{get;set;}
        public List<GetTripsListDTO>? tripList{get;set;}
        

    }

        public class TripsInfoDTO
    {
        public int? import{get;set;}
        public int? export{get;set;}
        public int? local{get;set;}
        public int? total{get;set;}
        public int? ready{get;set;}
        public int? in_progress{get;set;}
        public int? completed{get;set;}
        public int? not_started{get;set;}
        
    }
    public class VihiclesInfoDTO
    {
        public int? total{get;set;}
        public int? owned{get;set;}
        public int? outsourced{get;set;}
        public int? available{get;set;}
        public int? in_progress{get;set;}
        public int? in_maintenance{get;set;}    
    }
    public class GetTripsListDTO
    {
        public int tripId{get;set;}
        public string? pickupPoint{get;set;}
        public string? destination{get;set;}
        public string? type{get;set;}
        public int? status{get;set;}
        public string? date{get;set;}
        public DateTime? created_at{get;set;}
        public Truck? truck{get;set;}
        
    }
}