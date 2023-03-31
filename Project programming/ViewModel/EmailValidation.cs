using InputKit.Shared.Validations;
using WorkWithEmail;

namespace Validations
{
    public class EmailValidation : IValidation
    {
        public string Message => "This string can not be an Email";

        public bool Validate(object value)
        {
            return CheckEmailCorectness.IsValidEmail(value.ToString());
        }
    }
}
