using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src.Data;
using TMS.src;

namespace TMS.src
{
    public interface IExpenseService
    {
        Task<List<ExpenseTypeDTO>> expenseType();
        Task<bool> addExpenseService(AddExpenseDTO add,string imageUrl);
        Task validateExpenseIds(AddExpenseDTO add);
    }
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepo _expenseRepo;
        private readonly IUserRepo _userRepo;
        private readonly ITripRepo _tripRepo;

        public ExpenseService(IExpenseRepo expenseRepo,IUserRepo userRepo,ITripRepo tripRepo)
        {
            _expenseRepo = expenseRepo;
            _userRepo= userRepo;
            _tripRepo=tripRepo;
        }

        public async Task<List<ExpenseTypeDTO>> expenseType()
        {
            var expenseType= _expenseRepo.getAllExpenseCategories();
            var expenseTypeList = await expenseType.Select(e=> new ExpenseTypeDTO
            {
                id=e.id,
                name=e.name
            }).ToListAsync();

            return expenseTypeList;
        }

// Service ke andar ek helper method banayen
public async Task validateExpenseIds(AddExpenseDTO add)
{
    var isTripExist = await _tripRepo.getTripByIdRepo(add.tripId.Value);
    var isDriverExist = await _userRepo.GetUserById(add.driverId.Value);
    var isCoDriverExist = await _userRepo.GetUserById(add.co_driverId.Value);
    var isExpenseTypeId = await _expenseRepo.GetExpenseCategoryById(add.expensetTypeId.Value);

    // 2. Validation Logic
    if (isTripExist == null)
    {
        throw new ApiException("tripId", "Trip Id is not found in database");
    }
    if (isDriverExist == null)
    {
        throw new ApiException("driverId", "Driver Id is not found in database");
    }
    if (isCoDriverExist == null)
    {
        throw new ApiException("co_driverId", "Co-Driver Id is not found in database");
    }
    if (isExpenseTypeId == null)
    {
        throw new ApiException("expensetTypeId", "Expense Type Id is not found in database");
    }
}
public async Task<bool> addExpenseService(AddExpenseDTO add, string imageUrl)
{
    await validateExpenseIds(add);

    await _expenseRepo.addExpenseRepo(add, imageUrl);
    return true;
}


    }
}