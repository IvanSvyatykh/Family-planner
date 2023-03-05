using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PageLogic
{
    public class ForgottenPagePasswordLogic
    {
        public static bool CompareAnswerAndCode(int? answer, int? code) => answer == code;
        public static bool CheckCountTry(int countTry) => countTry < 3;
        public static bool CheckTheTime(DateTime? date) => date > DateTime.Now;

    }
}
