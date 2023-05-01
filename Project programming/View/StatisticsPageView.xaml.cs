namespace StatisticsPage
{
    public partial class StatisticsPageView : ContentPage
    {
        public StatisticsPageView()
        {
            InitializeComponent();
            BindingContext = new StatisticsPageViewModel();
        }
    }
}

