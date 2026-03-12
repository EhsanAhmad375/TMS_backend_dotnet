using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    }
}