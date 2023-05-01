using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Classes
{
    public class DataForGrapics
    {
        public string Day { get; set; }
        public long? Count { get; set; }      
        public DataForGrapics(int Day, long? Count)
        {
            this.Day = Day.ToString();
            this.Count = Count;

        }
    }
}
