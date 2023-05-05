using Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector
{
    public class AppData : IAppData
    {
        public Family Family { get; private set; } 
        public User User { get; private set; }      

        public void AddFamily(Family family)
        {
           Family= family;
        }

        public void AddUser(User user)
        {
            User = user;
        }
    }
}
