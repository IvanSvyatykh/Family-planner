namespace FamilyRegistrationPage;

public partial class FamilyRegisrationPageView : ContentPage
{
	public FamilyRegisrationPageView()
	{
		InitializeComponent();
		BindingContext =  new FamilyRegistrationViewModel();
	}
}