using WorkWithEmail;

namespace Project_programming;
public partial class ForgottenPasswordPage : ContentPage
{
    private string Email;
    public ForgottenPasswordPage()
    {
        InitializeComponent();
    }
    private void NameEntry_Email(object sender, TextChangedEventArgs e) => Email = emailEntry.Text;

    private void SendEmail(object sender, EventArgs e)
    {
        EmailWriter.SendMessage(Email, "Ex", "Hello");
    }

}