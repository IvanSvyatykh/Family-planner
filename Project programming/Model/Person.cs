using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members
{
    public class FamilyMember
    {
        public string MemberName { get; set; }
        public string MemberEmail { get; set; }
        public string MemberSalary { get; set; }

        public FamilyMember(string MemberName, string MemberEmail, string MemberSalary)
        {
            this.MemberName = MemberName;
            this.MemberEmail = MemberEmail; 
            this.MemberSalary = MemberSalary;   
        }
    }
}
