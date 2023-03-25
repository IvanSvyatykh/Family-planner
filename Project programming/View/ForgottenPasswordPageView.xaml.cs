namespace ForgottenPasswordPage
{
    public partial class ForgottenPasswordPageView : ContentPage
    {
        public ForgottenPasswordPageView()
        {
            InitializeComponent();
            BindingContext = new ForgottenPasswordPageViewModel();
        }
    }
}
