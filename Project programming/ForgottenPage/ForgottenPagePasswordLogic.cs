using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageLogic
{
    public class ForgottenPagePasswordLogic
    {
        public static bool CompareAnswerAndCode(int? answer, int code) => answer== code;    

    }
}
