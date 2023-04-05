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
        public ushort FamilyId { get; set; }
        public FamilyMember(string MemberName, string MemberEmail , ushort FamilyId)
        {
            this.MemberName = MemberName;
            this.MemberEmail = MemberEmail;
            this.FamilyId = FamilyId;
        }

    }
}
