using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.src
{
    public class ExpenseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ye auto-increment karta hai
        public int expenseId{get;set;}
        public int? trip_id{get;set;}
        [ForeignKey("trip_id")]
        public TripModel? trip{get;set;}
        public int? driver_id{get;set;}
        [ForeignKey("driver_id")]
        public UserModel? driver{get;set;}
        public int? co_driver_id{get;set;}

        [ForeignKey("co_driver_id")]
        public UserModel? co_driver{get;set;}
        public string? client_Name{get;set;}
        
        public int? e_c_id{get;set;}
        [ForeignKey("e_c_id")]
        public ExpenseCategory? expenseCategory{get;set;}
        public double? amount{get;set;}
        public string? date{get;set;}
        public string? time{get;set;}
        public string? location{get;set;}
        public string? receipt_url{get;set;}
        public string? notes{get;set;}
        public int? added_by{get;set;}
        [ForeignKey("added_by")]
        public UserModel? user{get;set;}
        public DateTime created_at{get;set;}=DateTime.UtcNow;
        public DateTime updated_at{get;set;}=DateTime.UtcNow;









        
    }

    public class ExpenseCategory
    {
        [Key]    
        public int id{get;set;}
        public string? name{get;set;}

    }
}