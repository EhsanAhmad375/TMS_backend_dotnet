
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
        public IFormFile? receiptImage{get;set;}

    }
}