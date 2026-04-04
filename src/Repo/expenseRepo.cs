using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src;
using TMS.src.Data;

namespace TMS.src
{
    public interface IExpenseRepo
    {
        IQueryable<ExpenseCategory> getAllExpenseCategories();
        Task<ExpenseCategory> GetExpenseCategoryById(int id);
        public IQueryable<ExpenseModel> getAllExpenseList();
        public IQueryable<ExpenseModel> getExpenseListByTripId(int tripId);
        Task<bool> addExpenseRepo(AddExpenseDTO addExpenseDTO,string imagePath);
        Task saveChangesAsync();
    }
    public class ExpenseRepo :IExpenseRepo
    {
        private readonly AppDbContext _appDbContext;
        public ExpenseRepo(AppDbContext appDbContext)
        {
            _appDbContext=appDbContext;
        }

        public Task saveChangesAsync()
        {
            return  _appDbContext.SaveChangesAsync();
        }

        public async Task<ExpenseCategory> GetExpenseCategoryById(int id)
        {
            return await _appDbContext.expenseCategories.FirstOrDefaultAsync(e=>e.id==id);

        }
        public IQueryable<ExpenseCategory> getAllExpenseCategories()
        {
         var expense=_appDbContext.expenseCategories;
         return expense;   
        }

        public IQueryable<ExpenseModel> getAllExpenseList()
        {
         var expense=_appDbContext.expenses;
         return expense;   
        }
        public IQueryable<ExpenseModel> getExpenseListByTripId(int tripId)
        {
         var expense=_appDbContext.expenses.Where(e=>e.trip_id==tripId);
         return expense;   
        }


        public async Task<bool> addExpenseRepo(AddExpenseDTO addExpenseDTO,string imagePath)
        {
            var expense= new ExpenseModel
            {
                trip_id=addExpenseDTO.tripId,
                e_c_id=addExpenseDTO.expensetTypeId,
                amount=addExpenseDTO.amount,
                notes=addExpenseDTO.note,
                receipt_url=imagePath,
                co_driver_id=addExpenseDTO.co_driverId,
                driver_id=addExpenseDTO.driverId
                
            };
            await _appDbContext.expenses.AddAsync(expense);
             await saveChangesAsync();
             return true;
        }
  
  
  
    }

    
}
