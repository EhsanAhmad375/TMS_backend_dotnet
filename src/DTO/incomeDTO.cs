
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src;

namespace TMS.src
{
    public class AddIncomeDTO
    {
    
        public int? sourceId{get;set;}
        public double? total_amount{get;set;}
        public double? received_amount{get;set;}
        public string? date{get;set;}
        public string? notes{get;set;}
        public int? added_by{get;set;}

    }
}