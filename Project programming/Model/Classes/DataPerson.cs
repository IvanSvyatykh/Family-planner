using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class DataPerson
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        public DataPerson(string Name, string CurrentEmail,  string FamilyEmail)
        {
            this.Name = Name;
            Email = CurrentEmail;
            if (CurrentEmail.Equals(FamilyEmail)) Status = "Creator";
            else Status = "User";
        }
    }
}
