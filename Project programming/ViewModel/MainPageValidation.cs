using InputKit.Shared.Validations;
using WorkWithEmail;

namespace MainPage
{
    public class MainPageEmailValidation : IValidation
    {
        public string Message => "This string can not be an Email";

        public bool Validate(object value)
        {
            return CheckEmailCorectness.IsValidEmail(value.ToString());
        }
    }
}
