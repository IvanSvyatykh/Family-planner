using WorkWithEmail;

namespace ForgottenPage
{
    public partial class ForgottenPasswordPage : ContentPage
    {
        public ForgottenPasswordPage()
        {
            InitializeComponent();
            BindingContext = new ForgottenPasswordPageViewModel();
        }
    }
}
