using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_programming.Database
{
    public class Family
    {
        public int Id { get; }
        public string FamilyName { get; }
        public int Balance { get; }
        public Family(int id, string familyName, int balance)
        {
            Id = id;
            FamilyName = familyName;
            Balance = balance;
        }

    }
}
