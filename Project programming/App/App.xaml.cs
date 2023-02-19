
namespace Project_programming;
public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		Routing.RegisterRoute("SignInPage",typeof(SignInPage));
        Routing.RegisterRoute("RegistrationPage", typeof(RegistrationPage));
        Routing.RegisterRoute("ForgottenPasswordPage", typeof(ForgottenPasswordPage));
        MainPage = new AppShell();
	}
}
