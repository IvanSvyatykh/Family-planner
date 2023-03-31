

namespace RegistrationPage
{
    public partial class RegistrationPageView : ContentPage
    {
        
        public RegistrationPageView()
        {
            
            InitializeComponent();
            BindingContext = new RegistrationPageViewModel();
        }
    }
}
