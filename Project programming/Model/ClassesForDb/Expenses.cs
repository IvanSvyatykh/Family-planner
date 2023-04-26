using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Expenses
    {
        public uint Id { get; set; }
        public uint? UserId { get; set; }
        public string? ExpensesName { get; set; }
        public DateOnly ExpensesDate { get; set; }
        public uint? Cost { get; set; }
    }
}
