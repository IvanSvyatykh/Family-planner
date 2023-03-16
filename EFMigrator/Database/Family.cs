using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;

namespace Families
{
    public class Family
    {
        public ushort Id { get; set; }
        public string Name { get; set; }
        public ushort Balance { get; set; }
        public ushort Code { get; set; }

        public User? User { get; set; }
    }
}
