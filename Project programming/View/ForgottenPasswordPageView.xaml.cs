using PasswordLogic;
using Project_programming.WorkWithEmail;
using Project_programming.ForgottenPage;

namespace Project_programming;
public partial class ForgottenPasswordPage : ContentPage
{
    public ForgottenPasswordPage()
    {
        InitializeComponent();
        BindingContext = new ForgottenPasswordPageViewModel();
    }
}