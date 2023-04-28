using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class FamilyMember
    {
        public uint Id { get; set; }    
        public string MemberName { get; set; }
        public string MemberEmail { get; set; }
        public uint FamilyId { get; set; }
        public FamilyMember(string MemberName, string MemberEmail , uint FamilyId , uint Id)
        {
            this.Id = Id;
            this.MemberName = MemberName;
            this.MemberEmail = MemberEmail;
            this.FamilyId = FamilyId;
        }

    }
}
