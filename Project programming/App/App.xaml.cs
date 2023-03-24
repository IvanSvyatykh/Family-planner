using AccountPage;
using DialogService;
using ForgottenPage;
using RegistrationPage;

namespace AppService
{
    public partial class App : Application
    {
        public static IServiceProvider Services;
        public static IAlertService AlertSvc;
        public string UserEmail;
        public ushort UserId;
        public App(IServiceProvider provider)
        {
            InitializeComponent();
            Services = provider;
            AlertSvc = Services.GetService<IAlertService>();
            Routing.RegisterRoute("RegistrationPage", typeof(RegistrationPageView));
            Routing.RegisterRoute("AccountPageView", typeof(AccountPageView));
            Routing.RegisterRoute("ForgottenPasswordPage", typeof(ForgottenPasswordPage));
            MainPage = new AppShell();
        }
    }

}
