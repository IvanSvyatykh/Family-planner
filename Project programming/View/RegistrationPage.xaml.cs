using PasswordLogic;
using Project_programming.WorkWithEmail;

namespace Project_programming;
public partial class RegistrationPage : ContentPage
{
    public RegistrationPage()
    {
        InitializeComponent();
        BindingContext = new RegistrationPageViewModel();
    }
}