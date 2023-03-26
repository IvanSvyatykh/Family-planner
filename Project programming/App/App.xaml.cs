using AccountPage;
using Classes;
using DialogService;
using ForgottenPasswordPage;
using RegistrationPage;


namespace AppService
{
    public partial class App : Application
    {
        public static IServiceProvider Services;
        public static IAlertService AlertSvc;
        public User _user;
        public Family _family;
        public App(IServiceProvider provider)
        {
            InitializeComponent();
            Services = provider;
            AlertSvc = Services.GetService<IAlertService>();
            Routing.RegisterRoute("RegistrationPage", typeof(RegistrationPageView));
            Routing.RegisterRoute("AccountPageView", typeof(AccountPageView));
            Routing.RegisterRoute("ForgottenPasswordPage", typeof(ForgottenPasswordPageView));
            MainPage = new AppShell();
        }
    }

}
