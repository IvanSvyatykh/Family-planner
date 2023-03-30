using UraniumUI.Material.Controls;

namespace Expenses
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

