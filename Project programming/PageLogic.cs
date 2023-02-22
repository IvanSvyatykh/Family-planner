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
        public static bool CheckCountTry(int countTry)
        {

            return countTry <= 3;

        }

    }
}
