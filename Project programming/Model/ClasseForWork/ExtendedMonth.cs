using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class ExtendedMonth
    {
        private Dictionary<string, byte> Monthes = new Dictionary<string, byte>()
        {
            {"January" , 01 },
            {"February" , 02 },
            {"March" , 03 },
            {"April" , 04 },
            {"May" , 05 },
            {"June" , 06 },
            {"July" , 07 },
            {"August" , 08 },
            {"September" , 09 },
            {"October" , 10 },
            {"November" , 11 },
            {"December" , 12 },
        };
      
        public byte GetMonthInByteFromString(string month)
        {
            try
            {
                return Monthes[month];
            }
            catch
            {
                throw new Exception("Incorrect format name of Month ");
            }
        }

        public List<string> GetAllMonthes() => Monthes.Keys.ToList();

        public string GetMonthInStringFromByte(byte num) => Monthes.FirstOrDefault(m => m.Value == num).Key;
    }
}
