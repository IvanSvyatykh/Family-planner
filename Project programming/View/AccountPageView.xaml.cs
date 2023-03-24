namespace AccountPage
{
    public  partial  class AccountPageView : ContentPage
    {
        public AccountPageView()
        {
            InitializeComponent();
            BindingContext = new AccountPageViewModel();
        }      
    }
}


