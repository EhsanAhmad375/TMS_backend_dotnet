using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src.Data;
using TMS.src;
using System.ComponentModel.DataAnnotations;

namespace TMS.src
{
    public class CreateTripDTO
    {
        public int? truck_id{get;set;}
        public int? driver_id{get;set;}
        public int? co_driver_id{get;set;}
        public string? type{get;set;}
        public string? pickup_location{get;set;}
        public string? destination{get;set;}
        public int? allowance{get;set;}
        public string? scheduled_date{get;set;}
        public string? notes{get;set;}
        public string? client_name{get;set;}
        public string? client_contact{get;set;}
        public string? client_company{get;set;}   
    }

    /// Get All Trips
    

    public class GetAllTripsDTO
    {
        public int tripId{get;set;}

        public string? pickupPoint{get;set;}
        public string? destination{get;set;}
        public string? type{get;set;}
        public string? status{get;set;}
        public double? allowance{get;set;}
        public int? created_at{get;set;}
        public TruckInfoDTO? truck{get;set;}
        public GetUserRegisterDTo? driver{get;set;}
    }



    public class getTripDetailsByIdDTO
    {
        public int trip_id{get;set;}
        public string? type{get;set;}
        public string? status{get;set;}
        public Truck? truck{get;set;}
        public Driver? driver{get;set;}
        public Driver? co_driver{get;set;}
        public Client? client{get;set;}
        public Route? route{get;set;}
        public Allowance? allowance{get;set;}
        public List<TripExpense>? expenses{get;set;}
        public DateTime? createdAt{get;set;}=DateTime.Now;
        

    }

    public class Client
    {
        public string? name{get;set;}
        public string? contact{get;set;}
        public string? company{get;set;}
        
    }
    public class TripExpense
    {
        public int? expense_id{get;set;}
        public int? expense_category_id{get;set;}
        public string? expense_category_name{get;set;}
        public double? amount{get;set;}
        public string? note{get;set;}
        public string? date{get;set;}
        public string? receiptImage{get;set;}
        public string? created_at{get;set;}
    }


        public class Route
    {
        public string? from{get;set;}
        public string? to{get;set;}
        public double? distance_km{get;set;}
        public int? estimated_time{get;set;}
        public double? lats{get;set;}
        public double? longs{get;set;}
    }

    public class Allowance
    {
        public double? total{get;set;}
        public double? used{get;set;}
        public double? remaining{get;set;}
        
    }


}