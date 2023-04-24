using UraniumUI.Material.Controls;

namespace ExpensesPage
{
    public partial class ExpensesView : ContentPage
    {
        public ExpensesView()
        {
            InitializeComponent();
            BindingContext = new ExpensesViewModel();
        }
    }
}

