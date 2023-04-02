using InputKit.Shared.Validations;
using WorkWithEmail;

namespace Validations
{
    public class EmailValidation : IValidation
    {
        public string Message => "This string can not be an Email";

        public bool Validate(object value)
        {
            if (value is string text)
            {
                return text.Count(x => x == '@') == 1 && text.Split('@').Last().Length >= 2;
            }
            return false;
        }
    }
}
