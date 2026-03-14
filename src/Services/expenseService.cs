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
        Task<List<ExpenseListDTO>> getAllExpenseList();
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

        public async Task validateExpenseIds(AddExpenseDTO add)
{
    if (add.tripId != null) 
    {
        var isTripExist = await _tripRepo.getTripByIdRepo(add.tripId.Value);
        if (isTripExist == null)
        {
            add.tripId = null; 
        }
    }
     if (add.driverId != null)
    {
        var isDriverExist = await _userRepo.GetUserById(add.driverId.Value);
        if (isDriverExist == null)
        {
            add.driverId = null;
        }
    }

    if (add.co_driverId != null)
    {
        var isCoDriverExist = await _userRepo.GetUserById(add.co_driverId.Value);
        if (isCoDriverExist == null)
        {
            add.co_driverId = null;
        }
    }

    
    if (add.expensetTypeId != null)
    {
        var isExpenseTypeExist = await _expenseRepo.GetExpenseCategoryById(add.expensetTypeId.Value);
        if (isExpenseTypeExist == null)
        {
            add.expensetTypeId = null;
        }
    }
}

        public async Task<bool> addExpenseService(AddExpenseDTO add, string imageUrl)
{
    await validateExpenseIds(add);

    await _expenseRepo.addExpenseRepo(add, imageUrl);
    return true;
}

        public async Task<List<ExpenseListDTO>> getAllExpenseList()
        {
            var expens= _expenseRepo.getAllExpenseList();
            var expendData=await expens.Select(e=>new ExpenseListDTO
            {
                expenseid=e.expenseId,
                expensetTypeId=e.e_c_id,
                tripId=e.trip_id,
                driverId=e.driver_id,
                co_driverId=e.co_driver_id,
                amount=e.amount,
                note=e.notes,
                date=e.date,
                receiptImage=e.receipt_url

            }).ToListAsync();
            

            return expendData; 
        }



    }
}