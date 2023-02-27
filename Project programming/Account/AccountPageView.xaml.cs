namespace Project_programming.Account;
using AccountViewModel;

public partial class AccountPageView : ContentPage
{
	public AccountPageView()
	{
		InitializeComponent();
		BindingContext = new AccountPageViewModel();
	}
}