
using Project_programming.JoinFamily;
namespace Project_programming;
public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		Routing.RegisterRoute("SignInPage",typeof(SignInPage));
        Routing.RegisterRoute("RegistrationPage", typeof(RegistrationPage));
        Routing.RegisterRoute("ForgottenPasswordPage", typeof(ForgottenPasswordPage));
		Routing.RegisterRoute("JoinFamilyPage" , typeof(JoinFamilyPage));
        MainPage = new AppShell();
	}
}
