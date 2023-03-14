using Project_programming.Account;
using Project_programming.Model;

namespace Project_programming;
public partial class App : Application
{
    public static IServiceProvider Services;
    public static IAlertService AlertSvc;
    public App(IServiceProvider provider)
	{       
        InitializeComponent();
        Services = provider;
        AlertSvc = Services.GetService<IAlertService>();
        Routing.RegisterRoute("RegistrationPage", typeof(RegistrationPage));
        Routing.RegisterRoute("AccountPageView", typeof(AccountPageView));
        Routing.RegisterRoute("ForgottenPasswordPage", typeof(ForgottenPasswordPage));
        MainPage = new AppShell();
	}
}
