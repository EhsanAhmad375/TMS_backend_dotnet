using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src.Data;
using TMS.src;
using System.Globalization;

namespace TMS.src
{
    public interface IExpenseService
    {
        Task<List<ExpenseTypeDTO>> expenseType();
        Task<bool> addExpenseService(AddExpenseDTO add, string imageUrl);
        Task validateExpenseIds(AddExpenseDTO add);
        Task<List<ExpenseListDTO>> getAllExpenseList();
        Task<List<ExpenseCatagoryDTO>> getAllExpenseCategories();
        Task<Finance> GetFinanceReport(int year, int? month, int? date);
    }

    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepo _expenseRepo;
        private readonly IUserRepo _userRepo;
        private readonly ITripRepo _tripRepo;
        private readonly IncomeRepo _incomeRepo;

        public ExpenseService(IExpenseRepo expenseRepo, IUserRepo userRepo, ITripRepo tripRepo, IncomeRepo incomeRepo)
        {
            _expenseRepo = expenseRepo;
            _userRepo = userRepo;
            _tripRepo = tripRepo;
            _incomeRepo = incomeRepo;
        }

        public async Task<List<ExpenseTypeDTO>> expenseType()
        {
            var expenseType = _expenseRepo.getAllExpenseCategories();
            var expenseTypeList = await expenseType.Select(e => new ExpenseTypeDTO
            {
                id = e.id,
                name = e.name
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
            add.curr_lat = add.curr_lat ?? "0";
            add.curr_lng = add.curr_lng ?? "0";

            await _tripRepo.UpdateTruckCurrentLocation(add.tripId.Value, add.curr_lat, add.curr_lng);
            await _expenseRepo.addExpenseRepo(add, imageUrl);
            return true;
        }

        public async Task<List<ExpenseListDTO>> getAllExpenseList()
        {
            var expens = _expenseRepo.getAllExpenseList();
            var expendData = await expens.Select(e => new ExpenseListDTO
            {
                expenseid = e.expenseId,
                expensetTypeId = e.e_c_id,
                tripId = e.trip_id,
                driverId = e.driver_id,
                co_driverId = e.co_driver_id,
                amount = e.amount,
                note = e.notes,
                date = e.date,
                receiptImage = e.receipt_url
            }).ToListAsync();

            return expendData;
        }

        public async Task<List<ExpenseCatagoryDTO>> getAllExpenseCategories()
        {
            var expens = _expenseRepo.getAllExpenseCategories();
            var expendData = await expens.Select(e => new ExpenseCatagoryDTO
            {
                id = e.id,
                name = e.name
            }).ToListAsync();

            return expendData;
        }

        public async Task<Finance> GetFinanceReport(int year, int? month, int? date)
        {
            // 1. Fetching Data
            var allExpenses = await _expenseRepo.getAllExpenseList().ToListAsync();
            var categories = await _expenseRepo.getAllExpenseCategories().ToListAsync();
            var allIncomes = await _incomeRepo.getAllIncomeRepo().ToListAsync();
            
            // Fetch all trips
            var allTrips = await _tripRepo.getAllTripRepo().ToListAsync();

            bool isDaily = date.HasValue && month.HasValue;
            bool isMonthly = !date.HasValue && month.HasValue;
            bool isYearly = !date.HasValue && !month.HasValue;

            DateTime selectedPeriodStart;
            DateTime previousPeriodStart;

            if (isDaily)
            {
                selectedPeriodStart = new DateTime(year, month.Value, date.Value);
                previousPeriodStart = selectedPeriodStart.AddDays(-1);
            }
            else if (isMonthly)
            {
                selectedPeriodStart = new DateTime(year, month.Value, 1);
                previousPeriodStart = selectedPeriodStart.AddMonths(-1);
            }
            else
            {
                selectedPeriodStart = new DateTime(year, 1, 1);
                previousPeriodStart = selectedPeriodStart.AddYears(-1);
            }

            bool IsInSelectedPeriod(DateTime scheduledDate)
            {
                if (isDaily)
                {
                    return scheduledDate.Date == selectedPeriodStart.Date;
                }
                else if (isMonthly)
                {
                    return scheduledDate.Year == selectedPeriodStart.Year && scheduledDate.Month == selectedPeriodStart.Month;
                }
                else
                {
                    return scheduledDate.Year == selectedPeriodStart.Year;
                }
            }

            bool IsInPreviousPeriod(DateTime scheduledDate)
            {
                if (isDaily)
                {
                    return scheduledDate.Date == previousPeriodStart.Date;
                }
                else if (isMonthly)
                {
                    return scheduledDate.Year == previousPeriodStart.Year && scheduledDate.Month == previousPeriodStart.Month;
                }
                else
                {
                    return scheduledDate.Year == previousPeriodStart.Year;
                }
            }

            var selectedTripIds = allTrips
                .Where(t => !string.IsNullOrEmpty(t.scheduled_date))
                .Select(t => new { t.tripId, scheduledDate = DateTime.TryParseExact(t.scheduled_date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate) ? parsedDate : DateTime.MinValue })
                .Where(x => x.scheduledDate != DateTime.MinValue)
                .Where(x => IsInSelectedPeriod(x.scheduledDate))
                .Select(x => x.tripId)
                .ToHashSet();

            var previousTripIds = allTrips
                .Where(t => !string.IsNullOrEmpty(t.scheduled_date))
                .Select(t => new { t.tripId, scheduledDate = DateTime.TryParseExact(t.scheduled_date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate) ? parsedDate : DateTime.MinValue })
                .Where(x => x.scheduledDate != DateTime.MinValue)
                .Where(x => IsInPreviousPeriod(x.scheduledDate))
                .Select(x => x.tripId)
                .ToHashSet();

            var filteredEx = allExpenses
                .Where(e => e.trip_id.HasValue && selectedTripIds.Contains(e.trip_id.Value))
                .ToList();

            var previousEx = allExpenses
                .Where(e => e.trip_id.HasValue && previousTripIds.Contains(e.trip_id.Value))
                .ToList();

            var filteredIn = allIncomes
                .Where(i => i.trip_id.HasValue && selectedTripIds.Contains(i.trip_id.Value))
                .ToList();

            var previousIn = allIncomes
                .Where(i => i.trip_id.HasValue && previousTripIds.Contains(i.trip_id.Value))
                .ToList();

            double totalExp = filteredEx.Sum(e => e.amount ?? 0);
            double totalRev = filteredIn.Sum(i => i.total_amount ?? 0);
            double totalProfit = totalRev - totalExp;

            double previousExp = previousEx.Sum(e => e.amount ?? 0);
            double previousRev = previousIn.Sum(i => i.total_amount ?? 0);
            double previousProfit = previousRev - previousExp;

            int profitChangePercentage;
            string profitTrend;

            if (previousProfit == 0)
            {
                profitChangePercentage = totalProfit == 0 ? 0 : 100;
                profitTrend = totalProfit >= 0 ? "up" : "down";
            }
            else
            {
                var change = ((totalProfit - previousProfit) / Math.Abs(previousProfit)) * 100.0;
                profitChangePercentage = (int)Math.Round(Math.Abs(change));
                profitTrend = change >= 0 ? "up" : "down";
            }

            return new Finance
            {
                peroid = isDaily ? "daily" : isMonthly ? "monthly" : "yearly",
                date = isDaily ? $"{selectedPeriodStart:yyyy-MM-dd}" : isMonthly ? $"{selectedPeriodStart:yyyy-MM}" : $"{selectedPeriodStart:yyyy}",
                summary = new Summary
                {
                    total_revenue = totalRev,
                    total_expense = totalExp,
                    total_net_profit = totalProfit,
                    profit_change_percentage = profitChangePercentage,
                    profit_trends = profitTrend
                },
                expense_breakdown = filteredEx
                    .GroupBy(e => e.e_c_id)
                    .ToDictionary(
                        g => categories.FirstOrDefault(c => c.id == g.Key)?.name?.ToLower() ?? "others",
                        g => g.Sum(e => e.amount ?? 0)
                    )
            };
        }
    }
}