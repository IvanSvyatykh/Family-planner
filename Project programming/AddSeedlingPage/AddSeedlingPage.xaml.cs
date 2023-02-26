using AddSeedlingPage;

namespace Project_programming.AddSeedlingPage
{
    public partial class AddSeedlingPage : ContentPage
    {
        public AddSeedlingPage()
        {
            InitializeComponent();
            BindingContext = new SeedlngPageViewModel();
        }
    }
}

