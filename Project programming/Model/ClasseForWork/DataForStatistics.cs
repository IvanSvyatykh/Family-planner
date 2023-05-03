using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class DataForStatistics
    {
        public string NameOfExpense { get; } = string.Empty;
        public uint AllExpense { get; }
        public uint MaxExpense { get; }
        public uint MinExpense { get; } = uint.MaxValue;

        private ExtendedMonth _extendedMonth = new ExtendedMonth();

        public DataForStatistics(List<Expenses> expensesInMonth)
        {
            uint count = 0;
            foreach (var expense in expensesInMonth)
            {
                count += expense.Cost;
                MaxExpense = Math.Max(MaxExpense, expense.Cost);
                MinExpense = Math.Min(MinExpense, expense.Cost);

            }
            AllExpense = count;
            if (expensesInMonth.Count != 0)
            {                 
                NameOfExpense = expensesInMonth[0].ExpensesName;
            }
        }
    }
}
