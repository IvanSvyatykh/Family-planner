using System.Xml;
using Project_programming.WorkWithEmail;
using Project_programming.Database;
namespace Project_programming;
public partial class MainPage : ContentPage
{
    private string Email;
    private string Password;

    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }
    private void NameEntry_Password(object sender, TextChangedEventArgs e) => Password = passwordEntry.Text;
    private void NameEntry_Email(object sender, TextChangedEventArgs e) => Email = emailEntry.Text;
    private async void SignInButtonIsPressed(object sender, EventArgs e)
    {
        if (!CheckEmailCorectness.ConnectionAvailable())
        {
            await DisplayAlert("Attention", "There is no internet connection", "Ok");
            return;
        }
        if (Password == null || Email == null)
        {
            await DisplayAlert("Attention", "All fields must be field", "Ok");
            return;
        }
        else
        {
            await Shell.Current.GoToAsync("SignInPage");
        }
    }

}