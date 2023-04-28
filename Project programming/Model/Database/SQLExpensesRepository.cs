﻿using Classes;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class SQLExpensesRepository
    {
        private readonly ApplicationContext db;

        public SQLExpensesRepository()
        {
            db = new ApplicationContext();
        }

        public List<Expenses> GetUserExpensesByCategoty(uint userId, string categoryName, byte monthInByte)
        {
            var expenses = db.Expenses.Where(e => e.UserId == userId && e.ExpensesDate.Month == monthInByte && e.ExpensesName.Equals(categoryName)).ToList();
            if (expenses.Count == 0)
            {
                expenses.Add(new Expenses() { Cost = null, ExpensesName = null, UserId = null, Id = 0 });
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

        public async Task<bool> RemoveExpense(Expenses expense)
        {
            try
            {
                await Task.Run(async () =>
                {
                    db.Expenses.Remove(await db.Expenses.Where(e => e.Id == expense.Id).FirstOrDefaultAsync());
                    await db.SaveChangesAsync();
                });
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> RevomveAllCategoryExpensesAsync(uint userId, string categoryName)
        {
            try
            {
                await Task.Run(async () =>
                {
                    var expenses = db.Expenses.Where(e => e.UserId == userId && e.ExpensesName == categoryName).ToList();

                    foreach (var expense in expenses)
                    {
                        db.Expenses.Remove(expense);
                        await db.SaveChangesAsync();
                    }

                });
                return true;
            }
            catch
            {
                return false;
            }
            

        }
    }
}