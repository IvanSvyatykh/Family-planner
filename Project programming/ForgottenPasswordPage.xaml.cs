using WorkWithEmail;
using PasswordLogic;

namespace Project_programming;
public partial class ForgottenPasswordPage : ContentPage
{
    private string Email;
    private DateTime Time;
    public ForgottenPasswordPage()
    {
        InitializeComponent();
    }
    private void NameEntry_Email(object sender, TextChangedEventArgs e) => Email = emailEntry.Text;

    private async void SendEmail(object sender, EventArgs e)
    {

        int Ñonfirmationcode = PasswordLog.RandomNumberGenerator();
        EmailWriter.SendMessage(Email, "Ñonfirmation code", "Code: " + Ñonfirmationcode.ToString());

        Dictionary<int, string[]> messages = new Dictionary<int, string[]>
        {
            { 0, new string[] { "", "We have alredy sent you messege", "Ok" } },
            { 1, new string[] { } },
            { 2, new string[] {"Attention", "Check code correctness", "Ok"}},
            { 3, new string[] { "Great", "You can restore your password", "Ok" } },
            { 4, new string[] {}}
        };
    }
}
