
using Microsoft.AspNetCore.Http;
namespace TMS.src
{
    public class ExpenseTypeDTO
    {
        public int id{get;set;}
        public string? name{get;set;}
    }


    public class AddExpenseDTO
    {
        public int? tripId{get;set;}
        public int? expensetTypeId{get;set;}
        public int? driverId{get;set;}
        public int? co_driverId{get;set;}
        public double? amount{get;set;}
        public string? note{get;set;}
        public string? curr_lat{get;set;}
        public string? curr_lng{get;set;}
        public IFormFile? receiptImage{get;set;}

    }
    public class ExpenseListDTO
    {
        public int? expenseid{get;set;}
        public int? tripId{get;set;}
        public int? expensetTypeId{get;set;}
        public int? driverId{get;set;}
        public int? co_driverId{get;set;}
        public double? amount{get;set;}
        public string? note{get;set;}
        public string? date{get;set;}
        public string? receiptImage{get;set;}

    }

    public class ExpenseCatagoryDTO // Default mein ye internal hoti hai
{
    public int id { get; set; }
    public string name { get; set; }
}





/// Finance Related DTOs
 
    public class Finance
    {
        public string? peroid{get;set;}
        public string? date{get;set;}

        public Summary? summary{get;set;}
        
        public Dictionary<string, double>? expense_breakdown { get; set; }


    }

    public class Summary
    {
        public double? total_revenue{get;set;}
        public double? total_expense{get;set;}
        public double? total_net_profit{get;set;}
        public int? profit_change_percentage{get;set;}
        public string? profit_trends{get;set;}
    }




}