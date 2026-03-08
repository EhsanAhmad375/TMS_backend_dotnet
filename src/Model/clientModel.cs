using System.ComponentModel.DataAnnotations;

namespace TMS.src
{
    public class ClientModel
    {
        [Key]
        public int clientId{get;set;}
        public string? full_name{get;set;}
        public string? cnic{get;set;}
        public string? phone{get;set;}
        public int? age{get;set;}
        public string? company{get;set;}
        public string? address{get;set;}
        public bool? is_active{get;set;}
        public DateTime created_at{get;set;}=DateTime.UtcNow;

    }
}