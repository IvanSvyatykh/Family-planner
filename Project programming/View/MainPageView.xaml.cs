using InputKit.Shared.Validations;
using UraniumUI.Views;

namespace MainPage
{
    public partial class MainPageView : ContentPage
    {
        public MainPageView()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }
    }


}