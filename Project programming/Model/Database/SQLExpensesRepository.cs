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

        public List<Expenses> GetUserExpensesByCategoty(uint userId, string categoryName) => db.Expenses.Where(e => e.UserId == userId && e.ExpensesName.Equals(categoryName)).ToList();





    }
}
