using AccountViewModel;
namespace Project_programming.Account
{
    public partial class AccountPageView : ContentPage
    {
        public AccountPageView()
        {
            InitializeComponent();
            BindingContext = new AccountPageViewModel();
        }       
    }
}


