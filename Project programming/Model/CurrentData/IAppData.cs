using Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector
{
    public interface IAppData
    {
        public Family Family { get; }

        public User User { get;}

        public void AddFamily(Family family);

        public void AddUser(User user);
    }
}
