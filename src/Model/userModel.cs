using System.ComponentModel.DataAnnotations;

namespace TMS.src
{
    public class UserModel
    {
        [Key]
        public int userId {get;set;}
        public string? f_Name {get; set;}
        public string? l_Name {get;set;}
        [EmailAddress]
        public string? email {get;set;}
        [Phone]
        public string? phone_no {get;set;}
        public string? age {get;set;}
        public string? role {get;set;}
        public string? profile_image {get;set;}
        public string? device_token {get;set;}
        public string? is_verified {get;set;}
        public bool is_active {get;set;}=false;
        public bool is_available {get;set;}=false;
        public string? password {get;set;}
        public string? cnic {get;set;}
        public string? license_number {get;set;}
        public string? address {get;set;}
        public string? emergency_contact {get;set;}
        public int ?experience_years{get;set;}
        public int ?status{get;set;}
        public int ?assigned_truck_id{get;set;}
        public string ?verification_status{get;set;}

        

        public DateTime created_at{get;set;}=DateTime.UtcNow;
        public DateTime updated_at{get;set;}=DateTime.UtcNow;




    }
}