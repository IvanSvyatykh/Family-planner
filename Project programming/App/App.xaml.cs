using DialogManager;
using AddSeedlingPage;

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
        Routing.RegisterRoute("SeedlingPage", typeof(SeedlingPage));
        Routing.RegisterRoute("ForgottenPasswordPage", typeof(ForgottenPasswordPage));
        MainPage = new AppShell();
	}
}
