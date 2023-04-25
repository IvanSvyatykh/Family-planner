using Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class SQLExpensesRepository
    {
        private readonly ApplicationContext db;

        public SQLExpensesRepository()
        {
            db = new ApplicationContext();
        }

        public List<Expenses> GetUserExpensesByCategoty(uint userId, string categoryName)
        {
            var expenses = db.Expenses.Where(e => e.UserId == userId && e.ExpensesName.Equals(categoryName)).ToList();
            if (expenses.Count == 0)
            {
                expenses.Add(new Expenses() { Cost = null, ExpensesDate = null, ExpensesName = null, UserId = null, Id = 0 });
            }
            return expenses;
        }

        public async Task<bool> AddExpenseAsync(Expenses expenses)
        {
            try
            {
                await db.Expenses.AddAsync(expenses);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
