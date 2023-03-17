namespace FamilyPage
{
    public partial class FamilyView : ContentPage
    {
        public FamilyView()
        {
            InitializeComponent();
            BindingContext = new FamilyViewModel();
        }
    }
}

