using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace TMS.src
{
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService=expenseService;
        }


        [HttpGet("expense-type-list")]
        public async Task<IActionResult> getAllExpenseType()
        {
            var type=await _expenseService.expenseType();
            try
            {
                return StatusCode(200,new {success=true, message="Retrive expense type successfully", data=type});
            }
            catch (Exception ex)
            {
                return StatusCode(500,new{success=false,message="internal server error",error=ex.Message});
            }
        }






        [HttpPost("add-expense")]
        public async Task<ActionResult> addExpense([FromForm] AddExpenseDTO add)    
        {
        try
        {
        string dbImagePath = null;

        // 1. Image Logic
        if (add.receiptImage != null && add.receiptImage.Length > 0) 
            {
            // Behtar tareeka: Path direct wwwroot se uthayein
            string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string uploadsFolder = Path.Combine(rootPath, "uploads");

            // Agar folder delete kar diya hai, to ye line usay dobara bana degi
            if (!Directory.Exists(uploadsFolder)) 
            {
                Directory.CreateDirectory(uploadsFolder);
                Console.WriteLine("Uploads folder created again!");
            }

            string fileName = Guid.NewGuid().ToString() + "_" + add.receiptImage.FileName;
            string fullPath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await add.receiptImage.CopyToAsync(stream);
            }

            dbImagePath = "/uploads/" + fileName;
            Console.WriteLine($"Image saved at: {fullPath}");
        }
        else 
        {
            Console.WriteLine("No image was received in the request.");
        }

        // 2. Service Call
        await _expenseService.addExpenseService(add, dbImagePath);

        return Ok(new { success = true, message = "Added Successfully" });
    }
    catch (ApiException ex)
    {
        return BadRequest(new { success = false, error = ex.Message });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"CRITICAL ERROR: {ex.Message}");
        return StatusCode(500, new { success = false, error = ex.Message }); // Temporary actual error dekhein
    }
}






        [HttpGet("get-expense-list")]
        public async Task<IActionResult> getExpenseList()
        {
            try
            {
                var expense=await _expenseService.getAllExpenseList();
                return StatusCode(200, new{success=true,message="Expense List Successfully Retrive",data=expense});
                
            }catch(Exception ex)
            {
                return BadRequest(new {success=false, error=ex.Message});
            }
        }

    
        [HttpGet("get-expense-category-list")]
        public async Task<IActionResult> getExpenseCategoryList()
        {
            try
            {
                var expense=await _expenseService.getAllExpenseCategories();
                return StatusCode(200, new{success=true,message="Expense Category List Successfully Retrive",data=expense});
                
            }catch(Exception ex)
            {
                return BadRequest(new {success=false, error=ex.Message});
            }
        }


        [HttpGet("get-financial-report")]
        public async Task<IActionResult> GetMonthlyFinanceReport([FromQuery] int year, [FromQuery] int? month, [FromQuery] int? date)
        {
            // Basic validation taake ghalat values na aaye
            if (month.HasValue && (month.Value < 1 || month.Value > 12))
            {
                return BadRequest(new { success = false, message = "Invalid month. Please provide a value between 1 and 12." });
            }

            if (date.HasValue)
            {
                if (!month.HasValue)
                {
                    return BadRequest(new { success = false, message = "Month is required when filtering by date." });
                }

                if (date.Value < 1 || date.Value > 31)
                {
                    return BadRequest(new { success = false, message = "Invalid date. Please provide a value between 1 and 31." });
                }

                if (!DateTime.TryParseExact($"{date.Value:D2}-{month.Value:D2}-{year}", "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    return BadRequest(new { success = false, message = "Invalid date for the selected month/year." });
                }
            }

            try
            {
                var report = await _expenseService.GetFinanceReport(year, month, date);
                
                return Ok(new 
                { 
                    success = true, 
                    data = report 
                });
            }
            catch (Exception ex)
            {
                // Error handling agar DB mein koi masla ho
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

    
    }
}